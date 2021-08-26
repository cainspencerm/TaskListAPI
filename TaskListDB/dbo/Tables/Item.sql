CREATE TABLE [dbo].[Item] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [ListId]      INT            NOT NULL,
    [Name]        VARCHAR (50)   NOT NULL,
    [Description] VARCHAR (1000) NOT NULL,
    [Priority]    INT            NOT NULL,
    [Start] DATETIME2 NULL, 
    [Stop] DATETIME2 NULL, 
    [Deadline] DATETIME2 NULL, 
    [IsComplete] INT NULL, 
    [Discriminator] VARCHAR(20) NOT NULL, 
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([ListId]) REFERENCES [dbo].[TaskList] ([Id])
);


