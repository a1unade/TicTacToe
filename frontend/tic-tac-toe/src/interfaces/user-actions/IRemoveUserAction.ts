import {UserActionTypes} from "../../types/actions/user-actions.ts";

interface IRemoveUserScore {
    // Тип операции - понижение рейтинга пользователя
    type: UserActionTypes.REMOVE_USER_SCORE;
    // Сколько очков нужно убрать из рейтинга пользователя
    payload: number;
}

export default IRemoveUserScore;