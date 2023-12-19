import { ReactNode, createContext, useEffect, useState } from "react";
import LoginResult from "../models/LoginResult";
import { AUTH_URI, get, post } from "../services/api";
import { LoginResponseDto } from "../models/LoginResponseDto";
import { toastifyError } from "../services/toastService";
import { ResponseDto } from "../models/ResponseDto";

type LoginContextState = {
	login?: (username: string, password: string) => void;
	logout?: () => void;
	loginResult?: LoginResult;
};

export const LoginContext = createContext<LoginContextState>({});

interface Props {
	children?: ReactNode | undefined;
}

export const LoginProvider = ({ children }: Props) => {
	const token = localStorage.getItem("jwt");

	const [state, setState] = useState<LoginResult>({
		isLoggingIn: true,
		loginError: undefined,
	});

	useEffect(() => {
		const fetchUser = async () => {
			setState((prev) => ({ ...prev, isLoggingIn: true, loginError: undefined }));
			try {
				const loginResponse = await get<ResponseDto<LoginResponseDto>>(`${AUTH_URI}/api/auth`);
				setState({
					user: loginResponse.result.user,
					roles: loginResponse.result.roles,
					token: loginResponse.result.token,
					isLoggingIn: false,
				});
			} catch (e) {
				console.error(e);
				setState({
					isLoggingIn: false,
				});
			}
		};
		if (token) {
			fetchUser();
		}
	}, [token]);

	const login = async (username: string, password: string) => {
		setState((prev) => ({ ...prev, isLoggingIn: true, loginError: undefined }));
		try {
			const loginResponse = await post<ResponseDto<LoginResponseDto>>(`${AUTH_URI}/api/auth/login`, {
				username,
				password,
			});

			setToken(loginResponse.result.token);
			setState({
				user: loginResponse.result.user,
				roles: loginResponse.result.roles,
				token: loginResponse.result.token,
				isLoggingIn: false,
			});
		} catch (e) {
			console.error(e);
			setState({
				isLoggingIn: false,
				loginError: `${e}`,
			});
			toastifyError(`${e}`);
		}
	};

	const setToken = (token: string) => {
		localStorage.setItem("jwt", token);
	};

	const logout = async () => {
		localStorage.removeItem("jwt");
		setState({
			isLoggingIn: false,
		});
	};

	return (
		<LoginContext.Provider value={{ login: login, loginResult: state, logout: logout }}>
			{children}
		</LoginContext.Provider>
	);
};
