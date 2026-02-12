-- Complete cleanup script for all MasterData duplicates

PRINT '=== FINDING ALL DUPLICATES ==='

-- 1. Country duplicates
PRINT 'Country duplicates:'
SELECT 'Country' AS Entity, Name, COUNT(*) as Count
FROM Countries
WHERE Deleted IS NULL
GROUP BY Name
HAVING COUNT(*) > 1;

-- 2. BloodGroup duplicates
PRINT 'BloodGroup duplicates:'
SELECT 'BloodGroup' AS Entity, Name, COUNT(*) as Count
FROM BloodGroups
WHERE Deleted IS NULL
GROUP BY Name
HAVING COUNT(*) > 1;

-- 3. MaritalStatus duplicates
PRINT 'MaritalStatus duplicates:'
SELECT 'MaritalStatus' AS Entity, Name, COUNT(*) as Count
FROM MaritalStatuses
WHERE Deleted IS NULL
GROUP BY Name
HAVING COUNT(*) > 1;

-- 4. Nationality duplicates
PRINT 'Nationality duplicates:'
SELECT 'Nationality' AS Entity, Name, COUNT(*) as Count
FROM Nationalities
WHERE Deleted IS NULL
GROUP BY Name
HAVING COUNT(*) > 1;

-- 5. Specialty duplicates
PRINT 'Specialty duplicates:'
SELECT 'Specialty' AS Entity, Name, COUNT(*) as Count
FROM Specialties
WHERE Deleted IS NULL
GROUP BY Name
HAVING COUNT(*) > 1;

-- 6. City duplicates (per country)
PRINT 'City duplicates (per country):'
SELECT 'City' AS Entity, Name, CountryId, COUNT(*) as Count
FROM Cities
WHERE Deleted IS NULL
GROUP BY Name, CountryId
HAVING COUNT(*) > 1;

-- 7. Department duplicates (per facility)
PRINT 'Department duplicates (per facility):'
SELECT 'Department' AS Entity, Name, FacilityId, COUNT(*) as Count
FROM Departments
WHERE Deleted IS NULL
GROUP BY Name, FacilityId
HAVING COUNT(*) > 1;

-- 8. Location duplicates (per facility)
PRINT 'Location duplicates (per facility):'
SELECT 'Location' AS Entity, Name, FacilityId, COUNT(*) as Count
FROM Locations
WHERE Deleted IS NULL
GROUP BY Name, FacilityId
HAVING COUNT(*) > 1;

PRINT ''
PRINT '=== CLEANUP COMMANDS (UNCOMMENT TO EXECUTE) ==='
PRINT ''

-- UNCOMMENT TO DELETE DUPLICATES (keeps first record by Id)
/*
-- Delete duplicate Countries
WITH DuplicateCountries AS (
    SELECT Id, ROW_NUMBER() OVER (PARTITION BY Name ORDER BY Id) AS RowNum
    FROM Countries WHERE Deleted IS NULL
)
DELETE FROM Countries WHERE Id IN (SELECT Id FROM DuplicateCountries WHERE RowNum > 1);

-- Delete duplicate BloodGroups
WITH DuplicateBloodGroups AS (
    SELECT Id, ROW_NUMBER() OVER (PARTITION BY Name ORDER BY Id) AS RowNum
    FROM BloodGroups WHERE Deleted IS NULL
)
DELETE FROM BloodGroups WHERE Id IN (SELECT Id FROM DuplicateBloodGroups WHERE RowNum > 1);

-- Delete duplicate MaritalStatuses
WITH DuplicateMaritalStatuses AS (
    SELECT Id, ROW_NUMBER() OVER (PARTITION BY Name ORDER BY Id) AS RowNum
    FROM MaritalStatuses WHERE Deleted IS NULL
)
DELETE FROM MaritalStatuses WHERE Id IN (SELECT Id FROM DuplicateMaritalStatuses WHERE RowNum > 1);

-- Delete duplicate Nationalities
WITH DuplicateNationalities AS (
    SELECT Id, ROW_NUMBER() OVER (PARTITION BY Name ORDER BY Id) AS RowNum
    FROM Nationalities WHERE Deleted IS NULL
)
DELETE FROM Nationalities WHERE Id IN (SELECT Id FROM DuplicateNationalities WHERE RowNum > 1);

-- Delete duplicate Specialties
WITH DuplicateSpecialties AS (
    SELECT Id, ROW_NUMBER() OVER (PARTITION BY Name ORDER BY Id) AS RowNum
    FROM Specialties WHERE Deleted IS NULL
)
DELETE FROM Specialties WHERE Id IN (SELECT Id FROM DuplicateSpecialties WHERE RowNum > 1);

-- Delete duplicate Cities
WITH DuplicateCities AS (
    SELECT Id, ROW_NUMBER() OVER (PARTITION BY Name, CountryId ORDER BY Id) AS RowNum
    FROM Cities WHERE Deleted IS NULL
)
DELETE FROM Cities WHERE Id IN (SELECT Id FROM DuplicateCities WHERE RowNum > 1);

-- Delete duplicate Departments
WITH DuplicateDepartments AS (
    SELECT Id, ROW_NUMBER() OVER (PARTITION BY Name, FacilityId ORDER BY Id) AS RowNum
    FROM Departments WHERE Deleted IS NULL
)
DELETE FROM Departments WHERE Id IN (SELECT Id FROM DuplicateDepartments WHERE RowNum > 1);

-- Delete duplicate Locations
WITH DuplicateLocations AS (
    SELECT Id, ROW_NUMBER() OVER (PARTITION BY Name, FacilityId ORDER BY Id) AS RowNum
    FROM Locations WHERE Deleted IS NULL
)
DELETE FROM Locations WHERE Id IN (SELECT Id FROM DuplicateLocations WHERE RowNum > 1);

PRINT 'Duplicates cleaned up successfully!'
*/
