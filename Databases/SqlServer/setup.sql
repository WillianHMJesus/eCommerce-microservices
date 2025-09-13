create database [Authentication];
go
use [Authentication];

create table [Users](
	[Id] uniqueidentifier not null,
	[UserName] varchar(50) not null,
	[EmailAddress] varchar(100) not null,
	[PasswordHash] nvarchar(200) not null,

	constraint [PK_Users] primary key ([Id])
);
go

create table [Profiles](
	[Id] uniqueidentifier not null,
	[Name] varchar(50) not null,

	constraint [PK_Profiles] primary key ([Id])
);
go

create table [Roles](
	[Id] uniqueidentifier not null,
	[Name] varchar(50) not null,

	constraint [PK_Roles] primary key ([Id])
);
go

create table [User_Profiles](
	[UserId] uniqueidentifier not null,
	[ProfileId] uniqueidentifier not null,

	constraint [PK_User_Profiles] primary key ([UserId], [ProfileId]),
	constraint [FK_User_Profiles_Users] foreign key ([UserId]) references [Users] ([Id]),
	constraint [FK_User_Profiles_Profiles] foreign key ([ProfileId]) references [Profiles] ([Id])
);
go

create table [Profile_Roles](
	[ProfileId] uniqueidentifier not null,
	[RoleId] uniqueidentifier not null,

	constraint [PK_Profile_Roles] primary key ([ProfileId], [RoleId]),
	constraint [FK_Profile_Roles_Profiles] foreign key ([ProfileId]) references [Profiles] ([Id]),
	constraint [FK_Profile_Roles_Roles] foreign key ([RoleId]) references [Roles] ([Id])
);
go

declare @userId uniqueidentifier = newid();
declare @managerProfileId uniqueidentifier = newid();
declare @roleId uniqueidentifier = newid();

insert into [Users] ([Id], [UserName], [EmailAddress], [PasswordHash]) values (newid(), 'Customer User', 'user@customer.com', 'AQAAAAIAAYagAAAAEOBNuPrPU4BM5FT3EYNE1VOQlV8BY2GxLkdc3aYrqWdo8ldQSlfzoaaMNnwaCTrYlA==');

insert into [Users] ([Id], [UserName], [EmailAddress], [PasswordHash]) values (@userId, 'Manager User', 'user@manager.com', 'AQAAAAIAAYagAAAAEOBNuPrPU4BM5FT3EYNE1VOQlV8BY2GxLkdc3aYrqWdo8ldQSlfzoaaMNnwaCTrYlA==');

insert into [Profiles] ([Id], [Name]) values (@managerProfileId, 'Manager');

insert into [User_Profiles] ([UserId], [ProfileId]) values (@userId, @managerProfileId);

insert into [Profiles] ([Id], [Name]) values (newid(), 'Customer');

insert into [Roles] ([Id], [Name]) values (@roleId, 'AddUser');

insert into [Profile_Roles] ([ProfileId], [RoleId]) values (@managerProfileId, @roleId);
set @roleId = newid();

insert into [Roles] ([Id], [Name]) values (@roleId, 'AddCategory');

insert into [Profile_Roles] ([ProfileId], [RoleId]) values (@managerProfileId, @roleId);
set @roleId = newid();

insert into [Roles] ([Id], [Name]) values (@roleId, 'DeleteCategory');

insert into [Profile_Roles] ([ProfileId], [RoleId]) values (@managerProfileId, @roleId);
set @roleId = newid();

insert into [Roles] ([Id], [Name]) values (@roleId, 'UpdateCategory');

insert into [Profile_Roles] ([ProfileId], [RoleId]) values (@managerProfileId, @roleId);
set @roleId = newid();

insert into [Roles] ([Id], [Name]) values (@roleId, 'AddProduct');

insert into [Profile_Roles] ([ProfileId], [RoleId]) values (@managerProfileId, @roleId);
set @roleId = newid();

insert into [Roles] ([Id], [Name]) values (@roleId, 'DeleteProduct');

insert into [Profile_Roles] ([ProfileId], [RoleId]) values (@managerProfileId, @roleId);
set @roleId = newid();

insert into [Roles] ([Id], [Name]) values (@roleId, 'UpdateProduct');

insert into [Profile_Roles] ([ProfileId], [RoleId]) values (@managerProfileId, @roleId);
set @roleId = newid();

insert into [Roles] ([Id], [Name]) values (@roleId, 'MakeAvailableProduct');

insert into [Profile_Roles] ([ProfileId], [RoleId]) values (@managerProfileId, @roleId);
set @roleId = newid();

insert into [Roles] ([Id], [Name]) values (@roleId, 'MakeUnavailableProduct');

insert into [Profile_Roles] ([ProfileId], [RoleId]) values (@managerProfileId, @roleId);
go

create database [Catalog];
go
use [Catalog];

create table [Categories](
	[Id] uniqueidentifier not null,
	[Code] smallint not null,
	[Name] varchar(50) not null,
	[Description] varchar(100) not null,
	[Active] bit not null,

	constraint [PK_Categories] primary key ([Id])
);
go

create table [Products](
	[Id] uniqueidentifier not null,
	[Name] varchar(50) not null,
	[Description] varchar(100) not null,
	[Value] decimal(8, 2) not null,
	[Quantity] smallint not null,
	[Image] varchar(50) not null,
	[Available] bit not null,
	[Active] bit not null,
	[CategoryId] uniqueidentifier not null,

	constraint [PK_Products] primary key ([Id]),
	constraint [FK_Products_Categories] foreign key ([CategoryId]) references [Categories] ([Id])
);
go

create database [Checkout];
go
use [Checkout];

create table [Orders](
	[Id] uniqueidentifier not null,
	[UserId] uniqueidentifier not null,
	[Number] varchar(50) not null,
	[OrderStatus] smallint not null,
	[Active] bit not null,

	constraint [PK_Orders] primary key ([Id])
);
go

create table [Items](
	[Id] uniqueidentifier not null,
	[ProductId] uniqueidentifier not null,
	[ProductName] varchar(50) not null,
	[ProductImage] varchar(50) not null,
	[Quantity] smallint not null,
	[Value] decimal(8, 2) not null,
	[Active] bit not null,
	[OrderId] uniqueidentifier not null,

	constraint [PK_Items] primary key ([Id]),
	constraint [FK_Items_Orders] foreign key ([OrderId]) references [Orders] ([Id])
);
go

create database [Payment];
go
use [Payment];

create table [Transactions](
	[Id] uniqueidentifier not null,
	[UserId] uniqueidentifier not null,
	[OrderId] uniqueidentifier not null,
	[Value] decimal(8, 2) not null,
	[CardNumber] varchar(20) not null,
	[Status] varchar(50) not null,
	[Date] DateTime not null,
	[Active] bit not null,

	constraint [PK_Transactions] primary key ([Id])
);
go