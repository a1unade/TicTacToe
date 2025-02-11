import React, { useState } from 'react';

const Game: React.FC = () => {
    const [board, setBoard] = useState<Array<string | null>>(Array(9).fill(null));
    const [isXNext, setIsXNext] = useState(true);
    const [winningLine, setWinningLine] = useState<number[] | null>(null);

    const players = [
        { name: 'Player 1', symbol: 'X', rating: 1200 },
        { name: 'Player 2', symbol: 'O', rating: 1150 }
    ];

    const handleClick = (index: number) => {
        if (board[index] || winningLine) return;
        const newBoard = board.slice();
        newBoard[index] = isXNext ? 'X' : 'O';
        setBoard(newBoard);
        setIsXNext(!isXNext);

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

    const winnerData = calculateWinner(board);
    const winner = winnerData ? winnerData.winner : null;
    const currentPlayer = isXNext ? players[0] : players[1];
    const status = winner ? `Winner: ${winner}` : `Next player: ${currentPlayer.name} (${currentPlayer.symbol})`;

    return (
        <div className="game">
            <div className="players">
                {players.map(player => (
                    <div key={player.symbol} className="player">
                        <strong>{player.name}</strong> (Rating: {player.rating})
                    </div>
                ))}
            </div>
            <h1 className="status">{status}</h1>
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
