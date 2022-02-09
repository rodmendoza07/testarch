import React from 'react';
import { Link, withRouter } from 'react-router-dom';
import Swal from 'sweetalert2';
import withReactContent from 'sweetalert2-react-content';

const Login = (props) => {

    const [usuario, setUsuario] = React.useState('')
    const [pass, setPass] = React.useState('')
    const [data, setData] = React.useState({})
    const [jsonData, setJsonData] = React.useState({})

    React.useEffect(() => {
        setData(
            {
                userName: usuario,
                pwd: pass
            }
        )   
    }, [usuario, pass])

    React.useEffect(() =>{
        setJsonData(
            {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(data)
            }
        )
    }, [data])

    const MySwal = withReactContent(Swal)

    const GetApiToken = async (e) => {
        e.preventDefault()
        
        const responseData = await fetch('http://localhost:61881/api/Login', jsonData);
        const rd = await responseData.json()

        if (rd.code != 200) {
            await MySwal.fire({
                title: <strong>¡Ups!</strong>,
                html: <i>{rd.response}</i>,
                icon: 'error'
            })
            return
        }

        setUsuario('')
        setPass('')
        window.localStorage.removeItem("uSession")
        window.localStorage.setItem("uSession", JSON.stringify(rd))
        props.history.push('/Tareas')
    }

  return (
    <div className="row">
        <div className="col-xs-10 col-xs-offset-1 col-sm-8 col-sm-offset-2 col-md-4 col-md-offset-4">
            <div className="login-panel panel panel-default">
                <div className="panel-heading">
                    <h3>Inicia sesi&oacute;n</h3>
                </div>
                <div className="panel-body">
                    <form onSubmit={GetApiToken}>
                        <fieldset>
                            <div className="form-group">
                                <input className="form-control" placeholder="E-mail" name="email" type="email"  onChange={e => setUsuario(e.target.value)} value={usuario} required/>
                            </div>
                            <div className="form-group">
                                <input className="form-control" placeholder="Password" name="password" type="password" onChange={e => setPass(e.target.value)}  value={pass} required/>
                            </div>
                            <Link to="/Register" className="btn btn-md btn-default">Registrarse</Link>
                            <button type='submit' className="btn btn-md btn-primary pull-right" >Iniciar Sesi&oacute;n</button>
                        </fieldset>
                    </form>
                </div>
            </div>
        </div>
    </div>
  );
};

export default withRouter(Login);
