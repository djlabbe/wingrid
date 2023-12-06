import { Button, Checkbox, Label, Radio, Select, Table } from "flowbite-react";
import { useState } from "react";
import { GATEWAY_URI, get, post } from "../../services/api";
import { EventDto } from "../../models/EventDto";
import { AiOutlineLoading } from "react-icons/ai";
import { FixtureDto } from "../../models/FixtureDto";
import { toastifyError, toastifySuccess } from "../../services/toastService";
import { useNavigate } from "react-router-dom";
import { ResponseDto } from "../../models/ResponseDto";
import LoadingButton from "../../components/LoadingButton";

const Admin = () => {
	const [season, setSeason] = useState<string>("2023");
	const [week, setWeek] = useState<number>(1);
	const [loadingEvents, setLoadingEvents] = useState(false);
	const [saving, setSaving] = useState(false);
	const [events, setEvents] = useState<EventDto[]>();
	const [selectedEventIds, setSelectedEventIds] = useState<string[]>([]);
	const [tiebreakEvent, setTiebreakEvent] = useState<string>();
	const isValid = tiebreakEvent && selectedEventIds?.length > 0;
	const navigate = useNavigate();

	const handleSearch = async () => {
		try {
			setLoadingEvents(true);
			const apiResponse = await get<ResponseDto<EventDto[]>>(`${GATEWAY_URI}/api/events?season=${season}&week=${week}`);
			setLoadingEvents(false);
			if (apiResponse.isSuccess) {
				setEvents(apiResponse.result);
				setSelectedEventIds([]);
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

			const fixture: FixtureDto = {
				id: 0,
				name: `${season} - Week ${week}`,
				eventIds: selectedEventIds,
				tiebreakerEventId: tiebreakEvent,
				locked: false,
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
		<div className="mx-auto max-w-screen-xl lg:py-8">
			<div className="grid grid-cols-10 grid-flow-col gap-4 items-end py-4 px-4 pb-8">
				<div className="col-span-4">
					<Label htmlFor="season" value="Season" />
					<Select id="season" value={season} onChange={(e) => setSeason(e.target.value)} required disabled>
						<option value="2023">2023</option>
					</Select>
				</div>
				<div className="col-span-4">
					<Label htmlFor="week" value="Week" />
					<Select id="week" value={week} onChange={(e) => setWeek(parseInt(e.target.value))} required>
						{[...Array(18)].map((_, i) => (
							<option key={i} value={i + 1}>
								{i + 1}
							</option>
						))}
					</Select>
				</div>
				<div className="col-span-2">
					<LoadingButton onClick={handleSearch} loading={loadingEvents} text="Get Events" />
				</div>
			</div>
			{events && (
				<>
					<div className="mb-8 overflow-x-auto">
						<Table>
							<Table.Head>
								<Table.HeadCell className="bg-gray-200 text-center">Select</Table.HeadCell>
								<Table.HeadCell className="bg-gray-200">Date</Table.HeadCell>
								<Table.HeadCell className="bg-gray-200">Away Team</Table.HeadCell>
								<Table.HeadCell className="bg-gray-200">Home Team</Table.HeadCell>
								<Table.HeadCell className="bg-gray-200">Neutral Site</Table.HeadCell>
								<Table.HeadCell className="bg-gray-200 text-center">Tiebreak</Table.HeadCell>
							</Table.Head>
							<Table.Body className="divide-y">
								{events.map((event) => (
									<Table.Row
										className={`${
											selectedEventIds.includes(event.id) ? "bg-amber-50" : "bg-white"
										} dark:border-gray-700 dark:bg-gray-800`}
										key={event.id}
									>
										<Table.Cell className="text-center">
											<Checkbox
												id={event.id}
												checked={selectedEventIds.includes(event.id)}
												onChange={(e) => handleSelectEvent(event.id, e.target.checked)}
											/>
										</Table.Cell>

										<Table.Cell>{new Date(event.date).toLocaleDateString()}</Table.Cell>
										<Table.Cell className="whitespace-nowrap font-medium text-gray-900 dark:text-white">
											{event.awayTeam.displayName}
										</Table.Cell>
										<Table.Cell className="whitespace-nowrap font-medium text-gray-900 dark:text-white">
											{event.homeTeam.displayName}
										</Table.Cell>
										<Table.Cell>{event.neutralSite}</Table.Cell>
										<Table.Cell className="text-center">
											{selectedEventIds.includes(event.id) && (
												<Radio
													name="tiebreak"
													id={event.id}
													value={event.id}
													checked={tiebreakEvent === event.id}
													// disabled={tiebreakEvent != undefined && tiebreakEvent !== event.id}
													onChange={(e) => handleSelectTiebreak(event.id, e.target.checked)}
												/>
											)}
										</Table.Cell>
									</Table.Row>
								))}
							</Table.Body>
						</Table>
					</div>
					<Button
						className="w-full mt-auto bg-green-700 enabled:hover:bg-green-800"
						disabled={!isValid || saving}
						onClick={handleSubmit}
						isProcessing={saving}
						processingSpinner={<AiOutlineLoading className="h-6 w-6 animate-spin" />}
					>
						{`Create Fixture with ${selectedEventIds.length} Event${selectedEventIds.length === 1 ? "" : "s"}`}
					</Button>
				</>
			)}
		</div>
	);
};

export default Admin;
