const express = require('express');
const sqlite3 = require('sqlite3').verbose();
const mysql = require('mysql2/promise');
const cors = require('cors');
const path = require('path');

const app = express();
const port = 3000;

// Configuración de middleware
app.use(cors({
    origin: 'http://localhost:3000',
    credentials: true
}));
app.use(express.json());
app.use(express.urlencoded({ extended: true }));
app.use(express.static(path.join(__dirname, 'public')));

// ==================================================
// CONEXIÓN A SQLITE (PARTICIPANTES)
// ==================================================
const sqliteDB = new sqlite3.Database('./db/ParticipantesContext.db', (err) => {
    if (err) console.error('Error SQLite:', err.message);
    else console.log('✅ Conectado a SQLite (Participantes)');
});

// ==================================================
// CONEXIÓN A MYSQL (DONACIONES)
// ==================================================
const mysqlPool = mysql.createPool({
    host: 'localhost',
    user: 'root',
    password: '0106662068',
    database: 'BD_Donaciones',
    waitForConnections: true,
    connectionLimit: 10,
    queueLimit: 0
});

// Verificar conexión MySQL
mysqlPool.getConnection()
    .then(conn => {
        console.log('✅ Conectado a MySQL (Donaciones)');
        conn.release();
    })
    .catch(err => console.error('❌ Error MySQL:', err));

// ==================================================
// RUTAS PARA PÁGINAS HTML
// ==================================================
app.get('/', (req, res) => {
    res.sendFile(path.join(__dirname, 'public', 'index.html'));
});

app.get('/registro', (req, res) => {
    res.sendFile(path.join(__dirname, 'public', 'registro.html'));
});

app.get('/edicion', (req, res) => {
    res.sendFile(path.join(__dirname, 'public', 'edit.html'));
});

app.get('/eliminacion', (req, res) => {
    res.sendFile(path.join(__dirname, 'public', 'delete.html'));
});

app.get('/administracion', (req, res) => {
    res.sendFile(path.join(__dirname, 'public', 'administracion.html'));
});

// ==================================================
// API REST - PARTICIPANTES (SQLITE)
// ==================================================
app.get('/api/participantes', (req, res) => {
    sqliteDB.all('SELECT * FROM Participantes', [], (err, rows) => {
        if (err) return res.status(400).json({ error: err.message });
        res.json({ data: rows });
    });
});

app.get('/api/participantes/:id', (req, res) => {
    const id = req.params.id;
    sqliteDB.get('SELECT * FROM Participantes WHERE Id = ?', [id], (err, row) => {
        if (err) return res.status(400).json({ error: err.message });
        if (!row) return res.status(404).json({ error: "Participante no encontrado" });
        res.json({ data: row });
    });
});

app.post('/api/participantes', (req, res) => {
    const { Nombre, Apellido, DNI, FechaNacimiento, Genero, Discapacidad, Correo, NumeroTelefono, Distrito, TipoSangre, FechaRegistro } = req.body;
    if (!Nombre || !Apellido || !DNI) return res.status(400).json({ error: "Faltan campos obligatorios" });

    const sql = `INSERT INTO Participantes (Nombre, Apellido, DNI, FechaNacimiento, Genero, Discapacidad, Correo, NumeroTelefono, Distrito, TipoSangre, FechaRegistro)
                 VALUES (?,?,?,?,?,?,?,?,?,?,?)`;
    const params = [Nombre, Apellido, DNI, FechaNacimiento, Genero, Discapacidad, Correo, NumeroTelefono, Distrito, TipoSangre, FechaRegistro];

    sqliteDB.run(sql, params, function(err) {
        if (err) return res.status(400).json({ error: err.message });
        res.json({ data: { Id: this.lastID, ...req.body } });
    });
});

