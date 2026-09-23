import { apiClient } from '../../../services/apiClient';
import { getCsrfToken } from '../../../features/auth/services/authService';
import type { 
    UserProfile,
    UserProfileResponse,
    UpdateProfileRequest,
    UpdateProfileResponse,
    ChangePasswordRequest,
    ChangePasswordResponse
} from '../types/profile.types'

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