import { toast } from "react-toastify";

const toastifyError = (message = "Error") => {
	toast.error(message, {
		position: "top-right",
		autoClose: false,
		hideProgressBar: true,
		closeOnClick: true,
		pauseOnHover: false,
		draggable: false,
		progress: undefined,
		theme: "colored",
	});
};

const toastifySuccess = (message = "Success", autoClose: number | false = 5000) => {
	toast.success(message, {
		position: "top-right",
		autoClose: autoClose,
		hideProgressBar: false,
		closeOnClick: true,
		pauseOnHover: false,
		draggable: false,
		progress: undefined,
		theme: "colored",
	});
};

const toastifyInfo = (message = "Info", autoClose: number | false = 5000) => {
	toast.success(message, {
		position: "top-right",
		autoClose: autoClose,
		hideProgressBar: false,
		closeOnClick: true,
		pauseOnHover: false,
		draggable: false,
		progress: undefined,
		theme: "colored",
	});
};

const toastifyWarning = (message = "Warning", autoClose: number | false = 5000) => {
	toast.success(message, {
		position: "top-right",
		autoClose: autoClose,
		hideProgressBar: false,
		closeOnClick: true,
		pauseOnHover: false,
		draggable: false,
		progress: undefined,
		theme: "colored",
	});
};

export { toastifyError, toastifyInfo, toastifySuccess, toastifyWarning };
