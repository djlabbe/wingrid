export interface EntryDto {
	userId: string;
	userName: string;
	fixtureId: number;
	eventEntries: EventEntryDto[];
	tiebreaker?: number;
	score?: number;
	submittedAt?: string;
	updatedAt?: string;
}

export interface EventEntryDto {
	eventId: string;
	homeWinnerSelected?: boolean;
	homeWinner?: boolean;
	isCorrect?: boolean;
}
