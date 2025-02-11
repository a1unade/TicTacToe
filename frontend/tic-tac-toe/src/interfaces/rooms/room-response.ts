export interface RoomResponse {
    // ID комнаты
    id: string,
    // username опльзователя, который создал комнату
    creatorUserName: string,
    // id автора комнаты
    creatorId: string,
    // дата и время создания комнаты
    createdAt: Date,
    // статус комнаты ( игра уже идет, ожидание игрока, игра завершена )
    status: string,
    // максимальный допустимый рейтинг комнаты
    maxScore: number,
}