import { AlertType } from './alert-type.ts';

export type AlertContextType = {
    alerts: AlertType[];
    addAlert: (message: string) => void;
    removeAlert: (id: number) => void;
};