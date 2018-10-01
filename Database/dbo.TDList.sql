USE [toDo]
GO

/****** Object: Table [dbo].[TDList] Script Date: 01/10/2018 14:25:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TDList] (
    [Id]    INT            IDENTITY (1, 1) NOT NULL,
	[BoardId] INT NOT NULL,
    [Title] NVARCHAR (256) NOT NULL
);


