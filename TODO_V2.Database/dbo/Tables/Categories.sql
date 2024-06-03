CREATE TABLE [dbo].[Categories]
(
    Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(255) NOT NULL,
    CreatedAt DATE NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATE NULL,
    UpdatedBy INT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    DeletedAt DATE NULL,
    DeletedBy INT NULL
);