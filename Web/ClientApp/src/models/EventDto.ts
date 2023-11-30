import { TeamDto } from "./TeamDto";

export interface EventDto {
    // Event
    id: string;
    date: string;
    name: string;
    shortName: string;

    // Season
    seasonType: number;
    season: number;
    seasonSlug: string;

    // Week
    week: number;

    // Competition
    attendance: number;
    neutralSite: boolean;
    timeValid: boolean;
    conferenceCompetition: boolean;
    playByPlayAvailable: boolean;
    recent: boolean;

    // Home Competitor
    homeWinner: boolean;
    homeTeamId: string;
    homeTeam: TeamDto;
    homeScore: string;

    // Away Competitor
    awayWinner: boolean;
    awayTeamId: string;
    awayTeam: TeamDto;
    awayScore: string;

    // Status
    displayClock: string;
    period: number;

    // Status Type
    statusId: string;
    statusName: string;
    statusState: string;
    statusCompleted: boolean;
    statusDescription: string;
    statusDetail: string;
    statusShortDetail: string;
}
