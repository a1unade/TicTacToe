export interface AuthResponse {
    entityId: string;
    token: string;
    isSuccessfully: boolean;
    message: string | null;
}