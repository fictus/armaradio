USE [armaradio]
GO
/****** Object:  StoredProcedure [dbo].[Arma_GetAlbumByMbid]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[Arma_GetAlbumByMbid]
(
	@mbid varchar(50)
)
AS
BEGIN
	set nocount on;

	with t_a as (
		select
			[Id],
			[MBAlbumId],
			[ArtistId],
			[MBArtistId],
			[Title],
			[TitleFlat],
			[Status],
			[PrimaryType],
			[FirstReleaseDate],
			[AddedDateTime],
			[DbSource] = 'A'
		from [arma_a].[dbo].[AlbumMapper]
		where
			[MBAlbumId] = @mbid
	),
	t_b as (
		select
			[Id],
			[MBAlbumId],
			[ArtistId],
			[MBArtistId],
			[Title],
			[TitleFlat],
			[Status],
			[PrimaryType],
			[FirstReleaseDate],
			[AddedDateTime],
			[DbSource] = 'B'
		from [arma_b].[dbo].[AlbumMapper]
		where
			[MBAlbumId] = @mbid
	),
	t_c as (
		select
			[Id],
			[MBAlbumId],
			[ArtistId],
			[MBArtistId],
			[Title],
			[TitleFlat],
			[Status],
			[PrimaryType],
			[FirstReleaseDate],
			[AddedDateTime],
			[DbSource] = 'C'
		from [arma_c].[dbo].[AlbumMapper]
		where
			[MBAlbumId] = @mbid
	),
	t_d as (
		select
			[Id],
			[MBAlbumId],
			[ArtistId],
			[MBArtistId],
			[Title],
			[TitleFlat],
			[Status],
			[PrimaryType],
			[FirstReleaseDate],
			[AddedDateTime],
			[DbSource] = 'D'
		from [arma_d].[dbo].[AlbumMapper]
		where
			[MBAlbumId] = @mbid
	),
	t_e as (
		select
			[Id],
			[MBAlbumId],
			[ArtistId],
			[MBArtistId],
			[Title],
			[TitleFlat],
			[Status],
			[PrimaryType],
			[FirstReleaseDate],
			[AddedDateTime],
			[DbSource] = 'E'
		from [arma_e].[dbo].[AlbumMapper]
		where
			[MBAlbumId] = @mbid
	),
	t_f as (
		select
			[Id],
			[MBAlbumId],
			[ArtistId],
			[MBArtistId],
			[Title],
			[TitleFlat],
			[Status],
			[PrimaryType],
			[FirstReleaseDate],
			[AddedDateTime],
			[DbSource] = 'F'
		from [arma_f].[dbo].[AlbumMapper]
		where
			[MBAlbumId] = @mbid
	),
	t_g as (
		select
			[Id],
			[MBAlbumId],
			[ArtistId],
			[MBArtistId],
			[Title],
			[TitleFlat],
			[Status],
			[PrimaryType],
			[FirstReleaseDate],
			[AddedDateTime],
			[DbSource] = 'G'
		from [arma_g].[dbo].[AlbumMapper]
		where
			[MBAlbumId] = @mbid
	),
	t_h as (
		select
			[Id],
			[MBAlbumId],
			[ArtistId],
			[MBArtistId],
			[Title],
			[TitleFlat],
			[Status],
			[PrimaryType],
			[FirstReleaseDate],
			[AddedDateTime],
			[DbSource] = 'H'
		from [arma_h].[dbo].[AlbumMapper]
		where
			[MBAlbumId] = @mbid
	),
	t_i as (
		select
			[Id],
			[MBAlbumId],
			[ArtistId],
			[MBArtistId],
			[Title],
			[TitleFlat],
			[Status],
			[PrimaryType],
			[FirstReleaseDate],
			[AddedDateTime],
			[DbSource] = 'I'
		from [arma_i].[dbo].[AlbumMapper]
		where
			[MBAlbumId] = @mbid
	),
	t_j as (
		select
			[Id],
			[MBAlbumId],
			[ArtistId],
			[MBArtistId],
			[Title],
			[TitleFlat],
			[Status],
			[PrimaryType],
			[FirstReleaseDate],
			[AddedDateTime],
			[DbSource] = 'J'
		from [arma_j].[dbo].[AlbumMapper]
		where
			[MBAlbumId] = @mbid
	),
	t_k as (
		select
			[Id],
			[MBAlbumId],
			[ArtistId],
			[MBArtistId],
			[Title],
			[TitleFlat],
			[Status],
			[PrimaryType],
			[FirstReleaseDate],
			[AddedDateTime],
			[DbSource] = 'K'
		from [arma_k].[dbo].[AlbumMapper]
		where
			[MBAlbumId] = @mbid
	),
	t_l as (
		select
			[Id],
			[MBAlbumId],
			[ArtistId],
			[MBArtistId],
			[Title],
			[TitleFlat],
			[Status],
			[PrimaryType],
			[FirstReleaseDate],
			[AddedDateTime],
			[DbSource] = 'L'
		from [arma_l].[dbo].[AlbumMapper]
		where
			[MBAlbumId] = @mbid
	),
	t_m as (
		select
			[Id],
			[MBAlbumId],
			[ArtistId],
			[MBArtistId],
			[Title],
			[TitleFlat],
			[Status],
			[PrimaryType],
			[FirstReleaseDate],
			[AddedDateTime],
			[DbSource] = 'M'
		from [arma_m].[dbo].[AlbumMapper]
		where
			[MBAlbumId] = @mbid
	),
	t_n as (
		select
			[Id],
			[MBAlbumId],
			[ArtistId],
			[MBArtistId],
			[Title],
			[TitleFlat],
			[Status],
			[PrimaryType],
			[FirstReleaseDate],
			[AddedDateTime],
			[DbSource] = 'N'
		from [arma_n].[dbo].[AlbumMapper]
		where
			[MBAlbumId] = @mbid
	),
	t_o as (
		select
			[Id],
			[MBAlbumId],
			[ArtistId],
			[MBArtistId],
			[Title],
			[TitleFlat],
			[Status],
			[PrimaryType],
			[FirstReleaseDate],
			[AddedDateTime],
			[DbSource] = 'O'
		from [arma_o].[dbo].[AlbumMapper]
		where
			[MBAlbumId] = @mbid
	),
	t_p as (
		select
			[Id],
			[MBAlbumId],
			[ArtistId],
			[MBArtistId],
			[Title],
			[TitleFlat],
			[Status],
			[PrimaryType],
			[FirstReleaseDate],
			[AddedDateTime],
			[DbSource] = 'P'
		from [arma_p].[dbo].[AlbumMapper]
		where
			[MBAlbumId] = @mbid
	),
	t_q as (
		select
			[Id],
			[MBAlbumId],
			[ArtistId],
			[MBArtistId],
			[Title],
			[TitleFlat],
			[Status],
			[PrimaryType],
			[FirstReleaseDate],
			[AddedDateTime],
			[DbSource] = 'Q'
		from [arma_q].[dbo].[AlbumMapper]
		where
			[MBAlbumId] = @mbid
	),
	t_r as (
		select
			[Id],
			[MBAlbumId],
			[ArtistId],
			[MBArtistId],
			[Title],
			[TitleFlat],
			[Status],
			[PrimaryType],
			[FirstReleaseDate],
			[AddedDateTime],
			[DbSource] = 'R'
		from [arma_r].[dbo].[AlbumMapper]
		where
			[MBAlbumId] = @mbid
	),
	t_s as (
		select
			[Id],
			[MBAlbumId],
			[ArtistId],
			[MBArtistId],
			[Title],
			[TitleFlat],
			[Status],
			[PrimaryType],
			[FirstReleaseDate],
			[AddedDateTime],
			[DbSource] = 'S'
		from [arma_s].[dbo].[AlbumMapper]
		where
			[MBAlbumId] = @mbid
	),
	t_t as (
		select
			[Id],
			[MBAlbumId],
			[ArtistId],
			[MBArtistId],
			[Title],
			[TitleFlat],
			[Status],
			[PrimaryType],
			[FirstReleaseDate],
			[AddedDateTime],
			[DbSource] = 'T'
		from [arma_t].[dbo].[AlbumMapper]
		where
			[MBAlbumId] = @mbid
	),
	t_u as (
		select
			[Id],
			[MBAlbumId],
			[ArtistId],
			[MBArtistId],
			[Title],
			[TitleFlat],
			[Status],
			[PrimaryType],
			[FirstReleaseDate],
			[AddedDateTime],
			[DbSource] = 'U'
		from [arma_u].[dbo].[AlbumMapper]
		where
			[MBAlbumId] = @mbid
	),
	t_v as (
		select
			[Id],
			[MBAlbumId],
			[ArtistId],
			[MBArtistId],
			[Title],
			[TitleFlat],
			[Status],
			[PrimaryType],
			[FirstReleaseDate],
			[AddedDateTime],
			[DbSource] = 'V'
		from [arma_v].[dbo].[AlbumMapper]
		where
			[MBAlbumId] = @mbid
	),
	t_w as (
		select
			[Id],
			[MBAlbumId],
			[ArtistId],
			[MBArtistId],
			[Title],
			[TitleFlat],
			[Status],
			[PrimaryType],
			[FirstReleaseDate],
			[AddedDateTime],
			[DbSource] = 'W'
		from [arma_w].[dbo].[AlbumMapper]
		where
			[MBAlbumId] = @mbid
	),
	t_x as (
		select
			[Id],
			[MBAlbumId],
			[ArtistId],
			[MBArtistId],
			[Title],
			[TitleFlat],
			[Status],
			[PrimaryType],
			[FirstReleaseDate],
			[AddedDateTime],
			[DbSource] = 'X'
		from [arma_x].[dbo].[AlbumMapper]
		where
			[MBAlbumId] = @mbid
	),
	t_y as (
		select
			[Id],
			[MBAlbumId],
			[ArtistId],
			[MBArtistId],
			[Title],
			[TitleFlat],
			[Status],
			[PrimaryType],
			[FirstReleaseDate],
			[AddedDateTime],
			[DbSource] = 'Y'
		from [arma_y].[dbo].[AlbumMapper]
		where
			[MBAlbumId] = @mbid
	),
	t_z as (
		select
			[Id],
			[MBAlbumId],
			[ArtistId],
			[MBArtistId],
			[Title],
			[TitleFlat],
			[Status],
			[PrimaryType],
			[FirstReleaseDate],
			[AddedDateTime],
			[DbSource] = 'Z'
		from [arma_z].[dbo].[AlbumMapper]
		where
			[MBAlbumId] = @mbid
	),
	t_num as (
		select
			[Id],
			[MBAlbumId],
			[ArtistId],
			[MBArtistId],
			[Title],
			[TitleFlat],
			[Status],
			[PrimaryType],
			[FirstReleaseDate],
			[AddedDateTime],
			[DbSource] = 'Num'
		from [arma_num].[dbo].[AlbumMapper]
		where
			[MBAlbumId] = @mbid
	),
	t_symb as (
		select
			[Id],
			[MBAlbumId],
			[ArtistId],
			[MBArtistId],
			[Title],
			[TitleFlat],
			[Status],
			[PrimaryType],
			[FirstReleaseDate],
			[AddedDateTime],
			[DbSource] = 'Symb'
		from [arma_symb].[dbo].[AlbumMapper]
		where
			[MBAlbumId] = @mbid
	),
	t_all as (
		select * from t_a
		union all
		select * from t_b
		union all
		select * from t_c
		union all
		select * from t_d
		union all
		select * from t_e
		union all
		select * from t_f
		union all
		select * from t_g
		union all
		select * from t_h
		union all
		select * from t_i
		union all
		select * from t_j
		union all
		select * from t_k
		union all
		select * from t_l
		union all
		select * from t_m
		union all
		select * from t_n
		union all
		select * from t_o
		union all
		select * from t_p
		union all
		select * from t_q
		union all
		select * from t_r
		union all
		select * from t_s
		union all
		select * from t_t
		union all
		select * from t_u
		union all
		select * from t_v
		union all
		select * from t_w
		union all
		select * from t_x
		union all
		select * from t_y
		union all
		select * from t_z
		union all
		select * from t_num
		union all
		select * from t_symb
	)

	select
		[Id],
		[MBAlbumId],
		[ArtistId],
		[MBArtistId],
		[Title],
		[TitleFlat],
		[Status],
		[PrimaryType],
		[FirstReleaseDate],
		[AddedDateTime],
		[DbSource]
	from t_all;

	/*
		exec Arma_GetAlbumByMbid
			'0cd85542-7bcd-4f23-a8d3-1462fee3f250'
	*/
END
GO
/****** Object:  StoredProcedure [dbo].[Arma_GetAlbumsForArtist]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[Arma_GetAlbumsForArtist]
(
	@artist_id int,
	@debug bit = 0
)
AS
BEGIN
	set nocount on;
	
	declare
		@db_source varchar(15),
		@sql nvarchar(max);

	select
		@db_source = [DBSource]
	from [arma_artists].[dbo].[ArmaArtistsMain]
	where
		[Id] = @artist_id;

	drop table if exists #tmp_allalbums_byartist;
	create table #tmp_allalbums_byartist
	(
		[Id] int,
		[ArtistId] int,
		[AlbumName] varchar(600),
		[AlbumName_Flat] varchar(600),
		[AlbumDetails] varchar(600),		
		[ReleaseDate] varchar(150),
		[Label] varchar(250),
		[IsSingle] bit,
		[DBSource] varchar(15),
		[FirstReleaseDate] varchar(50),
		[AddedDateTime] datetime
	)

	if @db_source is not null
	begin
		set @sql = '
			insert into #tmp_allalbums_byartist
			(
				[Id],
				[ArtistId],
				[AlbumName],
				[AlbumName_Flat],
				[AlbumDetails],
				[ReleaseDate],
				[Label],
				[IsSingle],
				[FirstReleaseDate],
				[DBSource],
				[AddedDateTime]
			)
			select
				[Id] = max([Id]),
				[ArtistId],
				[Title],
				[TitleFlat],
				[AlbumDetails] = '''',
				[ReleaseDate] = '''',
				[Label] = '''',
				[IsSingle] = (
					case
						when [PrimaryType] = ''Album'' and [Status] = ''Official'' then
							0
						else
							1
					end
				),
				[FirstReleaseDate],
				[DBSource] = ''' + @db_source + ''',
				[AddedDateTime] = max([AddedDateTime])
			from [arma_' + @db_source + '].[dbo].[AlbumMapper]
			where
				[ArtistId] = ' + cast(@artist_id as varchar(100)) + '
			group by
				[ArtistId],
				[Title],
				[TitleFlat],
				[Status],
				[PrimaryType],
				[FirstReleaseDate];
		';

		if @debug = 1
		begin
			select @sql;
		end

		exec sp_executesql @sql;
	end
	
	select
		[Id],
		[ArtistId],
		[AlbumName],
		[AlbumName_Flat],
		[AlbumDetails],
		[ReleaseDate],
		[Label],
		[IsSingle],
		[FirstReleaseDate],
		[DBSource],
		[AddedDateTime]
	from #tmp_allalbums_byartist
	where
		[IsSingle] = 0
	order by
		[AlbumName_Flat];
	
	select
		[Id],
		[ArtistId],
		[AlbumName],
		[AlbumName_Flat],
		[AlbumDetails],
		[ReleaseDate],
		[Label],
		[IsSingle],
		[FirstReleaseDate],
		[DBSource],
		[AddedDateTime]
	from #tmp_allalbums_byartist
	where
		[IsSingle] = 1
	order by
		[AlbumName_Flat];

	/*
		exec Arma_GetAlbumsForArtist
			153245

		exec Arma_GetAlbumsForArtist
			6954

		exec Arma_GetAlbumsForArtist
			1122
	*/
END
GO
/****** Object:  StoredProcedure [dbo].[Arma_GetArtistById]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[Arma_GetArtistById]
(
	@artist_id int
)
AS
BEGIN
	select
		[Id],
		[MBId],
		[Name],
		[SortName]
	from [arma_artists].dbo.[ArmaArtistsMain]

	/*
		exec Arma_GetArtistById
			565
	*/
