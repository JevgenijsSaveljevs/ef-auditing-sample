CREATE TABLE dbo.BicycleType (
	Code nvarchar(30),
	[Name] nvarchar(100)
)

CREATE TABLE dbo.BicycleComponent (
	Id int IDENTITY(1,1),
	ComponentId int not null,
	BicycleId int not null,
)

CREATE TABLE dbo.Component (
	Id int IDENTITY(1,1),
	[Name] nvarchar(100)
)

CREATE TABLE dbo.Brand (
	Id int IDENTITY(1,1),
	[Name] nvarchar(100)
)

CREATE TABLE dbo.Bicycle (
	Id int IDENTITY(1,1),
	Model nvarchar(100) not null,
	[Year] date not null,
	BrandId int not null,
	BicycleTypeCode nvarchar(30) not null,
)

INSERT INTO dbo.BicycleType (Code, [Name]) VALUES ('Gravel', 'Gravel')



-- drop table dbo.BicycleType
-- drop table dbo.BicycleComponent
-- drop table dbo.Component
-- drop table dbo.Brand
-- drop table dbo.Bicycle