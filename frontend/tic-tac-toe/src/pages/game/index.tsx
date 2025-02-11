import {useEffect, useState} from 'react';
import {useUserTypedSelector} from "../../hooks/use-typed-selector.ts";
import {useParams} from "react-router-dom";
import apiClient from "../../utils/api-client.ts";
import {GameResponse} from "../../interfaces/game/game-response.ts";
import {useSignalR} from "../../hooks/use-signalr.ts";

const Game = () => {
    const {id} = useUserTypedSelector(state => state.user);
    const {roomId} = useParams<string>();
    const [game, setGame] = useState<GameResponse>();
    const [isXNext, setIsXNext] = useState(true);
    const [winningLine, setWinningLine] = useState<number[] | null>(null);
    const {joinGame, sendMove, board, initializeBoard} = useSignalR()

    useEffect(() => {
        apiClient.get<GameResponse>(`Room/GetRoomById?roomId=${roomId}`)
            .then((response) => {
               if (response.status === 200) {
                   setGame(response.data);
                   initializeBoard(response.data.match.board);
               }
            });
    }, [initializeBoard, game, setGame]);

    const handleClick = async (index: number) => {
        if (board[index] || winningLine) return;
        const newBoard = board.slice();
        newBoard[index] = id === game!.firstPlayer.userId ? game!.firstPlayer.symbol : game!.secondPlayer.symbol;
        setIsXNext(!isXNext);

        await sendMove(id, roomId!, index);

        const winnerData = calculateWinner(newBoard);
        if (winnerData) {
            setWinningLine(winnerData.line);
        }
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

    //const winnerData = calculateWinner(board);
    //const winner = winnerData ? winnerData.winner : null;
    //const currentPlayer = isXNext ? players[0] : players[1];
    //const status = winner ? `Winner: ${winner}` : `Next player: ${currentPlayer.name} (${currentPlayer.symbol})`;

    return (
        <div className="game">
            <button className={"main-button"} onClick={() => joinGame(id, roomId!)}>Присоединиться</button>
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
