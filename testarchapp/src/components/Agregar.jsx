import React, {Fragment} from 'react';
import { Link, withRouter } from 'react-router-dom';
import Navbar from './Navbar';
import Sidebar from './Sidebar';
import Swal from 'sweetalert2';
import withReactContent from 'sweetalert2-react-content';

const Agregar = (props) => {

    const [titulo, setTitulo] = React.useState('')
    const [descripcion, setDescripcion] = React.useState('')
    const [tarea, setTarea] = React.useState({})
    const [errorLabelTitulo, setErrorLabelTitulo] = React.useState('')
    const [errorLabelDesc, setErrorLabelDesc] = React.useState('')
    const [jsonData, setJsonData] = React.useState({})

    React.useEffect(() => {
        VT()
    }, [])

    React.useEffect(() => {
        setTarea({
            nombre: titulo,
            descripcion: descripcion
        })
    }, [titulo, descripcion])

    React.useEffect(() => {
        if (window.localStorage.getItem("uSession")) {
            setJsonData({
                method: 'POST',
                headers: { 'Content-Type': 'application/json', 'Authorization': `Bearer ${JSON.parse(window.localStorage.getItem("uSession")).token}` },
                body: JSON.stringify(tarea)
            })
        } 
    }, [tarea])

    const VT = async () => {

        if (!window.localStorage.getItem("uSession")) {
            return props.history.push("/")
        }

        const vt = await fetch("http://localhost:61881/api/VT", {method:"POST", headers: { 'Content-Type': 'application/json' }, body: JSON.stringify({"token": `${JSON.parse(window.localStorage.getItem("uSession")).token}`})})
        const vtData = await vt.json()
        if (vtData.code === 401) {
            window.localStorage.removeItem("uSession")
            window.location.reload();
        }
    }

    const MySwal = withReactContent(Swal)

    const AgregarTarea = async (e) => {
        e.preventDefault();
        try {
         
            if (!titulo.trim()) {
                setErrorLabelTitulo("Campo obligatorio")
                return
            }
            if (!descripcion.trim()) {
                setErrorLabelDesc("Campo obligatorio")
                return
            }

            const responseData = await fetch("http://localhost:61881/api/Register/Task", jsonData)
            const rd = await responseData.json()

            setTitulo('')
            setDescripcion('')

            await MySwal.fire({
                title: <strong>¡Tarea registrada!</strong>,
                html: <i>Tarea: <strong>{titulo}</strong></i>,
                icon: 'success'
            })

            props.history.push("/Tareas")

        } catch (error) {
            const vvt = await VT()
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
                    <li className="active">Agregar tarea</li>
                </ol>
            </div>

            <div className="row">
                <div className="col-lg-12">
                    <h1 className="page-header">Agregar tarea</h1>
                </div>
            </div>

            <div className="row">
                <div className="col-md-12">
                    <div className="panel panel-default">
                        <div className="panel-heading">
                            Nueva Tarea
                        </div>
                        <form onSubmit={AgregarTarea}>
                            <div className="panel-body">
                                <div className="col-md-12">
                                    <div className="form-group">
                                        
                                        <label>T&iacute;tulo</label>{ errorLabelTitulo && <small className="label label-danger">{errorLabelTitulo}</small>}
                                        <input type="text" className="form-control" onChange={e => (setTitulo(e.target.value))} value={titulo} required/>
                                    </div>
                                </div>
                                <div className="col-md-12">
                                    <div className="form-group">
                                        <label>Descripci&oacute;n</label>{ errorLabelDesc && <small className="label label-danger">{errorLabelDesc}</small>}
                                        <textarea name="" id="" cols="30" rows="10" className="form-control" onChange={e => (setDescripcion(e.target.value))} value={descripcion} required></textarea>
                                    </div>
                                </div>
                                <div className="col-md-6">
                                    <button type="submit" className="btn btn-primary btn-block">Guardar</button>
                                </div>
                                <div className="col-md-6">
                                    <Link className="btn btn-danger btn-block" to="/Tareas">Cancelar</Link>
                                </div>
                            </div>
                        </form>
                    </div>
                </div>
            </div>
        </div> 
    </Fragment>   
  );
};

export default withRouter(Agregar);
