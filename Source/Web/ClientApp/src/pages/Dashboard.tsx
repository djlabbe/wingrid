import { useEffect, useState } from "react";
import { FixtureDto } from "../models/FixtureDto";
import { get } from "../services/api";
import { useNavigate } from "react-router-dom";
import { ResponseDto } from "../models/ResponseDto";
import { toastifyError } from "../services/toastService";
import LoadingContainer from "../components/LoadingContainer";
import LoadingButton from "../components/LoadingButton";
import { useLoginContext } from "../hooks/useLoginContext";
import { AiFillCheckCircle } from "react-icons/ai";

const Dashboard = () => {
	const { loginResult } = useLoginContext();
	const [fixtures, setFixtures] = useState<FixtureDto[]>();
	const [loading, setLoading] = useState(false);
	const navigate = useNavigate();
	const isAdmin = loginResult?.roles?.includes("ADMIN");

	useEffect(() => {
		const fetchFixtures = async () => {
			try {
				setLoading(true);
				const apiResponse = await get<ResponseDto<FixtureDto[]>>(`/api/fixtures`);
				setLoading(false);
				if (apiResponse.isSuccess) setFixtures(apiResponse.result);
				else toastifyError(apiResponse.message);
			} catch (e) {
				setLoading(false);
				toastifyError("Error loading fixtures. Please try again later.");
				console.error(e);
			}
		};
		fetchFixtures();
	}, []);

	const handleMakePicks = (fixtureId: number) => {
		navigate(`/fixtures/${fixtureId}`);
	};

	const handleClickGrid = (fixtureId: number) => {
		navigate(`/grid/${fixtureId}`);
	};

	if (loading) return <LoadingContainer />;

	return (
		<div className="mx-auto max-w-screen-xl py-8">
			<div className="mb-8 overflow-x-auto">
				<table className="w-full text-left text-sm text-gray-500 dark:text-gray-400">
					<thead className="group/head text-xs uppercase text-gray-700 dark:text-gray-400">
						<tr>
							<th
								className="bg-gray-200 px-6 py-3 group-first/head:first:rounded-tl-lg group-first/head:last:rounded-tr-lg dark:bg-gray-700"
								style={{ width: "20%" }}
							></th>
							<th className="bg-gray-200 px-6 py-3 group-first/head:first:rounded-tl-lg group-first/head:last:rounded-tr-lg dark:bg-gray-700">
								Name
							</th>
							<th
								className="bg-gray-200 px-6 py-3 text-center group-first/head:first:rounded-tl-lg group-first/head:last:rounded-tr-lg dark:bg-gray-700"
								style={{ width: "20%" }}
							>
								Deadline
							</th>
							<th
								className="bg-gray-200 px-6 py-3 text-center group-first/head:first:rounded-tl-lg group-first/head:last:rounded-tr-lg dark:bg-gray-700"
								style={{ width: "10%" }}
							>
								Events
							</th>
							<th
								className="bg-gray-200 px-6 py-3 text-center group-first/head:first:rounded-tl-lg group-first/head:last:rounded-tr-lg dark:bg-gray-700"
								style={{ width: "10%" }}
							>
								Entries
							</th>
							<th
								className="bg-gray-200 px-6 py-3 text-center group-first/head:first:rounded-tl-lg group-first/head:last:rounded-tr-lg dark:bg-gray-700"
								style={{ width: "10%" }}
							>
								Submitted
							</th>
						</tr>
					</thead>
					<tbody className="group/body divide-y">
						{fixtures?.map((fixture) => (
							<tr key={fixture.id} className="group/row dark:border-gray-700 dark:bg-gray-800">
								<td className="px-6 py-4 group-first/body:group-first/row:first:rounded-tl-lg group-first/body:group-first/row:last:rounded-tr-lg group-last/body:group-last/row:first:rounded-bl-lg group-last/body:group-last/row:last:rounded-br-lg">
									<div className="space-between flex">
										{(fixture.locked || isAdmin) && (
											<LoadingButton
												className="me-2"
												onClick={() => handleClickGrid(fixture.id)}
												disabled={!fixture.locked && !isAdmin}
											>
												<p className="text-xs">View Grid</p>
											</LoadingButton>
										)}
										{!fixture.locked && (
											<LoadingButton
												className="me-2"
												onClick={() => handleMakePicks(fixture.id)}
												disabled={fixture.locked}
											>
												<p className="text-xs">{fixture.hasSubmitted ? "Edit" : "Make"} Picks</p>
											</LoadingButton>
										)}
									</div>
								</td>
								<td className="px-6 py-4 group-first/body:group-first/row:first:rounded-tl-lg group-first/body:group-first/row:last:rounded-tr-lg group-last/body:group-last/row:first:rounded-bl-lg group-last/body:group-last/row:last:rounded-br-lg">
									{fixture.name}
								</td>
								<td className="px-6 py-4 text-center group-first/body:group-first/row:first:rounded-tl-lg group-first/body:group-first/row:last:rounded-tr-lg group-last/body:group-last/row:first:rounded-bl-lg group-last/body:group-last/row:last:rounded-br-lg">
									{fixture.deadline ? new Date(fixture.deadline).toLocaleString() : ""}
								</td>
								<td className="px-6 py-4 text-center group-first/body:group-first/row:first:rounded-tl-lg group-first/body:group-first/row:last:rounded-tr-lg group-last/body:group-last/row:first:rounded-bl-lg group-last/body:group-last/row:last:rounded-br-lg">
									{fixture.events?.length}
								</td>
								<td className="px-6 py-4 text-center group-first/body:group-first/row:first:rounded-tl-lg group-first/body:group-first/row:last:rounded-tr-lg group-last/body:group-last/row:first:rounded-bl-lg group-last/body:group-last/row:last:rounded-br-lg">
									{fixture.entryCount}
								</td>
								<td className="text-green-600 group-first/body:group-first/row:first:rounded-tl-lg group-first/body:group-first/row:last:rounded-tr-lg group-last/body:group-last/row:first:rounded-bl-lg group-last/body:group-last/row:last:rounded-br-lg">
									{fixture.hasSubmitted ? (
										<center>
											<AiFillCheckCircle />
										</center>
									) : (
										<></>
									)}
								</td>
							</tr>
						))}
					</tbody>
				</table>
			</div>
		</div>
	);
};

export default Dashboard;
