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

-- Tabela: AspNetRoles (role)
CREATE TABLE AspNetRoles (
    Id NVARCHAR(128) NOT NULL PRIMARY KEY,
    Name NVARCHAR(256) NULL,
    NormalizedName NVARCHAR(256) NULL,
    ConcurrencyStamp NVARCHAR(MAX) NULL
);

-- Tabela: AspNetUsers (użytkownicy)
CREATE TABLE AspNetUsers (
    Id NVARCHAR(128) NOT NULL PRIMARY KEY,
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

-- Tabela: Garage (garaże)
CREATE TABLE Garage (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Location VARCHAR(200) NOT NULL,
    OwnerId NVARCHAR(128),
    FOREIGN KEY (OwnerId) REFERENCES AspNetUsers(Id)
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
    OwnerId NVARCHAR(128),
    GarageId INT,
    FOREIGN KEY (OwnerId) REFERENCES AspNetUsers(Id),
    FOREIGN KEY (GarageId) REFERENCES Garage(Id)
);

-- Tabela relacyjna: GarageCar
CREATE TABLE GarageCars (
    GarageId INT NOT NULL,
    CarId INT NOT NULL,
    PRIMARY KEY (GarageId, CarId),
    FOREIGN KEY (GarageId) REFERENCES Garage(Id) ON DELETE CASCADE,
    FOREIGN KEY (CarId) REFERENCES Car(Id) ON DELETE CASCADE
);

-- Pozostałe tabele Identity
CREATE TABLE AspNetUserRoles (
    UserId NVARCHAR(128) NOT NULL,
    RoleId NVARCHAR(128) NOT NULL,
    CONSTRAINT PK_AspNetUserRoles PRIMARY KEY (UserId, RoleId),
    CONSTRAINT FK_AspNetUserRoles_AspNetUsers FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id) ON DELETE CASCADE,
    CONSTRAINT FK_AspNetUserRoles_AspNetRoles FOREIGN KEY (RoleId) REFERENCES AspNetRoles(Id) ON DELETE CASCADE
);

CREATE TABLE AspNetUserClaims (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    UserId NVARCHAR(128) NOT NULL,
    ClaimType NVARCHAR(MAX) NULL,
    ClaimValue NVARCHAR(MAX) NULL,
    CONSTRAINT FK_AspNetUserClaims_AspNetUsers_UserId FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id) ON DELETE CASCADE
);

CREATE TABLE AspNetRoleClaims (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    RoleId NVARCHAR(128) NOT NULL,
    ClaimType NVARCHAR(MAX) NULL,
    ClaimValue NVARCHAR(MAX) NULL,
    CONSTRAINT FK_AspNetRoleClaims_AspNetRoles FOREIGN KEY (RoleId) REFERENCES AspNetRoles(Id) ON DELETE CASCADE
);

-- Tabela: Maintenance (konserwacje)
CREATE TABLE Maintenance (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CarId INT NOT NULL,
    Date DATE NOT NULL,
    Description NVARCHAR(255),
    Cost DECIMAL(10, 2),
    OwnerId NVARCHAR(128),
    FOREIGN KEY (CarId) REFERENCES Car(Id),
    FOREIGN KEY (OwnerId) REFERENCES AspNetUsers(Id)
);

-- Wstawianie przykładowych danych
-- 1. Użytkownik
INSERT INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount, FirstName, LastName)
VALUES ('1', 'patka', 'PATKA', 'patka@email.pl', 'PATKA@EMAIL.PL', 1, '123', 'security-stamp', 'concurrency-stamp', '123456789', 1, 0, NULL, 0, 0, 'Patka', 'W');

-- 2. Garaż
INSERT INTO Garage (Name, Location, OwnerId)
VALUES ('Garaż Domowy', 'Staszow', '1');

-- 3. Samochody
INSERT INTO Car (Brand, Model, Year, Mileage, Status, WheelModel, TireSize, TireBrand, LastOilChange, LastTimingBeltChange, OwnerId, GarageId)
VALUES 
('Mazda', 'RX8', 2004, 45000, 'Active', 'Sport Alloy R19', '295/45 R18', 'Michelin', '2023-12-15', '2023-06-20', '1', 1),
('Hyundai', 'Tiburon', 2006, 78000, 'Active', 'Standard R17', '225/50 R17', 'Continental', '2023-11-10', '2023-04-15', '1', 1),
('Toyota', 'MR3', 1991, 5000, 'Active', 'AMG R18', '235/45 R18', 'Pirelli', '2024-01-05', NULL, '1', 1);

-- 4. Przypisanie aut do garażu
INSERT INTO GarageCars (GarageId, CarId)
VALUES 
(1, 1),
(1, 2),
(1, 3);

-- 5. Historia napraw
INSERT INTO Maintenance (CarId, Date, Description, Cost, OwnerId)
VALUES 
(1, '2023-12-15', 'Wymiana oleju i filtrów', 450.00, '1'),
(2, '2023-11-10', 'Przegląd okresowy', 350.00, '1'),
(3, '2024-01-05', 'Wymiana opon na zimowe', 200.00, '1');