import {BrowserRouter as Router, Switch, Route,} from "react-router-dom";
import Agregar from './components/Agregar';
import EditarTarea from "./components/EditarTarea.jsx";
import Inicio from './components/Inicio';
import Login from "./components/Login";
import Register from "./components/Register";

function App() {
  return (
    <div>
      <Router>
        <Switch>
          <Route path="/Editar/:GUID/Tarea">
            <EditarTarea/>
          </Route>
          <Route path="/Register">
            <Register/>
          </Route>
          <Route path="/Agregar">
              <Agregar/>
          </Route>
          <Route path="/Tareas">
            <Inicio/>
          </Route>
          <Route path="/" exact>
            <Login/>
          </Route>
        </Switch>
      </Router>
    </div>
  );
}

export default App;
