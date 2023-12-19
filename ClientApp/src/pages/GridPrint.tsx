import { EntryDto } from "../models/EntryDto";
import { EventDto } from "../models/EventDto";

interface Props {
	entries: EntryDto[];
	events: EventDto[];
	title: string;
}

const GridPrint = ({ title, events, entries }: Props) => {
	return (
		<div className="hidden print:block m-3">
			<h1 className="text-lg text-center">{title}</h1>
			<table className="table-auto text-xs border-solid border-2 border-neutral-800">
				<thead>
					<tr>
						<th className="border-solid border-2 border-neutral-800 p-1" style={{ width: "10%" }}>
							Name
						</th>
						{events.map((_, i) => (
							<th key={i} className="border-solid border-2 border-neutral-800 p-1" style={{ width: "50px" }}>
								{i}
							</th>
						))}
						<th className="border-solid border-2 border-neutral-800 p-1">TB</th>
					</tr>
				</thead>
				<tbody>
					{entries.map((ent) => (
						<tr key={ent.id}>
							<td className="border-solid border-2 border-neutral-800 p-1">{ent.userName}</td>
							{events.map((ev) => {
								const isHomeSelected = ent.eventEntries.find((ee) => ee.eventId === ev.id)?.homeWinnerSelected;
								const selectedTeam = isHomeSelected ? ev.homeTeam : ev.awayTeam;
								return (
									<td key={ev.id} className="border-solid border-2 border-neutral-800 text-center p-1">
										{selectedTeam.abbreviation}
									</td>
								);
							})}
							<td className="border-solid border-2 border-neutral-800 text-center p-1">{ent.tiebreaker}</td>
						</tr>
					))}
				</tbody>
			</table>
		</div>
	);
};

export default GridPrint;
