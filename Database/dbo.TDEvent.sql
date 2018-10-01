USE [toDo]
GO

/****** Object: Table [dbo].[TDEvent] Script Date: 01/10/2018 14:25:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TDEvent] (
    [Id]    INT            IDENTITY (1, 1) NOT NULL,
	[ListId] INT not null,
    [Title] NVARCHAR (256) NOT NULL,
    [Done]  BIT            NOT NULL
);


