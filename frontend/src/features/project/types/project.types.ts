export interface Project {
    id: string;
    name: string;
    description: string | null;
    createdAt: string;
    updatedAt: string | null;
    isActive: boolean;
}

export interface CreateProjectRequest {
    name: string;
    description?: string;
}

export interface UpdateProjectRequest {
    name: string;
    description?: string;
}