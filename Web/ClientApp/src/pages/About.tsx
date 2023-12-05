const About = () => {
	return (
		<div className="py-16 bg-white">
			<div className="container m-auto px-6 text-gray-600 md:px-12 xl:px-6">
				<div className="space-y-6 md:space-y-0 md:flex md:gap-6 lg:items-center lg:gap-12">
					<div className="md:5/12 lg:w-5/12">
						<img src="football2.png" alt="image" loading="lazy" width="" height="" />
					</div>
					<div className="md:7/12 lg:w-6/12">
						<h2 className="text-2xl text-gray-900 font-bold md:text-4xl">
							Predict, Play, Prevail: Pigskin Pick'em Perfection!
						</h2>
						<p className="mt-6 text-gray-600">
							Welcome to Wingrid, where the thrill of football meets the excitement of prediction! Wingrid is your go-to
							destination for an immersive and competitive pigskin picking experience. Whether you're a seasoned
							football aficionado or a casual fan looking to add an extra layer of fun to game day, Wingrid offers a
							user-friendly platform to showcase your predictive prowess.
						</p>
						<p className="mt-4 text-gray-600">
							Join a community of passionate football enthusiasts as you make weekly predictions, competing against
							friends, family, and fellow fans. Dive into the world of strategic decision-making as you choose winners
							for each game, earning points and climbing the leaderboards. Stay engaged throughout the season, as our
							intuitive interface keeps you updated with real-time scores and standings.
						</p>
						<p className="mt-4 text-gray-600">
							Customize your picks, create private leagues <small>(coming soon)</small>, and enjoy friendly banter with
							your rivals through our built-in messaging features. With Wingrid, the game goes beyond the field,
							bringing people together through the shared joy of predicting football outcomes.
						</p>
						<p className="mt-4 text-gray-600">
							Whether you're aiming for bragging rights or simply seeking a new way to enjoy the NFL season, Wingrid is
							your ultimate destination for gridiron excitement. Start making your picks today and experience football
							fandom like never before!
						</p>
					</div>
				</div>
			</div>
		</div>
	);
};

export default About;
