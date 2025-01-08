USE master;
GO

IF EXISTS(SELECT * FROM sys.databases WHERE name = 'Garage')
BEGIN
    ALTER DATABASE Garage SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE Garage;
END

CREATE DATABASE Garage;
GO

USE Garage;
GO

-- Tabela: AspNetUsers (użytkownicy)
CREATE TABLE AspNetUsers (
    Id NVARCHAR(450) NOT NULL PRIMARY KEY,
    UserName NVARCHAR(256) NULL,
    NormalizedUserName NVARCHAR(256) NULL,
    Email NVARCHAR(256) NULL,
    NormalizedEmail NVARCHAR(256) NULL,
    EmailConfirmed BIT NOT NULL DEFAULT 0,
    PasswordHash NVARCHAR(MAX) NULL,
    SecurityStamp NVARCHAR(MAX) NULL,
    ConcurrencyStamp NVARCHAR(MAX) NULL,
    PhoneNumber NVARCHAR(MAX) NULL,
    PhoneNumberConfirmed BIT NOT NULL DEFAULT 0,
    TwoFactorEnabled BIT NOT NULL DEFAULT 0,
    LockoutEnd DATETIMEOFFSET NULL,
    LockoutEnabled BIT NOT NULL DEFAULT 0,
    AccessFailedCount INT NOT NULL DEFAULT 0,
    FirstName NVARCHAR(MAX) NULL,
    LastName NVARCHAR(MAX) NULL
);

CREATE UNIQUE INDEX IX_AspNetUsers_NormalizedUserName ON AspNetUsers (NormalizedUserName)
WHERE NormalizedUserName IS NOT NULL;

CREATE INDEX IX_AspNetUsers_NormalizedEmail ON AspNetUsers (NormalizedEmail);

-- Tabela: Owner (właściciele)
CREATE TABLE Owner (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(100),
    LastName NVARCHAR(100),
    Email NVARCHAR(256),
    UserId NVARCHAR(450),
    FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id)
);

-- Tabela: Garage (garaże)
CREATE TABLE Garage (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Location NVARCHAR(200) NOT NULL,
    OwnerId INT,
    FOREIGN KEY (OwnerId) REFERENCES Owner(Id)
);

-- Tabela: Car (samochody)
CREATE TABLE Car (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Brand NVARCHAR(50) NOT NULL,
    Model NVARCHAR(50) NOT NULL,
    Year INT NOT NULL,
    Mileage INT NOT NULL,
    Status NVARCHAR(20) NOT NULL CHECK (Status IN ('Active', 'Repair', 'Broken')),
    WheelModel NVARCHAR(50),
    TireSize NVARCHAR(20),
    TireBrand NVARCHAR(50),
    LastOilChange DATE,
    LastTimingBeltChange DATE,
    OwnerId INT,
    FOREIGN KEY (OwnerId) REFERENCES Owner(Id)
);

-- Tabela relacyjna: GarageCar (przypisanie samochodów do garaży)
CREATE TABLE GarageCar (
    GarageId INT NOT NULL,
    CarId INT NOT NULL,
    PRIMARY KEY (GarageId, CarId),
    FOREIGN KEY (GarageId) REFERENCES Garage(Id) ON DELETE CASCADE,
    FOREIGN KEY (CarId) REFERENCES Car(Id) ON DELETE CASCADE
);

-- Tabela: Maintenance (konserwacje)
CREATE TABLE Maintenance (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CarId INT NOT NULL,
    Date DATE NOT NULL,
    Description NVARCHAR(255),
    Cost DECIMAL(10, 2),
    FOREIGN KEY (CarId) REFERENCES Car(Id)
);

-- Wstawianie danych
-- 1. Najpierw właściciel
INSERT INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount, FirstName, LastName)
VALUES ('1', 'patka', 'PATKA', 'email.email@email.pl', 'EMAIL.EMAIL@EMAIL.PL', 1, 'hashed-password', 'security-stamp', 'concurrency-stamp', '123456789', 1, 0, NULL, 0, 0, 'Patka', 'W');

INSERT INTO Owner (FirstName, LastName, Email, UserId)
VALUES ('Patka', 'W', 'email.email@email.pl', '1');

-- 2. Garaż
INSERT INTO Garage (Name, Location, OwnerId)
VALUES ('Garaż Domowy', 'Staszow', 1);

-- 3. Samochody (z OwnerId)
INSERT INTO Car (Brand, Model, Year, Mileage, Status, WheelModel, TireSize, TireBrand, LastOilChange, LastTimingBeltChange, OwnerId)
VALUES 
('Mazda', 'RX8', 2004, 45000, 'Active', 'Sport Alloy R19', '295/45 R18', 'Michelin', '2023-12-15', '2023-06-20', 1),
('Hyundai', 'Tiburon', 2006, 78000, 'Active', 'Standard R17', '225/50 R17', 'Continental', '2023-11-10', '2023-04-15', 1),
('Toyota', 'MR3', 1991, 5000, 'Active', 'AMG R18', '235/45 R18', 'Pirelli', '2024-01-05', NULL, 1);

-- 4. Przypisanie aut do garażu
INSERT INTO GarageCar (GarageId, CarId)
VALUES 
(1, 1),
(1, 2),
(1, 3);

-- 5. Historia napraw
INSERT INTO Maintenance (CarId, Date, Description, Cost)
VALUES 
(1, '2023-12-15', 'Wymiana oleju i filtrów', 450.00),
(2, '2023-11-10', 'Przegląd okresowy', 350.00),
(3, '2024-01-05', 'Wymiana opon na zimowe', 200.00);