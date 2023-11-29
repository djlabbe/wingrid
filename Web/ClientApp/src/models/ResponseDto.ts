export interface ResponseDto<T>
{
    isSuccess: boolean;
    message: string;
    result: T;
}