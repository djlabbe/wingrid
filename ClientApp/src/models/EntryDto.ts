export interface EntryDto {
	userId: string;
	userName: string;
	fixtureId: number;
	eventEntries: EventEntryDto[];
	tiebreaker?: number;
	tiebreakerResult?: number;
	submittedAt?: string;
	updatedAt?: string;
	score?: number;
	winner?: boolean;
}

export interface EventEntryDto {
	eventId: string;
	homeWinnerSelected?: boolean;
	homeWinner?: boolean;
	isCorrect?: boolean;
}
