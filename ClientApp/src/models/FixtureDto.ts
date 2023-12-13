import { EntryDto } from "./EntryDto";
import { EventDto } from "./EventDto";

export interface FixtureDto {
	id: number;
	name: string;
	events?: EventDto[];
	entries?: EntryDto[];
	deadline?: string;
	locked: boolean;
	tiebreakerEventId: string;
	isComplete: boolean;
}
