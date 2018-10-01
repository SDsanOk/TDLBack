USE [toDo]
GO

/****** Object: Table [dbo].[TDListTDEvent] Script Date: 01/10/2018 14:25:06 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TDListTDEvent] (
    [Id]        INT IDENTITY (1, 1) NOT NULL,
    [TDListId]  INT NOT NULL,
    [TDEventId] INT NOT NULL
);


