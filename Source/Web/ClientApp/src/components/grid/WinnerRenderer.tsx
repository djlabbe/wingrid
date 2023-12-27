import { ICellRendererParams } from "ag-grid-community";
import { EntryDto } from "../../models/EntryDto";
import { AiFillCheckCircle } from "react-icons/ai";
export default (props: ICellRendererParams<EntryDto, boolean>) => {
	console.log(props.value);
	return props.value === true ? (
		<div className="flex flex-wrap justify-center content-center h-full text-xl">
			<AiFillCheckCircle />
		</div>
	) : null;
};
