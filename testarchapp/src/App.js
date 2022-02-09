import {BrowserRouter as Router, Switch, Route,} from "react-router-dom";
import Login from "./components/Login";
import Inicio from './components/Inicio';

function App() {
  return (<div>
    <Router>
        <Switch>
          <Route path="/Tareas">
            <Inicio/>
          </Route>
          <Route path="/" exact>
            <Login/>
          </Route>
        </Switch>
      </Router>
  </div>);
}

export default App;
