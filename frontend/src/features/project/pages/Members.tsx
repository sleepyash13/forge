import { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import Swal  from 'sweetalert2';

import projectMemberService from '../services/projectMemberService';
import projectService from '../services/projectService';

import type { ProjectMember } from '../types/projectMember.types';
import type { Project } from '../types/project.types';

import ProjectNavigation from '../components/ProjectNavigation';

const roles = [
    'Owner',
    'Maintainer',
    'Developer',
    'Viewer',
];

const ProjectMembers = () => {
    const { projectId } = useParams<{ projectId: string }>();
    
    const [project, setProject] = useState<Project | null>(null);
    const [members, setMembers] = useState<ProjectMember[]>([]);

    const [email, setEmail] = useState('');
    const [role, setRole] = useState('Developer');

    const [loading, setLoading] = useState(true);
    const [adding, setAdding] = useState(false);

    useEffect(() => {
        if (!projectId) {
            return;
        }

        loadData();
    }, [projectId]);

    const loadData = async () => {
        if (!projectId) {
            return;
        }

        try {
            setLoading(true);

            const [projectData, memberData] = await Promise.all([
                projectService.getById(projectId),
                projectMemberService.getMembers(projectId),
            ]);

            setProject(projectData);
            setMembers(memberData);
        } catch (error: any) {
            console.error(error);

            await Swal.fire({
                icon: 'error',
                title: 'Unable to load members',
                text:
                    error?.response?.data?.message ??
                    'An unexpected error occurred.',
            });
        } finally {
            setLoading(false);
        }
    };

    const handleAddMember = async (event: React.FormEvent) => {
        event.preventDefault();

        if (!projectId) {
            return;
        }

        if (!email.trim()) {
            await Swal.fire({
                icon: 'warning',
                title: 'Email required',
                text: `Please enter the user's email address.`,
            });

            return;
        }

        try {
            setAdding(true);

            await projectMemberService.addMember(projectId, {
                email: email.trim(),
                role,
            });

            setEmail('');
            setRole('Developer');

            await Swal.fire({
                icon: 'success',
                title: 'Member added',
                text: 'The user has been added to the project.',
                timer: 1800,
                showConfirmButton: false,
            });

            await loadData();
        } catch (error: any) {
            console.error(error);

            await Swal.fire({
                icon: 'error',
                title: 'Unable to add member',
                text:
                    error?.response?.data?.message ??
                    'An unexpected error occurred.',
            });
        } finally {
            setAdding(false);
        }
    };

    const handleRoleChange = async (member: ProjectMember, newRole: string) => {
        if (!projectId || member.role === newRole) {
            return;
        }

        const result = await Swal.fire({
            icon: 'question',
            title: 'Change member role?',
            text: `${getMemberName(member)} will become ${newRole}.`,
            showCancelButton: true,
            confirmButtonText: 'Change role',
            cancelButtonText: 'Cancel',
        });

        if (!result.isConfirmed) {
            return;
        }

        try {
            await projectMemberService.updateRole(projectId, member.userId, {role: newRole,});

            await Swal.fire({
                icon: 'success',
                title: 'Role updated',
                timer: 1500,
                showConfirmButton: false,
            });

            await loadData();
        } catch (error: any) {
            console.error(error);

            await Swal.fire({
                icon: 'error',
                title: 'Unable to change role',
                text:
                    error?.response?.data?.message ??
                    'An unexpected error occurred.',
            });

            await loadData();
        }
    };

    const handleRemoveMember = async (member: ProjectMember) => {
        if (!projectId) {
            return;
        }

        const result = await Swal.fire({
            icon: 'warning',
            title: 'Remove member?',
            text: `${getMemberName(member)} will be removed from this project.`,
            showCancelButton: true,
            confirmButtonText: 'Remove',
            cancelButtonText: 'Cancel',
            confirmButtonColor: '#dc3545',
        });

        if (!result.isConfirmed) {
            return;
        }

        try {
            await projectMemberService.removeMember(projectId, member.userId);

            await Swal.fire({
                icon: 'success',
                title: 'Member removed',
                timer: 1500,
                showConfirmButton: false,
            });

            await loadData();
        } catch (error: any) {
            console.error(error);

            await Swal.fire({
                icon: 'error',
                title: 'Unable to remove member',
                text:
                    error?.response?.data?.message ??
                    'An unexpected error occurred.',
            });
        }
    };

    const getMemberName = (member: ProjectMember) => {
        if (member.displayName?.trim()) {
            return member.displayName;
        }

        return `${member.firstName} ${member.lastName}`;
    };

    const getRoleBadgeClass = (memberRole: string) => {
        switch (memberRole) {
            case 'Owner':
                return 'bg-danger';
            case 'Maintainer':
                return 'bg-warning text-dark';
            case 'Developer':
                return 'bg-primary';
            case 'Viewer':
                return 'bg-secondary';
            default:
                return 'bg-secondary';
        }
    };

    if (loading) {
        return (
            <div className='container-fluid py-4'>
                <div className='text-center py-5'>
                    <div className='spinner-border' role='status' >
                        <span className='visually-hidden'>
                            Loading...
                        </span>
                    </div>

                    <div className='mt-3'>
                        Loading project members...
                    </div>
                </div>
            </div>
        );
    }

    return (
        <div className='container-fluid py-4'>
            <ProjectNavigation />

            <div className='d-flex justify-content-between align-items-center mb-4'>
                <div>
                    <h2 className='mb-1'>
                        {project?.name ?? 'Project'} Members
                    </h2>

                    <p className='text-muted mb-0'>
                        Manage project members and their roles.
                    </p>
                </div>
            </div>

            <div className='card shadow-sm mb-4'>
                <div className='card-header bg-white'>
                    <h5 className='mb-0'>
                        Add Member
                    </h5>
                </div>

                <div className='card-body'>
                    <form onSubmit={handleAddMember}>
                        <div className='row g-3 align-items-end'>
                            <div className='col-md-6'>
                                <label htmlFor='memberEmail' className='form-label' >
                                    Email
                                </label>

                                <input id='memberEmail' type='email' className='form-control'
                                    placeholder='user@example.com' value={email}
                                    onChange={(event) => setEmail(event.target.value) }
                                    disabled={adding} />
                            </div>

                            <div className='col-md-6'>
                                <label htmlFor='memberRole' className='form-label' >
                                    Role
                                </label>

                                <select id='memberRole' className='form-select' value={role}
                                    onChange={(event) => setRole(event.target.value) }
                                    disabled={adding} >
                                    {roles .filter((item) => item !== 'Owner').map((item) => (
                                        <option key={item} value={item}>
                                            {item}
                                        </option>
                                    ))}
                                </select>
                            </div>

                            <div className='col-md-12'>
                                <small className='text-muted float-end'>
                                    Owner assignment is restricted by
                                    project authorization.
                                </small>
                            </div>

                            <div className='col-md-2'>
                                <button type='submit' className='btn btn-primary w-100'
                                    disabled={adding} >
                                    {adding ? (
                                        <>
                                            <span  className='spinner-border spinner-border-sm me-2'
                                                role='status' />
                                            Adding...
                                        </>
                                    ) : (
                                        'Add Member'
                                    )}
                                </button>
                            </div>
                        </div>
                    </form>
                </div>
            </div>

            <div className='card shadow-sm'>
                <div className='card-header bg-white d-flex justify-content-between align-items-center'>
                    <h5 className='mb-0'>
                        Members
                    </h5>

                    <span className='badge bg-secondary'>
                        {members.length}
                    </span>
                </div>

                <div className='card-body p-0'>
                    {members.length === 0 ? (
                        <div className='text-center py-5 text-muted'>
                            No project members found.
                        </div>
                    ) : (
                        <div className='table-responsive'>
                            <table className='table table-hover align-middle mb-0'>
                                <thead className='table-light'>
                                    <tr>
                                        <th className='px-4'> Member </th>
                                        <th> Email </th>
                                        <th> Role </th>
                                        <th> Added </th>
                                        <th className='text-end px-4'> Actions </th>
                                    </tr>
                                </thead>

                                <tbody>
                                    {members.map((member) => (
                                        <tr key={member.userId}>
                                            <td className='px-4'>
                                                <div className='fw-semibold'>
                                                    {getMemberName(member)}
                                                </div>

                                                <small className='text-muted'>
                                                    {member.firstName}{' '}
                                                    {member.middleName
                                                        ? `${member.middleName} `
                                                        : ''}
                                                    {member.lastName}
                                                </small>
                                            </td>
                                            <td> {member.email} </td>
                                            <td>
                                                <span className={`badge ${getRoleBadgeClass(member.role)}`} >
                                                    {member.role}
                                                </span>
                                            </td>
                                            <td> {new Date(member.createdAt).toLocaleDateString()}</td>
                                            <td className='text-end px-4'>
                                                <div className='d-flex justify-content-end gap-2'>

                                                    <select className='form-select form-select-sm'
                                                        style={{ width: '140px', }} value={member.role}
                                                        onChange={(event) => handleRoleChange(member, event.target.value)}>
                                                        {roles.map((item) => (
                                                            <option key={item} value={item}>
                                                                {item}
                                                            </option>
                                                        ))}
                                                    </select>

                                                    <button type='button' className='btn btn-sm btn-outline-danger'
                                                        onClick={() => handleRemoveMember(member)}>
                                                        Remove
                                                    </button>
                                                </div>
                                            </td>
                                        </tr>
                                    ))}
                                </tbody>
                            </table>
                        </div>
                    )}
                </div>
            </div>
        </div>
    );
};

export default ProjectMembers;