// Начальное состояние user state.
// Интерфейс для redux, хранит состояние пользователя в store.
interface IUserState {
    // ID пользователя
    id: string;
    // Никнейм пользователя
    username: string;
    // Рейтинг пользователя
    score: number;
}

export default IUserState;