import { useState } from "react";
import { GATEWAY_URI, get, post } from "../../services/api";
import { EventDto } from "../../models/EventDto";
import { FixtureDto } from "../../models/FixtureDto";
import { toastifyError, toastifySuccess } from "../../services/toastService";
import { useNavigate } from "react-router-dom";
import { ResponseDto } from "../../models/ResponseDto";
import LoadingButton from "../../components/LoadingButton";
import { AiFillCheckCircle } from "react-icons/ai";
import { CreateFixtureDto } from "../../models/CreateFixtureDto";

const Admin = () => {
	const [start, setStart] = useState<string>();
	const [end, setEnd] = useState<string>();
	const [name, setName] = useState("");

	const [loadingEvents, setLoadingEvents] = useState(false);
	const [saving, setSaving] = useState(false);
	const [events, setEvents] = useState<EventDto[]>();
	const [selectedEventIds, setSelectedEventIds] = useState<string[]>([]);
	const [tiebreakEvent, setTiebreakEvent] = useState<string>();
	const isValid = name && tiebreakEvent && selectedEventIds?.length > 0;
	const navigate = useNavigate();

	const handleSearch = async () => {
		if (!start || !end) {
			toastifyError("Must specify start and end dates.");
			return;
		}

		var startDate = new Date(start);
		var endDate = new Date(end);

		try {
			setLoadingEvents(true);
			const apiResponse = await get<ResponseDto<EventDto[]>>(
				`${GATEWAY_URI}/api/events?start=${startDate.toISOString()}&end=${endDate.toISOString()}`,
			);
			setLoadingEvents(false);
			if (apiResponse.isSuccess) {
				setEvents(apiResponse.result);
				setSelectedEventIds([]);
				setTiebreakEvent(undefined);
			} else toastifyError(apiResponse.message);
		} catch (e) {
			setLoadingEvents(false);
			console.error(e);
		}
	};

	const handleSelectEvent = (eventId: string, selected: boolean) => {
		if (selected) setSelectedEventIds((prev) => [...prev, eventId]);
		else setSelectedEventIds((prev) => prev.filter((id) => id !== eventId));
	};

	const handleSelectTiebreak = (eventId: string, selected: boolean) => {
		if (selected) setTiebreakEvent(eventId);
		else setTiebreakEvent(undefined);
	};

	const handleSubmit = async () => {
		if (!tiebreakEvent) {
			toastifyError("Tiebreaker is required.");
			return;
		}
		try {
			setSaving(true);

			const fixture: CreateFixtureDto = {
				name: name.trim(),
				eventIds: selectedEventIds,
				tiebreakerEventId: tiebreakEvent,
			};

			var apiResponse = await post<ResponseDto<FixtureDto>>(`${GATEWAY_URI}/api/fixtures`, fixture);
			setSaving(false);
			if (apiResponse.isSuccess) {
				toastifySuccess("Fixture created successfully.");
				navigate("/dashboard");
			} else {
				toastifyError(apiResponse.message);
			}
		} catch (e) {
			setSaving(false);
			console.error(e);
			toastifyError(`${e}`);
		}
	};

	return (
		<div className="mx-auto max-w-screen-xl py-8">
			<div className="grid grid-cols-10 grid-flow-col gap-4 items-end py-4 px-4">
				<div className="col-span-4">
					<label className="text-sm font-medium text-gray-900 dark:text-white" htmlFor="start">
						Start Date
					</label>
					<input
						type="date"
						value={start || ""}
						onChange={(e) => setStart(e.target.value)}
						required
						className="block w-full rounded-md border-0 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-green-600 sm:text-sm sm:leading-6"
					/>
				</div>
				<div className="col-span-4">
					<label className="text-sm font-medium text-gray-900 dark:text-white" htmlFor="end">
						End Date
					</label>
					<input
						type="date"
						value={end || ""}
						onChange={(e) => setEnd(e.target.value)}
						required
						className="block w-full rounded-md border-0 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-green-600 sm:text-sm sm:leading-6"
					/>
				</div>
				<div className="col-span-2">
					<LoadingButton onClick={handleSearch} loading={loadingEvents} disabled={!start || !end}>
						Get Events
					</LoadingButton>
				</div>
			</div>

			{events && events.length === 0 && (
				<div className="mb-8 overflow-x-auto text-center text-xl">No events found.</div>
			)}
			{events && events.length > 0 && (
				<>
					<div className="mb-8 overflow-x-auto">
						<table className="w-full text-left text-sm text-gray-500 dark:text-gray-400">
							<thead className="group/head text-xs uppercase text-gray-700 dark:text-gray-400">
								<tr>
									<th className="group-first/head:first:rounded-tl-lg group-first/head:last:rounded-tr-lg dark:bg-gray-700 px-6 py-3 bg-gray-200 text-center">
										Select
									</th>
									<th className="group-first/head:first:rounded-tl-lg group-first/head:last:rounded-tr-lg dark:bg-gray-700 px-6 py-3 bg-gray-200">
										Date
									</th>
									<th className="group-first/head:first:rounded-tl-lg group-first/head:last:rounded-tr-lg dark:bg-gray-700 px-6 py-3 bg-gray-200">
										Away Team
									</th>
									<th className="group-first/head:first:rounded-tl-lg group-first/head:last:rounded-tr-lg dark:bg-gray-700 px-6 py-3 bg-gray-200">
										Home Team
									</th>
									<th className="group-first/head:first:rounded-tl-lg group-first/head:last:rounded-tr-lg dark:bg-gray-700 px-6 py-3 bg-gray-200 text-center">
										Neutral Site
									</th>
									<th className="group-first/head:first:rounded-tl-lg group-first/head:last:rounded-tr-lg dark:bg-gray-700 px-6 py-3 bg-gray-200 text-center">
										Tiebreak
									</th>
								</tr>
							</thead>
							<tbody className="group/body divide-y">
								{events.map((event) => (
									<tr
										className={`group/row ${
											selectedEventIds.includes(event.id) ? "bg-amber-100" : "bg-white"
										} dark:border-gray-700 dark:bg-gray-800`}
										key={event.id}
									>
										<td className="group-first/body:group-first/row:first:rounded-tl-lg group-first/body:group-first/row:last:rounded-tr-lg group-last/body:group-last/row:first:rounded-bl-lg group-last/body:group-last/row:last:rounded-br-lg px-6 py-4 text-center">
											<input
												type="checkbox"
												id={event.id}
												checked={selectedEventIds.includes(event.id)}
												onChange={(e) => handleSelectEvent(event.id, e.target.checked)}
												className="h-4 w-4 rounded focus:ring-2 border border-gray-300 dark:border-gray-600 dark:bg-gray-700 bg-gray-100 focus:ring-green-600 dark:ring-offset-green-600 dark:focus:ring-green-600 text-green-600"
											/>
										</td>

										<td>{new Date(event.date).toLocaleDateString()}</td>
										<td className="group-first/body:group-first/row:first:rounded-tl-lg group-first/body:group-first/row:last:rounded-tr-lg group-last/body:group-last/row:first:rounded-bl-lg group-last/body:group-last/row:last:rounded-br-lg px-6 py-4whitespace-nowrap font-medium text-gray-900 dark:text-white">
											{event.awayTeam.displayName}
										</td>
										<td className="group-first/body:group-first/row:first:rounded-tl-lg group-first/body:group-first/row:last:rounded-tr-lg group-last/body:group-last/row:first:rounded-bl-lg group-last/body:group-last/row:last:rounded-br-lg px-6 py-4 whitespace-nowrap font-medium text-gray-900 dark:text-white">
											{event.homeTeam.displayName}
										</td>
										<td className="group-first/body:group-first/row:first:rounded-tl-lg group-first/body:group-first/row:last:rounded-tr-lg group-last/body:group-last/row:first:rounded-bl-lg group-last/body:group-last/row:last:rounded-br-lg px-6 py-4 flex justify-center text-green-600 text-lg">
											{event.neutralSite && <AiFillCheckCircle />}
										</td>
										<td className="group-first/body:group-first/row:first:rounded-tl-lg group-first/body:group-first/row:last:rounded-tr-lg group-last/body:group-last/row:first:rounded-bl-lg group-last/body:group-last/row:last:rounded-br-lg px-6 py-4 text-center">
											{selectedEventIds.includes(event.id) && (
												<input
													type="radio"
													name="tiebreak"
													id={event.id}
													value={event.id}
													checked={tiebreakEvent === event.id}
													// disabled={tiebreakEvent != undefined && tiebreakEvent !== event.id}
													onChange={(e) => handleSelectTiebreak(event.id, e.target.checked)}
													className="h-4 w-4 border border-gray-300 focus:ring-2 focus:ring-green-500 dark:border-gray-600 dark:bg-gray-700 dark:focus:bg-green-600 dark:focus:ring-green-600 text-green-600"
												/>
											)}
										</td>
									</tr>
								))}
							</tbody>
						</table>
					</div>
					<div className="grid grid-cols-10 grid-flow-col gap-4 items-end px-4 pb-8">
						<div className="col-span-10">
							<label className="text-sm font-medium text-gray-900 dark:text-white" htmlFor="name">
								Fixture Display Name
							</label>
							<input
								id="name"
								value={name || ""}
								onChange={(e) => setName(e.target.value)}
								placeholder="Provide a descriptive name / title"
								required
								className="block w-full rounded-md border-0 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-green-600 sm:text-sm sm:leading-6"
							/>
						</div>
						<LoadingButton disabled={!isValid || saving} onClick={handleSubmit} loading={saving}>
							{`Create Fixture with ${selectedEventIds.length} Event${selectedEventIds.length === 1 ? "" : "s"}`}
						</LoadingButton>
					</div>
				</>
			)}
		</div>
	);
};

export default Admin;
