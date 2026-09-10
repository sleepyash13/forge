import { Link } from 'react-router-dom';

const Profile = () => {
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
                                John Doe
                            </h4>
                            <p className="text-muted mb-1">
                                john.doe@example.com
                            </p>
                            <span className="badge bg-success">
                                Active
                            </span>
                        </div>
                    </div>
                    <hr />
                    <div className="row mt-4">
                        <div className="col-md-6 mb-3">
                            <small className="text-muted">
                                Full Name
                            </small>

                            <div>
                                John Doe
                            </div>
                        </div>

                        <div className="col-md-6 mb-3">
                            <small className="text-muted">
                                Email Address
                            </small>

                            <div>
                                john.doe@example.com
                            </div>
                        </div>

                        <div className="col-md-6 mb-3">
                            <small className="text-muted">
                                Account Created
                            </small>

                            <div>
                                April 26, 2026
                            </div>
                        </div>

                        <div className="col-md-6 mb-3">
                            <small className="text-muted">
                                Last Updated
                            </small>

                            <div>
                                April 26, 2026
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