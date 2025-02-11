import React, { useState, useEffect, useRef, useCallback } from 'react';
import { RoomResponse } from "../../interfaces/rooms/room-response.ts";
import { RoomsListResponse } from "../../interfaces/rooms/rooms-list-response.ts";
import apiClient from "../../utils/api-client.ts";
import RatingModal from "../../components/modal/rating-modal.tsx";
import {useSignalR} from "../../hooks/use-signalr.ts";
import {useUserTypedSelector} from "../../hooks/use-typed-selector.ts";
import {useNavigate} from "react-router-dom";
import {useUserActions} from "../../hooks/use-actions.ts";

const Main: React.FC = () => {
    const [rooms, setRooms] = useState<RoomResponse[]>([]);
    const [pageCount, setPageCount] = useState(0);
    const [fetching, setFetching] = useState(true);
    const [page, setPage] = useState(1);
    const [showRating, setShowRating] = useState(false);
    const [showCreateGame, setShowCreateGame] = useState(false);
    const [maxRating, setMaxRating] = useState(0);
    const navigate = useNavigate();
    const componentRef = useRef<HTMLDivElement | null>(null);
    const {id, score, username} = useUserTypedSelector(state => state.user);
    const {deleteUser} = useUserActions();

    const { createOrJoinRoom, joinRoom } = useSignalR();

    const handleScroll = useCallback((event: Event) => {
        const target = event.target as Document;
        if (
            target.documentElement.scrollHeight -
            (target.documentElement.scrollTop + window.innerHeight) <
            600 &&
            page < pageCount
        ) {
            setFetching(true);
            setPage(page + 1);
        }
    }, [page, pageCount]);

    useEffect(() => {
        const currentRef = componentRef.current;
        if (currentRef) {
            currentRef.addEventListener("scroll", handleScroll);
        }

        return () => {
            if (currentRef) {
                currentRef.removeEventListener("scroll", handleScroll);
            }
        };
    }, [handleScroll]);

    useEffect(() => {
        if (fetching) {
            apiClient
                .post<RoomsListResponse>(
                    `/Room/getRooms`,
                    {
                        page: page,
                        size: 6
                    }
                )
                .then((response) => {
                    if (response.data.gameDtos !== null) {
                        setRooms((prevData) => [
                            ...prevData,
                            ...response.data.gameDtos,
                        ]);
                    }

                    if (pageCount === 0) {
                        setPageCount(response.data.totalCount);
                    }
                })
                .catch((error) => {
                    console.error("Error fetching data:", error);
                })
                .finally(() => setFetching(false));
        }
    }, [fetching, page, pageCount]);

    const getStatusText = (status: string) => {
        switch (status) {
            case "Waiting":
                return "Ожидание";
            case "Started":
                return "В процессе";
            case "Finished":
                return "Завершена";
            default:
                return "Неизвестный статус";
        }
    };

    const handleGameJoin = async (gameId: string) => {
        await joinRoom(id, gameId)
            .then(r => console.log(r))

        navigate(`/game/${gameId}`);
    }

    return (
        <div className="game-selection">
            <h1 style={{ marginBottom: 40 }}>Выбор игр</h1>
            <div className={"buttons-list"}>
                <span>Твоя статистика: </span>
                <b><span>{username}</span></b>
                <b><span>{score}</span></b>
            </div>
            <div className={"buttons-list"}>
                <button className={"main-button"} onClick={() => setShowRating(true)}>Рейтинг</button>
                <button className={"main-button"} onClick={() => setShowCreateGame(true)}>Создать игру</button>
                <button className={"main-button"} onClick={() => deleteUser()}>Выйти из аккаунта</button>
            </div>

            <div className="game-list" ref={componentRef}>
                {rooms.length === 0 && !fetching ? (
                    <p>На данный момент нет доступных игр.</p>
                ) : (
                    rooms.map(room => (
                        <div key={room.id} className={`game-item ${room.status}`} onClick={() => handleGameJoin(room.id)}>
                            <p>Создатель: {room.creatorUserName}</p>
                            <p>Дата: {new Date(room.createdAt).toLocaleString()}</p>
                            <p>Статус: {getStatusText(room.status)}</p>
                        </div>
                    ))
                )}
            </div>

            <RatingModal showRating={showRating} setShowRating={setShowRating} />

            {showCreateGame && (
                <div className="modal">
                    <h2>Создать игру</h2>
                    <input type="number" value={maxRating} onChange={e => setMaxRating(parseInt(e.target.value))} placeholder="Макс. рейтинг" />
                    <button onClick={() => createOrJoinRoom(id, null, maxRating)}>Создать</button>
                </div>
            )}
        </div>
    );
};

export default Main;
