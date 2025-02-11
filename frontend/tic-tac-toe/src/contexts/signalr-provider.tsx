import React, { createContext, useEffect, useRef } from "react";
import { HubConnectionBuilder, HubConnection, HubConnectionState } from "@microsoft/signalr";

// Типы для соединения
interface JoinRoomConnection {
    Player: string;
    RoomId: string;
}

interface SendMoveRequest {
    RoomId: string;
    Position: number;
    PlayerId: string;
}

export interface SignalRContextProps {
    connectionRef: React.RefObject<HubConnection | null>;
    createOrJoinRoom: (userId: string, roomId: string | null, maxScore?: number) => Promise<void>;
    joinRoom: (userId: string, roomId: string) => Promise<void>;
    joinGame: (userId: string, roomId: string) => Promise<void>;
    sendMove: (userId: string, roomId: string, position: number) => Promise<void>;
}

export const SignalRContext = createContext<SignalRContextProps | undefined>(undefined);

export const SignalRProvider:React.FC<{ children: React.ReactNode }> = ({ children }) => {
    const connectionRef = useRef<HubConnection | null>(null);

    const startConnection = async () => {
        if (connectionRef.current && connectionRef.current.state === HubConnectionState.Connected) {
            console.log("Уже подключен к серверу");
            return;
        }

        const newConnection = new HubConnectionBuilder()
            .withUrl("http://localhost:8080/gameHub")
            .build();

        connectionRef.current = newConnection;

        newConnection.on("Info", (data) => {
            console.log("Info:", data);
        });

        newConnection.on("ReceiveMove", (data) => {
            console.log("Move:", data);
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

        const connection: { Player1: string; RoomId: string | null; MaxScore?: number } = { Player1: userId, RoomId: roomId, MaxScore: maxScore };
        try {
            const roomIdReturned = await connectionRef.current.invoke("CreateOrJoinRoom", connection);
            console.log("room: " + roomIdReturned);
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
        try {
            const roomIdReturned = await connectionRef.current.invoke("JoinRoom", connection);
            console.log("Ответ от сервера:", roomIdReturned);
        } catch (error) {
            console.error("Ошибка при входе в комнату:", error);
        }
    };

    const joinGame = async (userId: string, roomId: string) => {
        if (!connectionRef.current || connectionRef.current.state !== HubConnectionState.Connected) {
            console.error("Нет соединения с сервером");
            return;
        }

        const connection: JoinRoomConnection = { Player: userId, RoomId: roomId };
        console.log(connection);

        try {
            const roomIdReturned = await connectionRef.current.invoke("JoinGame", connection);
            console.log("Ответ от сервера:", roomIdReturned);
        } catch (error) {
            console.error("Ошибка при входе в игру:", error);
        }
    };

    const sendMove = async (userId: string, roomId: string, position: number) => {
        if (!connectionRef.current || connectionRef.current.state !== HubConnectionState.Connected) {
            console.error("Нет соединения с сервером");
            return;
        }

        const connection: SendMoveRequest = { RoomId: roomId, Position: position, PlayerId: userId, };
        console.log(connection);

        try {
            const roomIdReturned = await connectionRef.current.invoke("SendMove", connection);
            console.log("Ответ от сервера:", roomIdReturned);
        } catch (error) {
            console.error("Ошибка при входе в комнату:", error);
        }
    }

    useEffect(() => {
        startConnection();

        return () => {
            connectionRef.current?.stop();
        };
    }, []);

    return (
        <SignalRContext.Provider value={{ connectionRef, createOrJoinRoom, joinRoom, joinGame, sendMove }}>
            {children}
        </SignalRContext.Provider>
    );
};
