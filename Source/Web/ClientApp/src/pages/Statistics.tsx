import { useEffect, useRef, useState } from "react";
import { ResponseDto } from "../models/ResponseDto";
import { toastifyError } from "../services/toastService";
import StatisticsDto from "../models/StatisticsDto";
import { get } from "../services/api";
import { ColDef, ValueFormatterParams } from "ag-grid-community";
import LoadingContainer from "../components/LoadingContainer";
import { AgGridReact } from "ag-grid-react";

const Statistics = () => {
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
		{ field: "user.name", headerName: "Player", pinned: "left", suppressMovable: true },
		{ field: "entries", headerName: "Entries" },
		{ field: "wins", headerName: "Wins" },
		{
			field: "winPercentage",
			headerName: "Win %",
			valueFormatter: (params: ValueFormatterParams<StatisticsDto>) => {
				if (!params.data?.winPercentage) return undefined;
				const pct = params.data.winPercentage * 100;
				return pct.toFixed(1);
			},
		},
		{
			headerName: "Correct Picks (%)",
			children: [
				{
					field: "collegePercentage",
					headerName: "NCAA",
					valueFormatter: (params: ValueFormatterParams<StatisticsDto>) => {
						if (!params.data?.collegePercentage) return undefined;
						const pct = params.data.collegePercentage * 100;
						return pct.toFixed(1);
					},
				},
				{
					field: "proPercentage",
					headerName: "NFL",
					valueFormatter: (params: ValueFormatterParams<StatisticsDto>) => {
						if (!params.data?.proPercentage) return undefined;
						const pct = params.data.proPercentage * 100;
						return pct.toFixed(1);
					},
				},
				{
					field: "totalPercentage",
					headerName: "Overall",
					valueFormatter: (params: ValueFormatterParams<StatisticsDto>) => {
						if (!params.data?.totalPercentage) return undefined;
						const pct = params.data.totalPercentage * 100;
						return pct.toFixed(1);
					},
				},
			],
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
