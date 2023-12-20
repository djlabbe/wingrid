const Hero = () => {
	return (
		<section className="bg-white dark:bg-gray-900">
			<div className="py-8 px-4 mx-auto max-w-screen-xl text-center lg:py-16">
				<h1 className="mb-4 text-4xl font-extrabold tracking-tight leading-none text-gray-900 md:text-5xl lg:text-6xl dark:text-white">
					Welcome to WINGRID
				</h1>
				<p className="mb-8 text-lg font-normal text-gray-500 lg:text-xl sm:px-16 lg:px-48 dark:text-gray-400">
					$20+ and massive bragging rights available EVERY WEEK of the NFL season!
				</p>
				<div className="flex flex-col space-y-4 sm:flex-row sm:justify-center sm:space-y-0">
					<a
						href="/signup"
						className="inline-flex justify-center items-center py-3 px-5 text-base font-medium text-center text-white rounded-lg bg-green-700 hover:bg-green-800 focus:ring-4 focus:ring-green-500 focus:ring-offset-green-200"
					>
						Sign Up
						<svg
							className="w-3.5 h-3.5 ms-2 rtl:rotate-180"
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
						className="inline-flex justify-center items-center py-3 px-5 sm:ms-4 text-base font-medium text-center text-gray-900 rounded-lg border border-gray-300 hover:bg-gray-100 focus:ring-4 focus:ring-gray-100 dark:text-white dark:border-gray-700 dark:hover:bg-gray-700 dark:focus:ring-gray-800"
					>
						Learn More
					</a>
				</div>
				<p className="mt-8 mb-3 text-sm font-normal text-gray-500 lg:text-sm dark:text-gray-400">
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
