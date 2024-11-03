IF DB_ID('Bank') IS NOT NULL
BEGIN
    DROP DATABASE Bank;
END
GO

IF DB_ID('Bank') IS NULL
BEGIN
    CREATE DATABASE Bank;
END
GO

USE Bank;
GO

IF OBJECT_ID('dbo.Movimiento', 'U') IS NOT NULL
BEGIN
    DROP TABLE dbo.Movimiento;
END
GO


IF OBJECT_ID('dbo.Cuenta', 'U') IS NOT NULL
BEGIN
    DROP TABLE dbo.Cuenta;
END
GO

IF OBJECT_ID('dbo.Cliente', 'U') IS NOT NULL
BEGIN
    DROP TABLE dbo.Cliente;
END
GO

IF OBJECT_ID('dbo.Persona', 'U') IS NOT NULL
BEGIN
    DROP TABLE dbo.Persona;
END
GO

CREATE TABLE Persona (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL,
    Genero CHAR(1) NOT NULL,
    Edad INT NOT NULL CHECK (Edad >= 0),
    Identificacion NVARCHAR(50) NOT NULL UNIQUE,
    Direccion NVARCHAR(255),
    Telefono NVARCHAR(20)
);
GO

CREATE TABLE Cliente (
    ClienteId INT PRIMARY KEY,
    Contrasenia NVARCHAR(255) NOT NULL,
    Estado BIT NOT NULL,
    CONSTRAINT FK_Cliente_Persona FOREIGN KEY (ClienteId) REFERENCES Persona(Id),
    UNIQUE (ClienteId)
);
GO

CREATE TABLE Cuenta (
    CuentaId INT PRIMARY KEY IDENTITY(1,1),
    ClienteId INT NOT NULL,
    NumeroCuenta NVARCHAR(50) NOT NULL UNIQUE,
    TipoCuenta NVARCHAR(50) NOT NULL,
    SaldoInicial DECIMAL(18, 2) NOT NULL CHECK (SaldoInicial >= 0),
    Estado BIT NOT NULL,
    CONSTRAINT FK_Cuenta_Cliente FOREIGN KEY (ClienteId) REFERENCES Cliente(ClienteId)
);
GO

CREATE TABLE Movimiento (
    MovimientoId INT PRIMARY KEY IDENTITY(1,1),
    CuentaId INT NOT NULL,
    Fecha DATETIME NOT NULL DEFAULT GETDATE(),
    TipoMovimiento NVARCHAR(50) NOT NULL,
    Valor DECIMAL(18, 2) NOT NULL,
    Saldo DECIMAL(18, 2) NOT NULL,
    CONSTRAINT UQ_Movimiento UNIQUE (CuentaId, Fecha, TipoMovimiento),
    CONSTRAINT FK_Movimiento_Cuenta FOREIGN KEY (CuentaId) REFERENCES Cuenta(CuentaId)
);
GO

insert into Persona (Nombre, Genero, Edad, Identificacion, Direccion, Telefono) values ('Jose Lema', 'M', 25, '1742538690','Otavalo sn y principal', '098254785');
insert into Persona (Nombre, Genero, Edad, Identificacion, Direccion, Telefono) values ('Marianela Montalvo', 'F', 25, '1742948690','Amazonas y NNUU', '097548965');
insert into Persona (Nombre, Genero, Edad, Identificacion, Direccion, Telefono) values ('Juan Osorio', 'M', 25, '1742558690','13 junio y Equinoccial', '098874587');
go

insert into Cliente (ClienteId, Contrasenia, Estado) values (1, '1234', 1);
insert into Cliente (ClienteId, Contrasenia, Estado) values (2, '5678', 1);
insert into Cliente (ClienteId, Contrasenia, Estado) values (3, '1245', 1);
go


insert into Cuenta(ClienteId, NumeroCuenta, TipoCuenta, SaldoInicial, Estado) values (1, '478758', 'AHORROS', 2000, 1);
insert into Cuenta(ClienteId, NumeroCuenta, TipoCuenta, SaldoInicial, Estado) values (2, '225487', 'CORRIENTE', 100, 1);
insert into Cuenta(ClienteId, NumeroCuenta, TipoCuenta, SaldoInicial, Estado) values (3, '495878', 'AHORROS', 0, 1);
insert into Cuenta(ClienteId, NumeroCuenta, TipoCuenta, SaldoInicial, Estado) values (2, '496825', 'AHORROS', 540, 1);
go

