CREATE TABLE [dbo].[Log] (
    [Id] INT IDENTITY(1,1) NOT NULL,
    [Time_stamp] DATETIME NOT NULL,
    [Level] NVARCHAR(50) NOT NULL,
    [Logger] NVARCHAR(250) NULL,
    [Session_data] NVARCHAR(250) NULL,      
    [Message] NVARCHAR(MAX) NOT NULL,
    [MachineName] NVARCHAR(50) NOT NULL,
    [Call_site] NVARCHAR(MAX) NOT NULL,
    [Stacktrace] NVARCHAR(MAX) NULL,
    [Log_exception] NVARCHAR(MAX) NULL,    
    CONSTRAINT [PK_dbo.Log] PRIMARY KEY CLUSTERED ([Id] ASC)
        WITH (
            PAD_INDEX = OFF, 
            STATISTICS_NORECOMPUTE = OFF, 
            IGNORE_DUP_KEY = OFF, 
            ALLOW_ROW_LOCKS = ON, 
            ALLOW_PAGE_LOCKS = ON
        ) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];
