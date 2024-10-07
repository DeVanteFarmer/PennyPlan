USE [master];

IF db_id('PennyPlanDB') IS NULL
    CREATE DATABASE [PennyPlanDB];
GO

USE [PennyPlanDB];
GO

-- Drop existing tables if they exist
DROP TABLE IF EXISTS [Transactions];
DROP TABLE IF EXISTS [Bills];
DROP TABLE IF EXISTS [Budgets];
DROP TABLE IF EXISTS [Categories];
DROP TABLE IF EXISTS [Users];
GO

-- Create Users table
CREATE TABLE [Users] (
    [id] INT PRIMARY KEY IDENTITY,
    [userName] NVARCHAR(255) NOT NULL,
    [email] NVARCHAR(255) NOT NULL,
    [password_hash] NVARCHAR(255) NOT NULL,
    [avatar] VARBINARY(MAX), -- Assuming image is stored as VARBINARY for the avatar
    [created_at] DATETIME NOT NULL DEFAULT GETDATE(),
    [updated_at] DATETIME2 NOT NULL DEFAULT GETDATE()
);
GO

-- Create Categories table
CREATE TABLE [Categories] (
    [id] INT PRIMARY KEY IDENTITY,
    [name] NVARCHAR(255) NOT NULL,
    [description] NVARCHAR(255)
);
GO

-- Create Budgets table
CREATE TABLE [Budgets] (
    [id] INT PRIMARY KEY IDENTITY,
    [userId] INT NOT NULL,
    [total_income] INT NOT NULL,
    [total_bills] INT NOT NULL,
    [daily_spending_limit] INT NOT NULL,
    [created_at] DATETIME NOT NULL DEFAULT GETDATE(),
    [updated_at] DATETIME2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [FK_Budgets_Users] FOREIGN KEY ([userId]) REFERENCES [Users] ([id])
);
GO

-- Create Bills table
CREATE TABLE [Bills] (
    [id] INT PRIMARY KEY IDENTITY,
    [userId] INT NOT NULL,
    [bill_name] NVARCHAR(255) NOT NULL,
    [amount] INT NOT NULL,
    [due_date] DATE NOT NULL,
    [categoryId] INT NOT NULL,
    [paid] BIT NOT NULL,
    [created_at] DATETIME NOT NULL DEFAULT GETDATE(),
    [updated_at] DATETIME2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [FK_Bills_Users] FOREIGN KEY ([userId]) REFERENCES [Users] ([id]),
    CONSTRAINT [FK_Bills_Categories] FOREIGN KEY ([categoryId]) REFERENCES [Categories] ([id])
);
GO

-- Create Transactions table
CREATE TABLE [Transactions] (
    [id] INT PRIMARY KEY IDENTITY,
    [userId] INT NOT NULL,
    [transaction_name] NVARCHAR(255) NOT NULL,
    [amount] INT NOT NULL,
    [categoryId] INT NOT NULL,
    [date] DATE NOT NULL,
    [created_at] DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [FK_Transactions_Users] FOREIGN KEY ([userId]) REFERENCES [Users] ([id]),
    CONSTRAINT [FK_Transactions_Categories] FOREIGN KEY ([categoryId]) REFERENCES [Categories] ([id])
);
GO
