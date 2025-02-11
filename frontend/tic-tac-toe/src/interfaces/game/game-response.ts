import {UserResponse} from "./user-response.ts";
import {MatchResponse} from "./match-response.ts";

export interface GameResponse {
    firstPlayer: UserResponse;
    secondPlayer: UserResponse;
    match: MatchResponse;
}