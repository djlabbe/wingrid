import { useState } from "react";
import { AUTH_URI, post } from "../services/api";
import { RegistrationRequestDto } from "../models/RegistrationRequestDto";
import { useNavigate } from "react-router-dom";
import { toastifyError, toastifySuccess } from "../services/toastService";
import { ResponseDto } from "../models/ResponseDto";
import LoadingButton from "../components/LoadingButton";

const SignUp = () => {
	const [formData, setFormData] = useState<RegistrationRequestDto>();
	const [loading, setLoading] = useState(false);
	const navigate = useNavigate();

	const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
		e.preventDefault();
		try {
			setLoading(true);
			await post<ResponseDto<null>>(`${AUTH_URI}/api/auth/register`, formData);
			await post<ResponseDto<null>>(`${AUTH_URI}/api/auth/AssignRole`, { ...formData, role: "USER" });
			setLoading(false);
			toastifySuccess("Registration successful. Please login.");
			navigate("/login");
		} catch (e) {
			setLoading(false);
			toastifyError(`${e}`);
			console.error(e);
		}
	};

	const handleFieldChange = (e: React.ChangeEvent<HTMLInputElement>) => {
		const name = e.target.name;
		const value = e.target.value;
		setFormData({ ...formData, [name]: value });
	};

	return (
		<>
			<div className="flex min-h-full flex-1 flex-col justify-center px-6 py-12 lg:px-8">
				<div className="mt-10 sm:mx-auto sm:w-full sm:max-w-sm">
					<form className="space-y-6" onSubmit={(e) => handleSubmit(e)}>
						<div>
							<label htmlFor="name" className="block text-sm font-medium leading-6 text-gray-900">
								Name
							</label>
							<div className="mt-2">
								<input
									id="name"
									name="name"
									value={formData?.name || ""}
									onChange={(e) => handleFieldChange(e)}
									autoComplete="name"
									required
									className="block w-full rounded-md border-0 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-green-600 sm:text-sm sm:leading-6"
								/>
							</div>
						</div>
						<div>
							<label htmlFor="email" className="block text-sm font-medium leading-6 text-gray-900">
								Email
							</label>
							<div className="mt-2">
								<input
									id="email"
									name="email"
									type="email"
									value={formData?.email || ""}
									onChange={(e) => handleFieldChange(e)}
									autoComplete="email"
									required
									className="block w-full rounded-md border-0 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-green-600 sm:text-sm sm:leading-6"
								/>
							</div>
						</div>

						<div>
							<div className="flex items-center justify-between">
								<label htmlFor="password" className="block text-sm font-medium leading-6 text-gray-900">
									Password
								</label>
							</div>
							<div className="mt-2">
								<input
									id="password"
									name="password"
									type="password"
									value={formData?.password || ""}
									onChange={(e) => handleFieldChange(e)}
									autoComplete="current-password"
									required
									className="block w-full rounded-md border-0 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-green-600 sm:text-sm sm:leading-6"
								/>
							</div>
						</div>

						<div>
							<LoadingButton type="submit" loading={loading} text="Sign Up" />
						</div>
						<div>
							<a
								href="/login"
								className="flex justify-center text-sm font-semibold text-green-600 hover:text-green-500"
							>
								Already have an account?
							</a>
						</div>
					</form>
				</div>
			</div>
		</>
	);
};

export default SignUp;
