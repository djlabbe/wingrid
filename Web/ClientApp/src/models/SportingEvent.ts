import { Team } from "./Team";

export interface SportingEvent
{
    id: string;
    name: string;
    homeTeam: Team;
    awayTeam: Team;
}