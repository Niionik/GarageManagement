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

-- Tabela: Owner (Właściciele) - musi być pierwsza ze względu na relacje
CREATE TABLE Owner (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE
);

-- Tabela: Car (Auta) - z dodanym OwnerId
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

-- Tabela: Maintenance (Naprawy)
CREATE TABLE Maintenance (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CarId INT NOT NULL,
    Date DATETIME NOT NULL,
    Description NVARCHAR(200) NOT NULL,
    Cost DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (CarId) REFERENCES Car(Id) ON DELETE CASCADE
);

-- Tabela: Garage (Garaże)
CREATE TABLE Garage (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Location NVARCHAR(200) NOT NULL
);

-- Tabela relacyjna: GarageCars (Auta przypisane do garażu)
CREATE TABLE GarageCars (
    GarageId INT NOT NULL,
    CarId INT NOT NULL,
    PRIMARY KEY (GarageId, CarId),
    FOREIGN KEY (GarageId) REFERENCES Garage(Id) ON DELETE CASCADE,
    FOREIGN KEY (CarId) REFERENCES Car(Id) ON DELETE CASCADE
);

-- Wstawianie danych
-- 1. Najpierw właściciel
INSERT INTO Owner (FirstName, LastName, Email)
VALUES ('Patka', 'W', 'email.email@email.pl');

-- 2. Garaż
INSERT INTO Garage (Name, Location)
VALUES ('Garaż Domowy', 'Staszow');

-- 3. Samochody (z OwnerId)
INSERT INTO Car (Brand, Model, Year, Mileage, Status, WheelModel, TireSize, TireBrand, LastOilChange, LastTimingBeltChange, OwnerId)
VALUES 
('Mazda', 'RX8', 2004, 45000, 'Active', 'Sport Alloy R19', '295/45 R18', 'Michelin', '2023-12-15', '2023-06-20', 1),
('Hyundai', 'Tiburon', 2006, 78000, 'Active', 'Standard R17', '225/50 R17', 'Continental', '2023-11-10', '2023-04-15', 1),
('Toyota', 'MR3', 1991, 5000, 'Active', 'AMG R18', '235/45 R18', 'Pirelli', '2024-01-05', NULL, 1);

-- 4. Przypisanie aut do garażu
INSERT INTO GarageCars (GarageId, CarId)
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