import React, { useRef, useEffect } from "react";
import { HubConnectionBuilder, HubConnection, HubConnectionState } from "@microsoft/signalr";

interface GameConnection {
    Player1: string;
    RoomId: string | null;
    Player2Score?: number;
    MaxScore?: number;
}

interface useSignalRProps {
    setRoomId: React.Dispatch<React.SetStateAction<string | null>>;
    setGameStatus: React.Dispatch<React.SetStateAction<string>>;
    setMove: React.Dispatch<React.SetStateAction<number>>;
}

export const useSignalR = ({ setRoomId, setGameStatus, setMove }: useSignalRProps) => {
    const connectionRef = useRef<HubConnection | null>(null);

    const startConnection = async () => {
        if (connectionRef.current && connectionRef.current.state === HubConnectionState.Connected) {
            console.log("Уже подключен к серверу");
            return;
        }

        const newConnection = new HubConnectionBuilder()
            .withUrl("http://localhost:8080/gameHub", {
                accessTokenFactory: () => localStorage.getItem("token") || "", // Поддержка авторизации
            })
            .withAutomaticReconnect()
            .build();

        connectionRef.current = newConnection;

        newConnection.on("RequestToJoin", (connectionId: string, player1: string) => {
            console.log(`Запрос на вход: комната ${connectionId}, Игрок: ${player1}`);
        });

        newConnection.on("MoveMade", (position: number) => {
            console.log(`Ход сделан: ${position}`);
            setMove(position);
        });

        newConnection.on("GameStatusUpdated", (status: string) => {
            console.log(`Статус игры обновлён: ${status}`);
            setGameStatus(status);
        });

        newConnection.onclose(async () => {
            console.warn("Соединение разорвано. Переподключение...");
            setTimeout(startConnection, 5000); // Автоматическое переподключение через 5 сек
        });

        try {
            await newConnection.start();
            console.log("Подключение установлено");
        } catch (error) {
            console.error("Ошибка подключения:", error);
            setTimeout(startConnection, 5000); // Попытка переподключения через 5 сек
        }
    };

    const createOrJoinRoom = async (userId: string, roomId: string | null, maxScore?: number, player2score?: number) => {
        if (!connectionRef.current || connectionRef.current.state !== HubConnectionState.Connected) {
            console.error("Нет соединения с сервером");
            return;
        }

        const connection: GameConnection = { Player1: userId, RoomId: roomId, Player2Score: player2score, MaxScore: maxScore };
        console.log(connection);
        try {
            const roomIdReturned = await connectionRef.current.invoke("CreateOrJoinRoom", connection);
            setRoomId(roomIdReturned);
            console.log("room: " + roomIdReturned)
        } catch (error) {
            console.error("Ошибка при входе в комнату:", error);
        }
    };

    const confirmJoinRoom = async (roomId: string, playerId: string, isConfirmed: boolean) => {
        if (!connectionRef.current || connectionRef.current.state !== HubConnectionState.Connected) {
            console.error("Нет соединения с сервером");
            return;
        }

        try {
            await connectionRef.current.invoke("ConfirmJoinRoom", roomId, playerId, isConfirmed);
        } catch (error) {
            console.error("Ошибка подтверждения входа в комнату:", error);
        }
    };

    const sendMove = async (roomId: string, position: number, playerId: string) => {
        if (!connectionRef.current || connectionRef.current.state !== HubConnectionState.Connected) {
            console.error("Нет соединения с сервером");
            return;
        }

        try {
            await connectionRef.current.invoke("SendMove", roomId, position, playerId);
        } catch (error) {
            console.error("Ошибка отправки хода:", error);
        }
    };

    const leaveRoom = async (roomId: string | null) => {
        if (!connectionRef.current || connectionRef.current.state !== HubConnectionState.Connected) {
            console.error("Нет соединения с сервером");
            return;
        }

        try {
            await connectionRef.current.invoke("LeaveGame", roomId);
            setRoomId(null);
        } catch (error) {
            console.error("Ошибка выхода из комнаты:", error);
        }
    };

    useEffect(() => {
        startConnection();

        return () => {
            if (connectionRef.current?.state === HubConnectionState.Connected) {
                console.log("Отключение от сервера...");
                connectionRef.current?.stop();
            }
        };
    }, []);

    return {
        createOrJoinRoom,
        confirmJoinRoom,
        sendMove,
        leaveRoom,
        connectionRef,
    };
};
