import {useDispatch} from "react-redux";
import {bindActionCreators} from "redux";
import * as UserActionCreators from '../store/action-creators/users'

export const useUserActions = () => {
    const dispatch = useDispatch();
    return bindActionCreators(UserActionCreators, dispatch);
}