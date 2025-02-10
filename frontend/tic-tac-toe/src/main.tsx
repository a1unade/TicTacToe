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

export const App = () => {
    const { alerts, removeAlert } = useAlerts();

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
    <AlertProvider>
        <BrowserRouter>
            <App />
        </BrowserRouter>
    </AlertProvider>
)
