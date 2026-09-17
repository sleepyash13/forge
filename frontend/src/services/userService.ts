import { apiClient } from './apiClient';
import { getCsrfToken } from '../services/authService';

export interface UserProfile {
    id: string;
    email: string;
    firstName: string;
    middleName: string | null;
    lastName: string;
    displayName: string | null;
    createdAt: string;
    updatedAt: string;
}

interface UserProfileResponse {
    success: boolean;
    message: string;
    data: UserProfile;
}

export interface UpdateProfileRequest {
    displayName: string | null;
    firstName: string;
    middleName: string | null;
    lastName: string;
    email: string;
}

interface UpdateProfileResponse {
    success: boolean;
    message: string;
    data: UserProfile;
}

export interface ChangePasswordRequest {
    currentPassword: string;
    newPassword: string;
    confirmPassword: string;
}

interface ChangePasswordResponse {
    success: boolean;
    message: string;
}

export async function getUserDetails(): Promise<UserProfile> {
    const response = await apiClient.get<UserProfileResponse>("/users/me");

    return response.data.data;
}

export async function updateProfile(request: UpdateProfileRequest): Promise<UserProfile> {
    await getCsrfToken();

    const response = await apiClient.put<UpdateProfileResponse>("/users/profile", request);

    return response.data.data;
}

export async function changePassword(request: ChangePasswordRequest): Promise<void> {
    await getCsrfToken();

    await apiClient.put<ChangePasswordResponse>("/auth/password", request);
}