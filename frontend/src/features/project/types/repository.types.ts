export interface Repository {
    id: string;
    projectId: string;
    repositoryUrl: string;
    defaultBranch: string;
    buildCommand: string | null;
    deployConfiguration: string | null;
    createdAt: string;
    updatedAt: string | null;
}

export interface CreateRepositoryRequest {
    repositoryUrl: string;
    defaultBranch: string;
    buildCommand?: string;
    deployConfiguration?: string;
}

export interface UpdateRepositoryRequest {
    repositoryUrl: string;
    defaultBranch: string;
    buildCommand?: string;
    deployConfiguration?: string;
}