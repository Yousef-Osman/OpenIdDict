import React, { useEffect, useState } from 'react';
import { getUser, logout } from '../services/AuthService';
import { getResources } from '../services/ResourcesService';
import { Navigate } from "react-router-dom";

function Resources() {
    const [isAuthenticated, setIsAuthenticated] = useState(false);
    const [user, setUser] = useState(null);
    const [data, setData] = useState(null);
    const [isLoading, setIsLoading] = useState(true);

    async function fetchData() {
        const user = await getUser();
        const accessToken = user?.access_token;

        setUser(user);

        if (accessToken) {
            setIsAuthenticated(true);

            const data = await getResources(accessToken);
            setData(data);
        }

        setIsLoading(false);
    }

    useEffect(() => {
        fetchData();
    }, [isAuthenticated]);

    if (isLoading) {
        return (<div className='Text-center'>Loading...</div>)
    }

    if (!user?.access_token)
        return <Navigate to='/unauthorized' replace />;

    return (
        <div className='container'>
            <h3 className='text-center'>Store Data</h3>
            <hr></hr>
            {
                data &&
                <fieldset disabled>
                    <h4>Resource Server Result</h4>
                    <div className="mb-3">
                        <label htmlFor="resources" className="form-label">Data</label>
                        <input type="text" id="resources" className="form-control" value={data} readOnly />
                    </div>
                </fieldset>
            }
            <hr></hr>
            {
                user &&
                <fieldset disabled>
                    <h4>Identity Server Result</h4>
                    <div className="mb-3">
                        <label htmlFor="access_token" className="form-label">Access Token</label>
                        <textarea type="text" id="access_token" className="form-control" value={user.access_token} rows={4} readOnly />
                    </div>
                    <div className="mb-3">
                        <label htmlFor="id_token" className="form-label">Id Token</label>
                        <textarea type="text" id="id_token" className="form-control" value={user.id_token} rows={4} readOnly />
                    </div>
                    <div className="mb-3">
                        <label htmlFor="token_type" className="form-label">Token Type</label>
                        <input type="text" id="scope" className="form-control" value={user.token_type} readOnly />
                    </div>
                    <div className="mb-3">
                        <label htmlFor="scope" className="form-label">Scope</label>
                        <input type="text" id="scope" className="form-control" value={user.scope} readOnly />
                    </div>
                </fieldset>
            }
        </div>
    )
}

export default Resources;