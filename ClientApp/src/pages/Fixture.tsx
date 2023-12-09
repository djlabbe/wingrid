import { useNavigate, useParams } from "react-router-dom";
import { GATEWAY_URI, get, post } from "../services/api";
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
		fixture?.eventIds.every((id) => choices.get(id) !== undefined) && tiebreaker !== undefined && tiebreaker >= 0;

	useEffect(() => {
		const getFixture = async () => {
			try {
				setLoading(true);
				const apiFixtureData = await get<ResponseDto<FixtureDto>>(`${GATEWAY_URI}/api/fixtures/${id}`);
				setFixture(apiFixtureData.result);
				const apiEntryData = await get<ResponseDto<EntryDto>>(`${GATEWAY_URI}/api/entries/${id}`);
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
			var response = await post<ResponseDto<EntryDto>>(`${GATEWAY_URI}/api/entries`, entry);
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
				className={`md:w-1/2 rounded-lg hover:ring-8 hover:ring-amber-400 hover:cursor-pointer my-4 ${
					isSelected && "ring-amber-400 ring-8"
				} ${hasChosen && !isSelected && "opacity-50"}`}
				onClick={() => handleSelectTeam(eventId, isHome)}
			>
				<div
					className="p-5 rounded-t-lg text-center"
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
				<div className="w-full rounded-b-lg bg-zinc-800 p-2 text-white text-center text-sm font-bold">
					{team.displayName?.toLocaleUpperCase()}
				</div>
			</div>
		);
	};

	if (loading) return <LoadingContainer />;

	return (
		<div className="mx-auto max-w-screen-md lg:py-8">
			<h1 className="text-2xl text-center font-extrabold text-neutral-600">{fixture?.name}</h1>
			{fixture?.events?.map((e: EventDto) => (
				<div
					key={e.id}
					className="flex rounded-lg border border-gray-200 bg-white shadow-md dark:border-gray-700 dark:bg-gray-800 flex-col my-4"
				>
					<div className="flex h-full flex-col justify-center gap-2 p-2 px-6">
						<p className="text-center text-xs text-neutral-600 font-bold">Who will win this matchup?</p>
						<hr />
						<div className="items-center justify-center sm:flex sm:space-x-6 sm:space-y-0">{renderEvent(e)}</div>
						<hr />
						<div className="text-center text-xs text-neutral-600">{new Date(e?.date).toLocaleString()}</div>
					</div>
				</div>
			))}
			<div className="flex rounded-lg border border-gray-200 bg-white shadow-md dark:border-gray-700 dark:bg-gray-800 flex-col my-4">
				<div className="flex h-full flex-col justify-center gap-2 p-2 px-6">
					<p className="text-center text-xs text-neutral-600 font-bold">
						How many total points will be scored in this matchup?
					</p>
					<hr />
					<div className="items-center justify-center mx-auto">
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
							className="block w-full rounded-md border-0 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-green-600 sm:text-sm sm:leading-6"
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
