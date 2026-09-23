import { type SubmitEvent, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import projectService from '../services/projectService';

export default function CreateProject() {
    const navigate = useNavigate();

    const [name, setName] = useState('');
    const [description, setDescription] = useState('');
    const [error, setError] = useState('');
    const [isSubmitting, setIsSubmitting] = useState(false);

    const handleSubmit = async (event: SubmitEvent<HTMLFormElement>) => {
        event.preventDefault();

        setError('');

        if (!name.trim()) {
            setError('Project name is required.');
            return;
        }

        try {
            setIsSubmitting(true);

            const project = await projectService.create({
                name: name.trim(), 
                description: description.trim() || undefined
            });

            navigate(`/projects/${project.id}`);
        } catch (err: any) {
            setError(err.response?.data?.message || 'Unable to create project.');
        } finally {
            setIsSubmitting(false);
        }
    };

    return (
        <div className="container-fluid">
            <div className="row">
                <div className="col-lg-8">
                    <div className="card">
                        <div className="card-header">
                            <h5 className="mb-0">
                                Create Project
                            </h5>
                        </div>

                        <div className="card-body">
                            {error && (
                                <div className="alert alert-danger">
                                    {error}
                                </div>
                            )}

                            <form onSubmit={handleSubmit}>
                                <div className="mb-3">
                                    <label className="form-label">
                                        Project Name
                                    </label>

                                    <input
                                        type="text"
                                        className="form-control"
                                        value={name}
                                        onChange={(e) => setName(e.target.value)}
                                        maxLength={100}
                                    />
                                </div>

                                <div className="mb-3">
                                    <label className="form-label">
                                        Description
                                    </label>

                                    <textarea
                                        className="form-control"
                                        rows={4}
                                        value={description}
                                        onChange={(e) =>
                                            setDescription(
                                                e.target.value
                                            )
                                        }
                                    />
                                </div>

                                <button
                                    type="submit"
                                    className="btn btn-primary"
                                    disabled={isSubmitting}
                                >
                                    {isSubmitting ? "Creating..." : "Create Project"}
                                </button>
                            </form>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}