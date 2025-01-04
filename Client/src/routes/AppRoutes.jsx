import React from 'react';
import { Routes, Route } from 'react-router-dom';
import Home from '../pages/home.jsx';
import Flow from '../pages/flow.jsx';
import Resources from '../pages/Resources.jsx';
import UnAuthenticated from '../pages/unauthenticated.jsx';
import OAuthCallback from '../pages/oauth-callback.jsx';
import OAuthLogout from '../pages/oauth-logout.jsx';

const AppRoutes = () => {
    return (
        <Routes>
            <Route path={'/'} element={<Home />} />
            <Route path={'/flow'} element={<Flow />} />
            <Route path={'/resources'} element={<Resources />} />
            <Route path={'/unauthorized'} element={<UnAuthenticated />} />
            <Route path={'/signin-oidc'} element={<OAuthCallback />} />
            <Route path={'/signout-callback-oidc'} element={<OAuthLogout />} />
        </Routes>
    );
};

export default AppRoutes;
