
export interface ProjectMember {
    userId: string;
    email: string;
    firstName: string;
    middleName: string | null;
    lastName: string;
    displayName: string | null;
    role: string;
    createdAt: string;
    updatedAt: string | null;
}

export interface AddProjectMemberRequest {
    email: string;
    role: string;
}

export interface UpdateProjectMemberRoleRequest {
    role: string;
}