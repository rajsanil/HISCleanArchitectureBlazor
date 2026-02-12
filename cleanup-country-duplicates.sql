-- Step 1: Find all duplicate Country names
WITH DuplicateCountries AS (
    SELECT 
        Name,
        ROW_NUMBER() OVER (PARTITION BY Name ORDER BY Id) AS RowNum,
        Id, Code, NameArabic, Iso2Code, Iso3Code, PhoneCode, IsActive, Created
    FROM Countries
    WHERE Deleted IS NULL
)
SELECT * FROM DuplicateCountries WHERE RowNum > 1;

-- Step 2: DELETE duplicates (keep the first one by Id)
-- UNCOMMENT THE BELOW TO DELETE DUPLICATES
/*
WITH DuplicateCountries AS (
    SELECT 
        Id,
        ROW_NUMBER() OVER (PARTITION BY Name ORDER BY Id) AS RowNum
    FROM Countries
    WHERE Deleted IS NULL
)
DELETE FROM Countries
WHERE Id IN (
    SELECT Id FROM DuplicateCountries WHERE RowNum > 1
);
*/

-- Step 3: Verify no duplicates remain
SELECT Name, COUNT(*) as Count
FROM Countries
WHERE Deleted IS NULL
GROUP BY Name
HAVING COUNT(*) > 1;
