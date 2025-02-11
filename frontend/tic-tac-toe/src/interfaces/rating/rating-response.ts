// Пользователь из рейтинга игроков
export interface RatingResponse {
    // ID пользователя
    userIdPostgres: string;
    // Рейтинг пользователя
    score: number;
    // Никнейм пользователя
    name: string;
}