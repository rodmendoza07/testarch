import React, {Fragment} from 'react';
import { Link, withRouter } from 'react-router-dom';
import Navbar from './Navbar';
import Sidebar from './Sidebar';

const Inicio = (props) => {

    const [tareas, setTareas] = React.useState([]);
    const [allTask, setAllTask] = React.useState([])

    React.useEffect(async() => {
        if (!window.localStorage.getItem("uSession")) {
            return props.history.push("/")
        }
        ObtenerTareas();
    }, [])

    const ObtenerTareas = async () => {

        try {
            const data = await fetch("http://localhost:61881/api/Task", {method: 'GET', headers: { 'Content-Type': 'application/json', 'Authorization': `Bearer ${JSON.parse(window.localStorage.getItem("uSession")).token}` }})
            const datas = await data.json()
            setTareas(datas.response)
            setAllTask(datas.response)   
        } catch (error) {
            const vt = await fetch("http://localhost:61881/api/VT", {method:"POST", headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({"token": `${JSON.parse(window.localStorage.getItem("uSession")).token}`})})
            const vtData = await vt.json()
            if (vtData.code === 401) {
                window.localStorage.removeItem("uSession")
                window.location.reload();
            }
        }
    }

    const FiltradoEstado = (estate) => {
        if (estate === 3) {
            setTareas(allTask)
        } else {
            const tareasFiltro = allTask.filter(item => item.estadoId == estate)
            setTareas(tareasFiltro)
        }
    }

    const Filtrado = async (filtro) => {
        try {
            const data = await fetch(`http://localhost:61881/api/Filtro/Task/${filtro}`, {method: 'GET', headers: { 'Content-Type': 'application/json', 'Authorization': `Bearer ${JSON.parse(window.localStorage.getItem("uSession")).token}` }})
            const datas = await data.json()
            setTareas(datas.response)  
        } catch (error) {
            const vt = await fetch("http://localhost:61881/api/VT", {method:"POST", headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({"token": `${JSON.parse(window.localStorage.getItem("uSession")).token}`})})
            const vtData = await vt.json()
            if (vtData.code === 401) {
                window.localStorage.removeItem("uSession")
                window.location.reload();
            }
        }
    }

  return (
        <Fragment>
            <Navbar/>
            <Sidebar/>
            <div className="col-sm-9 col-sm-offset-3 col-lg-10 col-lg-offset-2 main">
                <div className="row">
                    <ol className="breadcrumb">
                        <li><Link to="/">
                            <em className="fa fa-home"></em>
                        </Link></li>
                        <li className="active">Lista de Tareas</li>
                    </ol>
                </div>
                
                <div className="row">
                    <div className="col-lg-12">
                        <h1 className="page-header">Lista de Tareas</h1>
                    </div>
                </div>
                <div className="row">
                    <div className="col-md-12">
                        <div className="panel panel-default">
                            <div className="panel-heading">
                                Ordenar por 
                                <span className="pull-right panel-button-tab-right">
                                    <span className="pull-right panel-button-tab-right">
                                        <button type="button" className="btn btn-default" onClick={() => Filtrado(3)}>
                                            <i className="fa fa-sort-alpha-asc" aria-hidden="true"></i>
                                        </button>
                                    </span>
                                    <span className="pull-right panel-button-tab-left">
                                        <button type="button" className="btn btn-default" onClick={() => Filtrado(2)}>
                                            <i className="fa fa-sort" aria-hidden="true"></i>&nbsp;&nbsp;
                                            <i className="fa fa-calendar" aria-hidden="true"></i>
                                        </button>
                                    </span>
                                </span>
                                <span className="pull-right panel-button-tab-left">
                                    <span className="pull-right panel-button-tab-right">
                                        <div className="btn-group">
                                            <button type="button" className="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                                <i className="fa fa-filter" aria-hidden="true"></i>
                                                <span className="caret"></span>
                                            </button>
                                            <ul className="dropdown-menu">
                                                <li><a onClick={() => FiltradoEstado(1)}>Pendientes</a></li>
                                                <li><a onClick={() => FiltradoEstado(2)}>Completadas</a></li>
                                                <li role="separator" className="divider"></li>
                                                <li><a onClick={() => FiltradoEstado(3)}>Todas</a></li>
                                            </ul>
                                        </div>
                                    </span>
                                    <span className="pull-right panel-button-tab-left">
                                        <Link className="btn btn-default" to="/Agregar">
                                            <i className="fa fa-plus" aria-hidden="true"></i>&nbsp;&nbsp;Agregar Tarea
                                        </Link>
                                    </span>
                                </span>
                            </div>
                        </div>
                    </div>
                </div>
                <div className="row">
                    {
                        tareas.length == 0 ?
                        (
                            <div className="col-md-12">
                                <div className="panel panel-warning">
                                    <div className="panel-heading">No hay tareas</div>
                                </div>
                            </div>
                        ) : tareas.map(item => (
                            <div key={item.tareaId} className="col-md-4">
                                <div className={item.heading}>
                                    <div className="panel-heading">
                                        <small>{item.nombre + ' ' + item.fecha}</small>
                                        <span className="pull-right">
                                            <Link className="btn btn-default btn-sm" to={`/Editar/${item.GUID}/Tarea`}>
                                                Editar
                                            </Link>
                                        </span>
                                    </div>
                                    <div className="panel-body">
                                        <p>{item.descripcion}</p>
                                    </div>
                                </div>
                            </div>
                        ))
                    }
                    
                </div>
            </div>
        </Fragment>
    );
};

export default withRouter(Inicio);
