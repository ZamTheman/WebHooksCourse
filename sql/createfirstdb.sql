DROP DATABASE IF EXISTS AirlineWebDB
GO

CREATE DATABASE AirlineWebDB
PRINT 'AirlineWebDB created'
GO

USE AirlineWebDB
GO

CREATE TABLE dbo.WebhookSubscriptions
(
	Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	WebhookUri NVARCHAR(200) NOT NULL,
	[Secret] NVARCHAR(100) NOT NULL,
	WebhookType NVARCHAR(100) NOT NULL,
	WebhookPublisher NVARCHAR(200) NOT NULL
)
PRINT 'Creating WebhookSubscriptions table'
GO

CREATE TABLE dbo.FlightDetails
(
	Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	FlightCode NVARCHAR(10) NOT NULL,
	Price DECIMAL(19,4) NOT NULL
)
PRINT 'Creating FlightDetail table'
GO

