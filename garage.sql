-- Tabela: Car (Auta)
use Garage;

CREATE TABLE Car (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Make NVARCHAR(50) NOT NULL,
    Model NVARCHAR(50) NOT NULL,
    Year INT NOT NULL,
    Mileage INT NOT NULL,
    Status NVARCHAR(20) NOT NULL CHECK (Status IN ('Active', 'Repair', 'Broken')),
    WheelModel NVARCHAR(50),
    TireSize NVARCHAR(20),
    TireBrand NVARCHAR(50),
    LastOilChange DATE,
    LastTimingBeltChange DATE
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

-- Tabela: Owner (W³aœciciele)
CREATE TABLE Owner (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE
);

-- Tabela: Garage (Gara¿e)
CREATE TABLE Garage (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Location NVARCHAR(200) NOT NULL
);

-- Tabela relacyjna: Garage_Car (Auta przypisane do gara¿u)
CREATE TABLE GarageCar (
    GarageId INT NOT NULL,
    CarId INT NOT NULL,
    PRIMARY KEY (GarageId, CarId),
    FOREIGN KEY (GarageId) REFERENCES Garage(Id) ON DELETE CASCADE,
    FOREIGN KEY (CarId) REFERENCES Car(Id) ON DELETE CASCADE
);
