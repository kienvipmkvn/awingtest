import React, { useState } from 'react';
import TreasureHuntForm from './components/TreasureHuntForm';
import SubmissionsHistory from './components/SubmissionsHistory';
import { AppBar, Tabs, Tab, Box } from '@mui/material';

const defaultN = 3, defaultM = 3, defaultP = 2;

function App() {
  const [tab, setTab] = useState(0);
  const [n, setN] = useState<number>(defaultN);
  const [m, setM] = useState<number>(defaultM);
  const [p, setP] = useState<number>(defaultP);
  const [matrix, setMatrix] = useState<number[][]>(Array(defaultN).fill(0).map(() => Array(defaultM).fill(0)));
  const handleApply = (n: number, m: number, p: number, matrix: number[][]) => {
    setN(n);
    setM(m);
    setP(p);
    setMatrix(matrix.map(row => [...row])); // deep copy
    setTab(0);
  };
  return (
    <Box>
      <AppBar position="static">
        <Tabs value={tab} onChange={(_, v) => setTab(v)} centered>
          <Tab label="Solver" />
          <Tab label="History" />
        </Tabs>
      </AppBar>
      <Box>
        {tab === 0 && <TreasureHuntForm n={n} setN={setN} m={m} setM={setM} p={p} setP={setP} matrix={matrix} setMatrix={setMatrix} />}
        {tab === 1 && <SubmissionsHistory onApply={handleApply} />}
      </Box>
    </Box>
  );
}

export default App;
