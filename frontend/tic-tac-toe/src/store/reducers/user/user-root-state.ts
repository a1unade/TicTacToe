// Root state для хука typed selector
export type UserRootState = {
    user: {
        // ID пользователя
        id: string,
        // Никнейм пользователя
        username: string;
        // Рейтинг пользователя
        score: number
    }
}