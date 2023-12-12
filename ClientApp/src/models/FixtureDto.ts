import { EntryDto } from "./EntryDto";
import { EventDto } from "./EventDto";

export interface FixtureDto {
	id: number;
	name: string;
	events?: EventDto[];
	entries?: EntryDto[];
	locked: boolean;
	deadline?: string;
	tiebreakerEventId: string;
}
