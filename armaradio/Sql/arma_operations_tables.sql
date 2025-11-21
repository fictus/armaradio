USE [arma_operations]
GO
/****** Object:  Table [dbo].[ArmaErrors]    Script Date: 11/21/2025 9:22:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ArmaErrors](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ErrorController] [varchar](150) NOT NULL,
	[ErrorMethod] [varchar](150) NULL,
	[ErrorMessage] [varchar](5000) NOT NULL,
	[DateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_ArmaErrors] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ArmaUserOperations]    Script Date: 11/21/2025 9:22:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ArmaUserOperations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Timestamp] [datetime] NOT NULL,
	[IpAddress] [varchar](50) NULL,
	[UserId] [varchar](50) NULL,
	[UserName] [varchar](150) NULL,
	[RequestPath] [varchar](1000) NULL,
	[RequestMethod] [varchar](50) NULL,
	[UserAgent] [varchar](700) NULL,
	[Referrer] [varchar](1000) NULL,
	[RequestHeaders] [varchar](max) NULL,
	[QueryString] [varchar](1500) NULL,
	[FromApi] [bit] NULL,
	[RequestBody] [nvarchar](max) NULL,
	[Duration] [float] NULL,
 CONSTRAINT [PK_ArmaUserOperations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[ArmaErrors] ADD  CONSTRAINT [DF_ArmaErrors_DateTime]  DEFAULT (getdate()) FOR [DateTime]
GO
