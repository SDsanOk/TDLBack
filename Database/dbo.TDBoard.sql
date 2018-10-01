USE [toDo]
GO

/****** Object: Table [dbo].[TDBoard] Script Date: 01/10/2018 14:25:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TDBoard] (
    [Id]    INT            IDENTITY (1, 1) NOT NULL,
    [Title] NVARCHAR (256) NOT NULL
);


