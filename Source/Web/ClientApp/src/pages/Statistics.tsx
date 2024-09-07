import { useEffect, useState } from "react";
import { useLoginContext } from "../hooks/useLoginContext";
import { ResponseDto } from "../models/ResponseDto";
import { toastifyError } from "../services/toastService";
import StatisticsDto from "../models/StatisticsDto";
import { get } from "../services/api";

const Statistics = () => {
	const { loginResult } = useLoginContext();
	const [statistics, setStatistcs] = useState<StatisticsDto[]>();
	const [loadingInitial, setLoadingInitial] = useState(true);

	useEffect(() => {
		const getFixture = async () => {
			try {
				setLoadingInitial(true);
				const apiResponse = await get<ResponseDto<StatisticsDto[]>>("/api/statistics");
				if (apiResponse.isSuccess) setStatistcs(apiResponse.result);
				else toastifyError(apiResponse.message);
				setLoadingInitial(false);
			} catch (e) {
				console.error(e);
				toastifyError(`${e}`);
				setLoadingInitial(false);
			}
		};
		getFixture();
	}, []);

	return <></>;
};

export default Statistics;
