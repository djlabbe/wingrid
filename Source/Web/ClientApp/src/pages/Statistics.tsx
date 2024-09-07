import { useEffect, useRef, useState } from "react";
// import { useLoginContext } from "../hooks/useLoginContext";
import { ResponseDto } from "../models/ResponseDto";
import { toastifyError } from "../services/toastService";
import StatisticsDto from "../models/StatisticsDto";
import { get } from "../services/api";
import { ColDef } from "ag-grid-community";
import LoadingContainer from "../components/LoadingContainer";
import { AgGridReact } from "ag-grid-react";

const Statistics = () => {
	// const { loginResult } = useLoginContext();
	const [statistics, setStatistcs] = useState<StatisticsDto[]>();
	const [loadingInitial, setLoadingInitial] = useState(true);
	const gridRef = useRef<AgGridReact<StatisticsDto>>(null);

	useEffect(() => {
		const getFixture = async () => {
			try {
				setLoadingInitial(true);
				const apiResponse = await get<ResponseDto<StatisticsDto[]>>("/api/statistics");
				if (apiResponse.isSuccess) setStatistcs(apiResponse.result);
				else toastifyError(apiResponse.message);
				setLoadingInitial(false);
			} catch (e) {
				console.error(e);
				toastifyError(`${e}`);
				setLoadingInitial(false);
			}
		};
		getFixture();
	}, []);

	const colDefs: ColDef<StatisticsDto>[] = [
		{ field: "user.name", headerName: "Player", suppressMovable: true },
		{ field: "entries", headerName: "Entries", suppressMovable: true },
		{ field: "wins", headerName: "Wins", suppressMovable: true },
		{ field: "winPercentage", headerName: "Winning %", suppressMovable: true },
		{ field: "collegePercentage", headerName: "NCAA Pick %", suppressMovable: true },
		{ field: "proPercentage", headerName: "NFL Pick %", suppressMovable: true },
		{ field: "totalPercentage", headerName: "Overall Pick %", suppressMovable: true },
		{ field: "averageTieBreakerError", headerName: "Avg. TB Error", suppressMovable: true },
	] as ColDef<StatisticsDto>[];

	return (
		<>
			<div className="w-full p-8">
				{loadingInitial && <LoadingContainer />}
				{!loadingInitial && (
					<>
						<div className="flex justify-between">
							<h1 className="mb-2 text-2xl">Statistics</h1>
						</div>
						<div id="myGrid" className="ag-theme-quartz" style={{ height: "80vh" }}>
							<AgGridReact<StatisticsDto> ref={gridRef} rowData={statistics} columnDefs={colDefs} />
						</div>
					</>
				)}
			</div>
		</>
	);
};

export default Statistics;