END
GO
/****** Object:  StoredProcedure [dbo].[Arma_GetArtistByMBId]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[Arma_GetArtistByMBId]
(
	@artist_mbid varchar(50)
)
AS
BEGIN
	set nocount on;

	select
		[Id],
		[ArtistName] = [NameSearch],
		[ArtistName_Flat] = [NameFlat],
		[DBDesignator] = [DBSource],
		[EntryDateTime] = [AddedDateTime],
		[count] = 1
	from [arma_artists].[dbo].[ArmaArtistsMain]
	where
		[MBId] = @artist_mbid;

	--select
	--	rs.[Id],
	--	rs.[ArtistName],
	--	rs.[ArtistName_Flat],
	--	rs.[DBDesignator],
	--	rs.[EntryDateTime],
	--	rs.[count]
	--	--rs.[inlcude]
	--from t_distict rs
	--order by
	--	rs.[SortOrder],
	--	rs.[ArtistName_Flat];

	/*
		exec Arma_GetArtistByMBid
			'b77c86fb-8b31-44d3-95fe-398fbb12c7ce'

	*/
END
GO
/****** Object:  StoredProcedure [dbo].[Arma_GetArtistByMBIds]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[Arma_GetArtistByMBIds]
(	
	@mb_ids varchar(max)
)
AS
BEGIN
	select
		am.[MBId],
		am.[Name]
	from [arma_artists].dbo.[ArmaArtistsMain] am
	join (
		select
			[Token]
		from dbo.SplitStringToTableVarchar(@mb_ids, ',')
	) ai
		on ai.[Token] = am.[MBId];

	/*
		exec Arma_GetArtistByMBIds
			'4A069029-4F64-4946-B650-01AEB0B55D9D,5db9f569-cadd-4f8b-b460-d4031b0b3716'
	*/
END
GO
/****** Object:  StoredProcedure [dbo].[Arma_GetArtistNameByMBId]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luis Valle
-- =============================================
create PROCEDURE [dbo].[Arma_GetArtistNameByMBId]
(
	@artist_mbid varchar(50)
)
AS
BEGIN
	select
		[Name]
	from [arma_artists].dbo.[ArmaArtistsMain]
	where
		[MBId] = @artist_mbid;

	/*
		exec Arma_GetArtistByMBId
			'4A069029-4F64-4946-B650-01AEB0B55D9D'
	*/
END
GO
/****** Object:  StoredProcedure [dbo].[Arma_GetArtistSimplenIfoByMBId]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[Arma_GetArtistSimplenIfoByMBId]
(	
	@mb_id varchar(50)
)
AS
BEGIN
	select
		[MBId],
		[Name]
	from [arma_artists].dbo.[ArmaArtistsMain]
	where
		[MBId] = @mb_id;

	/*
		exec Arma_GetArtistSimplenIfoByMBId
			'4A069029-4F64-4946-B650-01AEB0B55D9D'
	*/
END
GO
/****** Object:  StoredProcedure [dbo].[Arma_GetRandomFromPlaylists]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[Arma_GetRandomFromPlaylists]
(
	@user_identity varchar(50)
)
AS
BEGIN
	set nocount on;

	select top 150
		ps.[Artist],
		ps.[Song],
		ps.[VideoId]
	from ArmaPlaylistData ps
	join ArmaPlaylistNames pn
		on pn.[Id] = ps.[PlaylistId]
	where
		ps.[VideoId] is not null
		and pn.[UserId] = @user_identity
	order by
		rand(checksum(*) * rand());
		--newid();

	/*
		exec Arma_GetRandomFromPlaylists
			'929a11b5-5e2e-46fd-97c8-198f7567170c'
	*/
END
GO
/****** Object:  StoredProcedure [dbo].[Arma_GetRelatedArtistMBIds]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[Arma_GetRelatedArtistMBIds]
(
	@artist_mbid varchar(50),
	@only_by_genre bit = 0,
	@use_spotify_genres bit = 0
)
AS
BEGIN
	set nocount on;
	declare
		@artist_country varchar(150);

	select
		@artist_country = [Country]
	from [arma_artists].[dbo].[ArmaArtistsMain]
	where
		[MBId] = @artist_mbid;

	if @only_by_genre = 1
	begin
		declare @mpt_artist_mbids table
		(
			[ArtistMBId] varchar(50)
		);

		declare @mpt_artist_results table
		(
			[ArtistMBId] varchar(50),
			[Name] varchar(300)
		);

		declare @mpt_t_genres table
		(
			[GenreId] varchar(50)
		);

		declare @mpt_t_genres_distinct table
		(
			[GenreId] varchar(50)
		);

		if @use_spotify_genres = 0
		begin
			insert into @mpt_t_genres
			(
				[GenreId]
			)
			select
				[GenreId] = am.[MBId]
			from [arma_artists].[dbo].[ArmaArtistGenreMap] gm
			join [arma_artists].[dbo].[ArmaArtistsMain] ar
				on ar.[MBId] = @artist_mbid
				and ar.[Id] = gm.[ArtistId]
			join [arma_artists].[dbo].[ArmaGenres] am
				on am.[Id] = gm.[GenreId];

			insert into @mpt_t_genres
			(
				[GenreId]
			)
			select
				[GenreId] = st.[MBId]
			from [arma_artists].[dbo].[ArmaGenresStaged] st
			left join @mpt_t_genres ag
				on ag.[GenreId] = st.[MBId]
			where
				st.[ArtistMBId] = @artist_mbid
				and ag.[GenreId] is null;
		end

		if @use_spotify_genres = 1 or (select count(*) from @mpt_t_genres) = 0
		begin
			insert into @mpt_t_genres
			(
				[GenreId]
			)
			select
				[GenreId] = am.[genre_identity]
			from [arma_genres].[dbo].[genre_artists] gm
			join [arma_artists].[dbo].[ArmaArtistsMain] ar
				on ar.[MBId] = gm.[artist_id]
			join [arma_genres].[dbo].[genre_root] am
				on am.[id] = gm.[genre_id]
			where
				ar.[MBId] = @artist_mbid;


			insert into @mpt_t_genres_distinct
			(
				[GenreId]
			)
			select distinct
				[GenreId]
			from @mpt_t_genres;

			insert into @mpt_artist_mbids
			(
				[ArtistMBId]
			)
			select distinct
				cast(ag.[artist_id] as varchar(50))
			from [arma_genres].dbo.[genre_artists] ag
			join [arma_genres].dbo.[genre_root] gg
				on gg.[id] = ag.[genre_id]
			join @mpt_t_genres_distinct gr
				on gr.[GenreId] = gg.[genre_identity]
			where
				ag.[artist_id] != @artist_mbid;

			--insert into @mpt_artist_mbids
			--(
			--	[ArtistMBId]
			--)
			--select
			--	[ArtistMBId]
			--from @mpt_artist_mbids;

			insert into @mpt_artist_results
			(
				[ArtistMBId],
				[Name]
			)
			select top 300
				gr.[ArtistMBId],
				ar.[Name]
			from @mpt_artist_mbids gr
			join [arma_artists].[dbo].[ArmaArtistsMain] ar
				on ar.[MBId] = gr.[ArtistMBId]
			where
				coalesce(ar.[RatingValue], 0) >= 3
				and (
					(@artist_country is null)
					or
					(ar.[Country] = @artist_country)
				);
		end
		else
		begin
			insert into @mpt_t_genres_distinct
			(
				[GenreId]
			)
			select distinct
				[GenreId]
			from @mpt_t_genres;

			insert into @mpt_artist_mbids
			(
				[ArtistMBId]
			)
			select distinct
				ag.[ArtistMBId]
			from ArmaArtistsWithGenresView ag
			join @mpt_t_genres_distinct gr
				on gr.[GenreId] = ag.[GenreId]
			where
				ag.[ArtistMBId] != @artist_mbid;

			--insert into @mpt_artist_mbids
			--(
			--	[ArtistMBId]
			--)
			--select
			--	[ArtistMBId]
			--from @mpt_artist_mbids;

			insert into @mpt_artist_results
			(
				[ArtistMBId],
				[Name]
			)
			select top 300
				gr.[ArtistMBId],
				ar.[Name]
			from @mpt_artist_mbids gr
			join [arma_artists].[dbo].[ArmaArtistsMain] ar
				on ar.[MBId] = gr.[ArtistMBId]
			where
				coalesce(ar.[RatingValue], 0) >= 3
				and (
					(@artist_country is null)
					or
					(ar.[Country] = @artist_country)
				);
		end
	end
	else
	begin
		if @only_by_genre = 0 and @artist_country is not null
		begin
			insert into @mpt_artist_results
			(
				[ArtistMBId],
				[Name]
			)
			select top 300
				[MBId],
				[Name]
			from [arma_artists].[dbo].[ArmaArtistsMain]
			where
				[Country] = @artist_country
				and coalesce([RatingValue], 0) >= 3
			order by
				newid();
		end
	end

	select
		[ArtistMBId],
		[Name]
	from @mpt_artist_results
	order by
		newid();

	/*
		exec Arma_GetRelatedArtistMBIds
			'5f8dfb7a-3cf6-47a1-a361-eb3b187f09f6'

		exec Arma_GetRelatedArtistMBIds
			'b77c86fb-8b31-44d3-95fe-398fbb12c7ce'

		exec Arma_GetRelatedArtistMBIds_BreakIT
			'b77c86fb-8b31-44d3-95fe-398fbb12c7ce',
			1

		exec Arma_GetRelatedArtistMBIds_BreakIT
			'5f8dfb7a-3cf6-47a1-a361-eb3b187f09f6',
			1
	*/
END
GO
/****** Object:  StoredProcedure [dbo].[Arma_GetRelatedArtistMBIds_deprecate_20241219]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[Arma_GetRelatedArtistMBIds_deprecate_20241219]
(
	@artist_mbid varchar(50)
)
AS
BEGIN
	set nocount on;
	declare
		@artist_country varchar(150);

	select
		@artist_country = [Country]
	from [arma_artists].[dbo].[ArmaArtistsMain]
	where
		[MBId] = @artist_mbid;

	drop table if exists #mpt_artist_mbids;
	create table #mpt_artist_mbids
	(
		[ArtistMBId] varchar(50)
	);

	drop table if exists #mpt_artist_results;
	create table #mpt_artist_results
	(
		[ArtistMBId] varchar(50),
		[Name] varchar(300)
	);

	with t_genres as (
		select
			[GenreId] = am.[MBId]
		from [arma_artists].[dbo].[ArmaArtistGenreMap] gm
		join [arma_artists].[dbo].[ArmaArtistsMain] ar
			on ar.[MBId] = @artist_mbid
			and ar.[Id] = gm.[ArtistId]
		join [arma_artists].[dbo].[ArmaGenres] am
			on am.[Id] = gm.[GenreId]
	),
	t_staged as (
		select
			[GenreId] = [MBId]
		from [arma_artists].[dbo].[ArmaGenresStaged]
		where
			[ArtistMBId] = @artist_mbid
	),
	t_all as (
		select [GenreId] from t_genres
		union all
		select [GenreId] from t_staged
	),
	t_distinct as (
		select distinct
			[GenreId]
		from t_all
	),
	t_artists_in_genres as (
		select distinct
			ag.[ArtistMBId]
		from ArmaArtistsWithGenresView ag
		join t_distinct gr
			on gr.[GenreId] = ag.[GenreId]
		where
			ag.[ArtistMBId] != @artist_mbid
	)

	insert into #mpt_artist_mbids
	(
		[ArtistMBId]
	)
	select
		[ArtistMBId]
	from t_artists_in_genres;

	with t_names as (
		select
			gr.[ArtistMBId],
			ar.[Name]
		from #mpt_artist_mbids gr
		join [arma_artists].[dbo].[ArmaArtistsMain] ar
			on ar.[MBId] = gr.[ArtistMBId]
		where
			coalesce(ar.[RatingValue], 0) >= 3
			and (
				(@artist_country is null)
				or
				(ar.[Country] = @artist_country)
			)
	)

	insert into #mpt_artist_results
	(
		[ArtistMBId],
		[Name]
	)
	select top 300
		[ArtistMBId],
		[Name]
	from t_names;

	select
		[ArtistMBId],
		[Name]
	from #mpt_artist_results
	order by
		newid();

	drop table if exists #mpt_artist_mbids;
	drop table if exists #mpt_artist_results;

	/*
		exec Arma_GetRelatedArtistMBIds
			'5f8dfb7a-3cf6-47a1-a361-eb3b187f09f6'
	*/
END
GO
/****** Object:  StoredProcedure [dbo].[Arma_GetRelatedArtistMBIds_deprecate_20241226]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[Arma_GetRelatedArtistMBIds_deprecate_20241226]
(
	@artist_mbid varchar(50),
	@only_by_genre bit = 0
)
AS
BEGIN
	set nocount on;
	declare
		@artist_country varchar(150);

	select
		@artist_country = [Country]
	from [arma_artists].[dbo].[ArmaArtistsMain]
	where
		[MBId] = @artist_mbid;

	declare @mpt_artist_mbids table
	(
		[ArtistMBId] varchar(50)
	);

	declare @mpt_artist_results table
	(
		[ArtistMBId] varchar(50),
		[Name] varchar(300)
	);

	declare @mpt_t_genres table
	(
		[GenreId] varchar(50)
	);

	declare @mpt_t_genres_distinct table
	(
		[GenreId] varchar(50)
	);

	insert into @mpt_t_genres
	(
		[GenreId]
	)
	select
		[GenreId] = am.[MBId]
	from [arma_artists].[dbo].[ArmaArtistGenreMap] gm
	join [arma_artists].[dbo].[ArmaArtistsMain] ar
		on ar.[MBId] = @artist_mbid
		and ar.[Id] = gm.[ArtistId]
	join [arma_artists].[dbo].[ArmaGenres] am
		on am.[Id] = gm.[GenreId];

	insert into @mpt_t_genres
	(
		[GenreId]
	)
	select
		[GenreId] = st.[MBId]
	from [arma_artists].[dbo].[ArmaGenresStaged] st
	left join @mpt_t_genres ag
		on ag.[GenreId] = st.[MBId]
	where
		st.[ArtistMBId] = @artist_mbid
		and ag.[GenreId] is null;

	insert into @mpt_t_genres_distinct
	(
		[GenreId]
	)
	select distinct
		[GenreId]
	from @mpt_t_genres;

	if (select count(*) from @mpt_t_genres) > 0
	begin
		insert into @mpt_artist_mbids
		(
			[ArtistMBId]
		)
		select distinct
			ag.[ArtistMBId]
		from ArmaArtistsWithGenresView ag
		join @mpt_t_genres_distinct gr
			on gr.[GenreId] = ag.[GenreId]
		where
			ag.[ArtistMBId] != @artist_mbid;

		insert into @mpt_artist_mbids
		(
			[ArtistMBId]
		)
		select
			[ArtistMBId]
		from @mpt_artist_mbids;

		insert into @mpt_artist_results
		(
			[ArtistMBId],
			[Name]
		)
		select top 300
			gr.[ArtistMBId],
			ar.[Name]
		from @mpt_artist_mbids gr
		join [arma_artists].[dbo].[ArmaArtistsMain] ar
			on ar.[MBId] = gr.[ArtistMBId]
		where
			coalesce(ar.[RatingValue], 0) >= 3
			and (
				(@artist_country is null)
				or
				(ar.[Country] = @artist_country)
			);
	end
	else
	begin
		if @only_by_genre = 0 and @artist_country is not null
		begin
			insert into @mpt_artist_results
			(
				[ArtistMBId],
				[Name]
			)
			select top 300
				[MBId],
				[Name]
			from [arma_artists].[dbo].[ArmaArtistsMain]
			where
				[Country] = @artist_country
				and coalesce([RatingValue], 0) >= 3
			order by
				newid();
		end
	end

	select
		[ArtistMBId],
		[Name]
	from @mpt_artist_results
	order by
		newid();

	/*
		exec Arma_GetRelatedArtistMBIds
			'5f8dfb7a-3cf6-47a1-a361-eb3b187f09f6'

		exec Arma_GetRelatedArtistMBIds
			'b77c86fb-8b31-44d3-95fe-398fbb12c7ce'

		exec Arma_GetRelatedArtistMBIds
			'b77c86fb-8b31-44d3-95fe-398fbb12c7ce'

		exec Arma_GetRelatedArtistMBIds_BreakIT
			'5f8dfb7a-3cf6-47a1-a361-eb3b187f09f6'
	*/
