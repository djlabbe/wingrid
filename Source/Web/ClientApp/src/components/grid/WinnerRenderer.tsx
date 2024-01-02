import { ICellRendererParams } from "ag-grid-community";
import { EntryDto } from "../../models/EntryDto";
import { AiFillCheckCircle } from "react-icons/ai";
export default (props: ICellRendererParams<EntryDto, boolean>) => {
	return props.value === true ? (
		<div className="flex h-full flex-wrap content-center justify-center text-xl">
			<AiFillCheckCircle />
		</div>
	) : null;
};
