import {useContext} from "react";
import {SignalRContext, SignalRContextProps} from "../contexts/signalr-provider.tsx";

export const useSignalR = (): SignalRContextProps => {
    const context = useContext(SignalRContext);
    if (!context) {
        throw new Error("useSignalR must be used within a SignalRProvider");
    }
    return context;
};
