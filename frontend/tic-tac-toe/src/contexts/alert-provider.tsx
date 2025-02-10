import React, { createContext, useState } from 'react';
import { AlertContextType } from '../types/alert/alert-context-type'; // Ваши типы
import { AlertType } from '../types/alert/alert-type';

export const AlertContext = createContext<AlertContextType | undefined>(undefined);

export const AlertProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
    const [alerts, setAlerts] = useState<AlertType[]>([]);

    const addAlert = (message: string) => {
        const id = Date.now();
        setAlerts([...alerts, { id, message }]);
    };

    const removeAlert = (id: number) => {
        setAlerts(alerts.filter((alert) => alert.id !== id));
    };

    return (
        <AlertContext.Provider value={{ alerts, addAlert, removeAlert }}>
            {children}
        </AlertContext.Provider>
    );
};