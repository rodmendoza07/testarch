import React, {Fragment} from 'react';
import { Link, withRouter, useParams } from 'react-router-dom';
import Navbar from './Navbar';
import Sidebar from './Sidebar';
import Swal from 'sweetalert2';
import withReactContent from 'sweetalert2-react-content';

const EditarTarea = (props) => {

    const [titulo, setTitulo] = React.useState('')
    const [descripcion, setDescripcion] = React.useState('')
    const [estadoTarea, setEstadoTarea] = React.useState(0)
    const [fechaC, setFechaC] = React.useState('')
    const [tarea, setTarea] = React.useState({})
    const [jsonData, setJsonData] = React.useState({})
    const [errorLabelTitulo, setErrorLabelTitulo] = React.useState('')
    const [errorLabelDesc, setErrorLabelDesc] = React.useState('') 
    const [errorLabelEstado, setErrorLabelEstado] = React.useState('')

    const {GUID} = useParams()

    React.useEffect(() => {
        VT()
        GetEstados()
        GetTarea()
    }, [])

    React.useEffect(() => {
      setTarea({
        nombre: titulo,
        descripcion: descripcion,
        Estado: estadoTarea,
        taskId: GUID
      })
    }, [titulo, descripcion, estadoTarea, GUID])

    React.useEffect(() => {
      setJsonData({
        method: 'POST',
        headers: { 'Content-Type': 'application/json', 'Authorization': `Bearer ${JSON.parse(window.localStorage.getItem("uSession")).token}` },
        body: JSON.stringify(tarea)
      })
    }, [tarea])

    const MySwal = withReactContent(Swal)

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

    const GetEstados = async () => {
        const estado = await fetch("http://localhost:61881/api/Estados/Task", {method: 'GET', headers: { 'Content-Type': 'application/json', 'Authorization': `Bearer ${JSON.parse(window.localStorage.getItem("uSession")).token}` }})
        const estadoJson = await estado.json()
        const comboEstado = estadoJson.response
        const s = document.getElementById("estadosTarea")
        s.innerHTML = ""
        const opt = document.createElement("option")
        opt.text = "--- Seleccione ---"
        opt.value = "0"
        s.appendChild(opt)
        comboEstado.map(item => {
          const option = document.createElement("option")
          option.text = item.estadoDesc
          option.value = item.estadoId
          s.appendChild(option)
        })
    }

    const GetTarea = async () => {
      const tResponse = await fetch(`http://localhost:61881/api/Task/${GUID}`, {method: 'GET', headers: { 'Content-Type': 'application/json', 'Authorization': `Bearer ${JSON.parse(window.localStorage.getItem("uSession")).token}` }})
      const tr = await tResponse.json()
      const trr = tr.response
      trr.map(item => {
        setTitulo(item.nombre)
        setDescripcion(item.descripcion)
        setFechaC(item.fecha)
        setEstadoTarea(item.estadoId)
      })
    }

    const UpdateTarea = async (e) => {
      e.preventDefault()
      try {
        if (!titulo.trim()) {
          setErrorLabelTitulo("Campo obligatorio")
          return
        }
        if (!descripcion.trim()) {
            setErrorLabelDesc("Campo obligatorio")
            return
        }
        if (estadoTarea === "0") {
            setErrorLabelEstado("Campo obligatorio")
            return
        }

        const dt = await fetch('http://localhost:61881/api/Update/Task', jsonData)
        const rdt = dt.json()

        await MySwal.fire({
          title: <strong>¡Tarea actualizada!</strong>,
          html: <i>Tarea: <strong>{titulo}</strong></i>,
          icon: 'success'
        })

        props.history.push("/Tareas")

      } catch (error) {
        const vvt = await VT()
      }
    }

    const DeleteTarea = async () => {

      Swal.fire({
        title: '¿Seguro de elimar esta tarea?',
        text: "Esta acción no se puede deshacer",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Si, seguro',
        cancelButtonText: 'Cancelar'
      }).then(async(result) => {
        if (result.isConfirmed) {
          try {
            const dt = await fetch('http://localhost:61881/api/Delete/Task', jsonData)
            const rdt = dt.json()
            Swal.fire(
              'Tarea eliminada',
              '',
              'success'
            )
            props.history.push("/Tareas") 
          } catch (error) {
            const vvt = await VT()
          }
        }
      })
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
                    <li className="active">Editar tarea</li>
                </ol>
            </div>

            <div className="row">
                <div className="col-lg-12">
                    <h1 className="page-header">Editar tarea</h1>
                </div>
            </div>

            <div className="row">
                <div className="col-md-12">
                    <div className="panel panel-default">
                        <div className="panel-heading">
                            Editar Tarea
                            <span className="pull-right panel-button-tab-right">
                              Fecha de creaci&oacute;n: {fechaC}
                          </span>
                        </div>
                        <form onSubmit={UpdateTarea}>
                            <div className="panel-body">
                                <div className="col-md-8">
                                    <div className="form-group">
                                        <label>T&iacute;tulo</label> { errorLabelTitulo && <small className="label label-danger">{errorLabelTitulo}</small>}
                                        <input type="text" className="form-control" onChange={e => (setTitulo(e.target.value))} value={titulo} required/>
                                    </div>
                                </div>
                                <div className="col-md-4">
                                    <div className="form-group">
                                        <label>Estado</label> { errorLabelEstado && <small className="label label-danger">{errorLabelEstado}</small>}
                                        <select className='form-control' id="estadosTarea" onChange={e => (setEstadoTarea(e.target.value))} value={estadoTarea} required>
                                            <option value="0">--Selecciona--</option>
                                        </select>
                                    </div>
                                </div>
                                <div className="col-md-12">
                                    <div className="form-group">
                                        <label>Descripci&oacute;n</label> { errorLabelDesc && <small className="label label-danger">{errorLabelDesc}</small>}
                                        <textarea name="" id="" cols="30" rows="10" className="form-control" onChange={e => (setDescripcion(e.target.value))} value={descripcion} required></textarea>
                                    </div>
                                </div>
                                <div className="col-md-4">
                                    <button type="submit" className="btn btn-primary btn-block">Actualizar</button>
                                </div>
                                <div className="col-md-4">
                                    <button type="button" className="btn btn-danger btn-block" onClick={DeleteTarea}>Eliminar</button>
                                </div>
                                <div className="col-md-4">
                                    <Link className="btn btn-warning btn-block" to="/Tareas">Cancelar</Link>
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

export default withRouter(EditarTarea);
