CREATE TABLE [dbo].[Categories]
(
    Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(255) NOT NULL,
    CreatedAt DATE NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATE NULL,
    UpdatedBy INT NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    DeletedAt DATE NULL,
    DeletedBy INT NULL
);