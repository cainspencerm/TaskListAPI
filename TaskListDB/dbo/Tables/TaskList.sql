CREATE TABLE [dbo].[TaskList] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Name]        VARCHAR (50)   NOT NULL,
    [Description] VARCHAR (1000) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

