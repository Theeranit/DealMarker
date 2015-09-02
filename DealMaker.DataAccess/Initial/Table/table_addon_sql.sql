USE [KKLMDB]
GO

/****** Object:  Table [dbo].[DA_TRN_CASHFLOW_TEMP]    Script Date: 10/04/2013 13:47:55 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DA_TRN_CASHFLOW_TEMP]') AND type in (N'U'))
DROP TABLE [dbo].[DA_TRN_CASHFLOW_TEMP]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MA_SPOT_RATE_TEMP]') AND type in (N'U'))
DROP TABLE [dbo].[MA_SPOT_RATE_TEMP]
GO

/****** Object:  Table [dbo].[DA_TRN_CASHFLOW_TEMP]    Script Date: 10/04/2013 13:47:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DA_TRN_CASHFLOW_TEMP](
	[ENGINE_DATE] [date] NULL,
	[EXT_DEAL_NO] [nvarchar](8) NULL,
	[SEQ] [int] NULL,
	[RATE] [decimal](18, 6) NULL,
	[FLOW_DATE] [datetime] NULL,
	[FLOW_AMOUNT] [decimal](18, 4) NULL,
	[FLAG_FIRST] [bit]  NULL,
	[LOG_INSERTBYUSERID] [uniqueidentifier] NULL,
	[LOG_INSERTDATE] [datetime] NULL,
	[LOG_MODIFYBYUSERID] [uniqueidentifier] NULL,
	[LOG_MODIFYDATE] [datetime] NULL,
	[FLOW_AMOUNT_THB] [decimal](18, 6) NULL
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[MA_SPOT_RATE_TEMP](
	[BR] [nvarchar](100) NOT NULL,
	[CURRENCY] [nchar](3) NOT NULL,
	[RATE] [decimal](18, 8) NOT NULL
) ON [PRIMARY]

GO
