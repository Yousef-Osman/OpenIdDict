import { useEffect, useRef } from "react";
import { useNavigate } from "react-router-dom";
import { handleOAuthCallback } from "../services/AuthService"

function OAuthCallback() {
    //rerendering the components does not change isProcessed, but remounting the component does change.
    const isProcessed = useRef(false);
    const navigate = useNavigate();

    useEffect(() => {
        async function processOAuthResponse() {
            //to prevent React.StrictMode from exchanging auth code for the second time
            if (isProcessed.current)
                return;

            isProcessed.current = true;

            try {
                const currentUrl = window.location.href;
                await handleOAuthCallback(currentUrl);

                navigate("/");
            } catch (error) {
                console.error("Error processing OAuth callback:", error);
            }
        }

        processOAuthResponse();
    }, [])
}

export default OAuthCallback;