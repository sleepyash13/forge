import { apiClient } from '../../../services/apiClient';
import { getCsrfToken } from '../../../features/auth/services/authService';
import type {
    Project,
    CreateProjectRequest,
    UpdateProjectRequest
} from '../types/project.types'

const projectService = {
    async create(request: CreateProjectRequest): Promise<Project> {
        const response = await apiClient.post<Project>("/projects", request);

        return response.data;
    },

    async getById(projectId: string): Promise<Project> {
        const response = await apiClient.get<Project>(`/projects/${projectId}`);

        return response.data;
    },

    async update(projectId: string, request: UpdateProjectRequest): Promise<Project> {
        const response = await apiClient.put<Project>(`/projects/${projectId}`, request);

        return response.data;
    },

    async delete(projectId: string): Promise<void> {
        await apiClient.delete(`/projects/${projectId}`);
    },

    async getMyProjects(): Promise<Project[]> {
        const response = await apiClient.get<Project[]>('/projects');

        return response.data;
    },
};

export default projectService;