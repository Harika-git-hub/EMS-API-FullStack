const express = require('express');
const sql = require('mssql');
const cors = require('cors');
const app = express();

app.use(cors());
app.use(express.json());

const config = {
    user: 'ems_user',
    password: 'Ems@12345',
    server: 'HARIKAKOPPUSETT\\MSSQLSERVER2025',
    database: 'EMSDB',
    options: {
        encrypt: false,
        trustServerCertificate: true,
        enableArithAbort: true,
        instanceName: 'MSSQLSERVER2025'
    }
};

// Get all employees
app.get('/api/employees', async (req, res) => {
    try {
        const pool = await sql.connect(config);
        const result = await pool.request().query('SELECT * FROM employees');
        res.json(result.recordset);
        pool.close();
    } catch (err) {
        console.log('SQL Error:', err.message);
        res.status(500).json({error: err.message});
    }
});

// Add employee
app.post('/api/employees', async (req, res) => {
    const {firstName, lastName, email, department, designation, salary} = req.body;
    try {
        const pool = await sql.connect(config);
        const result = await pool.request()
      .input('firstName', sql.VarChar, firstName)
      .input('lastName', sql.VarChar, lastName)
      .input('email', sql.VarChar, email)
      .input('department', sql.VarChar, department)
      .input('designation', sql.VarChar, designation)
      .input('salary', sql.Decimal(10,2), salary)
      .query('INSERT INTO employees (firstName, lastName, email, department, designation, salary) OUTPUT INSERTED.id VALUES (@firstName, @lastName, @email, @department, @designation, @salary)');
        res.json({success: true, id: result.recordset[0].id, message: 'Employee added'});
        pool.close();
    } catch (err) {
        console.log('SQL Error:', err.message);
        res.status(500).json({error: err.message});
    }
});

// Delete employee
app.delete('/api/employees/:id', async (req, res) => {
    try {
        const pool = await sql.connect(config);
        await pool.request()
      .input('id', sql.Int, req.params.id)
      .query('DELETE FROM employees WHERE id = @id');
        res.json({success: true, message: 'Employee deleted'});
        pool.close();
    } catch (err) {
        console.log('SQL Error:', err.message);
        res.status(500).json({error: err.message});
    }
});

app.listen(5000, () => console.log('Backend running on http://localhost:5000'));