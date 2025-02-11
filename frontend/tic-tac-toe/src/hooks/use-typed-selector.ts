import {TypedUseSelectorHook, useSelector} from "react-redux";
import {UserRootState} from "../store/reducers/user/user-root-state.ts";

// Typed Selector для пользователей (userReducer)
export const useUserTypedSelector: TypedUseSelectorHook<UserRootState> = useSelector;
