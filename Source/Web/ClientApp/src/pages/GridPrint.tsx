import { EntryDto } from "../models/EntryDto";
import { EventDto } from "../models/EventDto";

interface Props {
	entries: EntryDto[];
	events: EventDto[];
	title: string;
}

const GridPrint = ({ title, events, entries }: Props) => {
	return (
		<div className="m-3 hidden print:block">
			<h1 className="text-center text-lg">{title}</h1>
			<table className="table-auto border-2 border-solid border-neutral-800 text-xs">
				<thead>
					<tr>
						<th className="border-2 border-solid border-neutral-800 p-1">Name</th>
						{events.map((ev, i) => (
							<th key={ev.id} className="border-2 border-solid border-neutral-800 p-1" style={{ width: "50px" }}>
								{i + 1}
							</th>
						))}
						<th className="border-2 border-solid border-neutral-800 p-1" style={{ width: "50px" }}>
							TB
						</th>
					</tr>
				</thead>
				<tbody>
					{entries.map((ent) => (
						<tr key={ent.userId}>
							<td className="border-2 border-solid border-neutral-800 p-1">{ent.userName}</td>
							{events.map((ev) => {
								const isHomeSelected = ent.eventEntries.find((ee) => ee.eventId === ev.id)?.homeWinnerSelected;
								const selectedTeam = isHomeSelected ? ev.homeTeam : ev.awayTeam;
								return (
									<td key={ev.id} className="border-2 border-solid border-neutral-800 p-1 text-center">
										{selectedTeam.abbreviation}
									</td>
								);
							})}
							<td className="border-2 border-solid border-neutral-800 p-1 text-center">{ent.tiebreaker}</td>
						</tr>
					))}
				</tbody>
			</table>
		</div>
	);
};

export default GridPrint;