END
GO
/****** Object:  StoredProcedure [dbo].[Arma_GetSongByMbid]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[Arma_GetSongByMbid]
(
	@mbid varchar(50)
)
AS
BEGIN
	set nocount on;

	with t_a as (
		select
			[Id],
			[MBSongId],
			[AlbumId],
			[CDNumber],
			[SongNumber],
			[SongTitle],
			[SongTitleFlat],
			[AddedDateTime],
			[MBAlbumId],
			[DbSource] = 'A'
		from [arma_a].[dbo].[SongMapper]
		where
			[MBSongId] = @mbid
	),
	t_b as (
		select
			[Id],
			[MBSongId],
			[AlbumId],
			[CDNumber],
			[SongNumber],
			[SongTitle],
			[SongTitleFlat],
			[AddedDateTime],
			[MBAlbumId],
			[DbSource] = 'B'
		from [arma_b].[dbo].[SongMapper]
		where
			[MBSongId] = @mbid
	),
	t_c as (
		select
			[Id],
			[MBSongId],
			[AlbumId],
			[CDNumber],
			[SongNumber],
			[SongTitle],
			[SongTitleFlat],
			[AddedDateTime],
			[MBAlbumId],
			[DbSource] = 'C'
		from [arma_c].[dbo].[SongMapper]
		where
			[MBSongId] = @mbid
	),
	t_d as (
		select
			[Id],
			[MBSongId],
			[AlbumId],
			[CDNumber],
			[SongNumber],
			[SongTitle],
			[SongTitleFlat],
			[AddedDateTime],
			[MBAlbumId],
			[DbSource] = 'D'
		from [arma_d].[dbo].[SongMapper]
		where
			[MBSongId] = @mbid
	),
	t_e as (
		select
			[Id],
			[MBSongId],
			[AlbumId],
			[CDNumber],
			[SongNumber],
			[SongTitle],
			[SongTitleFlat],
			[AddedDateTime],
			[MBAlbumId],
			[DbSource] = 'E'
		from [arma_e].[dbo].[SongMapper]
		where
			[MBSongId] = @mbid
	),
	t_f as (
		select
			[Id],
			[MBSongId],
			[AlbumId],
			[CDNumber],
			[SongNumber],
			[SongTitle],
			[SongTitleFlat],
			[AddedDateTime],
			[MBAlbumId],
			[DbSource] = 'F'
		from [arma_f].[dbo].[SongMapper]
		where
			[MBSongId] = @mbid
	),
	t_g as (
		select
			[Id],
			[MBSongId],
			[AlbumId],
			[CDNumber],
			[SongNumber],
			[SongTitle],
			[SongTitleFlat],
			[AddedDateTime],
			[MBAlbumId],
			[DbSource] = 'G'
		from [arma_g].[dbo].[SongMapper]
		where
			[MBSongId] = @mbid
	),
	t_h as (
		select
			[Id],
			[MBSongId],
			[AlbumId],
			[CDNumber],
			[SongNumber],
			[SongTitle],
			[SongTitleFlat],
			[AddedDateTime],
			[MBAlbumId],
			[DbSource] = 'h'
		from [arma_h].[dbo].[SongMapper]
		where
			[MBSongId] = @mbid
	),
	t_i as (
		select
			[Id],
			[MBSongId],
			[AlbumId],
			[CDNumber],
			[SongNumber],
			[SongTitle],
			[SongTitleFlat],
			[AddedDateTime],
			[MBAlbumId],
			[DbSource] = 'i'
		from [arma_i].[dbo].[SongMapper]
		where
			[MBSongId] = @mbid
	),
	t_j as (
		select
			[Id],
			[MBSongId],
			[AlbumId],
			[CDNumber],
			[SongNumber],
			[SongTitle],
			[SongTitleFlat],
			[AddedDateTime],
			[MBAlbumId],
			[DbSource] = 'j'
		from [arma_j].[dbo].[SongMapper]
		where
			[MBSongId] = @mbid
	),
	t_k as (
		select
			[Id],
			[MBSongId],
			[AlbumId],
			[CDNumber],
			[SongNumber],
			[SongTitle],
			[SongTitleFlat],
			[AddedDateTime],
			[MBAlbumId],
			[DbSource] = 'k'
		from [arma_k].[dbo].[SongMapper]
		where
			[MBSongId] = @mbid
	),
	t_l as (
		select
			[Id],
			[MBSongId],
			[AlbumId],
			[CDNumber],
			[SongNumber],
			[SongTitle],
			[SongTitleFlat],
			[AddedDateTime],
			[MBAlbumId],
			[DbSource] = 'l'
		from [arma_l].[dbo].[SongMapper]
		where
			[MBSongId] = @mbid
	),
	t_m as (
		select
			[Id],
			[MBSongId],
			[AlbumId],
			[CDNumber],
			[SongNumber],
			[SongTitle],
			[SongTitleFlat],
			[AddedDateTime],
			[MBAlbumId],
			[DbSource] = 'm'
		from [arma_m].[dbo].[SongMapper]
		where
			[MBSongId] = @mbid
	),
	t_n as (
		select
			[Id],
			[MBSongId],
			[AlbumId],
			[CDNumber],
			[SongNumber],
			[SongTitle],
			[SongTitleFlat],
			[AddedDateTime],
			[MBAlbumId],
			[DbSource] = 'n'
		from [arma_n].[dbo].[SongMapper]
		where
			[MBSongId] = @mbid
	),
	t_o as (
		select
			[Id],
			[MBSongId],
			[AlbumId],
			[CDNumber],
			[SongNumber],
			[SongTitle],
			[SongTitleFlat],
			[AddedDateTime],
			[MBAlbumId],
			[DbSource] = 'o'
		from [arma_o].[dbo].[SongMapper]
		where
			[MBSongId] = @mbid
	),
	t_p as (
		select
			[Id],
			[MBSongId],
			[AlbumId],
			[CDNumber],
			[SongNumber],
			[SongTitle],
			[SongTitleFlat],
			[AddedDateTime],
			[MBAlbumId],
			[DbSource] = 'p'
		from [arma_p].[dbo].[SongMapper]
		where
			[MBSongId] = @mbid
	),
	t_q as (
		select
			[Id],
			[MBSongId],
			[AlbumId],
			[CDNumber],
			[SongNumber],
			[SongTitle],
			[SongTitleFlat],
			[AddedDateTime],
			[MBAlbumId],
			[DbSource] = 'q'
		from [arma_q].[dbo].[SongMapper]
		where
			[MBSongId] = @mbid
	),
	t_r as (
		select
			[Id],
			[MBSongId],
			[AlbumId],
			[CDNumber],
			[SongNumber],
			[SongTitle],
			[SongTitleFlat],
			[AddedDateTime],
			[MBAlbumId],
			[DbSource] = 'r'
		from [arma_r].[dbo].[SongMapper]
		where
			[MBSongId] = @mbid
	),
	t_s as (
		select
			[Id],
			[MBSongId],
			[AlbumId],
			[CDNumber],
			[SongNumber],
			[SongTitle],
			[SongTitleFlat],
			[AddedDateTime],
			[MBAlbumId],
			[DbSource] = 's'
		from [arma_s].[dbo].[SongMapper]
		where
			[MBSongId] = @mbid
	),
	t_t as (
		select
			[Id],
			[MBSongId],
			[AlbumId],
			[CDNumber],
			[SongNumber],
			[SongTitle],
			[SongTitleFlat],
			[AddedDateTime],
			[MBAlbumId],
			[DbSource] = 't'
		from [arma_t].[dbo].[SongMapper]
		where
			[MBSongId] = @mbid
	),
	t_u as (
		select
			[Id],
			[MBSongId],
			[AlbumId],
			[CDNumber],
			[SongNumber],
			[SongTitle],
			[SongTitleFlat],
			[AddedDateTime],
			[MBAlbumId],
			[DbSource] = 'u'
		from [arma_u].[dbo].[SongMapper]
		where
			[MBSongId] = @mbid
	),
	t_v as (
		select
			[Id],
			[MBSongId],
			[AlbumId],
			[CDNumber],
			[SongNumber],
			[SongTitle],
			[SongTitleFlat],
			[AddedDateTime],
			[MBAlbumId],
			[DbSource] = 'v'
		from [arma_v].[dbo].[SongMapper]
		where
			[MBSongId] = @mbid
	),
	t_w as (
		select
			[Id],
			[MBSongId],
			[AlbumId],
			[CDNumber],
			[SongNumber],
			[SongTitle],
			[SongTitleFlat],
			[AddedDateTime],
			[MBAlbumId],
			[DbSource] = 'w'
		from [arma_w].[dbo].[SongMapper]
		where
			[MBSongId] = @mbid
	),
	t_x as (
		select
			[Id],
			[MBSongId],
			[AlbumId],
			[CDNumber],
			[SongNumber],
			[SongTitle],
			[SongTitleFlat],
			[AddedDateTime],
			[MBAlbumId],
			[DbSource] = 'x'
		from [arma_x].[dbo].[SongMapper]
		where
			[MBSongId] = @mbid
	),
	t_y as (
		select
			[Id],
			[MBSongId],
			[AlbumId],
			[CDNumber],
			[SongNumber],
			[SongTitle],
			[SongTitleFlat],
			[AddedDateTime],
			[MBAlbumId],
			[DbSource] = 'y'
		from [arma_y].[dbo].[SongMapper]
		where
			[MBSongId] = @mbid
	),
	t_z as (
		select
			[Id],
			[MBSongId],
			[AlbumId],
			[CDNumber],
			[SongNumber],
			[SongTitle],
			[SongTitleFlat],
			[AddedDateTime],
			[MBAlbumId],
			[DbSource] = 'z'
		from [arma_z].[dbo].[SongMapper]
		where
			[MBSongId] = @mbid
	),
	t_num as (
		select
			[Id],
			[MBSongId],
			[AlbumId],
			[CDNumber],
			[SongNumber],
			[SongTitle],
			[SongTitleFlat],
			[AddedDateTime],
			[MBAlbumId],
			[DbSource] = 'num'
		from [arma_num].[dbo].[SongMapper]
		where
			[MBSongId] = @mbid
	),
	t_symb as (
		select
			[Id],
			[MBSongId],
			[AlbumId],
			[CDNumber],
			[SongNumber],
			[SongTitle],
			[SongTitleFlat],
			[AddedDateTime],
			[MBAlbumId],
			[DbSource] = 'symb'
		from [arma_symb].[dbo].[SongMapper]
		where
			[MBSongId] = @mbid
	),
	t_all as (
		select * from t_a
		union all
		select * from t_b
		union all
		select * from t_c
		union all
		select * from t_d
		union all
		select * from t_e
		union all
		select * from t_f
		union all
		select * from t_g
		union all
		select * from t_h
		union all
		select * from t_i
		union all
		select * from t_j
		union all
		select * from t_k
		union all
		select * from t_l
		union all
		select * from t_m
		union all
		select * from t_n
		union all
		select * from t_o
		union all
		select * from t_p
		union all
		select * from t_q
		union all
		select * from t_r
		union all
		select * from t_s
		union all
		select * from t_t
		union all
		select * from t_u
		union all
		select * from t_v
		union all
		select * from t_w
		union all
		select * from t_x
		union all
		select * from t_y
		union all
		select * from t_z
		union all
		select * from t_num
		union all
		select * from t_symb
	)

	select
		[Id],
		[MBSongId],
		[AlbumId],
		[CDNumber],
		[SongNumber],
		[SongTitle],
		[SongTitleFlat],
		[AddedDateTime],
		[MBAlbumId],
		[DbSource]
	from t_all;

	/*
		exec Arma_GetSongByMbid
			'1ea62ab6-8bba-42fa-ae7c-6f0bc3234629'
	*/
END
GO
/****** Object:  StoredProcedure [dbo].[Arma_GetSongsForAlbum]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[Arma_GetSongsForAlbum]
(
	@album_id int,
	@artist_id int
)
AS
BEGIN
	set nocount on;

	declare
		@sql nvarchar(max),
		@db_source varchar(15);

		select
			@db_source = [DBSource]
		from [arma_artists].dbo.[ArmaArtistsMain]
		where
			[Id] = @artist_id;

	set @sql = '
		select
			sm.[Id],
			sm.[MBSongId],
			sm.[AlbumId],
			sm.[CDNumber],
			sm.[SongNumber],
			sm.[SongTitle],
			sm.[SongTitleFlat],
			[ArtistId] = ' + cast(@artist_id as varchar(150)) + ',
			am.[NameSearch],
			sm.[AddedDateTime]
		from [arma_' + @db_source + '].[dbo].[SongMapper] sm
		join [arma_artists].dbo.[ArmaArtistsMain] am
			on am.[Id] = ' + cast(@artist_id as varchar(150)) + '
		where
			sm.[AlbumId] = ' + cast(@album_id as varchar(150)) + '
		order by
			sm.[CDNumber],
			sm.[SongNumber];
	';

	exec sp_executesql @sql;

	/*
		exec Arma_GetSongsForAlbum
			320322,
			243039
	*/
END
GO
/****** Object:  StoredProcedure [dbo].[Arma_GetTopLastFMTrendingTracks]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[Arma_GetTopLastFMTrendingTracks]
(
	@request_id uniqueidentifier
)
AS
BEGIN
	set nocount on;

	with t_res as (
		select
			[tid] = tb.[id],
			tb.[artist_mbid],
			[artist_name] = ar.[Name],
			tb.[track_name],
			tb.[usability_score]
		from brainz_tempholder tb
		join arma_artists.dbo.ArmaArtistsMain ar
			on ar.[MBId] = tb.[artist_mbid]
		where
			[request_id] = @request_id
	)

	select
		[tid],
		[artist_mbid],
		[artist_name],
		[track_name],
		[usability_score]
	from t_res
	order by
		[usability_score] desc;

	delete from brainz_tempholder
	where
		[request_id] = @request_id;
END
GO
/****** Object:  StoredProcedure [dbo].[Arma_GetTopUserRankedArtists4range]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[Arma_GetTopUserRankedArtists4range]
AS
BEGIN
	set nocount on;

	select top 300
		[tid] = [Id],
		[artist_name] = [Name],
		[Rank] = [RatingValue],
		[usability_score] = [RatingVotes]
	from arma_artists.dbo.ArmaArtistsMain
	where
		coalesce([RatingValue], 0) >= 4
		and coalesce([RatingValue], 0) < 5
		and coalesce([RatingVotes], 0) > 1
	order by
		[RatingVotes] desc,
		[RatingValue] desc;

	/*
		exec Arma_GetTopUserRankedArtists4range
	*/
END
GO
/****** Object:  StoredProcedure [dbo].[Arma_GetTopUserRankedArtists5range]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[Arma_GetTopUserRankedArtists5range]
AS
BEGIN
	set nocount on;

	select top 300
		[tid] = [Id],
		[artist_name] = [Name],
		[Rank] = [RatingValue],
		[usability_score] = [RatingVotes]
	from arma_artists.dbo.ArmaArtistsMain
	where
		coalesce([RatingValue], 0) >= 5
		and coalesce([RatingVotes], 0) > 1
	order by
		[RatingVotes] desc;

	/*
		exec Arma_GetTopUserRankedArtists5range
	*/
END
GO
/****** Object:  StoredProcedure [dbo].[Arma_SearchAlbums]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[Arma_SearchAlbums]
(
	@AlbumName varchar(600)
)
AS
BEGIN
	set nocount on;

	declare
		@sql nvarchar(max),
		@sqlBody nvarchar(max);

	drop table if exists #tmp_db_refs;
	create table #tmp_db_refs
	(
		[Reference] varchar(10)
	);

	insert into #tmp_db_refs
	(
		[Reference]
	)
	select
		[Reference]
	from ArmaDbReferences;
	
	drop table if exists #tmp_album_results;
	create table #tmp_album_results
	(
		[AlbumId] int,
		[ArtistId] int,
		[AlbumName] varchar(600),
		[AlbumName_Flat] varchar(600),
		[IsSingle] bit,
		[ReleaseDate] varchar(150),
		[dbSource] varchar(60)
	);

	select
		@sqlBody = stuff((
			select
				' union all (
					select
						[AlbumId] = [Id],
						[ArtistId],
						[AlbumName],
						[AlbumName_Flat],
						[IsSingle],
						[ReleaseDate],
						[dbSource] = ''' + rc.[Reference] + '''
					from [arma_' + rc.[Reference] + '].[dbo].[ArmaAlbums]
					where
						[AlbumName_Flat] like ''%' + @AlbumName + '%''
					)
				'
			from #tmp_db_refs rc
			where
				1=1
			for xml path(''), type).value('.', 'nvarchar(max)'), 1, 11, '');

	set @sql = '
		insert into #tmp_album_results
		(
			[AlbumId],
			[ArtistId],
			[AlbumName],
			[AlbumName_Flat],
			[IsSingle],
			[ReleaseDate],
			[dbSource]
		)
	' + @sqlBody;

	exec sp_executesql @sql;

	select top 500
		ti.[AlbumName],
		ti.[AlbumName_Flat],
		ti.[ArtistId],
		ar.[ArtistName],
		ar.[ArtistName_Flat],
		ti.[IsSingle],
		ti.[ReleaseDate],
		ti.[dbSource]
	from #tmp_album_results ti
	join [arma_artists].[dbo].[ArtistsRaw] ar
		on ar.[Id] = ti.[ArtistId]
	order by
		ti.[AlbumName_Flat];

	/*
		exec Arma_SearchAlbums
			'trance'

	*/
