import { Route, Routes, useLocation, useNavigate } from "react-router-dom";
import SignUp from "./pages/SignUp";
import Login from "./pages/Login";
import Splash from "./pages/Splash";
import { ReactElement, useEffect } from "react";
import { useLoginContext } from "./hooks/useLoginContext";
import NotAuthorized from "./pages/NotAuthorized";
import Dashboard from "./pages/Dashboard";
import ContainerLoading from "./components/ContainerLoading";
import Admin from "./pages/Admin";

interface AuthRequiredProps {
    authorizedRoles: string[],
    children: ReactElement;
}

const AuthRequired = ({authorizedRoles, children}: AuthRequiredProps) => {
    const {loginResult} = useLoginContext();
    if (loginResult?.isLoggingIn) {
        return <ContainerLoading message="Signing in"/>
    }
    const canNavigate = loginResult?.roles?.some((ur) => authorizedRoles.some((ar) => ar === ur))
    return canNavigate ? children : <NotAuthorized />
}

const AppRoutes = () => {
    const location = useLocation();
    const navigate = useNavigate();
    const {loginResult} = useLoginContext();

    useEffect(() => {
        if(loginResult?.user && location.pathname === "/") {
            navigate("/dashboard");
        }
    }, [loginResult])

    return (
        <Routes>
            <Route path="/signup" element={<SignUp />} />
            <Route path="/login" element={<Login />} />
            <Route path="/dashboard" element={
                <AuthRequired authorizedRoles={["USER"]}>
                    <Dashboard />
                </AuthRequired>
            }/>
            <Route path="/admin" element={
                <AuthRequired authorizedRoles={["ADMIN"]}>
                    <Admin />
                </AuthRequired>
            }/>
            <Route path="/" element={ <Splash />} />
        </Routes>
    )  
}


export default AppRoutes;