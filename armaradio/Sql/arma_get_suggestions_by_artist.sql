-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luis Valle
-- =============================================
alter PROCEDURE arma_get_suggestions_by_artist
(
	@artist_mbid varchar(50)
)
AS
BEGIN
	set nocount on;
	declare
		@all_artist_ids varchar(max),
		@sql nvarchar(max);

	drop table if exists #tmp_prefinalset;
	drop table if exists #tmp_artist_names;
	
	create table #tmp_prefinalset
	(
		[temp_weight] uniqueidentifier,
		[row_id] int,
		[song_name] nvarchar(350),
		[artist_mbid] uniqueidentifier,
		[user_count] int
	);
	create table #tmp_artist_names
	(
		[artist_mbid] varchar(50),
		[Name] nvarchar(350)
	);

	with topusersbyartist as (
		select top 25 
			[userid], 
			[total_artist_plays] = sum([times_heard])
		from [arma_main].[dbo].[SongHistoryView]
		where
			[artist_mbid] = @artist_mbid
		group by
			[userid]
		order by
			[total_artist_plays] desc
	),
	common_artists as (
		select top 25 
			[artist_mbid], 
			[user_count] = count(distinct [userid])
		from [arma_main].[dbo].[SongHistoryView]
		where
			[userid] in (
				select [userid] from topusersbyartist
			)
		group by
			[artist_mbid]
		order by
			[user_count] desc
	),
	songsuggestions_pre1 as (
		select top 2000
			[song_name], 
			[artist_mbid],
			[user_count] = count(distinct [userid])
		from [arma_main].[dbo].[SongHistoryView] sv
		where 
			[artist_mbid] in (
				select [artist_mbid] from common_artists
			)
			and [userid] in (
				select [userid] from topusersbyartist
			)
		group by
			[song_name],
			[artist_mbid]
		order by
			[user_count] desc
	),
	songsuggestions_pre as (
		select --top 100 percent
			[row_id] = row_number() over (order by [song_name], [artist_mbid], [user_count]), --(order by [user_count] desc),
			[song_name], 
			[artist_mbid],
			[user_count]
		from songsuggestions_pre1
		--order by
			--[user_count] desc
	),
	songs_distinct_max as (
		select top 50
			[row_id] = max([row_id]),
			[artist_mbid],
			[user_count] = max([user_count]),
			[temp_weight] = newid(),
			[priority] = 1
		from songsuggestions_pre
		where
			[user_count] > 1
		group by
			[artist_mbid]
	),
	songs_distinct_flat as (
		select top 50
			[row_id] = max([row_id]),
			[artist_mbid],
			[user_count] = max([user_count]),
			[temp_weight] = newid(),
			[priority] = 0
		from songsuggestions_pre
		where
			[user_count] = 1
		group by
			[artist_mbid]
	),
	songs_distinct_joined as (
		select
			[row_id],
			[artist_mbid],
			[user_count],
			[temp_weight],
			[priority]
		from songs_distinct_max
		union all
		select
			[row_id],
			[artist_mbid],
			[user_count],
			[temp_weight],
			[priority]
		from songs_distinct_flat
	),
	songs_distinct as (
		select top 25
			[temp_weight],
			[row_id],
			[artist_mbid],
			[user_count]
		from songs_distinct_joined
		order by
			[priority] desc,
			[temp_weight]
	),
	songsuggestions as (
		select
			sd.[temp_weight],
			ss.[row_id],
			ss.[song_name], 
			ss.[artist_mbid],
			ss.[user_count]
		from songsuggestions_pre ss
		join songs_distinct sd
			on sd.[row_id] = ss.[row_id]
	)

	insert into #tmp_prefinalset
	select
		*
	from songsuggestions
	order by
		[temp_weight];

	select
		@all_artist_ids = stuff((
			select distinct ',' + cast([artist_mbid] as varchar(50))
			from #tmp_prefinalset
			for xml path(''), type
		).value('.', 'varchar(max)'), 1, 1, '');
		
	set @sql = '		
		select
			*
		from OPENQUERY(arma_radio_server, ''exec armaradio.dbo.Arma_GetArtistByMBIds @mb_ids=''''' + @all_artist_ids + ''''''');
	';

	insert into #tmp_artist_names
	(
		[artist_mbid],
		[Name]
	)
	exec sp_executesql @sql;

	with t_rs as (
		select
			ss.[temp_weight],
			ss.[row_id],
			ss.[song_name], 
			[artist_mbid] = cast(ss.[artist_mbid] as varchar(50)),
			ss.[user_count],
			[artist_name] = an.[Name]
		from #tmp_prefinalset ss
		left join #tmp_artist_names an
			on an.[artist_mbid] = ss.[artist_mbid]
	)

	select
		[row_id],
		[song_name], 
		[artist_mbid],
		[user_count],
		[artist_name]
	from t_rs
	order by
		[temp_weight];

	/*
		exec arma_get_suggestions_by_artist
			'FFE16BBA-4D84-409B-8F22-5242C60B930F'

		exec arma_get_suggestions_by_artist
			'E1E05CCE-3922-44E1-8F20-015ABE5E309D'
	*/
END
GO