END
GO
/****** Object:  StoredProcedure [dbo].[Arma_SearchArtists]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[Arma_SearchArtists]
(
	@artist_name varchar(350),
	@debug bit = 0
)
AS
BEGIN
	set nocount on;

	declare
		@artist varchar(350) = convert(varchar(350), @artist_name collate SQL_Latin1_General_Cp1251_CS_AS),
		@artist_reverse varchar(350),
		@sql nvarchar(max),
		@sqlBody nvarchar(max);

	set @artist_reverse = reverse(@artist);

	drop table if exists #tmp_artist_search_results;
	create table #tmp_artist_search_results
	(
		[Id] int,
		[ArtistName] varchar(500),
		[ArtistName_Flat] varchar(500),
		[DBDesignator] varchar(15),
		[EntryDateTime] datetime,
		[SortOrder] int
	);

	drop table if exists #tmp_artist_names_res;
	create table #tmp_artist_names_res
	(
		[Id] int,
		[ArtistName] varchar(500),
		[ArtistName_Flat] varchar(500),
		[DBDesignator] varchar(15),
		[EntryDateTime] datetime,
		[SortOrder] int
	);

	insert into #tmp_artist_search_results
	(
		[Id],
		[ArtistName],
		[ArtistName_Flat],
		[DBDesignator],
		[EntryDateTime],
		[SortOrder]
	)
	select top 100
		[Id],
		[NameFlat],
		[NameSearch],
		[DBSource],
		[AddedDateTime],
		[SortOrder] = 1
	from [arma_artists].[dbo].[ArmaArtistsMain]
	where
		[NameFlat] = @artist;

	
	set @sql = '
		insert into #tmp_artist_search_results
		(
			[Id],
			[ArtistName],
			[ArtistName_Flat],
			[DBDesignator],
			[EntryDateTime],
			[SortOrder]
		)
		select top 100
			[Id],
			[NameFlat],
			[NameSearch],
			[DBSource],
			[AddedDateTime],
			[SortOrder] = 2
		from [arma_artists].[dbo].[ArmaArtistsMain]
		where
			[NameFlat] like ''' + @artist + '%'';
	';

	set @sql = @sql + '
	insert into #tmp_artist_search_results
	(
		[Id],
		[ArtistName],
		[ArtistName_Flat],
		[DBDesignator],
		[EntryDateTime],
		[SortOrder]
	)
	select top 100
		[Id],
		[NameFlat],
		[NameSearch],
		[DBSource],
		[AddedDateTime],
		[SortOrder] = 2
	from [arma_artists].[dbo].[ArmaArtistsMain]
	where
		[NameFlatReverse] like ''' + @artist_reverse + '%'';
	';

	set @sql = @sql + '
	insert into #tmp_artist_search_results
	(
		[Id],
		[ArtistName],
		[ArtistName_Flat],
		[DBDesignator],
		[EntryDateTime],
		[SortOrder]
	)
	select top 100
		[Id],
		[NameFlat],
		[NameSearch],
		[DBSource],
		[AddedDateTime],
		[SortOrder] = 2
	from [arma_artists].[dbo].[ArmaArtistsMain]
	where
		[NameSearch] like ''' + @artist + '%'';
	';

	set @sql = @sql + '
	insert into #tmp_artist_search_results
	(
		[Id],
		[ArtistName],
		[ArtistName_Flat],
		[DBDesignator],
		[EntryDateTime],
		[SortOrder]
	)
	select top 100
		[Id],
		[NameFlat],
		[NameSearch],
		[DBSource],
		[AddedDateTime],
		[SortOrder] = 2
	from [arma_artists].[dbo].[ArmaArtistsMain]
	where
		[NameSearchReverse] like ''' + @artist_reverse + '%'';
	';

	exec sp_executesql @sql;

	with t_data as (
		select distinct
			sr.[Id],
			sr.[ArtistName],
			sr.[ArtistName_Flat],
			sr.[DBDesignator],
			sr.[EntryDateTime],
			sr.[SortOrder]
		from #tmp_artist_search_results sr
	)

	insert into #tmp_artist_names_res
	(
		[Id],
		[ArtistName],
		[ArtistName_Flat],
		[DBDesignator],
		[EntryDateTime],
		[SortOrder]
	)
	select
		[Id],
		[ArtistName],
		[ArtistName_Flat],
		[DBDesignator],
		[EntryDateTime],
		[SortOrder] = min([SortOrder])
	from t_data
	group by
		[Id],
		[ArtistName],
		[ArtistName_Flat],
		[DBDesignator],
		[EntryDateTime];

	with t_final as (
		select distinct
			rs.[Id],
			rs.[ArtistName],
			rs.[ArtistName_Flat],
			rs.[DBDesignator],
			rs.[EntryDateTime],
			rs.[SortOrder],
			ct.[count]
		from #tmp_artist_names_res rs
		outer apply (
			select [count] = dbo.NumberOfArtistAlbums(rs.[Id], rs.[DBDesignator])
		) ct
	),
	t_distinct_a as (
		select distinct
			rs.[Id],
			rs.[ArtistName],
			rs.[ArtistName_Flat],
			rs.[DBDesignator],
			rs.[EntryDateTime],
			rs.[SortOrder],
			rs.[count]
		from t_final rs
		where
			rs.[count] <> 0
	),
	t_distinct_pre as (
		select
			rs.[Id],
			rs.[ArtistName],
			rs.[ArtistName_Flat],
			rs.[DBDesignator],
			rs.[EntryDateTime],
			rs.[SortOrder],
			rs.[count]
			--[inlcude] = row_number() over(
			--	partition by
			--		rs.[ArtistName_Flat],
			--		rs.[count]
			--	order by
			--		rs.[count] desc
			--)
		from t_distinct_a rs
	),
	t_grouped_x as (
		select
			rs.[ArtistName_Flat],
			[count] = max(rs.[count])
		from t_distinct_pre rs
		group by
			rs.[ArtistName_Flat]
	),
	t_post_a as (
		select
			rs.[Id],
			rs.[ArtistName],
			rs.[ArtistName_Flat],
			rs.[DBDesignator],
			rs.[EntryDateTime],
			rs.[SortOrder],
			rs.[count]
		from t_distinct_pre rs
		join t_grouped_x ds
			on ds.[ArtistName_Flat] = rs.[ArtistName_Flat]
			and ds.[count] = rs.[count]
	),
	t_distict as (
		select top 100
			rs.[Id],
			rs.[ArtistName],
			rs.[ArtistName_Flat],
			rs.[DBDesignator],
			rs.[EntryDateTime],
			rs.[SortOrder],
			rs.[count]
		from t_post_a rs
		order by
			[SortOrder],
			[ArtistName_Flat]
	)

	select
		rs.[Id],
		rs.[ArtistName],
		rs.[ArtistName_Flat],
		rs.[DBDesignator],
		rs.[EntryDateTime],
		rs.[count]
		--rs.[inlcude]
	from t_distict rs
	order by
		rs.[SortOrder],
		rs.[ArtistName_Flat];

	/*
		exec Arma_SearchArtists
			'rush'
			,1

		exec Arma_SearchArtists
			'nirvana'
			,1

		exec Arma_SearchArtists
			'bukis'
			,1

		exec Arma_SearchArtists
			'temerarios'
			,1

		exec Arma_SearchArtists
			'taylor sw'
			,1

	*/
END
GO
/****** Object:  StoredProcedure [dbo].[Arma_SearchArtists_deprecate_20240413]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[Arma_SearchArtists_deprecate_20240413]
(
	@artist_name varchar(250)
)
AS
BEGIN
	set nocount on;

	declare
		@artist varchar(250) = convert(varchar(600), @artist_name collate SQL_Latin1_General_Cp1251_CS_AS);

	select top 100
		[Id],
		[ArtistName],
		[ArtistName_Flat],
		[DBDesignator],
		[EntryDateTime]
	from [arma_artists].[dbo].[ArtistsRaw]
	where
		[ArtistName_Flat] like '%' + @artist + '%'
	order by
		[ArtistName_Flat];

	/*
		exec Arma_SearchArtists
			'Paul'
	*/
END
GO
/****** Object:  StoredProcedure [dbo].[Arma_SearchArtists_deprecate_20240416]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[Arma_SearchArtists_deprecate_20240416]
(
	@artist_name varchar(600),
	@debug bit = 0
)
AS
BEGIN
	set nocount on;

	declare
		@artist varchar(600) = convert(varchar(600), @artist_name collate SQL_Latin1_General_Cp1251_CS_AS),
		@sql nvarchar(max),
		@sqlBody nvarchar(max);

	select
		@sqlBody = stuff((
			select
				' and [ArtistName_Flat] like ''%' + ltrim(rtrim(coalesce([token], ''))) + '%'''
			from dbo.SplitStringToTableVarchar(@artist, ' ')
			where
				1=1
			for xml path(''), type).value('.', 'nvarchar(max)'), 1, 5, '');

	if @debug = 1
	begin
		select @sqlBody;
	end

	set @sql = '
		select distinct top 100
			[Id],
			[ArtistName],
			[ArtistName_Flat],
			[DBDesignator],
			[EntryDateTime]
		from [arma_artists].[dbo].[ArtistsRaw]
		where
			' + @sqlBody + '
		order by
			[ArtistName_Flat];
	';

	exec sp_executesql @sql;

	/*
		exec Arma_SearchArtists
			'cranberries'
			,1
	*/
END
GO
/****** Object:  StoredProcedure [dbo].[Arma_SearchArtists_deprecate_20240419]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[Arma_SearchArtists_deprecate_20240419]
(
	@artist_name varchar(600),
	@debug bit = 0
)
AS
BEGIN
	set nocount on;

	declare
		@artist varchar(600) = convert(varchar(600), @artist_name collate SQL_Latin1_General_Cp1251_CS_AS),
		@sql nvarchar(max),
		@sqlBody nvarchar(max);

	drop table if exists #tmp_artist_search_results;
	create table #tmp_artist_search_results
	(
		[Id] int,
		[ArtistName] varchar(500),
		[ArtistName_Flat] varchar(500),
		[DBDesignator] varchar(15),
		[EntryDateTime] datetime,
		[SortOrder] int
	);

	drop table if exists #tmp_artist_names_res;
	create table #tmp_artist_names_res
	(
		[Id] int,
		[ArtistName] varchar(500),
		[ArtistName_Flat] varchar(500),
		[DBDesignator] varchar(15),
		[EntryDateTime] datetime,
		[SortOrder] int
	);

	insert into #tmp_artist_search_results
	(
		[Id],
		[ArtistName],
		[ArtistName_Flat],
		[DBDesignator],
		[EntryDateTime],
		[SortOrder]
	)
	select top 10
		[Id],
		[NameFlat],
		[NameSearch],
		[DBSource],
		[AddedDateTime],
		[SortOrder] = 1
	from [arma_artists].[dbo].[ArmaArtists]
	where
		[NameSearch] = @artist;
	
	insert into #tmp_artist_search_results
	(
		[Id],
		[ArtistName],
		[ArtistName_Flat],
		[DBDesignator],
		[EntryDateTime],
		[SortOrder]
	)
	select top 10
		[Id],
		[NameFlat],
		[NameSearch],
		[DBSource],
		[AddedDateTime],
		[SortOrder] = 2
	from [arma_artists].[dbo].[ArmaArtists]
	where
		[NameSearch] like @artist + '%';

	--select * from #tmp_artist_search_results;

	select
		@sqlBody = stuff((
			select
				' and ar.[NameSearch] like ''%' + ltrim(rtrim(coalesce([token], ''))) + '%'''
			from dbo.SplitStringToTableVarchar(@artist, ' ')
			where
				1=1
			for xml path(''), type).value('.', 'nvarchar(max)'), 1, 5, '');

	if @debug = 1
	begin
		select @sqlBody;
	end

	set @sql = '
		insert into #tmp_artist_search_results
		(
			[Id],
			[ArtistName],
			[ArtistName_Flat],
			[DBDesignator],
			[EntryDateTime],
			[SortOrder]
		)
		select distinct top 30
			ar.[Id],
			ar.[NameFlat],
			ar.[NameSearch],
			ar.[DBSource],
			ar.[AddedDateTime],
			[SortOrder] = 3
		from [arma_artists].[dbo].[ArmaArtists] ar
		left join #tmp_artist_search_results fr
			on fr.[Id] = ar.[Id]
		where
			' + @sqlBody + '
			and fr.[Id] is null;
	';

	exec sp_executesql @sql;

	with t_data as (
		select distinct
			sr.[Id],
			sr.[ArtistName],
			sr.[ArtistName_Flat],
			sr.[DBDesignator],
			sr.[EntryDateTime],
			sr.[SortOrder]
		from #tmp_artist_search_results sr
	)

	insert into #tmp_artist_names_res
	(
		[Id],
		[ArtistName],
		[ArtistName_Flat],
		[DBDesignator],
		[EntryDateTime],
		[SortOrder]
	)
	select
		[Id],
		[ArtistName],
		[ArtistName_Flat],
		[DBDesignator],
		[EntryDateTime],
		[SortOrder] = min([SortOrder])
	from t_data
	group by
		[Id],
		[ArtistName],
		[ArtistName_Flat],
		[DBDesignator],
		[EntryDateTime];

	with t_final as (
		select distinct
			rs.[Id],
			rs.[ArtistName],
			rs.[ArtistName_Flat],
			rs.[DBDesignator],
			rs.[EntryDateTime],
			rs.[SortOrder],
			ct.[count]
		from #tmp_artist_names_res rs
		outer apply (
			select [count] = dbo.NumberOfArtistAlbums(rs.[Id], rs.[DBDesignator])
		) ct
	),
	t_distinct_a as (
		select distinct
			rs.[Id],
			rs.[ArtistName],
			rs.[ArtistName_Flat],
			rs.[DBDesignator],
			rs.[EntryDateTime],
			rs.[SortOrder],
			rs.[count]
		from t_final rs
		where
			rs.[count] <> 0
	),
	t_distinct_pre as (
		select
			rs.[Id],
			rs.[ArtistName],
			rs.[ArtistName_Flat],
			rs.[DBDesignator],
			rs.[EntryDateTime],
			rs.[SortOrder],
			rs.[count]
			--[inlcude] = row_number() over(
			--	partition by
			--		rs.[ArtistName_Flat],
			--		rs.[count]
			--	order by
			--		rs.[count] desc
			--)
		from t_distinct_a rs
	),
	t_grouped_x as (
		select
			rs.[ArtistName_Flat],
			[count] = max(rs.[count])
		from t_distinct_pre rs
		group by
			rs.[ArtistName_Flat]
	),
	t_post_a as (
		select
			rs.[Id],
			rs.[ArtistName],
			rs.[ArtistName_Flat],
			rs.[DBDesignator],
			rs.[EntryDateTime],
			rs.[SortOrder],
			rs.[count]
		from t_distinct_pre rs
		join t_grouped_x ds
			on ds.[ArtistName_Flat] = rs.[ArtistName_Flat]
			and ds.[count] = rs.[count]
	),
	t_distict as (
		select top 100
			rs.[Id],
			rs.[ArtistName],
			rs.[ArtistName_Flat],
			rs.[DBDesignator],
			rs.[EntryDateTime],
			rs.[SortOrder],
			rs.[count]
		from t_post_a rs
		order by
			[SortOrder],
			[ArtistName_Flat]
	)

	select
		rs.[Id],
		rs.[ArtistName],
		rs.[ArtistName_Flat],
		rs.[DBDesignator],
		rs.[EntryDateTime],
		rs.[count]
		--rs.[inlcude]
	from t_distict rs
	order by
		rs.[SortOrder],
		rs.[ArtistName_Flat];

	/*
		exec Arma_SearchArtists
			'rush'
			,1

		exec Arma_SearchArtists
			'nirvana'
			,1

		exec Arma_SearchArtists
			'bukis'
			,1

		exec Arma_SearchArtists
			'temerarios'
			,1
	*/
