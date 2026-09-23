import { Link } from 'react-router-dom';
import { useEffect, useState } from 'react';
import { getUserDetails } from '../../profile/services/userService';
import type { UserProfile } from '../../profile/types/profile.types'

const Profile = () => {
    const [user, setUser] = useState<UserProfile | null>(null);
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState("");

    useEffect(() => {
        loadProfile();
    }, []);

    async function loadProfile() {
        try {
            setIsLoading(true);
            setError("");

            const currentUser = await getUserDetails();

            setUser(currentUser);
        } catch (err) {
            console.error(err);

            setError(
                "Unable to load your profile."
            );
        } finally {
            setIsLoading(false);
        }
    }

    if (isLoading) {
        return (
            <div className="container-fluid">
                <h2>Profile</h2>
                <p>Loading profile...</p>
            </div>
        );
    }

    if (error) {
        return (
            <div className="container-fluid">
                <h2>Profile</h2>

                <div className="alert alert-danger">
                    {error}
                </div>
            </div>
        );
    }

    if (!user) {
        return (
            <div className="container-fluid">
                <h2>Profile</h2>
                <p>No profile information available.</p>
            </div>
        );
    }

    function formatFriendlyDate(dateString: string) {
        return new Date(dateString).toLocaleDateString("en-US", {
            year: "numeric",
            month: "long",
            day: "numeric",
            hour: "numeric",
            minute: "numeric"
        });
    }

    return (
        <div>
            <div className="mb-4">
                <h2>My Profile</h2>
                <p className="text-muted">
                    View and manage your profile information.
                </p>
            </div>

            <div className="card border-0 shadow-sm">
                <div className="card-body p-4">
                    <div className="d-flex align-items-center mb-4">
                        <div className="profile-avatar">
                            <i className="bi bi-person"></i>
                        </div>

                        <div className="ms-3">
                            <h4 className="mb-1">
                                {user.lastName}{', '}{user.firstName}{' '}{user.middleName ? `${user.middleName} ` : ''}
                            </h4>
                            <span className="badge bg-success">
                                Active
                            </span>
                        </div>
                    </div>
                    <hr />
                    <div className="row mt-4">
                        <div className="col-md-6 mb-3">
                            <small className="text-muted">
                                Display Name
                            </small>

                            <div>
                                {user.displayName}
                            </div>
                        </div>

                        <div className="col-md-6 mb-3">
                            <small className="text-muted">
                                Email Address
                            </small>

                            <div>
                                {user.email}
                            </div>
                        </div>

                        <div className="col-md-6 mb-3">
                            <small className="text-muted">
                                Account Created
                            </small>

                            <div>
                                {formatFriendlyDate(user.createdAt)}
                            </div>
                        </div>

                        <div className="col-md-6 mb-3">
                            <small className="text-muted">
                                Last Updated
                            </small>

                            <div>
                                {formatFriendlyDate(user.updatedAt)}
                            </div>
                        </div>
                    </div>

                    <Link
                        to="/profile/edit"
                        className="btn btn-primary"
                    >
                        <i className="bi bi-pencil me-2"></i>
                        Edit Profile
                    </Link>
                </div>
            </div>
        </div>
    );
};

export default Profile;