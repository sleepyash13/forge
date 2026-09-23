import { apiClient } from "../../../services/apiClient";
import type {
    RegisterRequest,
    LoginRequest,
    AuthResponse,
    AuthUser
} from '../types/auth.types';

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