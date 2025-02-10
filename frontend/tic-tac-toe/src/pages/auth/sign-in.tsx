import {useState} from "react";
import {makePasswordVisible} from "../../utils/button-handlers.ts";
import {validatePassword, validateUsername} from "../../utils/validator.ts";

const SignIn = () => {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');

    const handleNextButtonClick = async () => {
        const messagePassword = validatePassword(password);
        if (messagePassword.length > 0) {
            document.getElementById('password')!.classList.add('error', 'shake');
            document.getElementById('password-error')!.classList.remove('hidden');
            document.getElementById('password-message')!.textContent = messagePassword;
            setTimeout(() => {
                document.getElementById('password')!.classList.remove('shake');
            }, 500);
        }

        const messageUsername = validateUsername(username);
        if (messageUsername.length > 0) {
            document.getElementById('username')!.classList.add('error', 'shake');
            document.getElementById('username-error')!.classList.remove('hidden');
            document.getElementById('username-message')!.textContent = messageUsername;
            setTimeout(() => {
                document.getElementById('username')!.classList.remove('shake');
            }, 500);
        }

        // if (message.length === 0) {
        //     processAuth();
        // }
    };

    return (
        <div className="content">
            <div className="sign-container">
                <div>
                    <h1>Добро пожаловать!</h1>
                    <div className="input-container" style={{ marginBottom: 0 }}>
                        <input
                            type="username"
                            id="username"
                            value={username}
                            style={{ marginBottom: 0 }}
                            onChange={(e) => setUsername(e.target.value)}
                            placeholder="Имя пользователя"
                        />
                        <label>Имя пользователя</label>
                        <div id="username-error" className="error-message hidden" style={{ marginTop: 10 }}>
                            <span>
                                <svg
                                    aria-hidden="true"
                                    fill="currentColor"
                                    focusable="false"
                                    width="16px"
                                    height="16px"
                                    viewBox="0 0 24 24"
                                    xmlns={"https://www.w3.org/2000/svg"}
                                >
                                  <path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z" />
                                </svg>
                            </span>
                            <span id="username-message" />
                        </div>
                    </div>
                    <div className="input-container" style={{ marginBottom: 20 }}>
                        <input
                            id="password"
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                            type="password"
                            placeholder="Пароль"
                        />
                        <label>Пароль</label>
                        <div id="password-error" className="error-message hidden">
                            <span>
                                <svg
                                    aria-hidden="true"
                                    fill="currentColor"
                                    focusable="false"
                                    width="16px"
                                    height="16px"
                                    viewBox="0 0 24 24"
                                    xmlns={"https://www.w3.org/2000/svg"}
                                >
                                <path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z" />
                                </svg>
                            </span>
                            <span id="password-message" />
                        </div>
                    </div>
                    <div className="sign-buttons" style={{ marginTop: 10 }}>
                        <button className="password-button" id="showPasswordButton" onClick={makePasswordVisible}>
                            Показать пароль
                        </button>
                    </div>
                    <div className="sign-buttons">
                        <button
                            style={{ marginLeft: "auto" }}
                            className="right-button"
                            onClick={handleNextButtonClick}
                        >
                            Далее
                        </button>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default SignIn;