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