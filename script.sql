BEGIN TRANSACTION;
GO

UPDATE [ThColors] SET [Font] = N'#000000'
WHERE [Id] = N'18';
SELECT @@ROWCOUNT;

GO

UPDATE [ThColors] SET [Font] = N'#000000'
WHERE [Id] = N'2';
SELECT @@ROWCOUNT;

GO

UPDATE [ThColors] SET [Font] = N'#fffffff'
WHERE [Id] = N'20';
SELECT @@ROWCOUNT;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20230707045236_thColorsChanges', N'7.0.4');
GO

COMMIT;
GO