END
GO
/****** Object:  StoredProcedure [dbo].[Arma_SearchArtistsInternal]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[Arma_SearchArtistsInternal]
(
	@artist_name varchar(350),
	@debug bit = 0
)
AS
BEGIN
	set nocount on;

	declare
		@artist varchar(350) = convert(varchar(350), @artist_name collate SQL_Latin1_General_Cp1251_CS_AS),
		@artist_reverse varchar(350),
		@sql nvarchar(max),
		@sqlBody nvarchar(max);

	set @artist_reverse = reverse(@artist);

	drop table if exists #tmp_artist_search_results;
	create table #tmp_artist_search_results
	(
		[Id] int,
		[ArtistName] varchar(500),
		[ArtistName_Flat] varchar(500),
		[DBDesignator] varchar(15),
		[EntryDateTime] datetime,
		[SortOrder] int
	);

	drop table if exists #tmp_artist_names_res;
	create table #tmp_artist_names_res
	(
		[Id] int,
		[ArtistName] varchar(500),
		[ArtistName_Flat] varchar(500),
		[DBDesignator] varchar(15),
		[EntryDateTime] datetime,
		[SortOrder] int
	);

	insert into #tmp_artist_search_results
	(
		[Id],
		[ArtistName],
		[ArtistName_Flat],
		[DBDesignator],
		[EntryDateTime],
		[SortOrder]
	)
	select top 100
		[Id],
		[NameFlat],
		[NameSearch],
		[DBSource],
		[AddedDateTime],
		[SortOrder] = 1
	from [arma_artists].[dbo].[ArmaArtistsMain]
	where
		[NameFlat] = @artist;

	
	set @sql = '
		insert into #tmp_artist_search_results
		(
			[Id],
			[ArtistName],
			[ArtistName_Flat],
			[DBDesignator],
			[EntryDateTime],
			[SortOrder]
		)
		select top 100
			[Id],
			[NameFlat],
			[NameSearch],
			[DBSource],
			[AddedDateTime],
			[SortOrder] = 2
		from [arma_artists].[dbo].[ArmaArtistsMain]
		where
			[NameFlat] like ''' + @artist + '%'';
	';

	set @sql = @sql + '
	insert into #tmp_artist_search_results
	(
		[Id],
		[ArtistName],
		[ArtistName_Flat],
		[DBDesignator],
		[EntryDateTime],
		[SortOrder]
	)
	select top 100
		[Id],
		[NameFlat],
		[NameSearch],
		[DBSource],
		[AddedDateTime],
		[SortOrder] = 2
	from [arma_artists].[dbo].[ArmaArtistsMain]
	where
		[NameFlatReverse] like ''' + @artist_reverse + '%'';
	';

	set @sql = @sql + '
	insert into #tmp_artist_search_results
	(
		[Id],
		[ArtistName],
		[ArtistName_Flat],
		[DBDesignator],
		[EntryDateTime],
		[SortOrder]
	)
	select top 100
		[Id],
		[NameFlat],
		[NameSearch],
		[DBSource],
		[AddedDateTime],
		[SortOrder] = 2
	from [arma_artists].[dbo].[ArmaArtistsMain]
	where
		[NameSearch] like ''' + @artist + '%'';
	';

	set @sql = @sql + '
	insert into #tmp_artist_search_results
	(
		[Id],
		[ArtistName],
		[ArtistName_Flat],
		[DBDesignator],
		[EntryDateTime],
		[SortOrder]
	)
	select top 100
		[Id],
		[NameFlat],
		[NameSearch],
		[DBSource],
		[AddedDateTime],
		[SortOrder] = 2
	from [arma_artists].[dbo].[ArmaArtistsMain]
	where
		[NameSearchReverse] like ''' + @artist_reverse + '%'';
	';

	exec sp_executesql @sql;

	with t_data as (
		select distinct
			sr.[Id],
			sr.[ArtistName],
			sr.[ArtistName_Flat],
			sr.[DBDesignator],
			sr.[EntryDateTime],
			sr.[SortOrder]
		from #tmp_artist_search_results sr
	)

	insert into #tmp_artist_names_res
	(
		[Id],
		[ArtistName],
		[ArtistName_Flat],
		[DBDesignator],
		[EntryDateTime],
		[SortOrder]
	)
	select
		[Id],
		[ArtistName],
		[ArtistName_Flat],
		[DBDesignator],
		[EntryDateTime],
		[SortOrder] = min([SortOrder])
	from t_data
	group by
		[Id],
		[ArtistName],
		[ArtistName_Flat],
		[DBDesignator],
		[EntryDateTime];

	with t_final as (
		select distinct
			rs.[Id],
			rs.[ArtistName],
			rs.[ArtistName_Flat],
			rs.[DBDesignator],
			rs.[EntryDateTime],
			rs.[SortOrder],
			ct.[count]
		from #tmp_artist_names_res rs
		outer apply (
			select [count] = dbo.NumberOfArtistAlbums(rs.[Id], rs.[DBDesignator])
		) ct
	),
	t_distinct_a as (
		select distinct
			rs.[Id],
			rs.[ArtistName],
			rs.[ArtistName_Flat],
			rs.[DBDesignator],
			rs.[EntryDateTime],
			rs.[SortOrder],
			rs.[count]
		from t_final rs
		where
			rs.[count] <> 0
	),
	t_distinct_pre as (
		select
			rs.[Id],
			rs.[ArtistName],
			rs.[ArtistName_Flat],
			rs.[DBDesignator],
			rs.[EntryDateTime],
			rs.[SortOrder],
			rs.[count]
			--[inlcude] = row_number() over(
			--	partition by
			--		rs.[ArtistName_Flat],
			--		rs.[count]
			--	order by
			--		rs.[count] desc
			--)
		from t_distinct_a rs
	),
	t_grouped_x as (
		select
			rs.[ArtistName_Flat],
			[count] = max(rs.[count])
		from t_distinct_pre rs
		group by
			rs.[ArtistName_Flat]
	),
	t_post_a as (
		select
			rs.[Id],
			rs.[ArtistName],
			rs.[ArtistName_Flat],
			rs.[DBDesignator],
			rs.[EntryDateTime],
			rs.[SortOrder],
			rs.[count]
		from t_distinct_pre rs
		join t_grouped_x ds
			on ds.[ArtistName_Flat] = rs.[ArtistName_Flat]
			and ds.[count] = rs.[count]
	),
	t_distict as (
		select top 10
			rs.[Id],
			rs.[ArtistName],
			rs.[ArtistName_Flat],
			[Artist_MBId] = ai.[MBId],
			rs.[DBDesignator],
			rs.[EntryDateTime],
			rs.[SortOrder],
			rs.[count]
		from t_post_a rs
		join [arma_artists].[dbo].[ArmaArtistsMain] ai
			on ai.[Id] = rs.[Id]
		order by
			[SortOrder],
			[ArtistName_Flat]
	)

	select
		rs.[Id],
		rs.[ArtistName],
		rs.[ArtistName_Flat],
		rs.[Artist_MBId],
		rs.[DBDesignator],
		rs.[EntryDateTime],
		rs.[count]
		--rs.[inlcude]
	from t_distict rs
	order by
		rs.[SortOrder],
		rs.[ArtistName_Flat];

	/*
		exec Arma_SearchArtists
			'rush'
			,1

		exec Arma_SearchArtists
			'nirvana'
			,1

		exec Arma_SearchArtists
			'bukis'
			,1

		exec Arma_SearchArtists
			'temerarios'
			,1

		exec Arma_SearchArtistsInternal
			'taylor sw'
			,1

	*/
END
GO
/****** Object:  StoredProcedure [dbo].[Arma_SiteGetApiToken]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[Arma_SiteGetApiToken]
AS
BEGIN
	set nocount on;
	declare
		@token uniqueidentifier = newid();

	insert into ArmaApiUsageToken
	(
		[token]
	)
	values
	(
		@token
	);

	select
		[token] = @token;
END
GO
/****** Object:  StoredProcedure [dbo].[Arma_SiteUseApiToken]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[Arma_SiteUseApiToken]
(
	@token uniqueidentifier
)
AS
BEGIN
	set nocount on;
	declare
		@token_is_valid bit = 0;

	select
		@token_is_valid = 1
	from ArmaApiUsageToken
	where
		[token] = @token;

	delete from ArmaApiUsageToken
	where
		[token] = @token;

	select
		[token_is_valid] = @token_is_valid;
END
GO
/****** Object:  StoredProcedure [dbo].[ArmaError_LogError]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[ArmaError_LogError]
(
	@ErrorController varchar(150),
	@ErrorMethod varchar(150) = null,
	@ErrorMessage varchar(5000)
)
AS
BEGIN
	insert into arma_operations.dbo.ArmaErrors
	(
		[ErrorController],
		[ErrorMethod],
		[ErrorMessage]
	)
	values
	(
		@ErrorController,
		@ErrorMethod,
		@ErrorMessage
	);
END
GO
/****** Object:  StoredProcedure [dbo].[ArmaPlayList_AddSongToPlaylist]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[ArmaPlayList_AddSongToPlaylist]
(
	@playlist_id int,
	@artist nvarchar(255) = null,
	@song nvarchar(255) = null,
	@video_id varchar(50) = null
)
AS
BEGIN
	set nocount on;

	if not exists (
		select
			1
		from ArmaPlaylistData
		where
			[PlaylistId] = @playlist_id
			and [VideoId] = @video_id
	)
	begin
		insert into ArmaPlaylistData
		(
			[PlaylistId],
			[Artist],
			[Song],
			[VideoId]
		)
		values
		(
			@playlist_id,
			@artist,
			@song,
			@video_id
		);
	end

	/*
		exec ArmaPlayList_AddSongToPlaylist
			2
	*/
END
GO
/****** Object:  StoredProcedure [dbo].[ArmaPlayList_CheckIfPlaylistExists]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[ArmaPlayList_CheckIfPlaylistExists]
(
	@playlist_name nvarchar(100),
	@user_id varchar(50)
)
AS
BEGIN
	set nocount on;

	declare
		@playlistExists bit = 0;

	if exists (
		select
			1
		from ArmaPlaylistNames
		where
			[PlaylistName] = @playlist_name
			and [UserId] = @user_id
	)
	begin
		set @playlistExists = 1;
	end

	select
		[PlaylistExists] = @playlistExists;

	/*
		exec ArmaPlayList_CheckIfPlaylistExists
			''
	*/
END
GO
/****** Object:  StoredProcedure [dbo].[ArmaPlayList_DeleteSongFromPlaylist]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luis Valle
-- =============================================
create PROCEDURE [dbo].[ArmaPlayList_DeleteSongFromPlaylist]
(
	@song_id int,
	@user_id varchar(50)
)
AS
BEGIN
	set nocount on;

	delete pd
	from ArmaPlaylistData pd
	join ArmaPlaylistNames pn
		on pn.[Id] = pd.[PlaylistId]
	where
		pd.[Id] = @song_id
		and pn.[UserId] = @user_id;

	/*
		exec ArmaPlayList_DeleteSongFromPlaylist
			2
	*/
END
GO
/****** Object:  StoredProcedure [dbo].[ArmaPlayList_DeleteUserPlaylistAndData]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[ArmaPlayList_DeleteUserPlaylistAndData]
(
	@playlist_id int,
	@user_id varchar(50)
)
AS
BEGIN
	set nocount on;

	delete pd
	from ArmaPlaylistData pd
	join ArmaPlaylistNames pn
		on pn.[Id] = pd.[PlaylistId]
	where
		pn.[Id] = @playlist_id
		and pn.[UserId] = @user_id;

	delete from ArmaPlaylistNames
	where
		[Id] = @playlist_id
		and [UserId] = @user_id;

	/*
		exec ArmaPlayList_DeleteUserPlaylistAndData
			2
	*/
END
GO
/****** Object:  StoredProcedure [dbo].[ArmaPlayList_GetPlaylistById]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[ArmaPlayList_GetPlaylistById]
(
	@playlist_id int,
	@user_id varchar(50)
)
AS
BEGIN
	set nocount on;

	select
		pd.[Id],
		pd.[Artist],
		pd.[Song],
		pd.[VideoId]
	from ArmaPlaylistData pd
	join ArmaPlaylistNames pn
		on pn.[Id] = pd.[PlaylistId]
	where
		pn.[Id] = @playlist_id
		and pn.[UserId] = @user_id;

	/*
		exec ArmaPlayList_GetPlaylistById
			2
	*/
END
GO
/****** Object:  StoredProcedure [dbo].[ArmaPlayList_GetPlaylistByName]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[ArmaPlayList_GetPlaylistByName]
(
	@playlist_name nvarchar(100),
	@user_id varchar(50)
)
AS
BEGIN
	set nocount on;

	select
		pd.[Id],
		pd.[Artist],
		pd.[Song],
		pd.[VideoId]
	from ArmaPlaylistData pd
	join ArmaPlaylistNames pn
		on pn.[Id] = pd.[PlaylistId]
	where
		pn.[PlaylistName] = @playlist_name
		and pn.[UserId] = @user_id;

	/*
		exec ArmaPlayList_GetPlaylistByName
			'Trance',
			'929a11b5-5e2e-46fd-97c8-198f7567170c'
	*/
END
GO
/****** Object:  StoredProcedure [dbo].[ArmaPlayList_GetSharedPlaylistByToken]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[ArmaPlayList_GetSharedPlaylistByToken]
(
	@token varchar(100)
)
AS
BEGIN
	set nocount on;

	select
		[PlaylistId] = [Id],
		[PlaylistName]
	from ArmaPlaylistNames
	where
		[PublicShareToken] = @token;

	select
		pd.[Id],
		pd.[Artist],
		pd.[Song],
		pd.[VideoId]
	from ArmaPlaylistData pd
	join ArmaPlaylistNames pn
		on pn.[Id] = pd.[PlaylistId]
	where
		pn.[PublicShareToken] = @token;

	/*
		exec ArmaPlayList_GetSharedPlaylistByToken
			'5AE9D53AD23A4CC8843BE76B8EC39851'
	*/
END
GO
/****** Object:  StoredProcedure [dbo].[ArmaPlayList_GetSharedPlaylistToken]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[ArmaPlayList_GetSharedPlaylistToken]
(
	@playlist_id int,
	@user_id varchar(100)
)
AS
BEGIN
	set nocount on;

	select
		[PlaylistToken] = [PublicShareToken]
	from ArmaPlaylistNames
	where
		[Id] = @playlist_id
		and [UserId] = @user_id;

	/*
		exec ArmaPlayList_GetSharedPlaylistToken
			1,
			'929a11b5-5e2e-46fd-97c8-198f7567170c'
	*/
END
GO
/****** Object:  StoredProcedure [dbo].[ArmaPlayList_GetUserPlaylists]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luis Valle
-- =============================================
create PROCEDURE [dbo].[ArmaPlayList_GetUserPlaylists]
(
	@user_id varchar(50)
)
AS
BEGIN
	set nocount on;

	select
		pn.[Id],
		pn.[PlaylistName]
	from ArmaPlaylistNames pn
	where
		pn.[UserId] = @user_id
	order by
		pn.[PlaylistName];

	/*
		exec ArmaPlayList_GetUserPlaylists
			'929a11b5-5e2e-46fd-97c8-198f7567170c'
	*/
