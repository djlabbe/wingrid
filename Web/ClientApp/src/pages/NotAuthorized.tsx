import { AiFillWarning } from "react-icons/ai";

const NotAuthorized = () => {
	return (
		<div className="w-full flex flex-col justify-center items-center mt-8">
			<AiFillWarning className="text-amber-600 h-12 w-12 m-4" />
			<h3 className="text-xl">Not Authorized</h3>
		</div>
	);
};

export default NotAuthorized;
