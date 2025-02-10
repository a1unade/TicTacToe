import {UserActionTypes} from "../../types/actions/user-actions.ts";

interface IDeleteUserAction {
    // Тип операции - удаление стейта пользователя
    type: UserActionTypes.DELETE_USER;
}

export default IDeleteUserAction;