END
GO
/****** Object:  StoredProcedure [dbo].[ArmaPlayList_InsertPlaylist]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[ArmaPlayList_InsertPlaylist]
(
	@playlist_name nvarchar(100),
	@user_id varchar(50)
)
AS
BEGIN
	set nocount on;
	declare
		@new_id int;
	
	if not exists (
		select
			1
		from ArmaPlaylistNames
		where
			[PlaylistName] = @playlist_name
			and [UserId] = @user_id
	)
	begin
		insert into ArmaPlaylistNames
		(
			[PlaylistName],
			[UserId]
		)
		values
		(
			@playlist_name,
			@user_id
		);

		set @new_id = scope_identity();
		
		select
			[new_id] = @new_id;
	end
	else
	begin
		raiserror('Playlist already exists: %s', 16, 1, @playlist_name);
		return;
	end
END
GO
/****** Object:  StoredProcedure [dbo].[ArmaPlayList_InsertSongToPlaylist]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[ArmaPlayList_InsertSongToPlaylist]
(
	@PlaylistId int,
	@Artist nvarchar(255) = null,
	@Song nvarchar(255) = null
)
AS
BEGIN
	set nocount on;
	
	insert into ArmaPlaylistData
	(
		[PlaylistId],
		[Artist],
		[Song]
	)
	values
	(
		@PlaylistId,
		@Artist,
		@Song
	);
END
GO
/****** Object:  StoredProcedure [dbo].[ArmaUsers_ConfirmEmailViaToken]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[ArmaUsers_ConfirmEmailViaToken]
(
	@token varchar(150)
)
AS
BEGIN
	set nocount on;
	declare
		@previously_confirmed bit = 0,
		@is_success bit = 0;

	if exists (
		select
			1
		from ArmaUsers_EmailConfirmation
		where
			[ConfirmationToken] = @token
	)
	begin
		select
			@previously_confirmed = [EmailConfirmed]
		from ArmaUsers_EmailConfirmation
		where
			[ConfirmationToken] = @token;

		if @previously_confirmed = 0
		begin
			update ArmaUsers_EmailConfirmation
			set
				[EmailConfirmed] = 1,
				[ConfirmedDateTime] = getdate()
			where
				[ConfirmationToken] = @token;

			set @is_success = 1;
		end
	end

	set @is_success = (
		case
			when @previously_confirmed = 1 then
				0
			else
				@is_success
		end
	);

	select
		[PreviuslyConfirmed] = @previously_confirmed,
		[IsSuccess] = @is_success;

END
GO
/****** Object:  StoredProcedure [dbo].[ArmaUsers_CreateEmailConfirmationRequest]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[ArmaUsers_CreateEmailConfirmationRequest]
(
	@email varchar(350)
)
AS
BEGIN
	set nocount on;
	declare
		@user_identity varchar(150),
		@token varchar(150) = newid();

	select
		@user_identity = us.[Id]
	from [AspNetUsers] us
	where
		us.[Email] = @email;

	if @user_identity is not null
	begin
		if not exists (
			select
				1
			from ArmaUsers_EmailConfirmation ec
			where
				ec.[UserIdentity] = @user_identity
		)
		begin
			insert into ArmaUsers_EmailConfirmation
			(
				[UserIdentity],
				[EmailConfirmed],
				[ConfirmationToken],
				[SentDatetime]
			)
			values
			(
				@user_identity,
				0,
				@token,
				getdate()
			);
		end
		else
		begin
			set @token = null;

			select
				@token = [ConfirmationToken]
			from ArmaUsers_EmailConfirmation
			where
				[UserIdentity] = @user_identity;
		end
	end
	else
	begin
		set @token = null;
	end

	select
		[ConfirmationToken] = @token;
END
GO
/****** Object:  StoredProcedure [dbo].[ArmaUsers_GetEmailConfirmationToken]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[ArmaUsers_GetEmailConfirmationToken]
(
	@token varchar(150)
)
AS
BEGIN
	set nocount on;
	declare
		@email varchar(350);

	select
		@email = us.[Email]
	from ArmaUsers_EmailConfirmation ec
	join AspNetUsers us
		on us.[Id] = ec.[UserIdentity]
	where
		ec.[ConfirmationToken] = @token;

	select
		[Email] = @email;

	/*
		exec ArmaUsers_GetEmailConfirmationToken
			'37EADA93-726C-4E5E-8F9A-3209509896FC'
	*/
END
GO
/****** Object:  StoredProcedure [dbo].[ArmaUsers_GetEmailConfirmedStatus]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[ArmaUsers_GetEmailConfirmedStatus]
(
	@email varchar(350)
)
AS
BEGIN
	set nocount on;
	declare @is_confirmed bit = null;

	select
		@is_confirmed = ec.[EmailConfirmed]
	from ArmaUsers_EmailConfirmation ec
	join AspNetUsers us
		on us.[Id] = ec.[UserIdentity]
	where
		us.[Email] = @email;

	select
		[ConfirmedEmail] = @is_confirmed;

	/*
		exec ArmaUsers_GetEmailConfirmedStatus
			'luis.e.valle+junk@gmail.com'
	*/
END
GO
/****** Object:  StoredProcedure [dbo].[ArmaUsers_GetListOfUnsubscribedEmails]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[ArmaUsers_GetListOfUnsubscribedEmails]
AS
BEGIN
	set nocount on;

	select
		us.[Email]
	from ArmaUsers_EmailConfirmation ec
	join AspNetUsers us
		on us.[Id] = ec.[UserIdentity]
	where
		coalesce(ec.[DontSendEmails], 0) = 1

	/*
		exec ArmaUsers_GetListOfUnsubscribedEmails
	*/
END
GO
/****** Object:  StoredProcedure [dbo].[ArmaUsers_RemoveFromEmailList]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[ArmaUsers_RemoveFromEmailList]
(
	@email varchar(350)
)
AS
BEGIN
	update ec
	set
		ec.[DontSendEmails] = 1
	from ArmaUsers_EmailConfirmation ec
	join AspNetUsers us
		on us.[Id] = ec.[UserIdentity]
	where
		us.[Email] = @email;

	/*
		ArmaUsers_RemoveFromEmailList
			'luis.e.valle+junk@gmail.com'
	*/
END
GO
/****** Object:  StoredProcedure [dbo].[Artist_GetArtistList]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[Artist_GetArtistList]
(
	@search varchar(150)
)
AS
BEGIN
	set nocount on;

	select
		[id],
		[artist_name]
		--[artist_uri]
	from Artists
	where
		(ltrim(rtrim(coalesce(@search, ''))) = '')
		or
		[artist_name_flat] like concat('%', @search, '%')
	order by
		[artist_name];

	/*
		exec Artist_GetArtistList
			''
	*/
END
GO
/****** Object:  StoredProcedure [dbo].[Genre_GetArtistsRequiringGenres]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[Genre_GetArtistsRequiringGenres]
(
	@rebuild_staging_table bit = 0
)
AS
BEGIN
	set nocount on;

	if @rebuild_staging_table = 1
	begin
		delete from [arma_genres].dbo.[genre_staged_artists]
		where
			1=1;

		with t_artists_mbgenres as (
			select
				am.[id],
				[artist_mbid] = am.[MBId],
				am.[Name]
			from [arma_artists].dbo.[ArmaArtistsMain] am
			left join [arma_artists].dbo.[ArmaArtistGenreMap] gm
				on gm.[ArtistId] = am.[Id]
			where
				gm.[GenreId] is null
				and am.[id] is not null
		),
		t_artists_stagedgenres as (
			select
				am.[id],
				[artist_mbid] = am.[MBId],
				am.[Name]
			from [arma_artists].dbo.[ArmaArtistsMain] am
			left join [arma_artists].dbo.[ArmaGenresStaged] gs
				on gs.[ArtistMBId] = am.[MBId]
			where
				gs.[MBId] is null
				and am.[id] is not null
		),
		t_no_genres as (
			select
				ag.[id],
				ag.[artist_mbid],
				ag.[Name]
			from t_artists_mbgenres ag
			left join t_artists_stagedgenres ng
				on ng.[Id] = ag.[Id]
			where
				ng.[Id] is null
				and ag.[id] is not null
			union all
			select
				ag.[id],
				ag.[artist_mbid],
				ag.[Name]
			from t_artists_stagedgenres ng
			left join t_artists_mbgenres ag
				on ag.[Id] = ng.[Id]
			where
				ag.[Id] is null
				and ag.[id] is not null
		)

		insert into [arma_genres].dbo.[genre_staged_artists]
		(
			[id],
			[artist_mbid],
			[name]
		)
		select
			[id],
			[artist_mbid],
			[Name]
		from t_no_genres;
	end
	else
	begin
		select
			gs.[id],
			gs.[artist_mbid],
			gs.[Name]
		from [arma_genres].dbo.[genre_staged_artists] gs
		left join [arma_genres].dbo.[genre_artists] aa
			on aa.[artist_id] = gs.[artist_mbid]
		where
			aa.[artist_id] is null
			and gs.[artist_mbid] is not null;
	end

	/*
		exec Genre_GetArtistsRequiringGenres
			1
	*/
END
GO
/****** Object:  StoredProcedure [dbo].[Genre_InsertArtistGenre]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[Genre_InsertArtistGenre]
(
	@artist_mbid uniqueidentifier,
	@genre_id int
)
AS
BEGIN
	set nocount on;
	
	if not exists (
		select
			1
		from [arma_genres].dbo.[genre_artists]
		where
			[artist_id] = @artist_mbid
			and [genre_id] = @genre_id
	)
	begin
		insert into [arma_genres].dbo.[genre_artists]
		(
			[artist_id],
			[genre_id]
		)
		values
		(
			@artist_mbid,
			@genre_id
		);
	end
END
GO
/****** Object:  StoredProcedure [dbo].[Genre_InsertGenre]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[Genre_InsertGenre]
(
	@genre_name nvarchar(200)
)
AS
BEGIN
	set nocount on;
	declare
		@genre_id int,
		@genre_identity uniqueidentifier = newid();

	select
		@genre_id = [id]
	from [arma_genres].dbo.[genre_root]
	where
		[genre_name] = @genre_name;

	if @genre_id is null
	begin
		insert into [arma_genres].dbo.[genre_root]
		(
			[genre_identity],
			[genre_name]
		)
		values
		(
			@genre_identity,
			@genre_name
		);

		set @genre_id = scope_identity();
	end

	select
		[genre_id] = @genre_id;
END
GO
/****** Object:  StoredProcedure [dbo].[Operations_CheckIfMBAlbumIdExists]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[Operations_CheckIfMBAlbumIdExists]
(
	@mb_albumid varchar(50),
	@mb_artistid varchar(50)
)
AS
BEGIN
	set nocount on;
	declare
		@artist_id int,
		@db_source varchar(15),
		@sql nvarchar(max);

	select
		@db_source = [DbSource],
		@artist_id = [Id]
	from [arma_artists].dbo.[ArmaArtistsMain]
	where
		[MBId] = @mb_artistid;

	if coalesce(@db_source, '') <> ''
	begin
		set @sql = '
			declare @albumExists bit = 0;

			select
				@albumExists = 1
			from [arma_' + @db_source + '].[dbo].[AlbumMapper]
			where
				[MBAlbumId] = ''' + @mb_albumid + ''';

			select
				[AlbumExists] = @albumExists;
		';		
	
		exec sp_executesql @sql;
	end
	else
	begin
		select
			[AlbumExists] = 0;
	end

	/*
		exec Operations_CheckIfMBAlbumIdExists
			'ba635ce1-4846-4a98-b0aa-ebc82b493d4a',
			'95db1c7c-21b8-4956-82ad-20217cd5d395'
	*/
END
GO
/****** Object:  StoredProcedure [dbo].[Operations_CheckIfMBArtistIdExists]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[Operations_CheckIfMBArtistIdExists]
(
	@mb_artistid varchar(50)
)
AS
BEGIN
	set nocount on;
	declare
		@artistExists bit = 0;

	select
		@artistExists = 1
	from [arma_artists].dbo.[ArmaArtistsMain]
	where
		[MBId] = @mb_artistid;

	select
		[ArtistExists] = @artistExists;
		
	/*
		exec Operations_CheckIfMBArtistIdExists
			'da46be76-4daf-414a-b454-fcd0ea810758'
	*/
END
GO
/****** Object:  StoredProcedure [dbo].[Operations_GetAllDBsDiskUsage]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[Operations_GetAllDBsDiskUsage]
(
	@db_source varchar(10) = null
)
AS
BEGIN
	set nocount on;

	declare
		@sql nvarchar(max),
		@sqlBody nvarchar(max);

	drop table if exists #tmp_alldb_refs;
	create table #tmp_alldb_refs
	(
		[Reference] varchar(10)
	);

	if @db_source is not null
	begin
		insert into #tmp_alldb_refs
		(
			[Reference]
		)
		values
		(
			@db_source
		);
	end
	else
	begin
		insert into #tmp_alldb_refs
		(
			[Reference]
		)
		select
			[Reference]
		from ArmaDbReferences;
	end
	
	drop table if exists #tmp_alldbsizes_results;
	create table #tmp_alldbsizes_results
	(
		[TableName] varchar(500),
		[SchemaName] varchar(50),
		[RowCounts] int,
		[TotalSpaceMB] int,
		[UsedSpaceMB] int,
		[UnusedSpaceMB] int,
		[dbSource] varchar(60)
	);

	select
		@sqlBody = stuff((
			select
				' union all (
					select 
						[TableName] = t.NAME,
						[SchemaName] = s.Name,
						[RowCounts] = p.rows,
						[TotalSpaceMB] = (sum(a.total_pages) * 8 / 1024), 
						[UsedSpaceMB] = (sum(a.used_pages) * 8 / 1024), 
						[UnusedSpaceMB] = ((sum(a.total_pages) - sum(a.used_pages)) * 8 / 1024),
						[dbSource] = ''' + rc.[Reference] + '''
					from arma_' + rc.[Reference] + '.sys.tables t
					inner join arma_' + rc.[Reference] + '.sys.indexes i
						on t.object_id = i.object_id
					inner join arma_' + rc.[Reference] + '.sys.partitions p
						on i.object_id = p.object_id and i.index_id = p.index_id
					inner join arma_' + rc.[Reference] + '.sys.allocation_units a
						on p.partition_id = a.container_id
					left outer join arma_' + rc.[Reference] + '.sys.schemas s
						on t.schema_id = s.schema_id
					group by 
						t.name,
						s.name,
						p.rows				
				)'
			from #tmp_alldb_refs rc
			where
				1=1
			for xml path(''), type).value('.', 'nvarchar(max)'), 1, 11, '');
			
	set @sql = '
		insert into #tmp_alldbsizes_results
		(
			[TableName],
			[SchemaName],
			[RowCounts],
			[TotalSpaceMB],
			[UsedSpaceMB],
			[UnusedSpaceMB],
			[dbSource]
		)
	' + @sqlBody;

	exec sp_executesql @sql;

	select 
		[TableName],
		[SchemaName],
		[RowCounts],
		[TotalSpaceMB],
		[UsedSpaceMB],
		[UnusedSpaceMB],
		[dbSource]
	from #tmp_alldbsizes_results
	order by 
		TotalSpaceMB desc;

	/*
		exec Operations_GetAllDBsDiskUsage

		exec Operations_GetAllDBsDiskUsage
			'd'
	*/
END
GO
/****** Object:  StoredProcedure [dbo].[Operations_GetAllMBArtistIds]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[Operations_GetAllMBArtistIds]
AS
BEGIN
	set nocount on;

	select
		[MBId]
	from [arma_artists].dbo.[ArmaArtistsMain]
	order by
		[MBId];
		
	/*
		exec Operations_GetAllMBArtistIds
	*/
END
GO
/****** Object:  StoredProcedure [dbo].[Operations_GetAllMBArtistIdsWithDBSource]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[Operations_GetAllMBArtistIdsWithDBSource]
AS
BEGIN
	set nocount on;

	select
		[MBId],
		[DBSource]
	from [arma_artists].dbo.[ArmaArtistsMain];
		
	/*
		exec Operations_GetAllMBArtistIdsWithDBSource
	*/
END
GO
/****** Object:  StoredProcedure [dbo].[Operations_GetCachedToken]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[Operations_GetCachedToken]
AS
BEGIN
	set nocount on;

	delete from [arma_devops].dbo.[ApiTokens]
	where
		getdate() >= dateadd(hour, 1, [AssignedDateTime]);

	select top 1
		[Token]
	from [arma_devops].dbo.[ApiTokens]
	order by
		[AssignedDateTime] desc;

	/*
		exec Operations_GetCachedToken
	*/
END
GO
/****** Object:  StoredProcedure [dbo].[Operations_GetDbDiskUsage]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[Operations_GetDbDiskUsage]
AS
BEGIN	
	select 
		[TableName] = t.NAME,
		[SchemaName] = s.Name,
		[RowCounts] = p.rows,
		[TotalSpaceMB] = (sum(a.total_pages) * 8 / 1024), 
		[UsedSpaceMB] = (sum(a.used_pages) * 8 / 1024), 
		[UnusedSpaceMB] = ((sum(a.total_pages) - sum(a.used_pages)) * 8 / 1024)
	from 
		sys.tables t
	inner join sys.indexes i
		on t.object_id = i.object_id
	inner join sys.partitions p
		on i.object_id = p.object_id and i.index_id = p.index_id
	inner join sys.allocation_units a
		on p.partition_id = a.container_id
	left outer join sys.schemas s
	on t.schema_id = s.schema_id
	group by 
		t.name,
		s.name,
		p.rows
	order by 
		TotalSpaceMB desc;

	/*
		exec Operations_GetDbDiskUsage
	*/
