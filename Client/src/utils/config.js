export const AuthConfig = {
    authority: 'https://localhost:7229',
    client_id: 'shop-spa',
    redirect_uri: 'http://localhost:5173/signin-oidc',
    silent_redirect_uri: 'http://localhost:5173/signin-oidc',   //for refresh token
    post_logout_redirect_uri: 'http://localhost:5173/signout-callback-oidc',
    response_type: 'code',
    scope: 'openid profile user_identity'
};
