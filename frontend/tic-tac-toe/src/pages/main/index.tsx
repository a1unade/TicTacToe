import React, { useState, useEffect } from 'react';

interface Game {
    id: string;
    creator: string;
    createdAt: string;
    status: 'waiting' | 'started' | 'finished';
    maxRating: number;
}

const Main: React.FC = () => {
    const [games, setGames] = useState<Game[]>([]);
    const [page, setPage] = useState(1);
    const [showRating, setShowRating] = useState(false);
    const [showCreateGame, setShowCreateGame] = useState(false);
    const [maxRating, setMaxRating] = useState(0);
    const [userRating] = useState(1100); // Заглушка для рейтинга пользователя

    useEffect(() => {
        fetchGames().then(() => console.log("список игор обновлен!"));
    }, [page]);

    const getStatusText = (status: string) => {
        switch (status) {
            case "waiting":
                return "Ожидание";
            case "started":
                return "В процессе";
            case "finished":
                return "Завершена";
            default:
                return "Неизвестный статус";
        }
    };

    const fetchGames = async () => {
        // Заглушка для загрузки игр
        const newGames: Game[] = [
            { id: '1', creator: 'Alice', createdAt: '2024-02-10T10:00:00', status: 'waiting', maxRating: 1200 },
            { id: '2', creator: 'Bob', createdAt: '2024-02-10T09:30:00', status: 'started', maxRating: 1300 }
        ];
        setGames(prev => [...prev, ...newGames]);
    };

    return (
        <div className="game-selection">
            <h1>Выбор игр</h1>
            <div className={"buttons-list"}>
                <button className={"main-button"} onClick={() => setShowRating(true)}>Рейтинг</button>
                <button className={"main-button"} onClick={() => setShowCreateGame(true)}>Создать игру</button>
            </div>

            <div className="game-list">
                {games.map(game => (
                    <div key={game.id} className={`game-item ${game.status}`}>
                        <p>Создатель: {game.creator}</p>
                        <p>Дата: {new Date(game.createdAt).toLocaleString()}</p>
                        <p>Статус: {getStatusText(game.status)}</p>
                        <button className="join-button" disabled={userRating > game.maxRating}>
                            Войти
                        </button>
                    </div>
                ))}
            </div>
            <button className={"main-button"} onClick={() => setPage(page + 1)}>Загрузить ещё</button>

            {showRating && (
                <div className="modal">
                    <h2>Рейтинг игроков</h2>
                    <button onClick={() => setShowRating(false)}>Закрыть</button>
                </div>
            )}

            {showCreateGame && (
                <div className="modal">
                    <h2>Создать игру</h2>
                    <input type="number" value={maxRating} onChange={e => setMaxRating(Number(e.target.value))} placeholder="Макс. рейтинг" />
                    <button onClick={() => setShowCreateGame(false)}>Создать</button>
                </div>
            )}
        </div>
    );
};

export default Main;
