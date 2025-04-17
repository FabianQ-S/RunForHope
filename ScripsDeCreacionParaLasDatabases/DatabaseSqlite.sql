CREATE TABLE Participantes (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Nombre TEXT NOT NULL CHECK(LENGTH(Nombre) <= 50),
    Apellido TEXT NOT NULL CHECK(LENGTH(Apellido) <= 50),
    DNI TEXT NOT NULL CHECK(LENGTH(DNI) = 8 AND DNI GLOB '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'),
    FechaNacimiento TEXT NOT NULL CHECK(
        DATE(FechaNacimiento) <= DATE('now') AND
        CAST((julianday('now') - julianday(FechaNacimiento))/365.25 AS INTEGER) >= 12
    ),
    Genero TEXT NOT NULL CHECK(Genero IN ('Masculino', 'Femenino', 'Otro')),
    Discapacidad INTEGER NOT NULL CHECK(Discapacidad IN (0, 1)),
    Correo TEXT NOT NULL CHECK(Correo LIKE '%_@__%.__%'),
    NumeroTelefono TEXT NOT NULL CHECK(
        LENGTH(NumeroTelefono) = 9 AND
        NumeroTelefono GLOB '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'
    ),
    Distrito TEXT NOT NULL,
    TipoSangre TEXT NOT NULL CHECK(TipoSangre IN ('A+', 'A-', 'B+', 'B-', 'AB+', 'AB-', 'O+', 'O-')),
    FechaRegistro TEXT NOT NULL DEFAULT CURRENT_TIMESTAMP
);
