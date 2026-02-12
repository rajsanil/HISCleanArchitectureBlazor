-- Check for duplicate Country names
SELECT Name, COUNT(*) as Count
FROM Countries
WHERE Deleted IS NULL
GROUP BY Name
HAVING COUNT(*) > 1;

-- Check for duplicate BloodGroup names
SELECT Name, COUNT(*) as Count
FROM BloodGroups
WHERE Deleted IS NULL
GROUP BY Name
HAVING COUNT(*) > 1;

-- Check for duplicate MaritalStatus names
SELECT Name, COUNT(*) as Count
FROM MaritalStatuses
WHERE Deleted IS NULL
GROUP BY Name
HAVING COUNT(*) > 1;

-- Check for duplicate Nationality names
SELECT Name, COUNT(*) as Count
FROM Nationalities
WHERE Deleted IS NULL
GROUP BY Name
HAVING COUNT(*) > 1;

-- Check for duplicate Specialty names
SELECT Name, COUNT(*) as Count
FROM Specialties
WHERE Deleted IS NULL
GROUP BY Name
HAVING COUNT(*) > 1;

-- Check for duplicate City names per country
SELECT Name, CountryId, COUNT(*) as Count
FROM Cities
WHERE Deleted IS NULL
GROUP BY Name, CountryId
HAVING COUNT(*) > 1;

-- Check for duplicate Department names per facility
SELECT Name, FacilityId, COUNT(*) as Count
FROM Departments
WHERE Deleted IS NULL
GROUP BY Name, FacilityId
HAVING COUNT(*) > 1;

-- Check for duplicate Location names per facility
SELECT Name, FacilityId, COUNT(*) as Count
FROM Locations
WHERE Deleted IS NULL
GROUP BY Name, FacilityId
HAVING COUNT(*) > 1;
