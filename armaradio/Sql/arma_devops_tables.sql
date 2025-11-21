USE [arma_devops]
GO
/****** Object:  Table [dbo].[ApiTokens]    Script Date: 11/21/2025 9:21:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApiTokens](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Token] [varchar](500) NOT NULL,
	[AssignedDateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_ApiTokens] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SyncQueue]    Script Date: 11/21/2025 9:21:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SyncQueue](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[QueueKey] [varchar](30) NOT NULL,
	[StageDateTime] [datetime] NULL,
	[ArtistTaskStartDateTime] [datetime] NULL,
	[ArtistTaskEndDateTime] [datetime] NULL,
	[AlbumTaskStartDateTime] [datetime] NULL,
	[AlbumTaskEndDateTime] [datetime] NULL,
	[CompletedDateTime] [datetime] NULL,
	[IsCompleted] [bit] NULL,
	[ErrorsOcurred] [bit] NULL,
	[Artists] [nchar](10) NULL,
 CONSTRAINT [PK_SyncQueue] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SyncQueueErrorLog]    Script Date: 11/21/2025 9:21:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SyncQueueErrorLog](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[QueueId] [varchar](50) NOT NULL,
	[ErrorParent] [varchar](50) NOT NULL,
	[ErrorMessage] [varchar](5000) NULL,
	[JsonSource] [nvarchar](max) NULL,
	[EntryDateTime] [datetime] NULL,
 CONSTRAINT [PK_SyncQueueErrorLog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SyncSettings]    Script Date: 11/21/2025 9:21:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SyncSettings](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SettingName] [varchar](50) NOT NULL,
	[SettingValue] [varchar](3000) NOT NULL,
 CONSTRAINT [PK_SyncSettings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ApiTokens] ADD  CONSTRAINT [DF_ApiTokens_AssignedDateTime]  DEFAULT (getdate()) FOR [AssignedDateTime]
GO
