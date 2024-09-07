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
		{
			field: "winPercentage",
			headerName: "Win PCT",
			valueFormatter: (params) => {
				params.data?.winPercentage && params.data?.winPercentage < 1
					? params.data.winPercentage.toFixed(3).substring(1, 5)
					: params.data?.winPercentage?.toFixed(3);
			},
		},
		{
			field: "collegePercentage",
			headerName: "NCAA PCT",
			valueFormatter: (params) => {
				params.data?.collegePercentage && params.data?.collegePercentage < 1
					? params.data.collegePercentage.toFixed(3).substring(1, 5)
					: params.data?.collegePercentage?.toFixed(3);
			},
		},
		{
			field: "proPercentage",
			headerName: "NFL PCT",
			valueFormatter: (params) => {
				params.data?.proPercentage && params.data?.proPercentage < 1
					? params.data.proPercentage.toFixed(3).substring(1, 5)
					: params.data?.proPercentage?.toFixed(3);
			},
		},
		{
			field: "totalPercentage",
			headerName: "Overall PCT",
			valueFormatter: (params) => {
				params.data?.totalPercentage && params.data?.totalPercentage < 1
					? params.data.totalPercentage.toFixed(3).substring(1, 5)
					: params.data?.totalPercentage?.toFixed(3);
			},
		},
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
						<div className="mt-3 text-center text-xs">Updated statistics are tabulated weekly.</div>
					</>
				)}
			</div>
		</>
	);
};

export default Statistics;
