import React from 'react';
import { Link } from "react-router-dom";

function Home() {
    return (
        <div className='container-fluid col-xxl-9 col-md-10 col-sm-11 mt-5'>
            <h1 className='text-center text-primary-emphasis mb-4'>OAuth 2.0 and OIDC using OpenIdDict</h1>
            <p className='text-center' style={{ fontSize: '20px' }}>This demo application is designed to showcase the OAuth 2.0 Authorization Code Flow across three key components: a Client Application, an Identity Server,
                and a Resource Server. Each component plays a specific role in the authorization process, ensuring secure user authentication and API access.</p>
            <div className='d-flex justify-content-center my-4'>
                <Link to={"/resources"} className='btn btn-primary px-5 me-2'>Get Started</Link>
                <Link to={"/flow"} className='btn btn-secondary px-5'>Read More</Link>
            </div>
            <hr className='my-5' />
            <section className='paragraph-section'>
                <h3>1. Client Application</h3>
                <p className='text-primary-emphasis mb-1'><strong>Purpose:</strong></p>
                <ul>
                    <li>The client application is responsible for initiating the OAuth 2.0 authorization flow by redirecting the user to the Identity Server for authentication.</li>
                </ul>
                <p className='text-primary-emphasis mb-1'><strong>Technology:</strong></p>
                <ul>
                    <li>Built using <code>React</code> and configured with <code>oidc-client-ts</code> library for OAuth 2.0 and OpenID Connect.</li>
                </ul>
                <p className='text-primary-emphasis mb-1'><strong>Key Actions:</strong></p>
                <ul>
                    <li>Initiates the Authorization Code Flow by sending the user to the Identity Server's login page.</li>
                    <li>After successful login, it receives an Authorization Code from the Identity Server.</li>
                    <li>The Authorization Code is then exchanged for an Access Token (and optionally a Refresh Token) at the token endpoint of the Identity Server.</li>
                    <li>Uses the Access Token to authenticate API requests to the Resource Server.</li>
                </ul>
            </section>
            <hr className='my-5' />
            <section className='paragraph-section'>
                <h3>2. Identity Server</h3>
                <p className='text-primary-emphasis mb-1'><strong>Purpose:</strong></p>
                <ul>
                    <li>The Identity Server is responsible for handling authentication, issuing access tokens, and managing the OAuth 2.0 and OpenID Connect flows.</li>
                </ul>
                <p className='text-primary-emphasis mb-1'><strong>Technology:</strong></p>
                <ul>
                    <li>Built using <code>ASP.NET 8</code> and configured with <code>ASP.NET Identity</code> for user management and <code>OpenIddict</code> for OAuth 2.0 and OpenID Connect support.</li>
                </ul>
                <p className='text-primary-emphasis mb-1'><strong>Key Actions:</strong></p>
                <ul>
                    <li>Authenticates the user either via a local login (e.g., username/password) or external identity providers (e.g., Google, Facebook).</li>
                    <li>Issues an Authorization Code upon successful user authentication, which is sent back to the Client Application.</li>
                    <li>Verifies and issues Access Tokens (and optionally Refresh Tokens) after the Client Application exchanges the Authorization Code at the token endpoint.</li>
                    <li>Manages scopes, clients, and resources for the OAuth 2.0 flow.</li>
                </ul>
            </section>
            <hr className='my-5' />
            <section className='paragraph-section'>
                <h3>3. Resource Server</h3>
                <p className='text-primary-emphasis mb-1'><strong>Purpose:</strong></p>
                <ul>
                    <li>The Resource Server protects APIs and validates access tokens issued by the Identity Server.</li>
                </ul>
                <p className='text-primary-emphasis mb-1'><strong>Technology:</strong></p>
                <ul>
                    <li>Also built with <code>ASP.NET 8</code>.</li>
                </ul>
                <p className='text-primary-emphasis mb-1'><strong>Key Actions:</strong></p>
                <ul>
                    <li>Receives API requests from the Client Application, which include the Access Token in the authorization header.</li>
                    <li>Validates the Access Token by checking its signature and ensuring it was issued by the correct Identity Server.</li>
                    <li>If the token is valid, grants access to the requested resource. If the token is expired or invalid, it returns a <code>401 Unauthorized</code> response.</li>
                </ul>
            </section>
            <hr className='my-5' />
            <div className='mb-5'>
                <Link to={"/resources"} className='btn btn-primary px-4 me-2'>Get Started</Link>
                <Link to={"/flow"} className='btn btn-secondary px-4'>Read More</Link>
            </div>
        </div>
    )
}

export default Home;