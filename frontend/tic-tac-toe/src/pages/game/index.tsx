import {useEffect, useState} from 'react';
import {useUserTypedSelector} from "../../hooks/use-typed-selector.ts";
import {useParams} from "react-router-dom";
import apiClient from "../../utils/api-client.ts";
import {GameResponse} from "../../interfaces/game/game-response.ts";
import {useSignalR} from "../../hooks/use-signalr.ts";
import {useAlerts} from "../../hooks/use-alerts.ts";

const Game = () => {
    const {id, score} = useUserTypedSelector(state => state.user);
    const {roomId} = useParams<string>();
    const [game, setGame] = useState<GameResponse>();
    const [winningLine, setWinningLine] = useState<number[] | null>(null);
    const {addAlert} = useAlerts();
    const {joinGame, sendMove, board, initializeBoard, gameMessage, gameStatus} = useSignalR()

    useEffect(() => {
        apiClient.get<GameResponse>(`Room/GetRoomById?roomId=${roomId}`)
            .then((response) => {
               if (response.status === 200) {
                   setGame(response.data);
                   initializeBoard(response.data.match.board);
               }
            });
    }, [initializeBoard, game, setGame, roomId]);

    useEffect(() => {
        if (gameStatus === "GameOver" || gameStatus === "Draw") {
            console.log("Игра окончена:", gameMessage);
            const winnerData = calculateWinner(board);
            if (winnerData) {
                setWinningLine(winnerData.line);
            }
        }
    }, [gameStatus, gameMessage, board]);

    const handleClick = async (index: number) => {
        if (!game) return;

        const { firstPlayer, secondPlayer } = game;

        if (id !== firstPlayer.userId && id !== secondPlayer?.userId) {
            addAlert("Вы зритель, вы не можете ходить!");
            return;
        }

        if (game.match.currentPlayerId !== id) {
            addAlert("Сейчас не ваш ход!");
            return;
        }

        if (board[index] || winningLine) return;

        await sendMove(id, roomId!, index);
    };

    const handleJoinGame = async () => {
        if (!game) return;

        const { firstPlayer, secondPlayer } = game;

        if (firstPlayer.userId === id || secondPlayer?.userId === id) return;

        if (score > parseInt(game.match.score)) {
            addAlert("Ваш рейтинг слишком высок для этой комнаты!");
            return;
        }

        await joinGame(id, roomId!);
    };

    const calculateWinner = (squares: Array<string | null>) => {
        const lines = [
            [0, 1, 2], [3, 4, 5], [6, 7, 8],
            [0, 3, 6], [1, 4, 7], [2, 5, 8],
            [0, 4, 8], [2, 4, 6]
        ];
        for (const line of lines) {
            const [a, b, c] = line;
            if (squares[a] && squares[a] === squares[b] && squares[a] === squares[c]) {
                return { winner: squares[a], line };
            }
        }
        return null;
    };

    if (!game)
        return null;

    const isPlayerInGame = game.firstPlayer.userId === id || game.secondPlayer?.userId === id;

    return (
        <div className="game">
            {!isPlayerInGame && (
                <button className="main-button" onClick={handleJoinGame}>
                    Присоединиться
                </button>
            )}
            {gameStatus && (
                <div className="game-over">
                    <strong>{gameMessage}</strong>
                </div>
            )}
            <div className="players">
                <div key={game.firstPlayer.userId} className="player">
                    <strong>{game.firstPlayer.name}</strong> (Rating: {game.firstPlayer.score})
                </div>
                <div key={game.secondPlayer?.userId} className="player">
                    <strong>{game.secondPlayer?.name}</strong> (Rating: {game.secondPlayer?.score})
                </div>
            </div>
            <div className="board">
                {board.map((cell, index) => (
                    <button
                        key={index}
                        className={`cell ${winningLine && winningLine.includes(index) ? 'winner-cell' : ''}`}
                        onClick={() => handleClick(index)}
                    >
                        {cell}
                    </button>
                ))}
            </div>
        </div>
    );
};

export default Game;
