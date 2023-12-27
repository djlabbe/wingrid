import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { useLoginContext } from "../hooks/useLoginContext";
import LoadingButton from "../components/LoadingButton";
import { toastifyError, toastifySuccess } from "../services/toastService";
import { post } from "../services/api";
import { ForgotPasswordRequestDto } from "../models/ForgotPasswordRequestDto";

const ForgotPassword = () => {
	const { loginResult } = useLoginContext();
	const [loading, setLoading] = useState(false);
	const [email, setEmail] = useState("");

	const navigate = useNavigate();

	useEffect(() => {
		if (loginResult?.user) {
			navigate("/dashboard");
		}
	}, [loginResult]);

	const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
		e.preventDefault();
		try {
			setLoading(true);
			var dto: ForgotPasswordRequestDto = {
				email,
			};
			await post(`/api/auth/forgotpassword`, dto);
			setLoading(false);
			toastifySuccess(
				`If a matching account was found, an email was sent to ${email} to allow you to reset your password.`,
			);
			navigate("/");
		} catch (e) {
			setLoading(false);
			toastifyError(`${e}`);
		}
	};

	return (
		<>
			<div className="flex min-h-full flex-1 flex-col justify-center px-6 py-12 lg:px-8">
				<div className="mt-10 sm:mx-auto sm:w-full sm:max-w-sm">
					<form className="space-y-6" method="POST" onSubmit={(e) => handleSubmit(e)}>
						<div>
							<div className="flex items-center justify-between">
								<label htmlFor="email" className="block text-sm font-medium leading-6 text-gray-900">
									Email
								</label>
							</div>
							<div className="mt-2">
								<input
									id="email"
									name="email"
									type="email"
									value={email}
									onChange={(e) => setEmail(e.target.value)}
									autoComplete="email"
									required
									disabled={loading}
									className="block w-full rounded-md border-0 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-green-600 sm:text-sm sm:leading-6"
								/>
							</div>
							<div className="text-sm text-right mt-2"></div>
						</div>
						<LoadingButton type="submit" loading={loading} disabled={loading}>
							Submit
						</LoadingButton>
						<div className="flex justify-between">
							<a href="/login" className="text-sm font-semibold text-green-600 hover:text-green-500">
								Back to Login
							</a>
							<a href="/signup" className="text-sm font-semibold text-green-600 hover:text-green-500">
								Need an account?
							</a>
						</div>
					</form>
				</div>
			</div>
		</>
	);
};

export default ForgotPassword;
