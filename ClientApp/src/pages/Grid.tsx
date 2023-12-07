import { useEffect, useState } from "react";
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

const Grid = () => {
	const { id } = useParams();
	const [fixture, setFixture] = useState<FixtureDto>();
	const entries = fixture?.entries || [];
	const events = fixture?.events || [];

	const cellClassRules = (event: EventDto) => {
		return {
			"bg-yellow-100": (p: CellClassParams<EntryDto>) => {
				const homeWinner = event.homeWinner;
				const selectedWinner = p.data?.eventEntries.find(
					(ee: EventEntryDto) => ee.eventId === event.id,
				)?.homeWinnerSelected;
				return homeWinner === selectedWinner;
			},
		};
	};

	const eventCols = events.map(
		(ev, i) =>
			({
				headerName: `${i + 1}`,
				headerClass: "event-grid-col",
				headerTooltip: `${ev.name} - ${new Date(ev.date).toLocaleString()}`,
				width: 80,
				cellClass: "text-center font-bold ",
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

	const colDefs = [
		{ field: "userName", headerName: "Player", suppressMovable: true },
		{
			headerName: "Events",
			children: [...eventCols],
		},
		{ field: "tiebreaker", headerName: "Tiebreak", suppressMovable: true, width: 130 },
		{ field: "score", headerName: "Score", suppressMovable: true, width: 130 },
	] as ColDef[];

	useEffect(() => {
		const getFixture = async () => {
			try {
				const apiResponse = await get<ResponseDto<FixtureDto>>(`${GATEWAY_URI}/api/fixtures/${id}`);
				if (apiResponse.isSuccess) setFixture(apiResponse.result);
				else toastifyError(apiResponse.message);
			} catch (e) {
				console.error(e);
				toastifyError(`${e}`);
			}
		};
		getFixture();
	}, []);

	return (
		<div className="w-full p-8">
			<h1 className="text-2xl mb-2">{fixture?.name}</h1>
			<div className="ag-theme-quartz" style={{ height: "80vh" }}>
				<AgGridReact<EntryDto> rowData={entries} columnDefs={colDefs} />
			</div>
		</div>
	);
};

export default Grid;
