import { ReactNode, createContext, useEffect, useState } from "react";
import LoginResult from "../models/LoginResult";
import { get, post } from "../services/api";
import { LoginResponseDto } from "../models/LoginResponseDto";
import { toastifyError } from "../services/toastService";

type LoginContextState = {
    login?: (username: string, password: string) => void;
    logout?: () => void;
    loginResult?: LoginResult;
}

export const LoginContext = createContext<LoginContextState>({});

interface Props {
    children?: ReactNode | undefined;
}

export const LoginProvider = ({children}: Props) => {
    const token = localStorage.getItem("jwt");

    const [state, setState] = useState<LoginResult>({
        isLoggingIn: false,
        loginError: undefined,
    });

    useEffect(() => {
        const fetchUser = async () => {
            setState((prev) => ({...prev, isLoggingIn: true, loginError: undefined}));
            try {
               const loginResponse = await get<LoginResponseDto>("/api/auth");
               setState({
                   user: loginResponse.user,
                   roles: loginResponse.roles,
                   token: loginResponse.token,
                   isLoggingIn: false,
               });
            } catch (e) {
                console.error(e);
            }
        }
        if(token) {
            fetchUser();
        }
    }, [token])

   
    const login = async (username: string, password: string) => {
        setState((prev) => ({...prev, isLoggingIn: true, loginError: undefined}));
        try {
            const loginResponse = await post<LoginResponseDto>("/api/auth/login", {
                username,
                password
            });
            setToken(loginResponse.token);
            setState({
                user: loginResponse.user,
                roles: loginResponse.roles,
                token: loginResponse.token,
                isLoggingIn: false,
            });
        } catch (e) {
            console.error(e);
            setState({
                isLoggingIn: false,
                loginError: `${e}`
            });
            toastifyError(`${e}`)
        }
    }
     
    const setToken = (token: string) => {
        localStorage.setItem('jwt', token);
    }

    const logout = async () => {
        localStorage.removeItem('jwt');
        try {
            await get("/api/auth/logout");
            setState({
                isLoggingIn: false,
            })
          } catch (e) {
            toastifyError(`${e}`)
            console.error(e);
          }
    }

    return (
        <LoginContext.Provider value={{ login: login, loginResult: state, logout: logout}}>{children}</LoginContext.Provider>
    );
};