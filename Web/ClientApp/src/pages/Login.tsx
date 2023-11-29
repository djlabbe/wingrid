import { useEffect, useState } from "react";
import { LoginRequestDto } from "../models/LoginRequestDto";
import { useNavigate } from "react-router-dom";
import { useLoginContext } from "../hooks/useLoginContext";

const Login = () => {
  const { login, loginResult } = useLoginContext();
  const [formData, setFormData] = useState<LoginRequestDto>({
    username: "",
    password: "",
  });

  const navigate = useNavigate();

  useEffect(() => {
    if (loginResult?.user) {
      navigate("/dashboard");
    }
  }, [loginResult])

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    if (!login) return;
    login(formData.username, formData?.password);
  }

  const handleFieldChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const name = e.target.name;
    const value = e.target.value;
    setFormData({...formData, [name]: value})
  }

    return (
      <>
        <div className="flex min-h-full flex-1 flex-col justify-center px-6 py-12 lg:px-8">

          <div className="mt-10 sm:mx-auto sm:w-full sm:max-w-sm">
            <form className="space-y-6" method="POST" onSubmit={(e) => handleSubmit(e)}>
              <div>
                <label htmlFor="email" className="block text-sm font-medium leading-6 text-gray-900">
                  Email
                </label>
                <div className="mt-2">
                  <input
                    id="username"
                    name="username"
                    type="email"
                    value={formData?.username || ""}
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
                  <div className="text-sm">
                    <a href="#" className="font-semibold text-green-600 hover:text-green-500">
                      Forgot password?
                    </a>
                  </div>
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
                <button
                  type="submit"
                  className="flex w-full justify-center rounded-md bg-green-600 px-3 py-1.5 text-sm font-semibold leading-6 text-white shadow-sm hover:bg-green-500 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-green-600"
                >
                  Log In
                </button>
              </div>
              <div>
                <a href="/signup" className="flex justify-center text-sm font-semibold text-green-600 hover:text-green-500">Need an account?</a>
              </div>
            </form>
          </div>
        </div>
      </>
    )
  }

  export default Login;