app.put('/api/participantes/:id', (req, res) => {
    const { Nombre, Apellido, DNI, FechaNacimiento, Genero, Discapacidad, Correo, NumeroTelefono, Distrito, TipoSangre, FechaRegistro } = req.body;
    const id = req.params.id;
    if (!Nombre || !Apellido || !DNI) return res.status(400).json({ error: "Faltan campos obligatorios" });

    const sql = `UPDATE Participantes SET Nombre = ?, Apellido = ?, DNI = ?, FechaNacimiento = ?, Genero = ?, Discapacidad = ?,
                                          Correo = ?, NumeroTelefono = ?, Distrito = ?, TipoSangre = ?, FechaRegistro = ? WHERE Id = ?`;
    const params = [Nombre, Apellido, DNI, FechaNacimiento, Genero, Discapacidad, Correo, NumeroTelefono, Distrito, TipoSangre, FechaRegistro, id];

    sqliteDB.run(sql, params, function(err) {
        if (err) return res.status(400).json({ error: err.message });
        res.json({ data: { Id: id, ...req.body } });
    });
});

app.delete('/api/participantes/:id', (req, res) => {
    const id = req.params.id;
    sqliteDB.run('DELETE FROM Participantes WHERE Id = ?', id, function(err) {
        if (err) return res.status(400).json({ error: err.message });
        res.json({ message: "Participante eliminado" });
    });
});

// ==================================================
// API REST - DONACIONES (MYSQL)
// ==================================================
app.get('/api/donaciones', async (req, res) => {
    try {
        const [rows] = await mysqlPool.query('SELECT * FROM donaciones');
        res.json({ data: rows });
    } catch (err) {
        res.status(500).json({ error: err.message });
    }
});

app.get('/api/donaciones/:id', async (req, res) => {
    try {
        const [rows] = await mysqlPool.query('SELECT * FROM donaciones WHERE id = ?', [req.params.id]);
        if (rows.length === 0) return res.status(404).json({ error: "Donación no encontrada" });
        res.json({ data: rows[0] });
    } catch (err) {
        res.status(500).json({ error: err.message });
    }
});

app.post('/api/donaciones', async (req, res) => {
    try {
        const { nombre_patrocinador, monto, fecha, ruc } = req.body;
        if (!nombre_patrocinador || !monto || !fecha) {
            return res.status(400).json({ error: "Faltan campos obligatorios" });
        }

        const [result] = await mysqlPool.query(
            'INSERT INTO donaciones (nombre_patrocinador, monto, fecha, ruc) VALUES (?, ?, ?, ?)',
            [nombre_patrocinador, monto, fecha, ruc]
        );

        res.json({ data: { id: result.insertId, ...req.body } });
    } catch (err) {
        res.status(500).json({ error: err.message });
    }
});

app.put('/api/donaciones/:id', async (req, res) => {
    try {
        const { nombre_patrocinador, monto, fecha, ruc } = req.body;
        if (!nombre_patrocinador || !monto || !fecha) {
            return res.status(400).json({ error: "Faltan campos obligatorios" });
        }

        const [result] = await mysqlPool.query(
            'UPDATE donaciones SET nombre_patrocinador = ?, monto = ?, fecha = ?, ruc = ? WHERE id = ?',
            [nombre_patrocinador, monto, fecha, ruc, req.params.id]
        );

        if (result.affectedRows === 0) {
            return res.status(404).json({ error: "Donación no encontrada" });
        }

        res.json({ data: { id: req.params.id, ...req.body } });
    } catch (err) {
        res.status(500).json({ error: err.message });
    }
});

app.delete('/api/donaciones/:id', async (req, res) => {
    try {
        const [result] = await mysqlPool.query('DELETE FROM donaciones WHERE id = ?', [req.params.id]);

        if (result.affectedRows === 0) {
            return res.status(404).json({ error: "Donación no encontrada" });
        }

        res.json({ message: "Donación eliminada" });
    } catch (err) {
        res.status(500).json({ error: err.message });
    }
});

// Iniciar servidor
app.listen(port, () => {
    console.log(`🚀 Servidor corriendo en http://localhost:${port}`);
});
