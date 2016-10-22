CREATE TABLE [dbo].[Show]
    (
      [Id] UNIQUEIDENTIFIER NOT NULL
                            CONSTRAINT [PK_Show] PRIMARY KEY
    , ShowDate DATE
    , Venue VARCHAR(256)
    , City VARCHAR(64)
    , State VARCHAR(32)
    , Country VARCHAR(64)
    );
