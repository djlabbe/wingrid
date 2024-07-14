import { useNavigate, useParams } from "react-router-dom";
import { get, post } from "../services/api";
import { useEffect, useState } from "react";
import { FixtureDto } from "../models/FixtureDto";
import { TeamDto } from "../models/TeamDto";
import { useLoginContext } from "../hooks/useLoginContext";
import { EventDto } from "../models/EventDto";
import { toastifyError, toastifySuccess } from "../services/toastService";
import { EntryDto } from "../models/EntryDto";
import { ResponseDto } from "../models/ResponseDto";
import LoadingContainer from "../components/LoadingContainer";
import LoadingButton from "../components/LoadingButton";

const Fixture = () => {
	const { loginResult } = useLoginContext();
	const { id } = useParams();
	const navigate = useNavigate();
	const [loading, setLoading] = useState(false);
	const [submitting, setSubmitting] = useState(false);
	const [fixture, setFixture] = useState<FixtureDto>();
	const [choices, setChoices] = useState(new Map<string, boolean>());
	const [tiebreaker, setTiebreaker] = useState<number>();

	const isValid =
		fixture?.events?.every((f) => choices.get(f.id) !== undefined) && tiebreaker !== undefined && tiebreaker >= 0;

	useEffect(() => {
		const getFixture = async () => {
			try {
				setLoading(true);
				const apiFixtureData = await get<ResponseDto<FixtureDto>>(`/api/fixtures/${id}`);
				setFixture(apiFixtureData.result);
				const apiEntryData = await get<ResponseDto<EntryDto>>(`/api/fixtures/${id}/entry`);
				if (apiEntryData.isSuccess) {
					const existingChoices = new Map<string, boolean>();
					apiEntryData.result.eventEntries.forEach((ee) => {
						if (ee.homeWinnerSelected !== undefined) existingChoices.set(ee.eventId, ee.homeWinnerSelected);
					});
					setChoices(existingChoices);
					setTiebreaker(apiEntryData.result.tiebreaker);
				}
				setLoading(false);
			} catch (e) {
				setLoading(false);
			}
		};
		getFixture();
	}, []);

	const handleSelectTeam = (eventId: string, homeWinnerSelected: boolean) => {
		setChoices((prev) => {
			prev.set(eventId, homeWinnerSelected);
			return new Map(prev);
		});
	};

	const handleSubmit = async () => {
		const entry: EntryDto = {
			id: 0,
			userId: loginResult?.user?.id || "",
			userName: loginResult?.user?.name || "",
			fixtureId: fixture?.id || 0,
			eventEntries:
				fixture?.events?.map((e) => ({
					eventId: e.id,
					homeWinnerSelected: choices.get(e.id),
				})) || [],
			tiebreaker,
		};
		try {
			setSubmitting(true);
			var response = await post<ResponseDto<EntryDto>>(`/api/fixtures/submitentry`, entry);
			setSubmitting(false);
			if (response.isSuccess) {
				toastifySuccess(`${fixture?.name} entry saved. Good luck!`);
				navigate("/dashboard");
			} else {
				setSubmitting(false);
				toastifyError(response.message);
			}
		} catch (e) {
			setSubmitting(false);
			console.error(e);
			toastifyError(`Error submitting entry. Please try again later. ${e}`);
		}
	};

	const renderEvent = (ev: EventDto) => {
		return (
			<>
				{renderTeam(ev.awayTeam, ev.id, false)}
				{renderTeam(ev.homeTeam, ev.id, true)}
			</>
		);
	};

	const renderTeam = (team: TeamDto, eventId: string, isHome: boolean) => {
		const hasChosen = choices.get(eventId) !== undefined;
		const isSelected = choices.get(eventId) === isHome;

		return (
			<div
				className={`my-4 rounded-lg hover:cursor-pointer hover:ring-8 hover:ring-amber-400 md:w-1/2 ${
					isSelected && "ring-8 ring-amber-400"
				} ${hasChosen && !isSelected && "opacity-50"}`}
				onClick={() => handleSelectTeam(eventId, isHome)}
			>
				<div
					className="rounded-t-lg p-5 text-center"
					style={{
						backgroundColor: `#${team?.color}CC`,
						backgroundImage: `linear-gradient(${isHome ? "" : "-"}45deg, 
							rgba(255, 255, 255, 0.5) 0px, 
							rgba(255, 255, 255, 0.5) 18%, 
							rgba(255, 255, 255, 0.3) 18%, 
							rgba(255, 255, 255, 0.3) 28%, 
							rgba(255, 255, 255, 0) 28%, 
							rgba(255, 255, 255, 0) 72%, 
							rgba(0, 0, 0, 0.3) 72%, 
							rgba(0, 0, 0, 0.3) 82%, 
							rgba(0, 0, 0, 0.5) 82%, 
							rgba(0, 0, 0, 0.5) 100%
						)`,
					}}
				>
					<img src={team.logo} width={"100px"} height={"100px"} className="mx-auto" />
				</div>
				<div className="w-full rounded-b-lg bg-zinc-800 p-2 text-center text-sm font-bold text-white">
					{team.displayName?.toLocaleUpperCase()}
				</div>
			</div>
		);
	};

	if (loading) return <LoadingContainer />;

	return (
		<div className="mx-auto max-w-screen-md lg:py-8">
			<h1 className="text-center text-2xl font-extrabold text-neutral-600">{fixture?.name}</h1>
			{fixture?.events?.map((e: EventDto) => (
				<div
					key={e.id}
					className="my-4 flex flex-col rounded-lg border border-gray-200 bg-white shadow-md dark:border-white dark:bg-gray-900"
				>
					<div className="flex h-full flex-col justify-center gap-2 p-2 px-6">
						<p className="text-center text-xs font-bold text-neutral-600 dark:text-gray-400">
							Who will win this matchup?
						</p>
						<hr />
						<div className="items-center justify-center sm:flex sm:space-x-6 sm:space-y-0">{renderEvent(e)}</div>
						<hr />
						<div className="text-center text-xs text-neutral-600 dark:text-gray-400">
							{new Date(e?.date).toLocaleString()}
						</div>
					</div>
				</div>
			))}
			<div className="my-4 flex flex-col rounded-lg border border-gray-200 bg-white shadow-md dark:border-gray-700 dark:bg-gray-900">
				<div className="flex h-full flex-col justify-center gap-2 p-2 px-6">
					<p className="text-center text-xs font-bold text-neutral-600 dark:text-gray-400">
						How many total points will be scored in this matchup?
					</p>
					<hr />
					<div className="mx-auto items-center justify-center dark:text-white">
						{fixture?.events?.find((e) => e.id === fixture.tiebreakerEventId)?.name}
					</div>
					<hr />
					<div className="text-center text-xs text-neutral-600">
						<input
							id="tiebreaker"
							value={tiebreaker || ""}
							onChange={(e) => setTiebreaker(parseInt(e.target.value))}
							type="number"
							min={0}
							required
							className="block w-full rounded-md border-0 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-green-600 dark:bg-gray-800 dark:text-white sm:text-sm sm:leading-6"
						/>
					</div>
				</div>
			</div>
			<LoadingButton loading={submitting} disabled={!isValid} onClick={handleSubmit}>
				Submit Entry
			</LoadingButton>
		</div>
	);
};

export default Fixture;
