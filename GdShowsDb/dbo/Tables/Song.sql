CREATE TABLE [dbo].[Song]
    (
      [Id] UNIQUEIDENTIFIER NOT NULL
                            CONSTRAINT [PK_Song] PRIMARY KEY
    , SetId UNIQUEIDENTIFIER
        NOT NULL
        CONSTRAINT [FK_Song_Set] FOREIGN KEY REFERENCES dbo.[Set] ( id )
    , SongRefId UNIQUEIDENTIFIER
        NOT NULL
        CONSTRAINT [FK_Song_SongRef]
        FOREIGN KEY REFERENCES dbo.SongRef ( id )
    , Position TINYINT NOT NULL
    , IsSegued BIT NOT NULL
    );
