CREATE TABLE [dbo].[Set]
    (
      [Id] UNIQUEIDENTIFIER NOT NULL
                            CONSTRAINT [PK_Set] PRIMARY KEY
    , ShowId UNIQUEIDENTIFIER
        NOT NULL
        CONSTRAINT [FK_Set_Show] FOREIGN KEY REFERENCES dbo.Show( id )
    , SetNumber TINYINT
    , IsEncore BIT NOT NULL
    );
