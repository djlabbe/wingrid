import { ReactNode } from "react";
import { AiOutlineLoading } from "react-icons/ai";

interface Props {
	onClick?: () => void;
	loading?: boolean;
	type?: "button" | "submit" | "reset" | undefined;
	disabled?: boolean;
	children: ReactNode;
	className?: string;
}

const LoadingButton = ({ onClick, loading = false, children, disabled, className, type = "button" }: Props) => {
	return (
		<button
			type={type}
			disabled={disabled}
			onClick={onClick}
			className={`py-2 px-4 flex justify-center items-center bg-green-600 hover:bg-green-700 focus:ring-green-500 focus:ring-offset-green-200 text-white w-full transition ease-in duration-200 text-center text-base shadow-md focus:outline-none focus:ring-2 focus:ring-offset-2 rounded-lg disabled:bg-gray-400 ${className}`}
		>
			{loading ? <AiOutlineLoading className="h-6 w-6 animate-spin" /> : children}
		</button>
	);
};

export default LoadingButton;
