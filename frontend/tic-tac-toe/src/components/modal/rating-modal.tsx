import React, { useEffect, useState } from 'react';
import apiClient from "../../utils/api-client.ts";
import { RatingResponse } from "../../interfaces/rating/rating-response.ts";
import { RatingListResponse } from "../../interfaces/rating/rating-list-response.ts";
import { FaCrown } from "react-icons/fa";

const crownColors = ["#FFD700", "#C0C0C0", "#CD7F32"]; // Золото, серебро, бронза

const RatingModal = (props: {
    showRating: boolean,
    setShowRating: React.Dispatch<React.SetStateAction<boolean>>
}) => {
    const { showRating, setShowRating } = props;
    const [users, setUsers] = useState<RatingResponse[]>([]);

    useEffect(() => {
        if (!showRating) return;

        let isMounted = true;
        apiClient.get<RatingListResponse>("User/topUsers")
            .then((response) => {
                if (response.status === 200 && isMounted) {
                    setUsers(response.data.usersDtosScores);
                }
            })
            .catch((error) => console.error("Ошибка загрузки рейтинга:", error));

        return () => { isMounted = false };
    }, [showRating]);

    if (!showRating) return null;

    return (
        <div className="modal">
            <div className="modal-header">
                <h2>🏆 Рейтинг игроков</h2>
                <button className="modal-close-button" onClick={() => setShowRating(false)}>✖</button>
            </div>

            <div className="ranking-list">
                {users.length > 0 ? (
                    users.map((user, index) => (
                        <div key={user.id} className={`ranking-item ${index < 3 ? "top-player" : ""}`}>
                            {index < 3 ? (
                                <FaCrown className="crown-icon" style={{ color: crownColors[index] }} />
                            ) : (
                                <span className="ranking-number">{index + 1}</span>
                            )}
                            <span className="username">{user.name}</span>
                            <span className="rating">{user.score} 🏅</span>
                        </div>
                    ))
                ) : (
                    <p className="loading-text">Загрузка...</p>
                )}
            </div>
        </div>
    );
};

export default RatingModal;
