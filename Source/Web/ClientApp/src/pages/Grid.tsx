import { useCallback, useEffect, useRef, useState } from "react";
import { FixtureDto } from "../models/FixtureDto";
import { get } from "../services/api";
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
import { useLoginContext } from "../hooks/useLoginContext";

const Grid = () => {
	const { loginResult } = useLoginContext();
	const { id } = useParams();
	const [loadingInitial, setLoadingInitial] = useState(true);
	const [fixture, setFixture] = useState<FixtureDto>();
	const tbEventId = fixture?.tiebreakerEventId;
	const entries = fixture?.entries || [];
	const myEntry = entries.find((e) => e.userId == loginResult?.user?.id);
	const otherEntries = entries.filter((e) => e.userId !== loginResult?.user?.id);
	const events = fixture?.events || [];
	const gridRef = useRef<AgGridReact<EntryDto>>(null);

	const isAdmin = loginResult?.roles?.includes("ADMIN");

	const cellClassRules = (event: EventDto) => {
		return {
			"bg-amber-200": (p: CellClassParams<EntryDto>) => {
				const isCorrect = p.data?.eventEntries.find((ee: EventEntryDto) => ee.eventId === event.id)?.isCorrect;
				return isCorrect;
			},
			"bg-neutral-100": (p: CellClassParams<EntryDto>) => {
				const isCorrect = p.data?.eventEntries.find((ee: EventEntryDto) => ee.eventId === event.id)?.isCorrect;
				return event.statusCompleted && isCorrect === false;
			},
		};
	};

	const getEventToolTip = (ev: EventDto) => {
		if (ev.statusId === "3") {
			return `${ev.awayTeam.displayName} ${ev.awayScore} @ ${ev.homeTeam.displayName} ${ev.homeScore}`;
		}
		return `${ev.name} - ${new Date(ev.date).toLocaleString()}`;
	};

	const eventCols = events.map(
		(ev, i) =>
			({
				colId: `e${i + 1}`,
				headerName: `${i + 1}${ev.id === tbEventId ? "*" : ""}`,
				headerClass: "event-grid-col",
				headerTooltip: getEventToolTip(ev),
				width: 70,
				cellClass: "text-center font-bold text-[12px]",
				cellClassRules: cellClassRules(ev),
				suppressMovable: true,
				valueGetter: (p: ValueGetterParams<EntryDto>) => {
					const isHomeSelected = p.data?.eventEntries.find((ee: EventEntryDto) => ee.eventId === ev.id)
						?.homeWinnerSelected;
					const selectedTeam = isHomeSelected ? ev.homeTeam.abbreviation : ev.awayTeam.abbreviation;
					return selectedTeam;
				},
			}) as ColDef,
	);

	const colDefs: ColDef<EntryDto>[] = [
		{ field: "userName", headerName: "Player", pinned: "left", width: 180, suppressMovable: true },
		{
			headerName: "Events",
			children: [...eventCols],
		},
		{ field: "tiebreaker", headerName: "Tb", headerTooltip: "Predicted Total Score", suppressMovable: true, width: 70 },
		{ field: "score", headerName: "Score", headerTooltip: "Number of Correct Picks", suppressMovable: true, width: 80 },
		{
			field: "tiebreakerResult",
			headerName: "Tb Err.",
			headerTooltip: "Difference between predicted total score and actual total score",
			suppressMovable: true,
			width: 90,
		},
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
				const apiResponse = await get<ResponseDto<FixtureDto>>(`/api/fixtures/${id}`);
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
				{!loadingInitial && !fixture?.locked && !isAdmin && (
					<p>Please check back after {fixture?.deadline} to view the grid!</p>
				)}
				{!loadingInitial && (fixture?.locked || isAdmin) && (
					<>
						<div className="flex justify-between">
							<h1 className="mb-2 text-2xl">
								{fixture?.name} ({entries.length} entries)
							</h1>
							<button className="text-xl print:hidden" onClick={onBtPrint}>
								<AiFillPrinter />
							</button>
						</div>
						<div id="myGrid" className="ag-theme-quartz" style={{ height: "80vh" }}>
							<AgGridReact<EntryDto>
								ref={gridRef}
								pinnedTopRowData={myEntry ? [myEntry] : undefined}
								rowData={otherEntries}
								columnDefs={colDefs}
							/>
						</div>
						<div className="mt-3 text-center text-xs">
							Events are automatically updated hourly at approximately 15 minutes past the hour. Overall results are
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
