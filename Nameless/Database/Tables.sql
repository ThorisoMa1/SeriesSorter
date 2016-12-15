USE [NoName]
GO

/****** Object:  Table [dbo].[tbl_Formats]    Script Date: 15/12/2016 13:41:53 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tbl_Formats](
	[ID] [int] NOT NULL,
	[Formats] [text] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

CREATE TABLE [dbo].[tbl_NamingConvention](
	[ID] [nchar](10) NOT NULL,
	[Format] [text] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
USE [NoName]
GO



CREATE TABLE [dbo].[tbl_WatchedFolders](
	[ID] [numeric](18, 0) NOT NULL,
	[Name] [varchar](30) NOT NULL,
	[NamingConvention] [numeric](18, 0) NOT NULL,
	[InnerName] [varchar](30) NOT NULL,
	[EpisodeCount] [numeric](18, 0) NOT NULL,
	[LastEp] [numeric](18, 0) NOT NULL,
	[Path] [varchar](30) NOT NULL,
	[MaxSeason] [numeric](18, 0) NOT NULL,
	[MissonEpisode] [varchar](30) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO



