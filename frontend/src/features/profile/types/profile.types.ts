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

export interface UserProfileResponse {
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

export interface UpdateProfileResponse {
    success: boolean;
    message: string;
    data: UserProfile;
}

export interface ChangePasswordRequest {
    currentPassword: string;
    newPassword: string;
    confirmPassword: string;
}

export interface ChangePasswordResponse {
    success: boolean;
    message: string;
}