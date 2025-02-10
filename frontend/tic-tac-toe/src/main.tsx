import './index.css'
import { createRoot } from 'react-dom/client'
import {BrowserRouter, Route, Routes} from "react-router-dom";
import SignIn from "./pages/auth/sign-in.tsx";
import SignUp from "./pages/auth/sign-up.tsx";
import Game from "./pages/game";

createRoot(document.getElementById('root')!).render(
    <BrowserRouter>
        <Routes>
            <Route path={"/sign-in"} element={<SignIn />} />
            <Route path={"/sign-up"} element={<SignUp />} />
            <Route path="/game/:id" element={<Game />} />
        </Routes>
    </BrowserRouter>
)
