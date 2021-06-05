import React from 'react';
import './index.css';
import { useRoutes } from './Routes';
import { BrowserRouter as Router} from 'react-router-dom'


function App() {

  const routes = useRoutes(true);

  return (
    <Router>
    <div className = "container">
     {routes}
    </div>
    </Router>
  );
}

export default App;
