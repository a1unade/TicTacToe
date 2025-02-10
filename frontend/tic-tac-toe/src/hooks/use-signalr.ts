import React, { useRef, useEffect } from 'react';
import { HubConnectionBuilder, HubConnection, HubConnectionState } from '@microsoft/signalr';

interface ChatConnection {
    UserId: string;
    ChatId: string | null;
}

interface UseSignalRProps {
    setChatId: React.Dispatch<React.SetStateAction<string | null>>;
    setChatMessages: React.Dispatch<React.SetStateAction<ChatMessage[]>>;
}

export const useSignalr = ({ setChatId, setChatMessages }: UseSignalRProps) => {
    const connectionRef = useRef<HubConnection | null>(null);

    const joinChat = async (userId: string, chatId: string | null) => {
        if (!connectionRef.current || connectionRef.current.state !== HubConnectionState.Connected) {
            console.error('Connection not established');
            return;
        }

        const connection: ChatConnection = { UserId: userId, ChatId: chatId };

        try {
            const chatIdReturned = await connectionRef.current.invoke('JoinChat', connection);
            setChatId(chatIdReturned);
        } catch (error) {
            console.error('Error joining chat:', error);
        }
    };

    const sendMessage = async (
        message: string,
        userId: string,
        fileId: string | null,
        chatId: string | null,
    ) => {
        if (!connectionRef.current || connectionRef.current.state !== HubConnectionState.Connected) {
            console.error('Connection not established');
            return;
        }

        const request = { UserId: userId, ChatId: chatId, Message: message, FileId: fileId };

        try {
            await connectionRef.current.invoke('SendMessage', request);
        } catch (error) {
            console.error('Error sending message:', error);
        }
    };

    const startConnection = async () => {
        const newConnection = new HubConnectionBuilder()
            .withUrl('http://localhost:8080/supportChatHub')
            .withAutomaticReconnect()
            .build();

        connectionRef.current = newConnection;

        newConnection.on('ReceiveMessage', (message: ChatMessageResponse) => {
            console.log('Received message:', message);
            const mes: ChatMessage = {
                messageId: message.messageId,
                message: message.message,
                senderId: message.userId,
                attachment: message.attachment,
                time: message.time.split(':').slice(0, 2).join(':'),
                isRead: message.isRead,
                date: message.date,
            };
            setChatMessages((prevMessages: ChatMessage[] | null) =>
                prevMessages ? [mes, ...prevMessages] : [mes],
            );
        });

        newConnection.on('ReadMessages', (data: { messagesId: string[]; chatId: string }) => {
            const { messagesId } = data;
            if (Array.isArray(messagesId) && messagesId.length > 0) {
                setChatMessages((prevMessages) =>
                    prevMessages.map((msg) =>
                        messagesId.includes(msg.messageId) ? { ...msg, isRead: true } : msg,
                    ),
                );
            } else {
                console.error('Received data is not an array:', messagesId);
            }
        });

        try {
            await newConnection.start();
        } catch (error) {
            console.error('Error establishing connection:', error);
        }
    };

    const leaveChat = async (chatId: string | null) => {
        if (!connectionRef.current || connectionRef.current.state !== HubConnectionState.Connected) {
            console.error('Connection not established');
            return;
        }

        try {
            await connectionRef.current.invoke('LeaveChat', chatId);

            setChatId(null);
            setChatMessages([]);
        } catch (error) {
            console.error('Error leaving chat:', error);
        }
    };

    const readMessages = async (messagesIds: string[], chatId: string | null) => {
        if (!connectionRef.current || connectionRef.current.state !== HubConnectionState.Connected) {
            console.error('Connection not established');
            return;
        }

        try {
            await connectionRef.current.invoke('ReadMessages', {
                messagesId: messagesIds,
                chatId: chatId,
            });
        } catch (error) {
            console.error('Error sending read messages:', error);
        }
    };

    useEffect(() => {
        startConnection().then(() => console.log('Connection started'));

        return () => {
            connectionRef.current?.stop();
        };
    }, []);

    return {
        joinChat,
        sendMessage,
        connectionRef,
        readMessages,
        leaveChat,
        startConnection,
    };
};