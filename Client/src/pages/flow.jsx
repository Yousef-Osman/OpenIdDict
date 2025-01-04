import React from 'react';
import OAuthFlow from '../assets/OAuthFlow.png';

function Flow() {
    return (
        <div className='container-fluid col-xxl-9 col-md-10 col-sm-11 mt-5'>
            <section className='my-5'>
                <h1 className='mb-4 text-primary-emphasis'>OAuth 2.0 Authorization Code Flow</h1>
                <p className='font-size-22'>The OAuth 2.0 Authorization Code Flow involves several steps and endpoints to exchange an authorization code for an access token.
                    Below is a detailed flow, highlighting the endpoints in the process:
                </p>
            </section>

            <section className='mb-5 paragraph-section'>
                <h3 className='text-info-emphasis'>1. Discover OpenID Configuration (Client-side)</h3>
                <p>The client first calls the Identity Server, specifically the discovery endpoint defined by the OpenID Connect specification.</p>
                <pre><code>GET https://localhost:7229/.well-known/openid-configuration</code></pre>
                <p>This request retrieves the OpenID Connect configuration, which includes important details for interacting with
                    the authorization server. It tells the client (like a SPA or backend) where to send authentication requests and how to handle tokens.
                </p>
                <p>Example response (simplified):</p>
                <pre>
                    <span className="json-brace">{"{"}</span><br />
                    &nbsp;&nbsp;<span className="json-key">"issuer"</span>: <span className="json-value">"https://localhost:7229/"</span>,<br />
                    &nbsp;&nbsp;<span className="json-key">"authorization_endpoint"</span>: <span className="json-value">"https://localhost:7229/connect/authorize"</span>,<br />
                    &nbsp;&nbsp;<span className="json-key">"token_endpoint"</span>: <span className="json-value">"https://localhost:7229/connect/token"</span>,<br />
                    &nbsp;&nbsp;<span className="json-key">"end_session_endpoint"</span>: <span className="json-value">"https://localhost:7229/connect/endsession"</span>,<br />
                    &nbsp;&nbsp;<span className="json-key">"userinfo_endpoint"</span>: <span className="json-value">"https://localhost:7229/connect/userinfo"</span>,<br />
                    &nbsp;&nbsp;<span className="json-key">"jwks_uri"</span>: <span className="json-value">"https://localhost:7229/.well-known/jwks"</span><br />
                    <span className="json-brace">{"}"}</span>
                </pre>
            </section>

            <section className='mb-5 paragraph-section'>
                <h3 className='text-info-emphasis'>2. Initiating Authorization Request (Client-side)</h3>
                <p>The client (e.g., a Single Page Application) redirects the user to the Identity Server’s authorize endpoint to start the OAuth flow. The client includes several
                    parameters like <strong>client_id</strong>, <strong>redirect_uri</strong>, <strong>response_type</strong>, <strong>scope</strong>, <strong>state</strong>,
                    and <strong>code_challenge</strong> (for PKCE).
                </p>
                <pre>
                    <code>GET https://localhost:7229/connect/authorize?client_id=shop-spa&redirect_uri=http%3A%2F%2Flocalhost%3A5173%2Fsignin-oidc&response_type=code&scope=openid+profile+user_identity&state=64fbe770e77346a2b84bc9e19bc4a8d6&code_challenge=iDxAlyCUi94YMJySXRAdmMZNVLCyPv2HY0ocBvIDgVw&code_challenge_method=S256</code>
                </pre>
                <p className='mb-1'><strong>Query parameters:</strong></p>
                <ul>
                    <li><strong>client_id:</strong> Identifies the client application.</li>
                    <li><strong>redirect_uri:</strong> The URI where the user will be redirected after login.</li>
                    <li><strong>response_type=code:</strong> Indicates the Authorization Code flow.</li>
                    <li><strong>scope:</strong> Defines the permissions the client is requesting (e.g., openid, profile).</li>
                    <li><strong>state:</strong> A random string to protect against CSRF.</li>
                    <li><strong>code_challenge:</strong> The challenge for the PKCE method.</li>
                    <li><strong>code_challenge_method:</strong> The method used to create the code_challenge (e.g., S256 for SHA-256).</li>
                </ul>
            </section>

            <section className='mb-5 paragraph-section'>
                <h3 className='text-info-emphasis'>3. User Login (Identity Server)</h3>
                <p>If the user is not authenticated, they will be redirected to the Identity server's login page. Here, the user is prompted
                    to enter their credentials and log in. The ReturnUrl parameter ensures the user is redirected back to the authorization
                    request after a successful login.</p>
                <pre>
                    <code>GET https://localhost:7229/account/login?ReturnUrl=%2Fconnect%2Fauthorize%3Fclient_id%3Dshop-spa%26redirect_uri%3Dhttp%253A%252F%252Flocalhost%253A5173%252Fsignin-oidc%26response_type%3Dcode%26scope%3Dopenid%2520profile%2520user_identity%26state%3D64fbe770e77346a2b84bc9e19bc4a8d6%26code_challenge%3DiDxAlyCUi94YMJySXRAdmMZNVLCyPv2HY0ocBvIDgVw%26code_challenge_method%3DS256</code>
                </pre>
                <p className='mb-1'><strong>Query parameters:</strong></p>
                <ul>
                    <li><strong>ReturnUrl:</strong> Tells the Identity Server where to redirect the user after successful login and it contains the authorization
                        endpoint <strong>/connect/authorize</strong> and its query parameters, which are necessary for the OAuth 2.0 Authorization Code flow.</li>
                </ul>
                <p>The query parameters for the <strong>/connect/authorize</strong> endpoint are the same ones from the last request like <strong>client_id</strong>,
                    <strong>redirect_uri</strong>, <strong>response_type</strong>, <strong>scope</strong>, <strong>state</strong>, and <strong>code_challenge</strong> (for PKCE).
                </p>
            </section>

            <section className='mb-5 paragraph-section'>
                <h3 className='text-info-emphasis'>4. User Posts Login Data (Identity Server)</h3>
                <p>The user submits their credentials (e.g., username and password) via a POST request.</p>
                <pre><code>POST https://localhost:7229/Account/Login</code></pre>
                <p>Once the user is authenticated, the authorization server will redirect the user's browser back to the authorize endpoint to generate the authorization code.</p>
                <pre>
                    <code>GET https://localhost:7229/connect/authorize?client_id=shop-spa&redirect_uri=http%3A%2F%2Flocalhost%3A5173%2Fsignin-oidc&response_type=code&scope=openid%20profile%20user_identity&state=64fbe770e77346a2b84bc9e19bc4a8d6&code_challenge=iDxAlyCUi94YMJySXRAdmMZNVLCyPv2HY0ocBvIDgVw&code_challenge_method=S256</code>
                </pre>
                <p>After successfully authenticating the user and generating the authorization code, Identity Server redirects the user back to the specified redirect_uri.</p>
            </section>

            <section className='mb-5 paragraph-section'>
                <h3 className='text-info-emphasis'>5. Authorization Code Received (Client-side)</h3>
                <p>After a successful login, the user is redirected back to the <strong>redirect_uri</strong> with the authorization code in the query string. This is the
                    "authorization code" that the client will exchange for an access token.
                </p>
                <pre>
                    <code>GET http://localhost:5173/signin-oidc?code=6QHsQ5Fyewd8aO6csibqyPWBMTZsuMEo_4yyqsxVBBw&state=64fbe770e77346a2b84bc9e19bc4a8d6&iss=https%3A%2F%2Flocalhost%3A7229%2F</code>
                </pre>
                <p className='mb-1'><strong>Query parameters:</strong></p>
                <ul>
                    <li><strong>code:</strong> This is the authorization code that the client will use to request an access token from the token endpoint.</li>
                    <li><strong>state:</strong> The same state value that was sent in the authorization request. The client can verify it to ensure the request is valid.</li>
                    <li><strong>iss:</strong> The issuer, identifying the authorization server.</li>
                </ul>
            </section>

            <section className='mb-5 paragraph-section'>
                <h3 className='text-info-emphasis'>6. Token Request (Client-side)</h3>
                <p>The client now exchanges the authorization code for an access token. This is done by making a POST request to the token endpoint, passing in the code along with
                    other details like <strong>client_id</strong>, <strong>client_secret</strong> (if applicable), and the <strong>redirect_uri</strong> used in the previous steps.
                </p>
                <pre><code>POST https://localhost:7229/connect/token</code></pre>
                <p className='mb-1'><strong>Request Body:</strong></p>
                <ul>
                    <li><strong>grant_type:</strong> This indicates the flow being used (here, it's authorization_code).</li>
                    <li><strong>code:</strong> The authorization code received in the previous step.</li>
                    <li><strong>redirect_uri:</strong> Must match the one used during the authorization request.</li>
                    <li><strong>client_id:</strong> The client identifier.</li>
                    <li><strong>client_secret:</strong> (if required) The client secret.</li>
                    <li><strong>code_verifier:</strong> The original code verifier used to generate the code challenge (used for PKCE security).</li>
                </ul>
                <p>the server responds with a JSON object containing the access token, id token, (and optionally a refresh token)</p>
                <pre>
                    <span className="json-brace">{"{"}</span><br />
                    &nbsp;&nbsp;<span className="json-key">"access_token"</span>: <span className="json-value">"eyJhbGciOiJSUzI1NiIsImtpZCI6IjY3OUQ4RTQ3NTQ1QzAwQ0JBMjI3Qjc1Rjc4MDNFOTI1OEUyM0I1NTkiLCJ4NXQiOiJaNTJPUjFSY0FNdWlKN2RmZUFQcEpZNGp0VmsiLCJ0eXAiOiJhdCtqd3QifQ.eyJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo3MjI5LyIsImV4cCI6MTczNTkzOTI4OSwiaWF0IjoxNzM1OTM1Njg5LCJzY29wZSI6Im9wZW5pZCBwcm9maWxlIHVzZXJfaWRlbnRpdHkiLCJqdGkiOiJmNTVkMGZkNS0zMzU4LTQyMzQtOTBhYS1kYzYzOWEyMzAwODkiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJhZG1pbiIsInN1YiI6IjM1NDYzYTAzLWFhYjktNGI5OS04YzUyLWM0MGY2NTE2MTkyMCIsImVtYWlsIjoiYWRtaW5AZW1haWwuY29tIiwibmFtZSI6IkFkbWluIiwiZ2l2ZW5fbmFtZSI6IkFkbWluIiwicm9sZSI6ImFkbWluIiwib2lfcHJzdCI6InNob3Atc3BhIiwib2lfYXVfaWQiOiI1YjU5NzNiZC05Yjc0LTRmYTEtOThkYi04MzU1MTg0ZTdmNjYiLCJjbGllbnRfaWQiOiJzaG9wLXNwYSIsIm9pX3Rrbl9pZCI6IjBlZjgzM2UzLWY0YzEtNGVjNi1iMGZlLWU2YjQ5YWUxNzc2YiJ9.cpWkmLi-msiz0c42DmKiv6yotAebaF52VlMxLpLsrudzhGCz1D0SXvx6RMgkBYCMRZ9gFMAlT5NdaRKGUKC_lgt1b8YaENOCyTK0yBtkiyxy3Bsnsz420-IcYWwzPwSFTBpmoQaCmaV7EF8POEZSrCK2-JI7m7uGRvQh8NwJc1269dc1vr7EmJt-OHsSUassiQTafVCR3ImvlzFgK89g2ybf5v7zGuexQEKrjX6Jmtxa6x2x7CMjutewsrh1xYj7_qAqgJatmHyHASG0Ic4gjvAqX7FN9i9hRczOw4VFOx333m1P9eghCNI8zvR_HBVDtFo6qAuEntKL5SuPOZrqFw"</span>,<br />
                    &nbsp;&nbsp;<span className="json-key">"token_type"</span>: <span className="json-value">"Bearer"</span>,<br />
                    &nbsp;&nbsp;<span className="json-key">"expires_in"</span>: <span className="json-value">"3600"</span>,<br />
                    &nbsp;&nbsp;<span className="json-key">"id_token"</span>: <span className="json-value">"eyJhbGciOiJSUzI1NiIsImtpZCI6IjY3OUQ4RTQ3NTQ1QzAwQ0JBMjI3Qjc1Rjc4MDNFOTI1OEUyM0I1NTkiLCJ4NXQiOiJaNTJPUjFSY0FNdWlKN2RmZUFQcEpZNGp0VmsiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo3MjI5LyIsImV4cCI6MTczNTkzNjg4OSwiaWF0IjoxNzM1OTM1Njg5LCJhdWQiOiJzaG9wLXNwYSIsInN1YiI6IjM1NDYzYTAzLWFhYjktNGI5OS04YzUyLWM0MGY2NTE2MTkyMCIsIm9pX2F1X2lkIjoiNWI1OTczYmQtOWI3NC00ZmExLTk4ZGItODM1NTE4NGU3ZjY2IiwiYXpwIjoic2hvcC1zcGEiLCJhdF9oYXNoIjoiVzliT0hfR21mLTlxcWdORHBqUTJNdyIsIm9pX3Rrbl9pZCI6ImRhY2E1YjcyLTQ3ZTUtNDg0OS1iOWM2LWNmMWQ4Mzg1YTQ5ZCJ9.oWV1gL1u-ZE_EpVCUBVRUoSFXbsCjO7rlrxRx-JTo9_IkEmDDsUNDCKbxtZ9d76ks9n7D_a1-JmLBgX81NEpBFIq9ycRGpzRfl5LzVnRPlBYN9cPH1-gZflv4cMfJgkJE87F2Xji6bu8go4nxjiPs6MM_l2Z57f6AsxRe4dpMqTWnCexlw-SSOEIgrlLt1qT8QoTMyZZiOBSCAn_aw1KlrV4LXTM2hNy2rX7GOqfm0omB2ARCJ8dsZra9UIbvHMNHQM1lvo6z1jvqYLwXkPknqyGlj6g-jRZbichoBX_WwvUScCm7T9qfsu7uvkB3ujNtEuD54e1emv5wqR8WBnzEQ"</span>,<br />
                    &nbsp;&nbsp;<span className="json-key">"scope"</span>: <span className="json-value">"openid profile user_identity"</span>,<br />
                    <span className="json-brace">{"}"}</span>
                </pre>
            </section>
            <section className='paragraph-section' style={{marginBottom:100}}>
                <h3 className='text-info-emphasis'>OAuth 2.0 Flow Diagram</h3>
                <img className='col-12' src={OAuthFlow} alt="OAuthFlow" />
            </section>

        </div>
    )
}

export default Flow;