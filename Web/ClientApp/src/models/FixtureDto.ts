export interface FixtureDto {
	id: number;
	name: string;
	eventIds: string[];
	locked: boolean;
	lockedAt?: string;
	tiebreakerEventId: string;
}
