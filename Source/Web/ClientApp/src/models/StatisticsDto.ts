export interface StatisticsDto {
	userId: string;
	userName: string;
	entries: number;
	wins: number;
	collegePercentage: number;
	proPercentage: number;
	totalPercentage: number;
	winPercentage: number;
	averageTieBreakerError: number;
}

export default StatisticsDto;
