import {Dispatch} from "redux";
import apiClient from "../../utils/api-client.ts";
import {AuthResponse} from "../../interfaces/auth/auth-response.ts";
import {UserAction, UserActionTypes} from "../../types/actions/user-actions.ts";
import {jwtDecode} from "jwt-decode";
import {JWTTokenDecoded} from "../../interfaces/jwt-token/jwt-decoded.ts";
import axios from "axios";

// Создание стейта пользователя при регистрации/входе в аккаунт
export const createUser = (username: string, password: string, request: string) => {
    return async (dispatch: Dispatch<UserAction>) => {
        try {
            const response = await apiClient.post<AuthResponse>(`Auth/${request}`, {
                name: username,
                password: password,
            });

            if (response.status === 200) {
                const decodedToken = jwtDecode<JWTTokenDecoded>(response.data.token);

                dispatch({
                    type: UserActionTypes.CREATE_USER, payload: {
                        id: decodedToken.Id,
                        username: decodedToken.Name,
                        score: decodedToken.Score
                    }
                });

                document.cookie = `jwt=${response.data.token}; path=/;`;
            } else {
                if (request === "login") throw new Error(response.data.message || "Ошибка при входе в аккаунт");

                throw new Error(response.data.message || "Ошибка при регистрации");
            }
        } catch (error: unknown) {
            if (axios.isAxiosError(error)) {
                const errorMessage = error.response?.data || "Ошибка соединения с сервером";
                throw new Error(errorMessage);
            } else if (error instanceof Error) {
                throw new Error(error.message || "Ошибка выполнения запроса");
            } else {
                throw new Error("Неизвестная ошибка");
            }
        }
    };
};

// Создание пользователя по токену jwt
export const createUserFromToken = (token: string) => {
    return (dispatch: Dispatch<UserAction>) => {
        const decodedToken = jwtDecode<JWTTokenDecoded>(token);

        dispatch({
            type: UserActionTypes.CREATE_USER, payload: {
                id: decodedToken.Id,
                username: decodedToken.Name,
                score: decodedToken.Score
            }
        });
    };
}