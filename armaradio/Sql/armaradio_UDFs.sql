USE [armaradio]
GO
/****** Object:  UserDefinedFunction [dbo].[ArmaFunction_GetArtistByMBIds]    Script Date: 11/21/2025 9:19:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE FUNCTION [dbo].[ArmaFunction_GetArtistByMBIds]
(	
	@mb_ids varchar(max)
)
returns @results_tmp table
(
	[artist_mbid] varchar(50),
	[Name] nvarchar(350)
)	
AS
begin 
	declare @temp_values table
	(
		[artist_mbid] varchar(50)
	)
	
	insert into @temp_values
	(
		[artist_mbid]
	)
	select [Token] from dbo.SplitStringToTableVarchar(@mb_ids, ',');

	insert into @results_tmp
	select
		am.[MBId],
		am.[Name]
	from @temp_values ai
	join [arma_artists].dbo.[ArmaArtistsMain] am
		on am.[MBId] = [artist_mbid];
	
	return;

	/*
		select * from dbo.ArmaFunction_GetArtistByMBIds('4A069029-4F64-4946-B650-01AEB0B55D9D,5db9f569-cadd-4f8b-b460-d4031b0b3716');
	*/
end
GO
/****** Object:  UserDefinedFunction [dbo].[LastIndexOf]    Script Date: 11/21/2025 9:19:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[LastIndexOf](@source varchar(max), @pattern char)
RETURNS int
BEGIN  
    return (len(@source)) - charindex(@pattern, reverse(@source)) + 1;

	/*
		 select dbo.LastIndexOf('one, the', ',');
	*/
