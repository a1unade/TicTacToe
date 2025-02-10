// Начальное состояние пользователя
import IUserState from "../../../interfaces/user-actions/IUserRootState.ts";
import {UserAction, UserActionTypes} from "../../../types/actions/user-actions.ts";

const initialState: IUserState = {
    // ID пользователя
    id: "",
    // Никнейм пользователя
    username: "",
    // Рейтинг пользователя
    score: 0
}

export const userReducer = (state = initialState, action: UserAction) => {
    switch (action.type) {
        case UserActionTypes.CREATE_USER: // Создание нового пользователя и сохранение в стейт
            return {
                id: action.payload.id,
                username: action.payload.username,
                score: action.payload.score,
            };
        case UserActionTypes.DELETE_USER: // Выход из аккаунта
            return {state: initialState}
        case UserActionTypes.ADD_USER_SCORE: // Повышение рейтинга у пользователя
            return {...state, balance: state.score + action.payload};
        case UserActionTypes.REMOVE_USER_SCORE: // Понижение рейтинга у пользователя
            return {...state, balance: state.score - action.payload};
        default:
            return state
    }
}