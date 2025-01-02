import React, { useState, useEffect } from 'react';
import { isAuthenticated, sendOAuthRequest } from '../services/AuthService';
import { Navigate } from "react-router-dom";

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
        <div className='text-center'>
            <h3>You are logged out</h3>
        </div>
    )
}

export default OAuthLogout;