END;  
GO
/****** Object:  UserDefinedFunction [dbo].[NumberOfArtistAlbums]    Script Date: 11/21/2025 9:19:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[NumberOfArtistAlbums]
(
	@artist_id int,
	@db_source varchar(15)
)
RETURNS int
BEGIN  
	declare
		@return_count int = 0;

	if @db_source = 'a'
	begin
		select
			@return_count = count(*)
		from [arma_a].dbo.[AlbumMapper]
		where
			[ArtistId] = @artist_id;
	end
	else if @db_source = 'b'
	begin
		select
			@return_count = count(*)
		from [arma_b].dbo.[AlbumMapper]
		where
			[ArtistId] = @artist_id;
	end
	else if @db_source = 'c'
	begin
		select
			@return_count = count(*)
		from [arma_c].dbo.[AlbumMapper]
		where
			[ArtistId] = @artist_id;
	end
	else if @db_source = 'd'
	begin
		select
			@return_count = count(*)
		from [arma_d].dbo.[AlbumMapper]
		where
			[ArtistId] = @artist_id;
	end
	else if @db_source = 'e'
	begin
		select
			@return_count = count(*)
		from [arma_e].dbo.[AlbumMapper]
		where
			[ArtistId] = @artist_id;
	end
	else if @db_source = 'f'
	begin
		select
			@return_count = count(*)
		from [arma_f].dbo.[AlbumMapper]
		where
			[ArtistId] = @artist_id;
	end
	else if @db_source = 'g'
	begin
		select
			@return_count = count(*)
		from [arma_g].dbo.[AlbumMapper]
		where
			[ArtistId] = @artist_id;
	end
	else if @db_source = 'h'
	begin
		select
			@return_count = count(*)
		from [arma_h].dbo.[AlbumMapper]
		where
			[ArtistId] = @artist_id;
	end
	else if @db_source = 'i'
	begin
		select
			@return_count = count(*)
		from [arma_i].dbo.[AlbumMapper]
		where
			[ArtistId] = @artist_id;
	end
	else if @db_source = 'j'
	begin
		select
			@return_count = count(*)
		from [arma_j].dbo.[AlbumMapper]
		where
			[ArtistId] = @artist_id;
	end
	else if @db_source = 'k'
	begin
		select
			@return_count = count(*)
		from [arma_k].dbo.[AlbumMapper]
		where
			[ArtistId] = @artist_id;
	end
	else if @db_source = 'l'
	begin
		select
			@return_count = count(*)
		from [arma_l].dbo.[AlbumMapper]
		where
			[ArtistId] = @artist_id;
	end
	else if @db_source = 'm'
	begin
		select
			@return_count = count(*)
		from [arma_m].dbo.[AlbumMapper]
		where
			[ArtistId] = @artist_id;
	end
	else if @db_source = 'n'
	begin
		select
			@return_count = count(*)
		from [arma_n].dbo.[AlbumMapper]
		where
			[ArtistId] = @artist_id;
	end
	else if @db_source = 'o'
	begin
		select
			@return_count = count(*)
		from [arma_o].dbo.[AlbumMapper]
		where
			[ArtistId] = @artist_id;
	end
	else if @db_source = 'p'
	begin
		select
			@return_count = count(*)
		from [arma_p].dbo.[AlbumMapper]
		where
			[ArtistId] = @artist_id;
	end
	else if @db_source = 'q'
	begin
		select
			@return_count = count(*)
		from [arma_q].dbo.[AlbumMapper]
		where
			[ArtistId] = @artist_id;
	end
	else if @db_source = 'r'
	begin
		select
			@return_count = count(*)
		from [arma_r].dbo.[AlbumMapper]
		where
			[ArtistId] = @artist_id;
	end
	else if @db_source = 's'
	begin
		select
			@return_count = count(*)
		from [arma_s].dbo.[AlbumMapper]
		where
			[ArtistId] = @artist_id;
	end
	else if @db_source = 't'
	begin
		select
			@return_count = count(*)
		from [arma_t].dbo.[AlbumMapper]
		where
			[ArtistId] = @artist_id;
	end
	else if @db_source = 'u'
	begin
		select
			@return_count = count(*)
		from [arma_u].dbo.[AlbumMapper]
		where
			[ArtistId] = @artist_id;
	end
	else if @db_source = 'v'
	begin
		select
			@return_count = count(*)
		from [arma_v].dbo.[AlbumMapper]
		where
			[ArtistId] = @artist_id;
	end
	else if @db_source = 'w'
	begin
		select
			@return_count = count(*)
		from [arma_w].dbo.[AlbumMapper]
		where
			[ArtistId] = @artist_id;
	end
	else if @db_source = 'x'
	begin
		select
			@return_count = count(*)
		from [arma_x].dbo.[AlbumMapper]
		where
			[ArtistId] = @artist_id;
	end
	else if @db_source = 'y'
	begin
		select
			@return_count = count(*)
		from [arma_y].dbo.[AlbumMapper]
		where
			[ArtistId] = @artist_id;
	end
	else if @db_source = 'z'
	begin
		select
			@return_count = count(*)
		from [arma_z].dbo.[AlbumMapper]
		where
			[ArtistId] = @artist_id;
	end
	else if @db_source = 'num'
	begin
		select
			@return_count = count(*)
		from [arma_num].dbo.[AlbumMapper]
		where
			[ArtistId] = @artist_id;
	end
	else if @db_source = 'symb'
	begin
		select
			@return_count = count(*)
		from [arma_symb].dbo.[AlbumMapper]
		where
			[ArtistId] = @artist_id;
	end
	
	return @return_count;

	/*
		 select dbo.NumberOfArtistAlbums(1132, 'n');
	*/
END;  
GO
/****** Object:  UserDefinedFunction [dbo].[SplitStringToTableVarchar]    Script Date: 11/21/2025 9:19:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Luis Valle
-- =============================================
CREATE FUNCTION [dbo].[SplitStringToTableVarchar]
(
	@String varchar(max),   
    @Delimiter varchar(10) = ','   
)
returns @Output table   
(   
    InitialString varchar(max),  
    Token varchar(max)   
)   
as   
begin   
  
    if (len(@String) = 0)   
        return   
         
    declare @currentPos int   
    declare @nextPos int   
      
    set @currentPos = 1   
    set @nextPos = charindex(@Delimiter, @String, @currentPos + 1)   
    
    while (@NextPos > 0)   
    begin   
        insert @Output   
        select @string, substring(@String, @currentPos, @nextPos - @CurrentPos)   
              
        set @currentPos = @nextPos + len(@Delimiter)
        set @nextPos = charindex(@Delimiter, @String, @currentPos + 1)   
    end   
              
    insert @Output   
    select @string, substring(@String, @currentPos, len(@String) + 1 - @currentPos)   
              
    return   
      
	/*
		select * from dbo.SplitStringToTableVarchar('MM724353,MM9868533', ',');
	*/
end  
GO
