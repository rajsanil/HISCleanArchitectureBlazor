-- Check if favorites exist for user test@csh.ae
-- First, find the user ID
SELECT Id, UserName, Email 
FROM AspNetUsers 
WHERE Email = 'test@csh.ae' OR UserName = 'test@csh.ae';

-- Check UserFavorites table for this user
SELECT uf.*, u.UserName, u.Email
FROM UserFavorites uf
INNER JOIN AspNetUsers u ON uf.UserId = u.Id
WHERE u.Email = 'test@csh.ae' OR u.UserName = 'test@csh.ae'
ORDER BY uf.Created DESC;

-- Count of favorites
SELECT COUNT(*) as TotalFavorites
FROM UserFavorites uf
INNER JOIN AspNetUsers u ON uf.UserId = u.Id
WHERE u.Email = 'test@csh.ae' OR u.UserName = 'test@csh.ae';

-- Check all UserFavorites to see structure
SELECT TOP 10 * FROM UserFavorites ORDER BY Created DESC;
