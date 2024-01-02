const Hero = () => {
	return (
		<section className="bg-white dark:bg-gray-900">
			<div className="mx-auto max-w-screen-xl px-4 py-8 text-center lg:py-16">
				<h1 className="mb-4 text-4xl font-extrabold leading-none tracking-tight text-gray-900 dark:text-white md:text-5xl lg:text-6xl">
					Welcome to WINGRID
				</h1>
				<p className="mb-8 text-lg font-normal text-gray-500 dark:text-gray-400 sm:px-16 lg:px-48 lg:text-xl">
					$20+ and massive bragging rights available EVERY WEEK of the NFL season!
				</p>
				<div className="flex flex-col space-y-4 sm:flex-row sm:justify-center sm:space-y-0">
					<a
						href="/signup"
						className="inline-flex items-center justify-center rounded-lg bg-green-700 px-5 py-3 text-center text-base font-medium text-white hover:bg-green-800 focus:ring-4 focus:ring-green-500 focus:ring-offset-green-200"
					>
						Sign Up
						<svg
							className="ms-2 h-3.5 w-3.5 rtl:rotate-180"
							aria-hidden="true"
							xmlns="http://www.w3.org/2000/svg"
							fill="none"
							viewBox="0 0 14 10"
						>
							<path
								stroke="currentColor"
								strokeLinecap="round"
								strokeLinejoin="round"
								strokeWidth="2"
								d="M1 5h12m0 0L9 1m4 4L9 9"
							/>
						</svg>
					</a>
					<a
						href="/about"
						className="inline-flex items-center justify-center rounded-lg border border-gray-300 px-5 py-3 text-center text-base font-medium text-gray-900 hover:bg-gray-100 focus:ring-4 focus:ring-gray-100 dark:border-gray-700 dark:text-white dark:hover:bg-gray-700 dark:focus:ring-gray-800 sm:ms-4"
					>
						Learn More
					</a>
				</div>
				<p className="mb-3 mt-8 text-sm font-normal text-gray-500 dark:text-gray-400 lg:text-sm">
					Already have an account?
					<a href="/login" className="ms-1 text-green-700">
						Log In
					</a>
				</p>
			</div>
		</section>
	);
};

export default Hero;
