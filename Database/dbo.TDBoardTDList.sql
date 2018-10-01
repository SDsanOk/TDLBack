USE [toDo]
GO

/****** Object: Table [dbo].[TDBoardTDList] Script Date: 01/10/2018 14:25:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BoardList] (
    [Id]        INT IDENTITY (1, 1) NOT NULL,
    [BoardId] INT NOT NULL,
    [TDListId]  INT NOT NULL
);


