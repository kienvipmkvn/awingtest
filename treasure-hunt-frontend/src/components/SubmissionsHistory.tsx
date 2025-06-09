import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { Box, Typography, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Paper, CircularProgress, Alert } from '@mui/material';

const api = axios.create({
  baseURL: process.env.REACT_APP_API_BASE_URL,
});

type Submission = {
  id: number;
  n: number;
  m: number;
  p: number;
  matrix: number[][];
  result?: {
    minFuel: number;
    path: string;
  } | null;
};

const SubmissionsHistory: React.FC = () => {
  const [submissions, setSubmissions] = useState<Submission[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchData = async () => {
      setLoading(true);
      setError(null);
      try {
        const res = await api.get('/api/treasurehunt/submissions');
        setSubmissions(res.data);
      } catch (err: any) {
        setError('Failed to fetch submissions');
      } finally {
        setLoading(false);
      }
    };
    fetchData();
  }, []);

  return (
    <Box sx={{ p: 3, maxWidth: 1000, margin: 'auto', mt: 4 }}>
      <Typography variant="h5" gutterBottom>Submission History</Typography>
      {loading ? <CircularProgress /> : error ? <Alert severity="error">{error}</Alert> : (
        <TableContainer component={Paper}>
          <Table size="small">
            <TableHead>
              <TableRow>
                <TableCell>ID</TableCell>
                <TableCell>n</TableCell>
                <TableCell>m</TableCell>
                <TableCell>p</TableCell>
                <TableCell>Matrix</TableCell>
                <TableCell>Min Fuel</TableCell>
                <TableCell>Path</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {submissions.map(sub => (
                <TableRow key={sub.id}>
                  <TableCell>{sub.id}</TableCell>
                  <TableCell>{sub.n}</TableCell>
                  <TableCell>{sub.m}</TableCell>
                  <TableCell>{sub.p}</TableCell>
                  <TableCell>
                    <pre style={{ margin: 0, fontSize: 12 }}>{sub.matrix.map(row => row.join(' ')).join('\n')}</pre>
                  </TableCell>
                  <TableCell>{sub.result?.minFuel?.toFixed(4) ?? '-'}</TableCell>
                  <TableCell>{sub.result?.path ?? '-'}</TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
        </TableContainer>
      )}
    </Box>
  );
};

export default SubmissionsHistory; 