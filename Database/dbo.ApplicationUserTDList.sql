USE [toDo]
GO

/****** Object: Table [dbo].[ApplicationUserTDList] Script Date: 01/10/2018 14:25:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ApplicationUserBoard] (
    [Id]                INT IDENTITY (1, 1) NOT NULL,
    [ApplicationUserId] INT NOT NULL,
    [BoardId]          INT NOT NULL
);


