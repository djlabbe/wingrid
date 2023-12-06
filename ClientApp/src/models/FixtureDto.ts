import { EntryDto } from "./EntryDto";
import { EventDto } from "./EventDto";

export interface FixtureDto {
	id: number;
	name: string;
	eventIds: string[];
	events?: EventDto[];
	entries?: EntryDto[];
	locked: boolean;
	lockedAt?: string;
	tiebreakerEventId: string;
}
