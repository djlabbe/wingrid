import { EventDto } from "./EventDto";

export interface FixtureDto {
	id: number;
	name: string;
	eventIds: string[];
	events?: EventDto[];
	locked: boolean;
	lockedAt?: string;
	tiebreakerEventId: string;
}
