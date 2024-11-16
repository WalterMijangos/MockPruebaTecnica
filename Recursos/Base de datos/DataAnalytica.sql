
CREATE DATABASE DataAnalytica
GO

USE DataAnalytica
GO

-- Crear la tabla de Clientes con ID autoincremental
CREATE TABLE Clientes (
    id_cliente INT PRIMARY KEY IDENTITY,
    nombre VARCHAR(50),
    apellido VARCHAR(50),
    correo_electronico VARCHAR(100)
)
GO

-- Crear la tabla de Productos con ID autoincremental
CREATE TABLE Productos (
    id_producto INT IDENTITY(1,1) PRIMARY KEY,
    codigo_barras VARCHAR(50),
    nombre_producto VARCHAR(100),
    descripcion VARCHAR(255),
    categoria VARCHAR(50),
    precio DECIMAL(10, 2)
)
GO

-- Crear la tabla de Ventas con ID autoincremental y referencias a Clientes y Productos
CREATE TABLE Ventas (
    id_venta INT PRIMARY KEY IDENTITY,
    fecha_venta DATE,
    id_cliente INT,
    id_producto INT,
    cantidad INT,
    total_venta DECIMAL(10, 2),
    FOREIGN KEY (id_cliente) REFERENCES Clientes(id_cliente),
    FOREIGN KEY (id_producto) REFERENCES Productos(id_producto)
)
GO
