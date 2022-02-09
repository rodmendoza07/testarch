import React, { useState } from 'react';
import { Link } from 'react-router-dom';

const Sidebar = () => {

    const [nombre, setNombre] = useState('')
    
    React.useEffect(()=> {
        if (window.localStorage.getItem("uSession")) {
            setNombre(JSON.parse(window.localStorage.getItem("uSession")).nombre)
        }
    }, [])

  return <div id="sidebar-collapse" className="col-sm-3 col-lg-2 sidebar">
            <div className="profile-sidebar">
                <div className="profile-usertitle">
                    <div className="profile-usertitle-name">
                        {nombre}
                    </div>
                </div>
                <div className="clear"></div>
            </div>
            <div className="divider"></div>
            
            <ul className="nav menu">
                <li className="active">
                    <Link to="/Tareas">
                        <em className="fa fa-th-list">&nbsp;</em> Tareas
                    </Link>
                </li>
            </ul>
        </div>;
};

export default Sidebar;
