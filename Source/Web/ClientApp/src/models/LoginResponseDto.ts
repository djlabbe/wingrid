import { UserDto } from "./UserDto";

export interface LoginResponseDto
{
    user: UserDto;
    roles: string[];
    token: string;
}