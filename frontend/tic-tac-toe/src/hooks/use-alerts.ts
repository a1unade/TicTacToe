import { useContext } from 'react';
import { AlertContextType } from '../types/alert/alert-context-type.ts';
import { AlertContext } from '../contexts/alert-provider.tsx';

export const useAlerts = (): AlertContextType => {
    const context = useContext(AlertContext);
    if (!context) {
        throw new Error('useAlerts must be used within an AlertProvider');
    }
    return context;
};