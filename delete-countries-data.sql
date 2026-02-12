-- Delete all Countries data to resolve duplicate issues
-- WARNING: This will delete ALL countries and related data. Backup data if needed.

SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;
GO

-- Clear all foreign key references first
UPDATE [Patients] SET CityId = NULL WHERE CityId IS NOT NULL;
UPDATE [Patients] SET CountryId = NULL WHERE CountryId IS NOT NULL;
GO

-- Delete Cities (has FK to Countries)
DELETE FROM [Cities];
GO

-- Now delete Countries
DELETE FROM [Countries];
GO

-- Reset identity seeds
DBCC CHECKIDENT ('[Cities]', RESEED, 0);
DBCC CHECKIDENT ('[Countries]', RESEED, 0);
GO

SELECT 'All Countries and Cities data deleted successfully' AS Result;
GO
