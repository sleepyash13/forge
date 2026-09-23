import { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import projectService from '../services/projectService';
import type { Project } from '../types/project.types';
import AuthLayout from '../../auth/components/AuthLayout';

const Projects = () => {
    const [projects, setProjects] = useState<Project[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");

    useEffect(() => {
        loadProjects();
    }, []);

    const loadProjects = async () => {
        try {
            setLoading(true);
            setError('');

            const result = await projectService.getMyProjects();

            setProjects(result);
        } catch (err: any) {
            setError(err.response?.data?.message || 'Unable to load projects.');
        } finally {
            setLoading(false);
        }
    };

    if (loading) {
        return (
            <div className="container-fluid">
                Loading projects...
            </div>
        );
    }

    return (
        <div className="container-fluid">
            <div className="d-flex justify-content-between align-items-center mb-4">
                <div>
                    <h2>Projects</h2>
                    <p className="text-muted mb-0">
                        Manage your Forge projects.
                    </p>
                </div>

                <Link
                    to="/projects/create"
                    className="btn btn-primary"
                >
                    Create Project
                </Link>
            </div>

            {error && (
                <div className="alert alert-danger">
                    {error}
                </div>
            )}

            {projects.length === 0 ? (
                <div className="card">
                    <div className="card-body text-center py-5">
                        <h5>
                            No projects yet
                        </h5>

                        <p className="text-muted">
                            Create your first Forge project.
                        </p>

                        <Link to="/projects/create" className="btn btn-primary">
                            Create Project
                        </Link>
                    </div>
                </div>
            ) : (
                <div className="row g-3">
                    {projects.map((project) => (
                        <div
                            className="col-md-6 col-xl-4"
                            key={project.id}
                        >
                            <div className="card h-100">
                                <div className="card-body">
                                    <h5>
                                        {project.name}
                                    </h5>

                                    <p className="text-muted">
                                        {project.description || 'No description.'}
                                    </p>
                                </div>

                                <div className="card-footer">
                                    <Link to={`/projects/${project.id}`} className="btn btn-outline-primary btn-sm">
                                        Open Project
                                    </Link>
                                </div>
                            </div>
                        </div>
                    ))}
                </div>
            )}
        </div>
    );
}

export default Projects;