create database [Catalog];
go
use [Catalog];

create table [Categories](
	[Id] uniqueidentifier not null,
	[Code] smallint not null,
	[Name] varchar(50) not null,
	[Description] varchar(100) not null,

	constraint [PK_Categories] primary key ([Id]),
);
go

DECLARE @ID NVARCHAR(max) = N'b3729d7f-d134-40a4-95bb-d3420dc5261b';

insert into [Categories]
values(CONVERT(uniqueidentifier, @ID), 9999, 'Integration Test', 'Categoria usada nos testes de integração');
go

create table [Products](
	[Id] uniqueidentifier not null,
	[Name] varchar(50) not null,
	[Description] varchar(100) not null,
	[Value] decimal(8, 2) not null,
	[Quantity] smallint not null,
	[Image] varchar(50) not null,
	[Available] bit not null,
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
	[OrderStatus] smallint not null,

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
	[OrderId] uniqueidentifier not null,

	constraint [PK_Items] primary key ([Id]),
	constraint [FK_Items_Orders] foreign key ([OrderId]) references [Orders] ([Id])
);
go