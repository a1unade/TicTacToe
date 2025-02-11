import React, { useRef, useEffect } from "react";
import { HubConnectionBuilder, HubConnection, HubConnectionState } from "@microsoft/signalr";

interface GameConnection {
    Player1: string;
    RoomId: string | null;
    MaxScore?: number;
}

interface JoinRoomConnection {
    Player: string;
    RoomId: string;
}

interface useSignalRProps {
    setRoomId: React.Dispatch<React.SetStateAction<string | null>>;
    setGameStatus: React.Dispatch<React.SetStateAction<string>>;
    setMove: React.Dispatch<React.SetStateAction<number>>;
}

export const useSignalR = ({ setRoomId }: useSignalRProps) => {
    const connectionRef = useRef<HubConnection | null>(null);

    const startConnection = async () => {
        if (connectionRef.current && connectionRef.current.state === HubConnectionState.Connected) {
            console.log("Уже подключен к серверу");
            return;
        }

        const newConnection = new HubConnectionBuilder()
            .withUrl("http://localhost:5026/gameHub")
            .build();

        connectionRef.current = newConnection;

        newConnection.on("Info", (data) => {
            console.log("Info:", data);
        });

        try {
            await newConnection.start();
            console.log("Подключение установлено");
        } catch (error) {
            console.error("Ошибка подключения:", error);
            setTimeout(startConnection, 5000);
        }
    };

    const createOrJoinRoom = async (userId: string, roomId: string | null, maxScore?: number) => {
        if (!connectionRef.current || connectionRef.current.state !== HubConnectionState.Connected) {
            console.error("Нет соединения с сервером");
            return;
        }

        const connection: GameConnection = { Player1: userId, RoomId: roomId, MaxScore: maxScore };
        console.log(connection);
        try {
            const roomIdReturned = await connectionRef.current.invoke("CreateOrJoinRoom", connection);
            setRoomId(roomIdReturned);
            console.log("room: " + roomIdReturned)
        } catch (error) {
            console.error("Ошибка при входе в комнату:", error);
        }
    };


    const joinRoom = async (userId: string, roomId: string) => {
        if (!connectionRef.current || connectionRef.current.state !== HubConnectionState.Connected) {
            console.error("Нет соединения с сервером");
            return;
        }

        const connection: JoinRoomConnection = { Player: userId, RoomId: roomId };

        console.log("Отправляем объект в JoinRoom:", JSON.stringify(connection));

        try {
            const roomIdReturned = await connectionRef.current.invoke("JoinRoom", connection);
            console.log("Ответ от сервера:", roomIdReturned);
        } catch (error) {
            console.error("Ошибка при входе в комнату:", error);
        }
    };


    useEffect(() => {
        startConnection();

        return () => {
            connectionRef.current?.stop();
        };
    }, []);

    return {
        createOrJoinRoom,
        joinRoom,
        connectionRef,
    };
};
