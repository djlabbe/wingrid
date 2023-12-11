import { useEffect, useState } from "react";
import { FixtureDto } from "../models/FixtureDto";
import { GATEWAY_URI, get } from "../services/api";
import { Table } from "flowbite-react";
import { useNavigate } from "react-router-dom";
import { ResponseDto } from "../models/ResponseDto";
import { toastifyError } from "../services/toastService";
import LoadingContainer from "../components/LoadingContainer";
import LoadingButton from "../components/LoadingButton";

const Dashboard = () => {
	const [fixtures, setFixtures] = useState<FixtureDto[]>();
	const [loading, setLoading] = useState(false);
	const navigate = useNavigate();

	useEffect(() => {
		const fetchFixtures = async () => {
			try {
				setLoading(true);
				const apiResponse = await get<ResponseDto<FixtureDto[]>>(`${GATEWAY_URI}/api/fixtures`);
				setLoading(false);
				if (apiResponse.isSuccess) setFixtures(apiResponse.result);
				else toastifyError(apiResponse.message);
			} catch (e) {
				setLoading(false);
				toastifyError("Error loading fixture. Please try again later.");
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
				<Table>
					<Table.Head>
						<Table.HeadCell className="bg-gray-200" style={{ width: "20%" }}></Table.HeadCell>
						<Table.HeadCell className="bg-gray-200">Name</Table.HeadCell>
						<Table.HeadCell className="bg-gray-200" style={{ width: "20%" }}>
							Deadline
						</Table.HeadCell>
						<Table.HeadCell className="bg-gray-200" style={{ width: "15%" }}>
							Event Count
						</Table.HeadCell>
					</Table.Head>
					<Table.Body className="divide-y">
						{fixtures?.map((fixture) => (
							<Table.Row key={fixture.id} className="dark:border-gray-700 dark:bg-gray-800">
								<Table.Cell>
									<div className="flex space-between">
										{!fixture.locked && (
											<LoadingButton className="me-2" onClick={() => handleMakePicks(fixture.id)}>
												<p className="text-xs">Make Picks</p>
											</LoadingButton>
										)}
										<LoadingButton onClick={() => handleClickGrid(fixture.id)}>
											<p className="text-xs">View Grid</p>
										</LoadingButton>
									</div>
								</Table.Cell>
								<Table.Cell className="whitespace-nowrap font-medium text-gray-900 dark:text-white">
									{fixture.name}
								</Table.Cell>
								<Table.Cell className="whitespace-nowrap font-medium text-gray-900 dark:text-white">
									{fixture.deadline ? new Date(fixture.deadline).toLocaleString() : ""}
								</Table.Cell>
								<Table.Cell className="whitespace-nowrap font-medium text-gray-900 dark:text-white">
									{fixture.eventIds.length}
								</Table.Cell>
							</Table.Row>
						))}
					</Table.Body>
				</Table>
			</div>
		</div>
	);
};

export default Dashboard;
