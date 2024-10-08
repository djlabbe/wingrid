import { Avatar, Dropdown, Navbar } from "flowbite-react";
import { useLoginContext } from "../hooks/useLoginContext";
import { NavLink, useNavigate } from "react-router-dom";

const NavBar = () => {
	const { loginResult, logout } = useLoginContext();
	const navigate = useNavigate();

	const handleSignOut = async () => {
		if (logout) logout();
		navigate("/");
	};

	const user = loginResult?.user;
	const isAdmin = loginResult?.roles?.includes("ADMIN");

	return (
		<Navbar fluid className="bg-gray-200 dark:bg-gray-900 print:hidden">
			<div className="flex w-full flex-wrap items-center justify-between">
				<div className="flex flex-wrap items-center ">
					<Navbar.Toggle className="me-2" />
					<div className="flex flex-wrap items-center">
						<Navbar.Brand as={NavLink} to={user ? "/dashboard" : "/"} className="me-8">
							<img src="/logo.svg" className="mr-3 h-6 sm:h-9" alt="Wingrid Logo" />
							<span className="self-center whitespace-nowrap text-xl font-semibold dark:text-white">WINGRID</span>
						</Navbar.Brand>
					</div>

					{user && (
						<Navbar.Collapse>
							<Navbar.Link as={NavLink} to="/dashboard" className="md:hover:text-green-600">
								Dashboard
							</Navbar.Link>
							<Navbar.Link as={NavLink} to="/statistics" className="md:hover:text-green-600">
								Statistics
							</Navbar.Link>
							{isAdmin && (
								<>
									<Navbar.Link as={NavLink} to="/admin" className="md:hover:text-green-600">
										Create Fixture
									</Navbar.Link>

									<a
										href={`/hangfire?jwt_token=${loginResult.token}`}
										target="_blank"
										className="block border-b border-gray-100 py-2 pl-3 pr-4 text-gray-700 hover:bg-gray-50 dark:border-gray-700 dark:text-gray-400 dark:hover:bg-gray-700 dark:hover:text-white md:border-0 md:p-0 md:hover:bg-transparent md:hover:text-green-600 md:dark:hover:bg-transparent md:dark:hover:text-white"
									>
										Jobs
									</a>
								</>
							)}
							<Navbar.Link as={NavLink} to="/about" className="md:hover:text-green-600">
								About
							</Navbar.Link>
						</Navbar.Collapse>
					)}
				</div>
				<div className="self-start sm:order-last">
					{user && (
						<Dropdown arrowIcon={false} inline label={<Avatar alt="User settings" img="/avatar.svg" rounded />}>
							<Dropdown.Header>
								<span className="block text-sm">{user.name}</span>
								<span className="block truncate text-sm font-medium">{user.email}</span>
							</Dropdown.Header>
							<Dropdown.Item onClick={handleSignOut}>Sign out</Dropdown.Item>
						</Dropdown>
					)}
				</div>
			</div>
		</Navbar>
	);
};

export default NavBar;
