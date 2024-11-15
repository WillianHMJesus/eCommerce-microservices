create database [Catalog];
go
use [Catalog];

create table [Categories](
	[Id] uniqueidentifier not null,
	[Code] smallint not null,
	[Name] varchar(50) not null,
	[Description] varchar(100) not null,
	[Active] bit not null,

	constraint [PK_Categories] primary key ([Id]),
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
	constraint [FK_Products_Catalog] foreign key ([CategoryId]) references [Categories] ([Id])
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