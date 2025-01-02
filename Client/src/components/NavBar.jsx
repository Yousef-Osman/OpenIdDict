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
      <nav class="navbar navbar-expand-lg bg-body-tertiary">
        <div class="container-fluid col-md-10 col-sm-11">
        <Link to={"/"} className="navbar-brand">Store</Link>
          <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
            <span class="navbar-toggler-icon"></span>
          </button>
          <div class="collapse navbar-collapse" id="navbarNav">
            <ul class="navbar-nav me-auto mb-2 mb-lg-0">
              <li class="nav-item">
                <Link to={"/"} className="nav-link">Home</Link>
              </li>
              <li class="nav-item">
              <Link to={"/resources"} className="nav-link">Resources</Link>
              </li>
            </ul>
            <ul class="navbar-nav mb-2 mb-lg-0">
              <li class="nav-item">
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