END
GO
/****** Object:  StoredProcedure [dbo].[Operations_GetLatestArtistAndAlbums]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[Operations_GetLatestArtistAndAlbums]
AS
BEGIN
	set nocount on;

	declare
		@sql nvarchar(max),
		@sqlBody nvarchar(max),
		@staged_date datetime = getdate(),
		@starting_date datetime;

	select top 1
		@staged_date = [StageDateTime]
	from [arma_devops].dbo.[SyncQueue]
	where
		[IsCompleted] = 1
	order by
		[StageDateTime] desc;

	set @starting_date = cast((cast(@staged_date as date)) as datetime);

	drop table if exists #tmp_alldb_refs;
	create table #tmp_alldb_refs
	(
		[Reference] varchar(10)
	);

	insert into #tmp_alldb_refs
	(
		[Reference]
	)
	select
		[Reference]
	from ArmaDbReferences;
	
	drop table if exists #tmp_new_artists_results;
	create table #tmp_new_artists_results
	(
		[ArtistName] nvarchar(500),
		[AddedDateTime] datetime
	);
	
	drop table if exists #tmp_new_album_results;
	create table #tmp_new_album_results
	(
		[ArtistName] nvarchar(500),
		[AlbumName] nvarchar(500),
		[AlbumId] int,
		[DbSource] varchar(10),
		[AddedDateTime] datetime
	);

	insert into #tmp_new_artists_results
	(
		[ArtistName],
		[AddedDateTime]
	)
	select
		[Name],
		[AddedDateTime]
	from [arma_artists].dbo.[ArmaArtistsMain]
	where
		[AddedDateTime] >= @starting_date;

	select
		@sqlBody = stuff((
			select
				' union all (
					select 
						[ArtistName] = ar.[Name],
						[AlbumName] = al.[Title],
						[AlbumId] = al.[Id],
						[DbSource] = ''' + rc.[Reference] + ''',
						al.[AddedDateTime]
					from arma_' + rc.[Reference] + '.dbo.AlbumMapper al
					left join arma_artists.dbo.ArmaArtistsMain ar
						on ar.[Id] = al.[ArtistId]
					left join arma_' + rc.[Reference] + '.dbo.SongMapper sm
						on sm.[AlbumId] = al.[Id]
					where
						al.[AddedDateTime] >= ''' + convert(varchar(25), @starting_date, 121) + '''
				)'
			from #tmp_alldb_refs rc
			where
				1=1
			for xml path(''), type).value('.', 'nvarchar(max)'), 1, 11, '');
			
	set @sql = '
		insert into #tmp_new_album_results
		(
			[ArtistName],
			[AlbumName],
			[AlbumId],
			[DbSource],
			[AddedDateTime]
		)		
	' + @sqlBody;

	exec sp_executesql @sql;

	with t_al_dist as (
		select
			[ArtistName],
			[AlbumName],
			[AlbumId],
			[DbSource],
			--[AddedDateTime],
			[count] = count(*)
		from #tmp_new_album_results
		group by
			[ArtistName],
			[AlbumName],
			[DbSource],
			--[AddedDateTime],
			[AlbumId]
	),
	t_al_max as (
		select
			[ArtistName],
			[AlbumName],
			[DbSource],
			[count] = max([count])
		from t_al_dist
		group by
			[ArtistName],
			[AlbumName],
			[DbSource]
	),
	t_al_ids as (
		select
			[ArtistName],
			[AlbumName],
			[AlbumId],
			[AddedDateTime]
		from #tmp_new_album_results
		group by
			[ArtistName],
			[AlbumName],
			[AlbumId],
			[AddedDateTime]
	),
	t_al_dist_recs as (
		select distinct
			am.[ArtistName],
			am.[AlbumName],
			ao.[AlbumId],
			am.[DbSource],
			am.[count],
			ao.[AddedDateTime]
		from t_al_max am
		join t_al_dist ac
			on ac.[AlbumName] = am.[AlbumName]
			and ac.[ArtistName] = am.[ArtistName]
			and ac.[count] = am.[count]
		join t_al_ids ao
			on ao.[AlbumId] = ac.[AlbumId]
	),
	t_al_priority as (
		select
			[ArtistName],
			[AlbumName],
			[AlbumId],
			[DbSource],
			[AddedDateTime],
			[priority] = row_number() over(
				partition by
					[ArtistName],
					[AlbumName]
				order by
					[count] desc,
					[AlbumId] desc
			)
		from t_al_dist_recs
	),
	t_al_clean as (
		select
			[ArtistName],
			[AlbumName],
			[AlbumId],
			[DbSource],
			[AddedDateTime]
		from t_al_priority
		where
			[priority] = 1
	)

	select
		[ArtistName],
		[AlbumName],
		[AlbumId],
		[DbSource],
		[AddedDateTime]
	from t_al_clean
	order by 
		[ArtistName],
		[AlbumName];

	select 
		[ArtistName],
		[AddedDateTime]
	from #tmp_new_artists_results
	order by 
		[AddedDateTime] desc;

	--select
	--	[ArtistName],
	--	[AlbumName],
	--	[AlbumId],
	--	[DbSource],
	--	[AddedDateTime]
	--from #tmp_new_album_results
	--order by 
	--	[ArtistName],
	--	[AlbumName];

	drop table if exists #tmp_new_artists_results;
	drop table if exists #tmp_new_album_results;

	/*
		exec Operations_GetLatestArtistAndAlbums
			'2024-05-18 00:00:00.000'
	*/
END
GO
/****** Object:  StoredProcedure [dbo].[Operations_GetTopSongIdFromAlbums]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[Operations_GetTopSongIdFromAlbums]
AS
BEGIN
	set nocount on;

	declare
		@sql nvarchar(max),
		@sqlBody nvarchar(max),
		@songsCount nvarchar(max),
		@songsTotalCount int;

	drop table if exists #tmp_db_songs_refs;
	create table #tmp_db_songs_refs
	(
		[Reference] varchar(10)
	);

	insert into #tmp_db_songs_refs
	(
		[Reference]
	)
	select
		[Reference]
	from ArmaDbReferences;
	
	drop table if exists #tmp_songs_Ids;
	create table #tmp_songs_Ids
	(
		[SongId] int,
		[AddedDateTime] datetime,
		[dbSource] varchar(10)
	);
	
	drop table if exists #tmp_songs_count;
	create table #tmp_songs_count
	(
		[SongsCount] int
	);

	select
		@sqlBody = stuff((
			select
				' insert into #tmp_songs_Ids ([SongId], [AddedDateTime], [dbSource]) select top 1 [Id], [AddedDateTime], [dbSource] = ''' + rc.[Reference] + ''' from [arma_' + rc.[Reference] + '].[dbo].[SongMapper] order by [AddedDateTime] desc;'
			from #tmp_db_songs_refs rc
			where
				1=1
			for xml path(''), type).value('.', 'nvarchar(max)'), 1, 0, '');

	select
		@songsCount = stuff((
			select
				' insert into #tmp_songs_count ([SongsCount]) select count(*) from [arma_' + rc.[Reference] + '].[dbo].[SongMapper];'
			from #tmp_db_songs_refs rc
			where
				1=1
			for xml path(''), type).value('.', 'nvarchar(max)'), 1, 0, '');

	set @sqlBody = @sqlBody + @songsCount;

	exec sp_executesql @sqlBody;

	select
		@songsTotalCount = sum([SongsCount])
	from #tmp_songs_count;

	select top 1
		[SongsCount] = @songsTotalCount,
		ti.[SongId],
		--ar.[ArtistName],
		ti.[dbSource],
		ti.[AddedDateTime]
	from #tmp_songs_Ids ti
	--join [arma_artists].[dbo].[ArtistsRaw] ar
	--	on ar.[Id] = ti.[ArtistId]
	order by
		ti.[AddedDateTime] desc;

	/*
		exec Operations_GetTopSongIdFromAlbums

	*/
END


GO
/****** Object:  StoredProcedure [dbo].[Operations_InsertArtistToRawList]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[Operations_InsertArtistToRawList]
(
	@ArtistName varchar(500),
	@AlbumsUrl varchar(1500),
	@DBDesignator varchar(15)
)
AS
BEGIN
	set nocount on;

	if not exists (
		select
			1
		from arma_artists.dbo.ArtistsRaw
		where
			[ArtistName] = @ArtistName
	)
	begin
		insert into arma_artists.dbo.ArtistsRaw
		(
			[ArtistName],
			[AlbumsUrl],
			[DBDesignator],
			[EntryDateTime]
		)
		values
		(
			@ArtistName,
			@AlbumsUrl,
			@DBDesignator,
			getdate()
		);
	end


END
GO
/****** Object:  StoredProcedure [dbo].[Operations_LogUserActivity]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[Operations_LogUserActivity]
(
	@Timestamp datetime,
	@IpAddress varchar(50) = null,
	@UserId varchar(50) = null,
	@UserName varchar(150) = null,
	@RequestPath varchar(1000) = null,
	@RequestMethod varchar(50) = null,
	@UserAgent varchar(700) = null,
	@Referrer varchar(1000) = null,
	@RequestHeaders varchar(max) = null,
	@QueryString varchar(1500) = null,
	@FromApi bit = 0,
	@RequestBody nvarchar(max) = null,
	@Duration float = null
)
AS
BEGIN
	set nocount on;

	insert into arma_operations.dbo.ArmaUserOperations
	(
		[Timestamp],
		[IpAddress],
		[UserId],
		[UserName],
		[RequestPath],
		[RequestMethod],
		[UserAgent],
		[Referrer],
		[RequestHeaders],
		[QueryString],
		[FromApi],
		[RequestBody],
		[Duration]
	)
	values
	(
		@Timestamp,
		@IpAddress,
		@UserId,
		@UserName,
		@RequestPath,
		@RequestMethod,
		@UserAgent,
		@Referrer,
		@RequestHeaders,
		@QueryString,
		@FromApi,
		@RequestBody,
		@Duration
	);

END
GO
/****** Object:  StoredProcedure [dbo].[Operations_MBInsertAlbumForArtist]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[Operations_MBInsertAlbumForArtist]
(
	@mb_artistid varchar(50),
	@mb_albumid varchar(50),
	@album_title nvarchar(350),
	@status varchar(25),
	@primary_type varchar(50),
	@first_release_date varchar(50)
)
AS
BEGIN
	set nocount on;
	declare
		@artist_id int,
		@db_source varchar(15),
		@sql nvarchar(max),
		@parameters nvarchar(max);
		
	select
		@db_source = [DbSource],
		@artist_id = [Id]
	from [arma_artists].dbo.[ArmaArtistsMain]
	where
		[MBId] = @mb_artistid;

	if @db_source is not null and @artist_id is not null
	begin
		set @parameters = '@artist_id int, @mb_albumid varchar(50), @mb_artistid varchar(50), @album_title nvarchar(350), @status varchar(25), @primary_type varchar(50), @first_release_date varchar(50), @db_source varchar(15)';

		set @sql = '
			if not exists (
				select
					1
				from [arma_' + @db_source + '].[dbo].[AlbumMapper]
				where
					[MBAlbumId] = @mb_albumid
			)
			begin
				declare @new_artist_id int;

				insert into [arma_' + @db_source + '].[dbo].[AlbumMapper]
				(
					[MBAlbumId],
					[ArtistId],
					[MBArtistId],
					[Title],
					[TitleFlat],
					[Status],
					[PrimaryType],
					[FirstReleaseDate],
					[AddedDateTime]
				)
				values
				(
					@mb_albumid,
					@artist_id,
					@mb_artistid,
					@album_title,
					convert(varchar(350), @album_title collate SQL_Latin1_General_Cp1251_CS_AS),
					@status,
					@primary_type,
					@first_release_date,
					getdate()
				);
			
				set @new_artist_id = scope_identity();

				select
					[new_id] = @new_artist_id,
					[db_source] = @db_source;
			end
		';		
	
		exec sp_executesql @sql, @parameters, @artist_id, @mb_albumid, @mb_artistid, @album_title, @status, @primary_type, @first_release_date, @db_source;
	end
END
GO
/****** Object:  StoredProcedure [dbo].[Operations_MBInsertAlbumSong]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[Operations_MBInsertAlbumSong]
(
	@db_source varchar(15),
	@mb_songid varchar(50),
	@album_id int,
	@cd_number int,
	@song_number int,
	@song_title nvarchar(350)
)
AS
BEGIN
	set nocount on;
	declare
		@artist_id int,
		@sql nvarchar(max),
		@parameters nvarchar(max);

	set @parameters = '@mb_songid varchar(50), @album_id int, @cd_number int, @song_number int, @song_title nvarchar(350)';

	set @sql = '
		insert into [arma_' + @db_source + '].[dbo].[SongMapper]
		(
			[MBSongId],
			[AlbumId],
			[CDNumber],
			[SongNumber],
			[SongTitle],
			[SongTitleFlat],
			[AddedDateTime]
		)
		values
		(
			@mb_songid,
			@album_id,
			@cd_number,
			@song_number,
			@song_title,
			convert(varchar(350), @song_title collate SQL_Latin1_General_Cp1251_CS_AS),
			getdate()
		);
	';		
	
	exec sp_executesql @sql, @parameters, @mb_songid, @album_id, @cd_number, @song_number, @song_title;
END
GO
/****** Object:  StoredProcedure [dbo].[Operations_MBInsertArtistGenre]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[Operations_MBInsertArtistGenre]
(
	@artist_id int,
	@mb_id varchar(50),
	@genre_name varchar(150),
	@count int,
	@disambiguation varchar(150)
)
AS
BEGIN
	set nocount on;

	declare
		@genre_id int;

	select
		@genre_id = [Id]
	from [arma_artists].dbo.[ArmaGenres]
	where
		[Name] = @genre_name;

	-- insert any new genres
	if @genre_id is null
	begin
		insert into [arma_artists].dbo.[ArmaGenres]
		(
			[MBId],
			[Name],
			[Count],
			[Disambiguation]
		)
		values
		(
			@mb_id,
			@genre_name,
			@count,
			@disambiguation
		);

		set @genre_id = scope_identity();
	end

	-- insert genre into artist profile
	if @genre_id is not null
	begin
		insert into [arma_artists].dbo.[ArmaArtistGenreMap]
		(
			[ArtistId],
			[GenreId]
		)
		values
		(
			@artist_id,
			@genre_id
		);
	end
END
GO
/****** Object:  StoredProcedure [dbo].[Operations_MBInsertArtistWithGenres]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[Operations_MBInsertArtistWithGenres]
(
	@mb_id varchar(50),
	@name nvarchar(350),
	@sort_name nvarchar(350),
	@country varchar(50),
	@type varchar(50),
	@type_id varchar(50),
	@rating_value float,
	@rating_votes int,
	@lifespan_begin varchar(150),
	@lifespan_end varchar(150),
	@lifespan_ended bit
)
AS
BEGIN
	set nocount on;

	declare
		@new_artist_id int,
		@flat_name varchar(350),
		@flat_nameReverse varchar(350),
		@name_search varchar(350),
		@name_searchReverse varchar(350),
		@first_char varchar(5),
		@db_source varchar(15);

	if not exists (
		select
			1
		from [arma_artists].dbo.[ArmaArtistsMain]
		where
			[MBId] = @mb_id
	)
	begin
		set @flat_name = convert(varchar(255), @sort_name collate SQL_Latin1_General_Cp1251_CS_AS);
		set @flat_nameReverse = reverse(@flat_name);
		set @name_search = (
			case
				when charindex(',', @flat_name) > 0 then -- select charindex(',', 'dfadfa,dfadfa')  select substring('dadfa', 2, len('dadfa') -1)
					concat(
						ltrim(rtrim(substring(@flat_name, (select [armaradio].dbo.LastIndexOf(@flat_name, ',')) + 1, len(@flat_name) -1))),
						' ',
						ltrim(rtrim(substring(@flat_name, 1, (select [armaradio].dbo.LastIndexOf(@flat_name, ',')) -1)))
					)
				else
					@flat_name
			end
		);
		set @name_searchReverse = reverse(@name_search);

		set @first_char = lower(substring(@flat_name, 1, 1));

		if isnumeric(@first_char) = 1
		begin
			set @db_source = 'num';
		end
		else if @first_char like '[a-zA-Z]%'   -- if '!' like '[a-zA-Z]%' begin select 1 end else begin select 0 end
		begin
			set @db_source = @first_char;
		end
		else
		begin
			set @db_source = 'symb';
		end
		
		insert into [arma_artists].dbo.[ArmaArtistsMain]
		(
			[MBId],
			[Name],
			[SortName],
			[NameFlat],
			[NameFlatReverse],
			[NameSearch],
			[NameSearchReverse],
			[Country],
			[Type],
			[TypeId],
			[RatingValue],
			[RatingVotes],
			[LifeSpanBegin],
			[LifeSpanEnd],
			[LifeSpanEnded],
			[DBSource],
			[AddedDateTime]
		)
		values
		(
			@mb_id,
			@name,
			@sort_name,
			@flat_name,
			@flat_nameReverse,
			@name_search,
			@name_searchReverse,
			@country,
			@type,
			@type_id,
			@rating_value,
			@rating_votes,
			@lifespan_begin,
			@lifespan_end,
			@lifespan_ended,
			@db_source,
			getdate()
		);

		set @new_artist_id = scope_identity();
	end

	select
		[new_artist_id] = @new_artist_id;
END
GO
/****** Object:  StoredProcedure [dbo].[Operations_MBInsertGenreByName]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[Operations_MBInsertGenreByName]
(
	@artist_id int,
	@genre_name varchar(150)
)
AS
BEGIN
	set nocount on;

	declare
		@genre_id int;

	select top 1
		@genre_id = [Id]
	from [arma_artists].dbo.[ArmaGenres]
	where
		[Name] = @genre_name
	order by
		[Count] desc;

	-- insert genre into artist profile
	if @genre_id is not null
	begin
		if not exists (
			select
				1
			from [arma_artists].dbo.[ArmaArtistGenreMap]
			where
				[ArtistId] = @artist_id
				and [GenreId] = @genre_id
		)
		begin
			insert into [arma_artists].dbo.[ArmaArtistGenreMap]
			(
				[ArtistId],
				[GenreId]
			)
			values
			(
				@artist_id,
				@genre_id
			);
		end
	end
END
GO
/****** Object:  StoredProcedure [dbo].[Operations_RecreateArtistList]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[Operations_RecreateArtistList]
AS
BEGIN
	drop table if exists Artists;
	CREATE TABLE Artists (
		id INT IDENTITY(1,1) NOT NULL,
		artist_name VARCHAR(255),
		artist_name_flat VARCHAR(255),
		artist_uri VARCHAR(255)
	 CONSTRAINT [PK_Artists] PRIMARY KEY CLUSTERED 
	(
		[id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY];

	insert into Artists
	(
		[artist_name],
		[artist_name_flat],
		[artist_uri]
	)
	select
		t.[artist_name],
		convert(varchar(255), t.[artist_name] collate SQL_Latin1_General_Cp1251_CS_AS),
		t.[artist_uri]
	from Tracks t
	group by
		t.[artist_name],
		t.[artist_uri];

	/*
		exec Operations_RecreateArtistList
	*/
