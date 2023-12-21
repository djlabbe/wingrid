import { useCallback, useEffect, useRef, useState } from "react";
import { FixtureDto } from "../models/FixtureDto";
import { GATEWAY_URI, get } from "../services/api";
import { ResponseDto } from "../models/ResponseDto";
import { toastifyError } from "../services/toastService";
import { useParams } from "react-router-dom";
import { CellClassParams, ColDef, ValueGetterParams } from "ag-grid-community";
import { AgGridReact } from "ag-grid-react";
import { EntryDto, EventEntryDto } from "../models/EntryDto";
import "ag-grid-community/styles/ag-grid.css"; // Core CSS
import "ag-grid-community/styles/ag-theme-quartz.css"; // Theme
import { EventDto } from "../models/EventDto";
import LoadingContainer from "../components/LoadingContainer";
import { AiFillPrinter } from "react-icons/ai";
import GridPrint from "./GridPrint";
import WinnerRenderer from "../components/grid/WinnerRenderer";

const Grid = () => {
	const { id } = useParams();
	const [loadingInitial, setLoadingInitial] = useState(true);
	const [fixture, setFixture] = useState<FixtureDto>();
	const entries = fixture?.entries || [];
	const events = fixture?.events || [];
	const gridRef = useRef<AgGridReact<EntryDto>>(null);

	const cellClassRules = (event: EventDto) => {
		return {
			"bg-green-200": (p: CellClassParams<EntryDto>) => {
				const isCorrect = p.data?.eventEntries.find((ee: EventEntryDto) => ee.eventId === event.id)?.isCorrect;
				return isCorrect;
			},
			"bg-neutral-300": (p: CellClassParams<EntryDto>) => {
				const isCorrect = p.data?.eventEntries.find((ee: EventEntryDto) => ee.eventId === event.id)?.isCorrect;
				return event.statusCompleted && isCorrect === false;
			},
		};
	};

	const eventCols = events.map(
		(ev, i) =>
			({
				colId: `e${i + 1}`,
				headerName: `${i + 1}`,
				headerClass: "event-grid-col",
				headerTooltip: `${ev.name} - ${new Date(ev.date).toLocaleString()}`,
				width: 70,
				cellClass: "text-center font-bold text-[12px]",
				cellClassRules: cellClassRules(ev),
				suppressMovable: true,
				valueGetter: (p: ValueGetterParams<EntryDto>) => {
					const isHomeSelected = p.data?.eventEntries.find(
						(ee: EventEntryDto) => ee.eventId === ev.id,
					)?.homeWinnerSelected;
					const selectedTeam = isHomeSelected ? ev.homeTeam.abbreviation : ev.awayTeam.abbreviation;
					return selectedTeam;
				},
			} as ColDef),
	);

	const colDefs: ColDef<EntryDto>[] = [
		{ field: "userName", headerName: "Player", suppressMovable: true },
		{
			headerName: "Events",
			children: [...eventCols],
		},
		{ field: "tiebreaker", headerName: "TB", suppressMovable: true, width: 70 },
		{ field: "score", headerName: "Score", suppressMovable: true, width: 80 },
		{ field: "tiebreakerResult", headerName: "TB Res.", suppressMovable: true, width: 90 },
		{
			field: "winner",
			headerName: "Win",
			suppressMovable: true,
			width: 65,
			cellRenderer: WinnerRenderer,
			cellClass: "text-center text-green-600 justify-center",
		},
	] as ColDef<EntryDto>[];

	useEffect(() => {
		const getFixture = async () => {
			try {
				setLoadingInitial(true);
				const apiResponse = await get<ResponseDto<FixtureDto>>(`${GATEWAY_URI}/api/fixtures/${id}`);
				if (apiResponse.isSuccess) setFixture(apiResponse.result);
				else toastifyError(apiResponse.message);
				setLoadingInitial(false);
			} catch (e) {
				console.error(e);
				toastifyError(`${e}`);
				setLoadingInitial(false);
			}
		};
		getFixture();
	}, [id]);

	const onBtPrint = useCallback(() => {
		print();
	}, [print]);

	return (
		<>
			<div className="w-full p-8 print:hidden">
				{loadingInitial && <LoadingContainer />}
				{!loadingInitial && (
					<>
						<div className="flex justify-between">
							<h1 className="text-2xl mb-2">{fixture?.name}</h1>
							<button className="text-xl print:hidden" onClick={onBtPrint}>
								<AiFillPrinter />
							</button>
						</div>
						<div id="myGrid" className="ag-theme-quartz" style={{ height: "80vh" }}>
							<AgGridReact<EntryDto> ref={gridRef} rowData={entries} columnDefs={colDefs} />
						</div>
						<div className="text-xs text-center mt-3">
							Events are automatically updated every hour at approximately 8 minutes past the hour. Overall results are
							tabulated at midnight (00:00 MST) following the completion of all events.
						</div>
					</>
				)}
			</div>
			<GridPrint entries={entries} events={events} title={fixture?.name || "The Grid"} />
		</>
	);
};

export default Grid;
