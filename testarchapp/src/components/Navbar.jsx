import React from 'react';
import { withRouter } from 'react-router-dom';

const Navbar = () => {

  const CerrarSesion = () => {
    window.localStorage.removeItem("uSession")
    window.location.reload();
  }

  return <nav className="navbar navbar-custom navbar-fixed-top" role="navigation">
  <div className="container-fluid">
    <div className="navbar-header">
      <button type="button" className="navbar-toggle collapsed" data-toggle="collapse" data-target="#sidebar-collapse"><span className="sr-only">Toggle navigation</span>
        <span className="icon-bar"></span>
        <span className="icon-bar"></span>
        <span className="icon-bar"></span></button>
      <a className="navbar-brand" href="#"><span>Tareas</span>&nbsp;&nbsp;Admin</a>
      <ul className="nav navbar-top-links navbar-right">
        <li className="dropdown">
          <a className="dropdown-toggle count-info" data-toggle="dropdown" href="#">
            <em className="fa fa-user"></em>
          </a>
          <ul className="dropdown-menu dropdown-messages">
            <li>
              <a onClick={() => CerrarSesion()}>
                <div>
                  <em className="fa fa-sign-out"></em> Cerrar sesi&oacute;n
                </div>
              </a>
            </li>
          </ul>
        </li>
      </ul>
    </div>
  </div>
  </nav>;
};

export default withRouter(Navbar);