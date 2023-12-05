import { useEffect, useState } from "react";
import { FixtureDto } from "../models/FixtureDto";
import { GATEWAY_URI, get } from "../services/api";
import { Button, Table } from "flowbite-react";
import { useNavigate } from "react-router-dom";
import { ResponseDto } from "../models/ResponseDto";
import { toastifyError } from "../services/toastService";
import LoadingContainer from "../components/LoadingContainer";

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

	if (loading) return <LoadingContainer />;

	return (
		<div className="mx-auto max-w-screen-xl lg:py-8">
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
								<div className="flex">
									<Button
										className="mt-auto bg-green-700 enabled:hover:bg-green-800 text-sm"
										onClick={() => handleMakePicks(fixture.id)}
									>
										<p className="text-xs">Make Picks</p>
									</Button>
									<Button
										className="mt-auto bg-green-700 enabled:hover:bg-green-800 ms-2"
										onClick={() => handleMakePicks(fixture.id)}
									>
										<p className="text-xs">View Grid</p>
									</Button>
								</div>
							</Table.Cell>
							<Table.Cell className="whitespace-nowrap font-medium text-gray-900 dark:text-white">
								{fixture.name}
							</Table.Cell>
							<Table.Cell className="whitespace-nowrap font-medium text-gray-900 dark:text-white">
								{fixture.lockedAt ? new Date(fixture.lockedAt).toLocaleString() : ""}
							</Table.Cell>
							<Table.Cell className="whitespace-nowrap font-medium text-gray-900 dark:text-white">
								{fixture.eventIds.length}
							</Table.Cell>
						</Table.Row>
					))}
				</Table.Body>
			</Table>
			<div className="mt-3 text-center"></div>
		</div>
	);
};

export default Dashboard;
