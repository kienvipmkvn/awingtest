import React, { useState } from 'react';
import { Box, Button, Grid, TextField, Typography, Paper, Alert, CircularProgress } from '@mui/material';
import axios from 'axios';

const api = axios.create({
  baseURL: process.env.REACT_APP_API_BASE_URL,
});

const defaultN = 3, defaultM = 3, defaultP = 2;

type ResultType = { minFuel: number; path: string };

const TreasureHuntForm: React.FC = () => {
  const [n, setN] = useState<number>(defaultN);
  const [m, setM] = useState<number>(defaultM);
  const [p, setP] = useState<number>(defaultP);
  const [matrix, setMatrix] = useState<number[][]>(Array(defaultN).fill(0).map(() => Array(defaultM).fill(0)));
  const [error, setError] = useState<string | null>(null);
  const [result, setResult] = useState<ResultType | null>(null);
  const [loading, setLoading] = useState(false);

  // Update matrix size when n, m change
  React.useEffect(() => {
    setMatrix(prev => {
      const newMatrix = Array(n).fill(0).map((_, i) =>
        Array(m).fill(0).map((_, j) => (prev[i] && prev[i][j] !== undefined ? prev[i][j] : 0))
      );
      return newMatrix;
    });
  }, [n, m]);

  const handleMatrixChange = (i: number, j: number, value: string) => {
    const num = Number(value);
    if (isNaN(num)) return;
    setMatrix(prev => {
      const copy = prev.map(row => [...row]);
      copy[i][j] = num;
      return copy;
    });
  };

  const validate = () => {
    if (n < 1 || m < 1 || p < 1) return 'n, m, p must be >= 1';
    if (n > 500 || m > 500 || p > n * m) return 'n, m must be <= 500, p <= n*m';
    for (let i = 0; i < n; i++)
      for (let j = 0; j < m; j++)
        if (matrix[i][j] < 0 || matrix[i][j] > p)
          return `Matrix values must be between 0 and ${p}`;
    return null;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);
    setResult(null);
    const err = validate();
    if (err) {
      setError(err);
      return;
    }
    setLoading(true);
    try {
      const response = await api.post('/api/treasurehunt/solve', {
        n, m, p, matrix
      });
      setResult({ minFuel: response.data.minFuel, path: response.data.path });
    } catch (err: any) {
      setError(err.response?.data || 'Error solving problem');
    } finally {
      setLoading(false);
    }
  };

  return (
    <Paper sx={{ p: 3, maxWidth: 800, margin: 'auto', mt: 4 }}>
      <Typography variant="h5" gutterBottom>Treasure Hunt Solver</Typography>
      <form onSubmit={handleSubmit}>
        <Grid container spacing={2} alignItems="center">
          <Grid>
            <TextField label="Rows (n)" type="number" value={n} onChange={e => setN(Number(e.target.value))} fullWidth inputProps={{ min: 1, max: 500 }} />
          </Grid>
          <Grid>
            <TextField label="Columns (m)" type="number" value={m} onChange={e => setM(Number(e.target.value))} fullWidth inputProps={{ min: 1, max: 500 }} />
          </Grid>
          <Grid>
            <TextField label="Keys (p)" type="number" value={p} onChange={e => setP(Number(e.target.value))} fullWidth inputProps={{ min: 1, max: n * m }} />
          </Grid>
        </Grid>
        <Box mt={3}>
          <Typography variant="subtitle1">Matrix (each cell: 0 to p)</Typography>
          <Box sx={{ overflowX: 'auto' }}>
            <table style={{ borderCollapse: 'collapse', width: '100%' }}>
              <tbody>
                {matrix.map((row, i) => (
                  <tr key={i}>
                    {row.map((val, j) => (
                      <td key={j} style={{ border: '1px solid #ccc', padding: 4 }}>
                        <TextField
                          type="number"
                          value={val}
                          onChange={e => handleMatrixChange(i, j, e.target.value)}
                          inputProps={{ min: 0, max: p, style: { width: 50 } }}
                          size="small"
                        />
                      </td>
                    ))}
                  </tr>
                ))}
              </tbody>
            </table>
          </Box>
        </Box>
        {error && <Alert severity="error" sx={{ mt: 2 }}>{error}</Alert>}
        <Box mt={2}>
          <Button type="submit" variant="contained" color="primary" disabled={loading}>
            {loading ? <CircularProgress size={24} /> : 'Solve'}
          </Button>
        </Box>
      </form>
      {result && (
        <Box mt={4}>
          <Alert severity="success">
            <Typography>Minimum Fuel: <b>{result.minFuel.toFixed(4)}</b></Typography>
            <Typography>Path: {result.path}</Typography>
          </Alert>
        </Box>
      )}
    </Paper>
  );
};

export default TreasureHuntForm; 