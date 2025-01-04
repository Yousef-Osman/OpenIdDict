import React, { useState, useEffect } from 'react';
import { isAuthenticated, sendOAuthRequest } from '../services/AuthService';
import { Navigate } from "react-router-dom";

function UnAuthenticated() {

  const [userIsAuthenticated, setUserIsAuthenticated] = useState(false);

  useEffect(() => {
    const fetchData = async () => {
      setUserIsAuthenticated(await isAuthenticated());
    };

    fetchData();
  }, [userIsAuthenticated]);

  if (userIsAuthenticated)
    return <Navigate to='/' replace />;

  return (
    <div className='text-center mt-5'>
      <h3 className='text-info-emphasis'>You are not authenticated - You must login to access the data</h3>
      <br />
      <button className='btn btn-secondary px-4' onClick={sendOAuthRequest}>Login with OIDC</button>
    </div>
  )
}

export default UnAuthenticated;