import React, {Fragment} from 'react';
import { Link, withRouter } from 'react-router-dom';
import Navbar from './Navbar';
import Sidebar from './Sidebar';

const Inicio = () => {
  return (
    <Fragment>
        <Navbar/>
        <Sidebar/>
    </Fragment>
  )
}

export default Inicio