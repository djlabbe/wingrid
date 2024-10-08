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
		{ field: "entries", headerName: "Entries", width: 140 },
		{ field: "wins", headerName: "Wins", width: 140 },
		{
			field: "winPercentage",
			headerName: "Win %",
			valueFormatter: (params: ValueFormatterParams<StatisticsDto>) => {
				if (!params.data?.winPercentage) return undefined;
				const pct = params.data.winPercentage * 100;
				return pct.toFixed(1);
			},
			width: 140,
		},
		{
			field: "totalCorrectPicks",
			headerName: "Correct Picks",
			width: 140,
		},
		{
			headerName: "Correct Picks (%)",
			children: [
				{
					field: "totalPercentage",
					headerName: "Overall",
					valueFormatter: (params: ValueFormatterParams<StatisticsDto>) => {
						if (!params.data?.totalPercentage) return undefined;
						const pct = params.data.totalPercentage * 100;
						return pct.toFixed(1);
					},
					width: 140,
				},
				{
					field: "collegePercentage",
					headerName: "NCAA",
					valueFormatter: (params: ValueFormatterParams<StatisticsDto>) => {
						if (!params.data?.collegePercentage) return undefined;
						const pct = params.data.collegePercentage * 100;
						return pct.toFixed(1);
					},
					width: 140,
				},
				{
					field: "proPercentage",
					headerName: "NFL",
					valueFormatter: (params: ValueFormatterParams<StatisticsDto>) => {
						if (!params.data?.proPercentage) return undefined;
						const pct = params.data.proPercentage * 100;
						return pct.toFixed(1);
					},
					width: 140,
				},
			],
		},

		{
			field: "averageTieBreakerError",
			headerName: "Avg. TB Error",
			valueFormatter: (params: ValueFormatterParams<StatisticsDto>) => params.data?.averageTieBreakerError.toFixed(1),
			width: 140,
		},
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
						<div className="mt-3 text-center text-xs">Statistics are updated weekly.</div>
					</>
				)}
			</div>
		</>
	);
};

export default Statistics;
