import {UserActionTypes} from "../../types/actions/user-actions.ts";

interface IAddUserScore {
    // Тип операции - добавление очков к рейтингу пользователя
    type: UserActionTypes.ADD_USER_SCORE;
    // Сколько очков к рейтингу нужно добавить
    payload: number;
}

export default IAddUserScore;