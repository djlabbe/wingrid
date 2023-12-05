import { AiOutlineLoading } from "react-icons/ai";

interface Props {
	message?: string;
}

const LoadingContainer = ({ message }: Props) => {
	return (
		<div className="w-full flex flex-col justify-center items-center mt-8">
			<AiOutlineLoading className="text-green-600 h-12 w-12 animate-spin m-4" />
			{message && <h3 className="text-xl">{message}</h3>}
		</div>
	);
};

export default LoadingContainer;
