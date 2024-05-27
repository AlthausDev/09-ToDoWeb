CREATE TABLE [dbo].[Tasks]
(
    Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    CategoryId INT NOT NULL,
    UserId INT NOT NULL,
    Name NVARCHAR(255) NOT NULL,
    StateId INT NOT NULL DEFAULT 1,
    CreatedAt DATE NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATE NULL,
    UpdatedBy INT NULL,
    ExpirationDate DATE NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    DeletedAt DATE NULL,
    DeletedBy INT NULL,
    FOREIGN KEY (CategoryId) REFERENCES Categories(Id),
    FOREIGN KEY (UserId) REFERENCES Users(Id)
)
