import {UserActionTypes} from "../../types/actions/user-actions.ts";


interface ICreateUserAction {
    // Тип операции - создание стейта пользователя
    type: UserActionTypes.CREATE_USER;
    payload: {
        // ID пользователя
        id: string;
        // Ник пользователя
        username: string;
        // Рейтинг пользователя
        score: number
    };
}

export default ICreateUserAction;