import { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import Swal from 'sweetalert2';

import repositoryService from '../services/repositoryService';
import type {
    Repository,
    CreateRepositoryRequest,
    UpdateRepositoryRequest
} from '../types/repository.types';

import ProjectNavigation from '../components/ProjectNavigation';

const RepositorySettings = () => {
    const { projectId } = useParams<{ projectId: string }>();
        
    const [repository, setRepository] = useState<Repository | null>(null);
    const [loading, setLoading] = useState(true);
    const [saving, setSaving] = useState(false);

    const [repositoryUrl, setRepositoryUrl] = useState('');
    const [defaultBranch, setDefaultBranch] = useState('');
    const [buildCommand, setBuildCommand] = useState('');
    const [deployConfiguration, setDeployConfiguration] = useState('');

    useEffect(() => {
        if (!projectId) {
            return;
        }

        loadRepository(projectId);
    }, [projectId]);

    const loadRepository = async (id: string) => {
        try {
            setLoading(true);

            const data = await repositoryService.getByProjectId(id);

            setRepository(data);

            if (data) {
                setRepositoryUrl(data.repositoryUrl);
                setDefaultBranch(data.defaultBranch);
                setBuildCommand(data.buildCommand ?? '');
                setDeployConfiguration(data.deployConfiguration ?? '');
            }
        } catch (error) {
            // console.error('Failed to load repository configuration.', error);

            await Swal.fire({
                icon: 'error',
                title: 'Unable to load repository',
                text: 'Something went wrong while loading the repository configuration.'
            });
        } finally {
            setLoading(false);
        }
    };

    const handleSave = async (event: React.FormEvent) => {
        event.preventDefault();

        if (!projectId) {
            return;
        }

        if (!repositoryUrl.trim()) {
            await Swal.fire({
                icon: 'warning',
                title: 'Repository URL required',
                text: 'Please enter the repository URL.'
            });

            return;
        }

        if (!defaultBranch.trim()) {
            await Swal.fire({
                icon: 'warning',
                title: 'Default branch required',
                text: 'Please enter the default branch.'
            });

            return;
        }

        try {
            setSaving(true);

            if (repository) {
                const request: UpdateRepositoryRequest = {
                    repositoryUrl: repositoryUrl.trim(),
                    defaultBranch: defaultBranch.trim(),
                    buildCommand: buildCommand.trim() || undefined,
                    deployConfiguration: deployConfiguration.trim() || undefined
                };

                const updatedRepository =
                    await repositoryService.update(
                        projectId,
                        request
                    );

                setRepository(updatedRepository);

                await Swal.fire({
                    icon: 'success',
                    title: 'Repository updated',
                    text: 'Repository configuration has been updated successfully.'
                });
            } else {
                const request: CreateRepositoryRequest = {
                    repositoryUrl: repositoryUrl.trim(),
                    defaultBranch: defaultBranch.trim(),
                    buildCommand: buildCommand.trim() || undefined,
                    deployConfiguration: deployConfiguration.trim() || undefined
                };

                const createdRepository = await repositoryService.create(projectId, request);

                setRepository(createdRepository);

                await Swal.fire({
                    icon: 'success',
                    title: 'Repository configured',
                    text: 'Repository configuration has been saved successfully.'
                });
            }
        } catch (error: any) {
            // console.error(
            //     'Failed to save repository configuration.',
            //     error
            // );

            await Swal.fire({
                icon: 'error',
                title: 'Unable to save repository',
                text: error.response?.data?.message ?? 'Something went wrong while saving the repository configuration.'
            });
        } finally {
            setSaving(false);
        }
    };

    const handleDelete = async () => {
        if (!projectId || !repository) {
            return;
        }

        const result = await Swal.fire({
            icon: 'warning',
            title: 'Delete repository configuration?',
            text: 'This will remove the repository configuration from this project.',
            showCancelButton: true,
            confirmButtonText: 'Delete',
            cancelButtonText: 'Cancel'
        });

        if (!result.isConfirmed) {
            return;
        }

        try {
            setSaving(true);

            await repositoryService.delete(projectId);

            setRepository(null);
            setRepositoryUrl('');
            setDefaultBranch('');
            setBuildCommand('');
            setDeployConfiguration('');

            await Swal.fire({
                icon: 'success',
                title: 'Repository removed',
                text: 'Repository configuration has been removed successfully.'
            });
        } catch (error: any) {
            // console.error(
            //     'Failed to delete repository configuration.',
            //     error
            // );

            await Swal.fire({
                icon: 'error',
                title: 'Unable to delete repository',
                text:
                    error.response?.data?.message ??
                    'Something went wrong while deleting the repository configuration.'
            });
        } finally {
            setSaving(false);
        }
    };

    if (loading) {
        return (
            <div className='d-flex justify-content-center py-5'>
                <div className='spinner-border' role='status' aria-label='Loading'>
                    <span className='visually-hidden'>
                        Loading...
                    </span>
                </div>
            </div>
        );
    }

    return (
        <div className='container-fluid'>
            <ProjectNavigation />
            
            <div className='d-flex justify-content-between align-items-center mb-4'>
                <div>
                    <h2 className='mb-1'>Repository</h2>
                    <p className='text-muted mb-0'>
                        Configure the source repository for this project.
                    </p>
                </div>

                {repository && (
                    <button type='button' className='btn btn-outline-danger' onClick={handleDelete} disabled={saving} >
                        Delete Repository
                    </button>
                )}
            </div>

            {!repository && (
                <div className='alert alert-info'>
                    <strong>No repository configured.</strong>
                    <div className='mt-1'>
                        Configure a source repository to prepare this
                        project for builds and deployments.
                    </div>
                </div>
            )}

            <div className='card'>
                <div className='card-body'>
                    <form onSubmit={handleSave}>
                        <div className='mb-3'>
                            <label htmlFor='repositoryUrl' className='form-label' >
                                Repository URL
                            </label>

                            <input id='repositoryUrl' type='url' className='form-control' value={repositoryUrl}
                                onChange={(event) => setRepositoryUrl(event.target.value)}
                                placeholder='https://github.com/user/project'
                                disabled={saving}
                                required />

                            <div className='form-text'>
                                The source repository URL for this project.
                            </div>
                        </div>

                        <div className='mb-3'>
                            <label htmlFor='defaultBranch' className='form-label'>
                                Default Branch
                            </label>

                            <input id='defaultBranch' type='text' className='form-control' value={defaultBranch}
                                onChange={(event) => setDefaultBranch(event.target.value)}
                                placeholder='main'
                                disabled={saving}
                                required/>
                        </div>

                        <div className='mb-3'>
                            <label htmlFor='buildCommand' className='form-label' >
                                Build Command
                            </label>

                            <input id='buildCommand' type='text' className='form-control' value={buildCommand}
                                onChange={(event) => setBuildCommand(event.target.value)}
                                placeholder='npm run build'
                                disabled={saving}/>

                            <div className='form-text'>
                                Optional. This is stored for future build
                                execution.
                            </div>
                        </div>

                        <div className='mb-4'>
                            <label htmlFor='deployConfiguration' className='form-label'>
                                Deploy Configuration
                            </label>

                            <textarea id='deployConfiguration' className='form-control' rows={8} value={deployConfiguration}
                                onChange={(event) => setDeployConfiguration(event.target.value)}
                                placeholder='Deployment configuration...'
                                disabled={saving}/>

                            <div className='form-text'>
                                Optional. Deployment execution will be
                                implemented in a later Forge feature.
                            </div>
                        </div>

                        <div className='d-flex justify-content-end'>
                            <button type='submit' className='btn btn-primary' disabled={saving}>
                                {saving ? (
                                    <>
                                        <span
                                            className='spinner-border spinner-border-sm me-2'
                                            role='status'
                                            aria-hidden='true'
                                        />
                                        Saving...
                                    </>
                                ) : (
                                    'Save Repository'
                                )}
                            </button>
                        </div>
                    </form>
                </div>
            </div>

            {repository && (
                <div className='text-muted small mt-3'>
                    Last updated:{' '}
                    {new Date(
                        repository.updatedAt ?? repository.createdAt
                    ).toLocaleString()}
                </div>
            )}
        </div>
    );
};

export default RepositorySettings;