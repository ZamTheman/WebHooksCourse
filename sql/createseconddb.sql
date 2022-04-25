DROP DATABASE IF EXISTS TravelAgentWebDB
GO

CREATE DATABASE TravelAgentWebDB
PRINT 'TravelAgentWebDB created'
GO

USE TravelAgentWebDB
GO

CREATE TABLE dbo.SubscriptionSecrets
(
	Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[Secret] NVARCHAR(50) NOT NULL,
    Publisher NVARCHAR(100) NOT NULL	
)
PRINT 'Creating SubscriptionSecrets table'
GO