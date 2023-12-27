import { UserDto } from "./UserDto";

interface LoginResult {
    // loginResponse?: LoginResponseDto;
    user?: UserDto;
    roles?: string[];
    token?: string;
    isLoggingIn: boolean;
    loginError?: string;
}

export default LoginResult;