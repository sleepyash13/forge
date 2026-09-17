import { apiClient } from "./apiClient";

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

export async function getCsrfToken(): Promise<void> {
    await apiClient.get("/auth/csrf");
}

export async function register(request: RegisterRequest): Promise<void> {
    await getCsrfToken();

    await apiClient.post(
        "/auth/register",
        request
    );
}

export async function login(request: LoginRequest): Promise<AuthResponse> {
    await getCsrfToken();

    const response = await apiClient.post<AuthResponse>(
        "/auth/login",
        request
    );

    return response.data;
}

export async function getCurrentUser(): Promise<AuthUser> {
    const response = await apiClient.get<{
        success: boolean;
        data: AuthUser;
    }>("/auth/me");

    return response.data.data;
}

export async function logout(): Promise<void> {
    await getCsrfToken();

    await apiClient.post("/auth/logout");
}