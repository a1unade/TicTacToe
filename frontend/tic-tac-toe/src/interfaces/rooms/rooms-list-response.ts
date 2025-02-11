import {RoomResponse} from "./room-response.ts";

export interface RoomsListResponse {
    gameDtos: RoomResponse[],
    totalCount: number
}