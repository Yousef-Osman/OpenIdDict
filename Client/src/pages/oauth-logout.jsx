import React, { useState, useEffect } from 'react';
import { isAuthenticated, sendOAuthRequest } from '../services/AuthService';
import { Navigate, Link } from "react-router-dom";

function OAuthLogout() {
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
            <h3 className='text-info-emphasis'>You are logged out</h3>
            <br />
            <Link to={"/"} className='btn btn-secondary px-4'>Home page</Link>
        </div>
    )
}

export default OAuthLogout;