export interface RegisterRequest {
    email: string;
    password: string;
    confirmPassword: string;
    displayName?: string;
    firstName: string;
    middleName?: string;
    lastName: string;
}

export interface LoginRequest {
    email: string;
    password: string;
}

export interface AuthUser {
    userId: string;
    email: string;
    displayName: string;
}

export interface AuthResponse {
    success: boolean;
    message?: string;
    data: AuthUser;
}