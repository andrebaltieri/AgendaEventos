-- Based on https://www.rfc-editor.org/errata_search.php?rfc=3696

USE [Agenda]
GO

ALTER TABLE [User]
	ALTER COLUMN [Email] VARCHAR(254) NOT NULL;

GO