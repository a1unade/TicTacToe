// Пользователь из рейтинга игроков
export interface RatingResponse {
    // ID пользователя
    id: string;
    // Рейтинг пользователя
    score: number;
    // Никнейм пользователя
    name: string;
}