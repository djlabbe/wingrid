import { UserDto } from "./UserDto";

export interface StatisticsDto {
	user: UserDto;
	entries: number;
	wins: number;
	totalCorrectPicks: number;
	collegePercentage: number;
	proPercentage: number;
	totalPercentage: number;
	winPercentage: number;
	averageTieBreakerError: number;
}

export default StatisticsDto;
