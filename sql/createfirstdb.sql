DROP DATABASE IF EXISTS AirlineWebDB
GO

CREATE DATABASE AirlineWebDB
PRINT 'AirlineWebDB created'
GO

USE AirlineWebDB
GO
CREATE TABLE dbo.WebhookSubscriptions
(
	Id INT PRIMARY KEY NOT NULL,
	WebhookUri NVARCHAR(200) NOT NULL,
	[Secret] NVARCHAR(100) NOT NULL,
	WebhookType NVARCHAR(100) NOT NULL,
	WebhookPublisher NVARCHAR(200) NOT NULL
)
PRINT 'Creating WebhookSubscriptions table'
GO