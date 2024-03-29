﻿CREATE DATABASE test;
GO
USE test

CREATE TABLE [Coupons] (
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](10) NOT NULL,
	[Active] [bit] NOT NULL,
	[PercentageDiscount] [int] NOT NULL,
    [DateOfUse] [DateTime], 
 	CONSTRAINT [PK_Coupons] PRIMARY KEY 
	(
		[Id] ASC
	)
);

CREATE TABLE [OrderStates] (
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](max) not null, 
 	CONSTRAINT [PK_OrderStates] PRIMARY KEY 
	(
		[Id] ASC
	)
);

CREATE TABLE [Products] (
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Price] [decimal](18, 2) NOT NULL,
	[StockQty] [int] NOT NULL,
	[Active] [bit] NOT NULL,
 	CONSTRAINT [PK_Products] PRIMARY KEY
	(
		[Id] ASC
	)
);

CREATE TABLE [UserInfos] (
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Surname] [nvarchar](max) NOT NULL,
	[Email] [nvarchar](max) NOT NULL,
	[Nationality] [nvarchar](2) NOT NULL,
	[SubscribedToNewsletter] [bit] NOT NULL,
	[RequestInvoice] [bit] NOT NULL,
	[VAT] [nvarchar](max),
	[TaxCode] [nvarchar](max),
 	CONSTRAINT [PK_Users] PRIMARY KEY
	(
		[Id] ASC
	)
);

CREATE TABLE [Orders] (
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserInfosId] [int],
    [CouponsId] [int],
	[OrderStatesId] [int] NOT NULL,
    [TotalPrice] [decimal](18, 2) NOT NULL,
	[TotalPriceWithoutDiscount] [decimal](18, 2) NOT NULL,
    [DateOrder] [datetime],
	FOREIGN KEY(UserInfosId) REFERENCES UserInfos(Id),
    FOREIGN KEY(CouponsId) REFERENCES Coupons(Id),
	FOREIGN KEY(OrderStatesId) REFERENCES OrderStates(Id),
 	CONSTRAINT [PK_Orders] PRIMARY KEY
	(
		[Id] ASC
	)
);

CREATE TABLE [Orders_Products] (
	[OrdersId] [int] NOT NULL,
    [ProductsId] [int] NOT NULL,
    [Quantity] [int] NOT NULL,
    FOREIGN KEY(OrdersId) REFERENCES Orders(Id),
    FOREIGN KEY(ProductsId) REFERENCES Products(Id),
 	CONSTRAINT [PK_Orders_Products] PRIMARY KEY
	(
		[OrdersId], [ProductsId]
	)
);

CREATE UNIQUE NONCLUSTERED INDEX [IX_UniqueCode_Coupons] ON [Coupons]
(
	[Code] ASC
)

ALTER TABLE [Coupons] ADD CONSTRAINT [DF_Coupons_Active]  DEFAULT ((1)) FOR [Active];
ALTER TABLE [Products] ADD CONSTRAINT [DF_Products_StockQty]  DEFAULT ((0)) FOR [StockQty];
ALTER TABLE [Products] ADD CONSTRAINT [DF_Products_Active]  DEFAULT ((1)) FOR [Active];
ALTER TABLE [Orders] ADD CONSTRAINT [DF_Orders_OrderStates]  DEFAULT ((1)) FOR [OrderStatesId];

/*
DROP TABLE [Orders];
DROP TABLE [Coupons];
DROP TABLE [Products];
DROP TABLE [UserInfos];
DROP TABLE [OrderStates];
DROP TABLE [Orders_Products];
*/
