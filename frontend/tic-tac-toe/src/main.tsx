import './index.css'
import { createRoot } from 'react-dom/client'
import {BrowserRouter, Route, Routes} from "react-router-dom";
import SignIn from "./pages/auth/sign-in.tsx";
import SignUp from "./pages/auth/sign-up.tsx";
import Game from "./pages/game";
import Main from "./pages/main";
import {useAlerts} from "./hooks/use-alerts.ts";
import {AlertProvider} from "./contexts/alert-provider.tsx";
import Alert from "./components/alert";
import {Provider} from "react-redux";
import store from "./store";
import {useEffect} from "react";
import {jwtDecode} from "jwt-decode";
import {JWTTokenDecoded} from "./interfaces/jwt-token/jwt-decoded.ts";
import {useUserActions} from "./hooks/use-actions.ts";

export const App = () => {
    const { alerts, removeAlert } = useAlerts();
    const {createUserFromToken} = useUserActions();

    useEffect(() => {
        const authCookie = document.cookie.split('; ').find((row) => row.startsWith('jwt='));

        if (authCookie) {
            const token = authCookie.split('=')[1];
            try {
                const decodedToken = jwtDecode<JWTTokenDecoded>(token);

                if (decodedToken.exp * 1000 < Date.now()) {
                    console.warn('Token has expired');
                    document.cookie = 'jwt=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;';
                } else {
                    createUserFromToken(token);
                }
            } catch (error) {
                console.error('Invalid token', error);
            }
        }
    }, [createUserFromToken]);

    return (
        <>
            <div className="alert-container">
                {alerts.map((alert) => (
                    <Alert key={alert.id} message={alert.message} onClose={() => removeAlert(alert.id)} />
                ))}
            </div>
            <Routes>
                <Route path={"/sign-in"} element={<SignIn />} />
                <Route path={"/sign-up"} element={<SignUp />} />
                <Route path={"/game/:id"} element={<Game />} />
                <Route path={"/"} element={<Main />} />
            </Routes>
        </>
    );
}

createRoot(document.getElementById('root')!).render(
    <Provider store={store}>
        <AlertProvider>
            <BrowserRouter>
                <App />
            </BrowserRouter>
        </AlertProvider>
    </Provider>
)
