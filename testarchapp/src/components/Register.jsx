import React from 'react';
import Swal from 'sweetalert2';
import withReactContent from 'sweetalert2-react-content';
import { Link } from 'react-router-dom';

const Register = () => {

    const [nombres, setNombres] = React.useState('')
    const [papellido, setPapellido] = React.useState('')
    const [sapellido, setSapellido] = React.useState('')
    const [correo, setCorreo] = React.useState('')
    const [correoConfirm, setCorreoConfirm] = React.useState('')
    const [pass, setPass] = React.useState('')
    const [passConfirm, setPassConfirm] = React.useState('')
    const [data, setData] = React.useState({})
    const [jsonData, setJsonData] = React.useState({})
    const [errorLabel, setErrorLabel] = React.useState('')
    const [errorLabelPass, setErrorLabelPass] = React.useState('')

    React.useEffect(() => {
        setData({
            Nombre: nombres,
            Papellido: papellido,
            Sapellido: sapellido,
            Email: correo,
            Pass: pass
        })
    }, [nombres, papellido, sapellido, correo, pass])

    React.useEffect(() => {
        setJsonData({
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(data)
        })
    }, [data])

    const MySwal = withReactContent(Swal)

    const RegisterUser = async (e) => {
        e.preventDefault()
        try {

            if (correo != correoConfirm) {
                setErrorLabel("E-mail no coincide")
                return
            }
            if (pass != passConfirm) {
                setErrorLabelPass("Password no coincide")
                return
            }
    
            const responseData = await fetch("http://localhost:61881/api/Register/User", jsonData)
            const rd = await responseData.json()

            if (rd.code != 200) {
                await MySwal.fire({
                    title: <strong>¡Ups!</strong>,
                    html: <i>{rd.response}</i>,
                    icon: 'error'
                })
                return
            } 

            await MySwal.fire({
                title: <strong>¡Registro exitoso!</strong>,
                html: <i>Regresa al Login para iniciar sesi&oacute;n</i>,
                icon: 'success'
            })

            setNombres('')
            setPapellido('')
            setSapellido('')
            setCorreoConfirm('')
            setCorreo('')
            setPass('')
            setPassConfirm('')
            setErrorLabel(null)

        } catch (error) {
            await MySwal.fire({
                title: <strong>¡Ups!</strong>,
                html: <i>{error.response}</i>,
                icon: 'error'
            })
        }
        
    }

  return (
    <div className="row">
        <div className="col-xs-10 col-xs-offset-1 col-sm-8 col-sm-offset-2 col-md-4 col-md-offset-4">
            <div className="login-panel panel panel-default">
                <div className="panel-heading">
                    <h3>Registro</h3>
                </div>
                <div className="panel-body">
                    <form onSubmit={RegisterUser}>
                        <fieldset>
                            <div className="form-group">
                                <input className="form-control" placeholder="Nombre(s)"  type="text" maxLength="50" onChange={e => setNombres(e.target.value)} value={nombres} required/>
                            </div>
                            <div className="form-group">
                                <input className="form-control" placeholder="Primer apellido" type="text" maxLength="30" onChange={e => setPapellido(e.target.value)} value={papellido} required/>
                            </div>
                            <div className="form-group">
                                <input className="form-control" placeholder="Segundo apellido" type="text" maxLength="30" onChange={e => setSapellido(e.target.value)} value={sapellido} required/>
                            </div>
                            <div className="form-group">
                                <input className="form-control" placeholder="E-mail" name="email" type="email" onChange={e => setCorreo(e.target.value)} value={correo} required/>
                            </div>
                            { errorLabel && <small className="label label-danger">{errorLabel}</small>}
                            <div className="form-group">
                                <input className="form-control" placeholder="Confirma E-mail" name="emailConfirm" type="email" onChange={e => setCorreoConfirm(e.target.value)} value={correoConfirm} required/>
                            </div>
                            <div className="form-group">
                                <input className="form-control" placeholder="Password" name="password" type="password" onChange={e => setPass(e.target.value)} value={pass} required/>
                            </div>
                            { errorLabelPass && <small className="label label-danger">{errorLabelPass}</small>}
                            <div className="form-group">
                                <input className="form-control" placeholder="Confirma Password" name="password" type="password" onChange={e => setPassConfirm(e.target.value)} value={passConfirm} required/>
                            </div>
                            <Link to="/" className="btn btn-md btn-default">Inicia sesi&oacute;n</Link>
                            <button type='submit' className="btn btn-primary pull-right">Registrate</button>
                        </fieldset>
                    </form>
                </div>
            </div>
        </div>
    </div>
  );;
};

export default Register;
