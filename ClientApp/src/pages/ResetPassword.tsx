import { useEffect, useState } from "react";
import { useNavigate, useSearchParams } from "react-router-dom";
import { useLoginContext } from "../hooks/useLoginContext";
import LoadingButton from "../components/LoadingButton";
import { ResetPasswordRequestDto } from "../models/ResetPasswordRequestDto";
import { toastifyError, toastifySuccess } from "../services/toastService";
import { AUTH_URI, post } from "../services/api";

const ResetPassword = () => {
	const { loginResult } = useLoginContext();
	const [searchParams] = useSearchParams();
	const [loading, setLoading] = useState(false);
	const [pw, setPw] = useState("");

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
			var dto: ResetPasswordRequestDto = {
				email: searchParams.get("email") || "",
				password: pw,
				token: searchParams.get("token") || "",
			};
			await post(`${AUTH_URI}/api/auth/resetpassword`, dto);
			setLoading(false);
			toastifySuccess("Password updated successfully. Please login to continue.");
			navigate("/login");
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
								<label htmlFor="password" className="block text-sm font-medium leading-6 text-gray-900">
									New Password
								</label>
							</div>
							<div className="mt-2">
								<input
									id="password"
									name="password"
									type="password"
									value={pw}
									onChange={(e) => setPw(e.target.value)}
									autoComplete="current-password"
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
					</form>
				</div>
			</div>
		</>
	);
};

export default ResetPassword;
