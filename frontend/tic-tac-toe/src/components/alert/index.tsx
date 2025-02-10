import { useEffect, useState } from 'react';

const Alert = (props: { message: string; onClose: () => void }) => {
    const { message, onClose } = props;
    const [visible, setVisible] = useState(false);

    useEffect(() => {
        const showTimer = setTimeout(() => {
            setVisible(true);
        }, 10);

        const hideTimer = setTimeout(() => {
            setVisible(false);
        }, 2000);

        return () => {
            clearTimeout(showTimer);
            clearTimeout(hideTimer);
        };
    }, []);

    useEffect(() => {
        if (!visible) {
            const removeTimer = setTimeout(() => {
                onClose();
            }, 500);

            return () => clearTimeout(removeTimer);
        }
    }, [visible, onClose]);

    return <div className={`alert ${visible ? 'alert-show' : 'alert-hide'}`}>{message}</div>;
};

export default Alert;