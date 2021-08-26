CREATE TABLE [dbo].[Attendance] (
    [Id]         INT IDENTITY (1, 1) NOT NULL,
    [ItemId]     INT NOT NULL,
    [AttendeeId] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([AttendeeId]) REFERENCES [dbo].[Attendee] ([Id]),
    FOREIGN KEY ([ItemId]) REFERENCES [dbo].[Item] ([Id])
);

