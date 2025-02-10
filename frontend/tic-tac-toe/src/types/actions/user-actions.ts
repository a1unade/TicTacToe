import ICreateUserAction from "../../interfaces/user-actions/ICreateUserActon.ts";
import IAddUserBalance from "../../interfaces/user-actions/IAddUserScore.ts";
import IRemoveUserBalance from "../../interfaces/user-actions/IRemoveUserAction.ts";
import IDeleteUserAction from "../../interfaces/user-actions/IDeleteUserAction.ts";

// Перечисление всех типов actions для user reducer
export enum UserActionTypes {
    CREATE_USER = 'CREATE_USER', // Создание redux-state пользователя при регистрации/входе
    ADD_USER_SCORE = 'ADD_USER_SCORE', // Повышение рейтинга у пользователя
    REMOVE_USER_SCORE = 'REMOVE_USER_SCORE', // Понижение рейтинга у пользователя
    DELETE_USER = 'DELETE_USER', // Выход пользователя из аккаунта
}

export type UserAction =
    ICreateUserAction
    | IDeleteUserAction
    | IAddUserBalance
    | IRemoveUserBalance;