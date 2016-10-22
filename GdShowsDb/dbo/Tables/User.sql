CREATE TABLE [dbo].[User]
    (
      [Id] UNIQUEIDENTIFIER NOT NULL
                            CONSTRAINT [PK_User] PRIMARY KEY
    , [PasswordHash] NVARCHAR(256) NOT NULL
    , [PasswordSalt] NVARCHAR(256) NOT NULL
    , [UserType] TINYINT NOT NULL
    , [FirstName] NVARCHAR(64) NOT NULL
    , [LastName] NVARCHAR(64) NOT NULL
    , [EmailAddress] NVARCHAR(128) NOT NULL
    , [Status] TINYINT NOT NULL
    , PasswordExpire DATETIMEOFFSET NULL
    , AccessFailedCount INT
        NOT NULL
        CONSTRAINT [DF_User_AccessFailedCount] DEFAULT ( 0 )
    , LockoutEnd DATETIMEOFFSET NULL
    );
