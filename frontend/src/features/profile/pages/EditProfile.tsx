import { useState, useEffect, type SubmitEvent } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import {
    updateProfile,
    getUserDetails,
} from '../services/userService';
import type { UserProfile } from '../types/profile.types'

const EditProfile = () => {
    const navigate = useNavigate();

    const [user, setUser] = useState<UserProfile | null>(null);
    const [displayName, setDisplayName] = useState('');
    const [loading, setLoading] = useState(false);
    const [saving, setSaving] = useState(false);
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');

    useEffect(() => {
        loadProfile();
    }, []);

    async function loadProfile() {
        try {
            setLoading(true);
            setError('');

            const profile = await getUserDetails();

            setUser(profile);
            setDisplayName(profile.displayName ?? '');
        } catch (err: any) {
            setError(
                err?.response?.data?.message ||
                'Unable to load your profile.'
            );
        } finally {
            setLoading(false);
        }
    }

    const handleSubmit = async (event: SubmitEvent<HTMLFormElement>) => {
        event.preventDefault();

        try {
            setSaving(true);
            setError('');
            setSuccess('');

            const trimmedDisplayName = displayName.trim();

            const updatedProfile = await updateProfile({
                displayName: trimmedDisplayName.length > 0 ? trimmedDisplayName : null,
                firstName: user?.firstName ? user?.firstName : '',
                lastName: user?.lastName ? user?.lastName : '',
                middleName: user?.middleName ? user?.middleName : '',
                email: user?.email ? user?.email : ''
            });

            setUser(updatedProfile);
            setDisplayName(updatedProfile.displayName ?? '');

            setSuccess('Your profile was updated successfully.');

            setTimeout(() => {
                navigate('/profile');
            }, 1000);
        } catch (err: any) {
            setError(
                err?.response?.data?.message ||
                'Unable to update your profile.'
            );
        } finally {
            setSaving(false);
        }
    };

    if (loading) {
        return (
            <div className="text-center py-5">
                <div className="spinner-border" role="status">
                    <span className="visually-hidden">
                        Loading profile...
                    </span>
                </div>

                <p className="mt-3 text-muted">
                    Loading your profile...
                </p>
            </div>
        );
    }

    return (
        <div>
            <div className="mb-4">
                <h2>Edit Profile</h2>

                <p className="text-muted">
                    Update your personal information.
                </p>
            </div>

            {error && (
                <div className="alert alert-danger" role="alert">
                    {error}
                </div>
            )}

            {success && (
                <div className="alert alert-success" role="alert">
                    {success}
                </div>
            )}

            <div className="card border-0 shadow-sm">
                <div className="card-body p-4">
                    <form onSubmit={handleSubmit} >
                        <div className="row mb-3">
                            <div className="col-md-4">
                                <label className="form-label">First Name</label>
                                <input
                                    type="text"
                                    className="form-control"
                                    value={user?.firstName}
                                    onChange={(e) =>
                                        setUser((prev) => prev ? { ...prev, firstName: e.target.value } : prev)
                                    }
                                />
                            </div>

                            <div className="col-md-4">
                                <label className="form-label">Middle Name</label>
                                <input
                                    type="text"
                                    className="form-control"
                                    value={user?.middleName ?? ''}
                                    onChange={(e) =>
                                        setUser((prev) => prev ? { ...prev, middleName: e.target.value } : prev)
                                    }
                                />
                            </div>

                            <div className="col-md-4">
                                <label className="form-label">Last Name</label>
                                <input
                                    type="text"
                                    className="form-control"
                                    value={user?.lastName}
                                    onChange={(e) =>
                                        setUser((prev) => prev ? { ...prev, lastName: e.target.value } : prev)
                                    }
                                />
                            </div>
                        </div>

                        <div className="mb-3">
                            <label className="form-label">
                                Email Address
                            </label>

                            <input
                                type="email"
                                className="form-control"
                                value={user?.email}
                                onChange={(e) =>
                                    setUser((prev) => prev ? { ...prev, email: e.target.value } : prev)
                                }
                            />
                        </div>

                        <div className="mb-4">
                            <label className="form-label">
                                Display Name
                            </label>

                            <input
                                type="text"
                                className="form-control"
                                placeholder="Display name"
                                value={displayName}
                                onChange={(e) => setDisplayName(e.target.value)}
                            />
                        </div>

                        <div className="d-flex gap-2">
                            <Link
                                type="button"
                                className="btn btn-secondary"
                                to="/profile"
                            >
                                Cancel
                            </Link>

                            <button
                                type="submit"
                                className="btn btn-primary"
                                disabled={saving}
                            >
                                {saving ? 'Saving...' : 'Save Changes'}
                            </button>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    );
};

export default EditProfile;