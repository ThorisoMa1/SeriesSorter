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
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

CREATE PROCEDURE sp_SetInnerName
	-- Add the parameters for the stored procedure here
	@name varchar(30) ,
    @newName varchar(30) 
AS
BEGIN

	Update tbl_WatchedFolders
	SET  innerName= @newName 
	WHERE Name=@name;

END
GO

CREATE PROCEDURE sp_ExistsCheck
	-- Add the parameters for the stored procedure here
	@name varchar(30) 
   
AS
BEGIN

	SELECT  Name 
	From tbl_watchedFolders
	WHERE Name=@name;

END
GO

CREATE PROCEDURE sp_insertFolder
	-- Add the parameters for the stored procedure here
	@name varchar(30) ,
	@namingConvention int,	
	@eCount int, 
	@lastEp varchar(30),
	@lastSeason varchar(30),
	@path varchar(30),
	@missinEp varchar(30)
	
   
AS

BEGIN

	          INSERT INTO   tbl_WatchedFolders
			  (Name,NamingConvention,innerName,EpisodeCount,LastEp,path,MaxSeason,MissonEpisode) 
			   VALUES(@name,@namingConvention,@name,@eCount,@lastEp,@path,@lastSeason,@missinEp)  


END
GO
