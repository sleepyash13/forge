import { apiClient } from '../../../services/apiClient';
import { getCsrfToken } from '../../auth/services/authService';
import type {
    ProjectMember,
    AddProjectMemberRequest,
    UpdateProjectMemberRoleRequest,
} from '../types/projectMember.types';

const projectMemberService = {
    async getMembers(projectId: string): Promise<ProjectMember[]> {
        const response = await apiClient.get(`/projects/${projectId}/members`);

        return response.data;
    },

    async addMember(projectId: string,request: AddProjectMemberRequest): Promise<ProjectMember> {
        await getCsrfToken();
        const response = await apiClient.post(`/projects/${projectId}/members`, request);

        return response.data;
    },

    async updateRole(projectId: string, userId: string, request: UpdateProjectMemberRoleRequest): Promise<ProjectMember> {
        await getCsrfToken();
        const response = await apiClient.put(`/projects/${projectId}/members/${userId}`, request);

        return response.data;
    },

    async removeMember(projectId: string, userId: string): Promise<void> {
        await getCsrfToken();

        await apiClient.delete(`/projects/${projectId}/members/${userId}`);
    },
};

export default projectMemberService;