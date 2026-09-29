import { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import projectService from '../services/projectService';
import type { Project } from '../types/project.types';
import Swal from 'sweetalert2';

export default function ProjectDetails() {
    const { projectId } = useParams();
    const navigate = useNavigate();

    const [project, setProject] = useState<Project | null>(null);

    const [name, setName] = useState('');
    const [description, setDescription] = useState('');

    const [loading, setLoading] = useState(true);
    const [saving, setSaving] = useState(false);

    const [message, setMessage] = useState('');
    const [error, setError] = useState('');

    useEffect(() => {
        if (!projectId) {
            return;
        }

        loadProject(projectId);
    }, [projectId]);

    const loadProject = async (id: string) => {
        try {
            setLoading(true);
            setError('');

            const result = await projectService.getById(id);

            setProject(result);
            setName(result.name);
            setDescription(result.description || '');
        } catch (err: any) {
            setError(err.response?.data?.message || 'Unable to load project.');
        } finally {
            setLoading(false);
        }
    };

    const handleUpdate = async () => {
        if (!projectId) {
            return;
        }

        try {
            setSaving(true);
            setError('');
            setMessage('');

            const updated = await projectService.update(projectId, {
                name: name.trim(),
                description: description.trim() || undefined
            });

            setProject(updated);
            setName(updated.name);
            setDescription(updated.description || '');

            setMessage('Project updated successfully.');
        } catch (err: any) {
            setError(err.response?.data?.message || 'Unable to update project.');
        } finally {
            setSaving(false);
        }
    };

    const handleDelete = async () => {
        if (!projectId) {
            return;
        }

        const confirmed = window.confirm('Are you sure you want to delete this project?');

        if (!confirmed) {
            return;
        }

        try {
            await projectService.delete(projectId);

            navigate("/projects");
        } catch (err: any) {
            setError(err.response?.data?.message || 'Unable to delete project.');
        }
    };

    const handleArchive = async () => {
        if (!projectId) {
            return;
        }

        const result = await Swal.fire({
            icon: 'warning',
            title: 'Archive project?',
            text: 'This project will be marked as archived.',
            showCancelButton: true,
            confirmButtonText: 'Archive',
            cancelButtonText: 'Cancel'
        });

        if (!result.isConfirmed) {
            return;
        }

        try {
            await projectService.archive(projectId);

            await Swal.fire({
                icon: 'success',
                title: 'Project archived',
                text: 'The project has been archived successfully.',
                timer: 1500,
                showConfirmButton: false
            });

            navigate("/projects");
        }
        catch (error: any) {
            await Swal.fire({
                icon: 'error',
                title: 'Unable to archive project',
                text: error?.response?.data?.message ??
                    'An unexpected error occurred.'
            });
        }
    };

    const handleMembers = () => {
        if (!projectId) {
            return;
        }
        navigate(`/projects/${projectId}/members`);
    };

    if (loading) {
        return (
            <div className="container-fluid">
                Loading project...
            </div>
        );
    }

    if (!project) {
        return (
            <div className="container-fluid">
                <div className="alert alert-danger">
                    {error || "Project not found."}
                </div>
            </div>
        );
    }

    return (
        <div className="container-fluid">
            <div className="row">
                <div className="col-lg-8">
                    <div className="card">
                        <div className="card-header d-flex justify-content-between align-items-center">
                            <h5 className="mb-0">Project</h5>

                            <button
                                className="btn btn-sm btn-outline-primary"
                                onClick={handleMembers}>
                                <i className="bi bi-person-plus me-1"></i>
                                Add Member
                            </button>
                        </div>


                        <div className="card-body">
                            {message && (
                                <div className="alert alert-success">
                                    {message}
                                </div>
                            )}

                            {error && (
                                <div className="alert alert-danger">
                                    {error}
                                </div>
                            )}

                            <div className="mb-3">
                                <label className="form-label">
                                    Project Name
                                </label>

                                <input className="form-control" value={name}
                                    onChange={(e) => setName(e.target.value)}
                                />
                            </div>

                            <div className="mb-3">
                                <label className="form-label">
                                    Description
                                </label>

                                <textarea className="form-control" rows={4} value={description}
                                    onChange={(e) => setDescription(e.target.value)}
                                />
                            </div>

                            <div className="d-flex gap-2">
                                <button className="btn btn-primary" onClick={handleUpdate} disabled={saving} >
                                    {saving ? "Saving..." : "Save Changes"}
                                </button>

                                <button type='button' className='btn btn-warning' onClick={handleArchive} 
                                    disabled={!project?.isActive}>
                                    Archive Project
                                </button>

                                <button className="btn btn-danger" onClick={handleDelete} >
                                    Delete Project
                                </button>
                            </div>
                        </div>

                        <div className="card-footer text-muted">
                            Created:{' '}{new Date(project.createdAt).toLocaleString()}
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}