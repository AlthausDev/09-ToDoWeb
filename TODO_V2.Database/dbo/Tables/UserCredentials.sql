CREATE TABLE [dbo].[UserCredentials]
(	
    UserId INT NOT NULL PRIMARY KEY,
    UserName NVARCHAR(255) NOT NULL,
    EncryptedPassword NVARCHAR(255) NOT NULL,
    CreatedAt DATE NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATE NULL,
    UpdatedBy INT NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    DeletedAt DATE NULL,
    DeletedBy INT NULL
    CONSTRAINT FK_UserCredentials_Users FOREIGN KEY (UserId) REFERENCES Users(Id)
)
