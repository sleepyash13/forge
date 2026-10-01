import { apiClient } from '../../../services/apiClient';
import { getCsrfToken } from '../../auth/services/authService';
import type {
    Repository,
    CreateRepositoryRequest,
    UpdateRepositoryRequest
} from '../types/repository.types';

const repositoryService = {
    async getByProjectId(projectId: string): Promise<Repository | null> {
        try {
            const response = await apiClient.get(`/projects/${projectId}/repository`);

            return response.data;
        } catch (error: any) {
            if (error.response?.status === 404) {
                return null;
            }

            throw error;
        }
    },

    async create(projectId: string, request: CreateRepositoryRequest): Promise<Repository> {
        await getCsrfToken();
        const response = await apiClient.post(`/projects/${projectId}/repository`, request);

        return response.data;
    },

    async update(projectId: string, request: UpdateRepositoryRequest): Promise<Repository> {
        await getCsrfToken();
        const response = await apiClient.put(`/projects/${projectId}/repository`, request);

        return response.data;
    },

    async delete(projectId: string): Promise<void> {
        await getCsrfToken();

        await apiClient.delete(`/projects/${projectId}/repository`);
    }
};

export default repositoryService;