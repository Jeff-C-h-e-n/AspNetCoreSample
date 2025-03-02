CREATE TABLE [dbo].[Currencies] (
    [Code] [NVARCHAR](10) PRIMARY KEY,
    [Name] [NVARCHAR](50) NOT NULL
);

INSERT INTO [Currencies] ([Code], [Name]) VALUES ('USD', 'US Dollar');
INSERT INTO [Currencies] ([Code], [Name]) VALUES ('EUR', 'Euro');
INSERT INTO [Currencies] ([Code], [Name]) VALUES ('GBP', 'British Pound Sterling');
