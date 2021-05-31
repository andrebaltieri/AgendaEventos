CREATE DATABASE [Agenda]
GO

USE [Agenda]
GO

--Admin, Organizer, Speaker and Attendee?
CREATE TABLE [Role]
(
	[Id] INT NOT NULL IDENTITY(1, 1),
	[Title] VARCHAR(50) NOT NULL, 		
	[Description] VARCHAR(500) NOT NULL,
	[CreatedDate] DATETIME NOT NULL DEFAULT(GETDATE()),
	[LastUpdatedDate] DATETIME NULL,
	CONSTRAINT [PK_Role] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [User]
(
	[Id] INT NOT NULL IDENTITY(1, 1),
	[Name] VARCHAR(100) NOT NULL,
	[Email] VARCHAR(60) NOT NULL,
	[Password] VARCHAR(20) NOT NULL,
	[CreatedDate] DATETIME NOT NULL DEFAULT(GETDATE()),
    [LastUpdatedDate] DATETIME NULL,
	CONSTRAINT [PK_User] PRIMARY KEY ([Id]),
	CONSTRAINT [UK_User_Email] UNIQUE ([Email])
);
GO

CREATE TABLE [UserRoles]
(
	[UserId] INT NOT NULL,
	[RoleId] INT NOT NULL,
	CONSTRAINT [PK_UserRoles] PRIMARY KEY ([UserId], [RoleId]),
	CONSTRAINT [FK_UserRoles_User_UserId] FOREIGN KEY ([UserId]) REFERENCES [User] ([Id]),
	CONSTRAINT [FK_UserRoles_Role_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Role] ([Id])
);
GO

--Backend, Frontend, Mobile, etc?
CREATE TABLE [Category]
(
	[Id] INT NOT NULL IDENTITY(1,1),
	[Title] VARCHAR(80) NOT NULL,
	[Description] VARCHAR(500) NOT NULL,
	[Active] BIT NOT NULL,
	[CreatedDate] DATETIME NOT NULL DEFAULT(GETDATE()),
    [LastUpdatedDate] DATETIME NULL,
	CONSTRAINT [PK_Category] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Event]
(
	[Id] INT NOT NULL IDENTITY(1,1),
	[Title] VARCHAR(100) NOT NULL,
	[SpeakerId] INT NOT NULL,
	[Description] TEXT NOT NULL,
	[StartDate] DATETIME NOT NULL,
	[DurationInMinutes] INT NOT NULL,
	[EnrollmentDeadlineDate] DATETIME NOT NULL,
	[UrlSegment] VARCHAR(1024) NOT NULL, 	
	[BannerUrl] VARCHAR(2000) NOT NULL, 
	[Active] BIT NOT NULL DEFAULT (0),
	[OrganizerId] INT NOT NULL,
	[CategoryId] INT NOT NULL,
	[CreatedDate] DATETIME NOT NULL DEFAULT(GETDATE()),
    [LastUpdatedDate] DATETIME NULL,
	CONSTRAINT [PK_Event] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_Event_Speaker_UserId] FOREIGN KEY ([SpeakerId]) REFERENCES [User] ([Id]),
	CONSTRAINT [FK_Event_Organizer_UserId] FOREIGN KEY ([OrganizerId]) REFERENCES [User] ([Id]),
	CONSTRAINT [FK_Event_Category_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [Category] ([Id])
);
GO

--Do we need to create indexes on tables Category and User? 
--Apparently, columns set as primary keys and/or unique identifiers 
--already have their indexes originally created 
CREATE INDEX [IX_Event_UrlSegment] ON [Event]([UrlSegment]);

CREATE TABLE [AttendeesEvents] 
(
	[AttendeeId] INT NOT NULL,
	[EventId] INT NOT NULL,
	CONSTRAINT [PK_AttendeesEvents] PRIMARY KEY ([AttendeeId], [EventId]),
	CONSTRAINT [FK_AttendeesEvents_Attendee] FOREIGN KEY ([AttendeeId]) REFERENCES [User] ([Id]),
	CONSTRAINT [FK_AttendeesEvents_Event] FOREIGN KEY ([EventId]) REFERENCES [Event] ([Id])
);
GO


--Create initial data on tables Role, User, UserRoles and Category? 


/*
	--Making sure database Agenda is deleted in case of script execution error:
	USE [master];
	DECLARE @kill varchar(8000) = '';
	SELECT @kill = @kill + 'kill ' + CONVERT(varchar(5), session_id) + ';'
	FROM sys.dm_exec_sessions
	WHERE database_id = db_id('Agenda')
	EXEC(@kill);

	GO

	DROP DATABASE [Agenda];
*/
