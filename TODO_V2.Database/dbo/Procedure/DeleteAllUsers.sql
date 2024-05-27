USE [TODO_V2]
GO

DELETE FROM UserCredentials;
GO

DELETE FROM Tasks;
DBCC CHECKIDENT ('Tasks', RESEED, 0);
GO

DELETE FROM Users;
DBCC CHECKIDENT ('Users', RESEED, 0);
GO

DELETE FROM Categories;
DBCC CHECKIDENT ('Categories', RESEED, 0);
GO
