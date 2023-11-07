﻿IF (DB_ID(N'Imbracaminte') IS NULL)
	CREATE DATABASE Imbracaminte
GO

USE Imbracaminte
GO

IF OBJECT_ID ('Articole') IS NULL
	CREATE TABLE Articole
	(
	Id INT NOT NULL IDENTITY(1, 1) CONSTRAINT PK_Articol PRIMARY KEY,
	[Name] NVARCHAR(256) NOT NULL,
	[Description] NVARCHAR(2000) NOT NULL, 
	Price NUMERIC(20, 2) NOT NULL,
	Size NUMERIC(20, 2) NOT NULL,
	[Color] NVARCHAR(256) NOT NULL,
	IsAvailable BIT NOT NULL,
	ImagePath NVARCHAR(1000) NULL

	)
GO