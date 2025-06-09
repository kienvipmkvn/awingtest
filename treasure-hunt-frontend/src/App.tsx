import React, { useState } from 'react';
import TreasureHuntForm from './components/TreasureHuntForm';
import SubmissionsHistory from './components/SubmissionsHistory';
import { AppBar, Tabs, Tab, Box } from '@mui/material';

function App() {
  const [tab, setTab] = useState(0);

  return (
    <Box>
      <AppBar position="static">
        <Tabs value={tab} onChange={(_, v) => setTab(v)} centered>
          <Tab label="Solver" />
          <Tab label="History" />
        </Tabs>
      </AppBar>
      <Box>
        {tab === 0 && <TreasureHuntForm />}
        {tab === 1 && <SubmissionsHistory />}
      </Box>
    </Box>
  );
}

export default App;