END
GO
/****** Object:  StoredProcedure [dbo].[Operations_RecreateTracksIndexed]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[Operations_RecreateTracksIndexed]
AS
BEGIN
	drop table if exists Tracks;
	create table Tracks (
		tid int identity(1,1) not null,
		artist_name varchar(255),
		artist_name_flat varchar(255),
		track_uri varchar(255),
		artist_uri varchar(255),
		track_name varchar(255),
		track_name_flat varchar(255),
		usability_score int
		constraint [PK_Tracks] primary key clustered 
		(
			[tid] ASC
		)with (pad_index = off, statistics_norecompute = off, ignore_dup_key = off, allow_row_locks = on, allow_page_locks = on, optimize_for_sequential_key = off) on [primary]
	) on [primary];

	insert into Tracks
	(
		[artist_name],
		[artist_name_flat],
		[track_uri],
		[artist_uri],
		[track_name],
		[track_name_flat],
		[usability_score]
	)
	select
		[artist_name],
		convert(varchar(255), [artist_name] collate SQL_Latin1_General_Cp1251_CS_AS),
		[track_uri],
		[artist_uri],
		[track_name],
		convert(varchar(255), [track_name] collate SQL_Latin1_General_Cp1251_CS_AS),
		[usability_score] = count(*)
	from RawTracks
	group by
		[artist_name],
		[track_uri],
		[artist_uri],
		[artist_uri],
		[track_name]
	order by
		[artist_name],
		[track_name];

	/*
		exec Operations_RecreateTracksIndexed
	*/
END
GO
/****** Object:  StoredProcedure [dbo].[Operations_SaveApiTokenToCache]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[Operations_SaveApiTokenToCache]
(
	@token varchar(500)
)
AS
BEGIN
	set nocount on;

	if ltrim(rtrim(coalesce(@token, ''))) <> ''
	begin
		delete from [arma_devops].dbo.[ApiTokens] where 1=1;

		insert into [arma_devops].dbo.[ApiTokens]
		(
			[Token]
		)
		values
		(
			@token
		);
	end
	
	/*
		exec Operations_SaveApiTokenToCache
			''
	*/
END
GO
/****** Object:  StoredProcedure [dbo].[Operations_Sync_AddVersionToCompleted]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luis Valle
-- =============================================
create PROCEDURE [dbo].[Operations_Sync_AddVersionToCompleted]
(
	@version_number varchar(50),
	@errors_occurred bit
)
AS
BEGIN
	set nocount on;
	
	update [arma_devops].dbo.[SyncQueue]
	set
		[CompletedDateTime] = getdate(),
		[IsCompleted] = 1,
		[ErrorsOcurred] = @errors_occurred
	where
		[QueueKey] = @version_number;

END
GO
/****** Object:  StoredProcedure [dbo].[Operations_Sync_AddVersionToStaging]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luis Valle
-- =============================================
create PROCEDURE [dbo].[Operations_Sync_AddVersionToStaging]
(
	@version_number varchar(50)
)
AS
BEGIN
	set nocount on;
	
	insert into [arma_devops].dbo.[SyncQueue]
	(
		[QueueKey],
		[StageDateTime]
	)
	values
	(
		@version_number,
		getdate()
	);

END
GO
/****** Object:  StoredProcedure [dbo].[Operations_Sync_AlbumTaskMarkAsCompleted]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luis Valle
-- =============================================
create PROCEDURE [dbo].[Operations_Sync_AlbumTaskMarkAsCompleted]
(
	@version_number varchar(50)
)
AS
BEGIN
	set nocount on;
	
	update [arma_devops].dbo.[SyncQueue]
	set
		[AlbumTaskEndDateTime] = getdate()
	where
		[QueueKey] = @version_number;

END
GO
/****** Object:  StoredProcedure [dbo].[Operations_Sync_AlbumTaskMarkAsStarted]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luis Valle
-- =============================================
create PROCEDURE [dbo].[Operations_Sync_AlbumTaskMarkAsStarted]
(
	@version_number varchar(50)
)
AS
BEGIN
	set nocount on;
	
	update [arma_devops].dbo.[SyncQueue]
	set
		[AlbumTaskStartDateTime] = getdate()
	where
		[QueueKey] = @version_number;

END
GO
/****** Object:  StoredProcedure [dbo].[Operations_Sync_ArtistTaskMarkAsCompleted]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luis Valle
-- =============================================
create PROCEDURE [dbo].[Operations_Sync_ArtistTaskMarkAsCompleted]
(
	@version_number varchar(50)
)
AS
BEGIN
	set nocount on;
	
	update [arma_devops].dbo.[SyncQueue]
	set
		[ArtistTaskEndDateTime] = getdate()
	where
		[QueueKey] = @version_number;

END
GO
/****** Object:  StoredProcedure [dbo].[Operations_Sync_ArtistTaskMarkAsStarted]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luis Valle
-- =============================================
create PROCEDURE [dbo].[Operations_Sync_ArtistTaskMarkAsStarted]
(
	@version_number varchar(50)
)
AS
BEGIN
	set nocount on;
	
	update [arma_devops].dbo.[SyncQueue]
	set
		[ArtistTaskStartDateTime] = getdate()
	where
		[QueueKey] = @version_number;

END
GO
/****** Object:  StoredProcedure [dbo].[Operations_Sync_CheckIfVersionHasBeenProcessed]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[Operations_Sync_CheckIfVersionHasBeenProcessed]
(
	@version_number varchar(50)
)
AS
BEGIN
	set nocount on;
	declare
		@hasBeenProcessed bit = 0,
		@firstTimeProcess bit = 0;

	select
		@hasBeenProcessed = 1
	from [arma_devops].dbo.[SyncQueue]
	where
		[QueueKey] = @version_number;

	if (select count(*) from [arma_devops].dbo.[SyncQueue]) = 0
	begin
		set @firstTimeProcess = 1;
	end

	select
		[HasBeenProcessed] = @hasBeenProcessed,
		[FirstTimeProcess] = @firstTimeProcess;

	/*
		exec Operations_Sync_CheckIfVersionHasBeenProcessed
			'20240417-001001'
	*/
END
GO
/****** Object:  StoredProcedure [dbo].[Operations_Sync_GetStartingTime]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[Operations_Sync_GetStartingTime]
AS
BEGIN
	set nocount on;
	declare
		@setting_value varchar(60),
		@start_time datetime;

	select
		@setting_value = [SettingValue]
	from [arma_devops].[dbo].[SyncSettings]
	where
		[SettingName] = 'SyncSartingTime';

	set @start_time = cast((concat((cast((cast(getdate() as date)) as varchar(100))), ' ', @setting_value)) as datetime);

	select
		[start_time] = @start_time;

	/*
		exec Operations_Sync_GetStartingTime
	*/
END
GO
/****** Object:  StoredProcedure [dbo].[Operations_Sync_LogError]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[Operations_Sync_LogError]
(
	@queue_key varchar(50),
	@error_parent varchar(50),
	@error_message varchar(5000),
	@json_source nvarchar(max)
)
AS
BEGIN
	insert into [arma_devops].dbo.[SyncQueueErrorLog]
	(
		[QueueId],
		[ErrorParent],
		[ErrorMessage],
		[JsonSource],
		[EntryDateTime]
	)
	values
	(
		@queue_key,
		@error_parent,
		@error_message,
		@json_source,
		getdate()
	);
END
GO
/****** Object:  StoredProcedure [dbo].[Raw_InsertPlaylist]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[Raw_InsertPlaylist]
(
	@name varchar(255),
	@collaborative bit,
	@modified_at int,
	@num_tracks int,
	@num_albums int,
	@num_followers int,
	@new_id int output
)
as
BEGIN

	insert into Rawlists
	(
		[name],
		[collaborative],
		[modified_at],
		[num_tracks],
		[num_albums],
		[num_followers]
	)
	values
	(
		@name,
		@collaborative,
		@modified_at,
		@num_tracks,
		@num_albums,
		@num_followers
	)

	select @new_id = scope_identity();
END
GO
/****** Object:  StoredProcedure [dbo].[Raw_InsertTrack]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[Raw_InsertTrack]
(
	@pid int,
	@pos int,
	@artist_name varchar(255),
	@track_uri varchar(255),
	@artist_uri varchar(255),
	@track_name varchar(255),
	@album_uri varchar(255),
	@duration_ms int,
	@album_name varchar(255)
)
AS
BEGIN
	insert into RawTracks
	(
		[pid],
		[pos],
		[artist_name],
		[track_uri],
		[artist_uri],
		[track_name],
		[album_uri],
		[duration_ms],
		[album_name]
	)
	values
	(
		@pid,
		@pos,
		@artist_name,
		@track_uri,
		@artist_uri,
		@track_name,
		@album_uri,
		@duration_ms,
		@album_name
	);
END
GO
/****** Object:  StoredProcedure [dbo].[Top100_BackupOldSongs]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[Top100_BackupOldSongs]
(
	@key_id int
)
AS
BEGIN
	set nocount on;

	insert into top100.dbo.Daily_Top100Archived
	(
		[key_id],
		[rank],
		[artist],
		[song]
	)
	select
		[key_id],
		[rank],
		[artist],
		[song]
	from top100.dbo.Daily_Top100
	where
		[key_id] <> @key_id
	order by
		[key_id],
		[rank];

	delete from top100.dbo.Daily_Top100
	where
		[key_id] <> @key_id;

	/*
		exec Top100_BackupOldSongs
			7
	*/
END
GO
/****** Object:  StoredProcedure [dbo].[Top100_GenerateDailyKey]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[Top100_GenerateDailyKey]
AS
BEGIN
	set nocount on;
	declare
		@today date = getdate(),
		@key_id int = null;

	if not exists (
		select
			1
		from top100.dbo.Daily_Keys
		where
			[DateKey] = @today
	)
	begin
		insert into top100.dbo.Daily_Keys
		(
			[DateKey]
		)
		values
		(
			@today
		)
	end

	select
		@key_id = [id]
	from top100.dbo.Daily_Keys
	where
		[DateKey] = @today;

	select
		[key_id] = @key_id;
END
GO
/****** Object:  StoredProcedure [dbo].[Top100_GetDailyKey]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[Top100_GetDailyKey]
AS
BEGIN
	set nocount on;
	declare
		@today date = getdate(),
		@key_id int = null;

	select
		@key_id = [id]
	from top100.dbo.Daily_Keys
	where
		[DateKey] = @today;

	select
		[key_id] = @key_id;

	/*
		exec Top100_GetDailyKey
	*/
END
GO
/****** Object:  StoredProcedure [dbo].[Top100_GetTodaysTopSongs]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[Top100_GetTodaysTopSongs]
(
	@key_id int
)
AS
BEGIN
	set nocount on;

	select
		[tid] = [id],
		[usability_score] = [rank],
		[artist_name] = [artist],
		[track_name] = [song]
	from top100.dbo.Daily_Top100
	where
		[key_id] = @key_id
	order by
		[rank];

	/*
		exec Top100_GetTodaysTopSongs
			1
	*/
END
GO
/****** Object:  StoredProcedure [dbo].[Top100_InsertSongData]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[Top100_InsertSongData]
(
	@key_id int,
	@rank int,
	@artist nvarchar(500) = null,
	@song nvarchar(1000) = null
)
AS
BEGIN
	set nocount on;

	insert into top100.dbo.Daily_Top100
	(
		[key_id],
		[rank],
		[artist],
		[song]
	)
	values
	(
		@key_id,
		@rank,
		@artist,
		@song
	)
END
GO
/****** Object:  StoredProcedure [dbo].[Tracks_GetSongsByArtist]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[Tracks_GetSongsByArtist]
(
	@artist_id int
)
AS
BEGIN
	set nocount on;

	declare
		@artist_uri varchar(255);

	select
		@artist_uri = [artist_uri]
	from Artists a
	where
		a.[id] = @artist_id;

	with all_artists as (
		select
			[artist_uri],
			[artist_name],
			[usability_score] = max([usability_score])
		from Tracks
		where
			[artist_uri] = @artist_uri
		group by
			[artist_uri],
			[artist_name]			
	),
	top_artist as (
		select
			[artist_uri],
			[artist_name],
			[usability_score]
		from all_artists
		group by
			[artist_uri],
			[artist_name],
			[usability_score]
	),
	top_songs as (
		select
			t.[tid],
			t.[artist_name],
			t.[track_uri],
			t.[artist_uri],
			t.[track_name],
			t.[usability_score]
		from top_artist ta
		join Tracks t
			on t.[artist_uri] = ta.[artist_uri]
			and t.[usability_score] = ta.[usability_score]
	)

	select
		t.[tid],
		t.[artist_name],
		t.[track_uri],
		t.[artist_uri],
		t.[track_name],
		t.[usability_score]
	from top_songs t
	order by
		t.[usability_score] desc;

	/*
		exec Tracks_GetSongsByArtist
			24940
	*/
END
GO
/****** Object:  StoredProcedure [dbo].[Tracks_GetTop50UserFavorites]    Script Date: 11/21/2025 9:18:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE PROCEDURE [dbo].[Tracks_GetTop50UserFavorites]
AS
BEGIN
	set nocount on;

	with all_artists as (
		select
			[artist_uri],
			[artist_name],
			[usability_score] = max([usability_score])
		from Tracks
		group by
			[artist_uri],
			[artist_name]
	),
	top_artist as (
		select top 50
			[artist_uri],
			[artist_name],
			[usability_score]
		from all_artists
		group by
			[artist_uri],
			[artist_name],
			[usability_score]
		order by
			[usability_score] desc
	),
	top_songs as (
		select
			t.[tid],
			t.[artist_name],
			t.[track_uri],
			t.[artist_uri],
			t.[track_name],
			t.[usability_score]
		from top_artist ta
		join Tracks t
			on t.[artist_uri] = ta.[artist_uri]
			and t.[usability_score] = ta.[usability_score]
	)

	select
		t.[tid],
		t.[artist_name],
		t.[track_uri],
		t.[artist_uri],
		t.[track_name],
		t.[usability_score]
	from top_songs t
	order by
		t.[usability_score] desc;

	/*
		exec Tracks_GetTop50UserFavorites
	*/
END
GO
