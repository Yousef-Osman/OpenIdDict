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

        const currentTime = Math.floor(Date.now() / 1000);
        
        if(currentTime > user?.expires_at)
            await logout();

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
        return (<div className='text-center mt-5'><p style={{ fontSize: 22 }}>Loading...</p></div>)
    }

    if (!user?.access_token)
        return <Navigate to='/unauthorized' replace />;

    return (
        <div className='container-fluid col-xxl-9 col-md-10 col-sm-11 mt-5'>
            <h1 className='text-primary-emphasis text-center mb-5'>OAuth 2.0 and OIDC Flow Result</h1>
            {
                data &&
                <fieldset disabled>
                    <h3 className='text-info-emphasis mb-3'>Resource Server Result</h3>
                    <div className="mb-3">
                        <label htmlFor="resources" className="form-label font-size-20">Data</label>
                        <input type="text" id="resources" className="form-control" value={data} readOnly />
                    </div>
                </fieldset>
            }
            <hr className='my-4'></hr>
            {
                user &&
                <fieldset disabled>
                    <h3 className='text-info-emphasis mb-3'>Identity Server Result</h3>
                    <div className="mb-3">
                        <label htmlFor="access_token" className="form-label font-size-20">Access Token</label>
                        <textarea type="text" id="access_token" className="form-control" value={user.access_token} rows={4} readOnly />
                    </div>
                    <div className="mb-3">
                        <label htmlFor="id_token" className="form-label font-size-20">Id Token</label>
                        <textarea type="text" id="id_token" className="form-control" value={user.id_token} rows={4} readOnly />
                    </div>
                    <div className="mb-3">
                        <label htmlFor="token_type" className="form-label font-size-20">Token Type</label>
                        <input type="text" id="scope" className="form-control" value={user.token_type} readOnly />
                    </div>
                    <div className="mb-3">
                        <label htmlFor="scope" className="form-label font-size-20">Scope</label>
                        <input type="text" id="scope" className="form-control" value={user.scope} readOnly />
                    </div>
                </fieldset>
            }
        </div>
    )
}

export default Resources;