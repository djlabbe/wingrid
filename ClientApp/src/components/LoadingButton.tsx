import { Button } from "flowbite-react";
import { AiOutlineLoading } from "react-icons/ai";

interface Props {
	onClick?: () => void;
	loading: boolean;
	text: string;
	type?: "button" | "submit" | "reset" | undefined;
	disabled?: boolean;
}

const LoadingButton = ({ onClick, loading, text, disabled, type = undefined }: Props) => {
	return (
		<Button
			type={type}
			className="w-full mt-auto bg-green-700 enabled:hover:bg-green-800"
			onClick={onClick}
			isProcessing={loading}
			processingSpinner={<AiOutlineLoading className="h-6 w-6 animate-spin" />}
			disabled={disabled || loading}
		>
			{text}
		</Button>
	);
};

export default LoadingButton;
