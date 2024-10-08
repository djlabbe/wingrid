import { Navigate, Route, Routes, useLocation, useNavigate } from "react-router-dom";
import SignUp from "./pages/SignUp";
import Login from "./pages/Login";
import { ReactElement, useEffect } from "react";
import { useLoginContext } from "./hooks/useLoginContext";
import NotAuthorized from "./pages/NotAuthorized";
import Dashboard from "./pages/Dashboard";
import LoadingOverlay from "./components/LoadingOverlay";
import Admin from "./pages/Admin/Admin";
import About from "./pages/About";
import Fixture from "./pages/Fixture";
import Grid from "./pages/Grid";
import Hero from "./components/Hero";
import ResetPassword from "./pages/ResetPassword";
import ForgotPassword from "./pages/ForgotPassword";
import Statistics from "./pages/Statistics";

interface AuthRequiredProps {
	authorizedRoles: string[];
	children: ReactElement;
}

const AuthRequired = ({ authorizedRoles, children }: AuthRequiredProps) => {
	const { loginResult } = useLoginContext();
	const canNavigate = loginResult?.roles?.some((ur) => authorizedRoles.some((ar) => ar === ur));
	return canNavigate ? children : <NotAuthorized />;
};

const AppRoutes = () => {
	const location = useLocation();
	const navigate = useNavigate();
	const { loginResult } = useLoginContext();

	useEffect(() => {
		if (loginResult?.user && location.pathname === "/") {
			navigate("/dashboard");
		}
	}, [loginResult]);

	if (loginResult?.isLoggingIn) {
		return <LoadingOverlay message="Signing in" />;
	}

	return (
		<Routes>
			<Route path="signup" element={<SignUp />} />
			<Route path="login" element={<Login />} />
			<Route path="forgotpassword" element={<ForgotPassword />} />
			<Route path="resetpassword" element={<ResetPassword />} />
			<Route path="about" element={<About />} />
			<Route
				path="fixtures/:id"
				element={
					<AuthRequired authorizedRoles={["USER"]}>
						<Fixture />
					</AuthRequired>
				}
			/>
			<Route
				path="grid/:id"
				element={
					<AuthRequired authorizedRoles={["USER"]}>
						<Grid />
					</AuthRequired>
				}
			/>
			<Route
				path="dashboard"
				element={
					<AuthRequired authorizedRoles={["USER"]}>
						<Dashboard />
					</AuthRequired>
				}
			/>
			<Route
				path="statistics"
				element={
					<AuthRequired authorizedRoles={["USER"]}>
						<Statistics />
					</AuthRequired>
				}
			/>
			<Route
				path="/admin"
				element={
					<AuthRequired authorizedRoles={["ADMIN"]}>
						<Admin />
					</AuthRequired>
				}
			/>
			<Route path="/" element={<Hero />} />
			<Route path="*" element={<Navigate to="/" replace />} />
		</Routes>
	);
};

export default AppRoutes;
