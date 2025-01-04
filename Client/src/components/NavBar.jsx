import React, { useState, useEffect } from 'react';
import { logout, sendOAuthRequest, isAuthenticated } from "../services/AuthService";
import { Link } from "react-router-dom";

const NavBar = () => {
  const [userIsAuthenticated, setUserIsAuthenticated] = useState(false);

  useEffect(() => {
    const fetchData = async () => {
      setUserIsAuthenticated(await isAuthenticated());
    };

    fetchData();
  }, [userIsAuthenticated]);

  return (
    <>
      <nav className="navbar navbar-expand-lg bg-body-tertiary">
        <div className="container-fluid col-xxl-9 col-md-10 col-sm-11">
        <Link to={"/"} className="navbar-brand">CLIENT</Link>
          <button className="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
            <span className="navbar-toggler-icon"></span>
          </button>
          <div className="collapse navbar-collapse" id="navbarNav">
            <ul className="navbar-nav me-auto mb-2 mb-lg-0">
              <li className="nav-item">
                <Link to={"/"} className="nav-link">Home</Link>
              </li>
              <li className="nav-item">
                <Link to={"/flow"} className="nav-link">Flow</Link>
              </li>
              <li className="nav-item">
              <Link to={"/resources"} className="nav-link">Resources</Link>
              </li>
            </ul>
            <ul className="navbar-nav mb-2 mb-lg-0">
              <li className="nav-item">
                {!userIsAuthenticated && <button className="nav-link" onClick={sendOAuthRequest}>login</button>}
                {userIsAuthenticated && <button className="nav-link" onClick={logout}>Logout</button>}
              </li>
            </ul>
          </div>
        </div>
      </nav>
    </>
  )
}

export default NavBar;