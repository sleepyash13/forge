const Dashboard = () => {
    return (
        <div>
            <div className="mb-4">
                <h2>Dashboard</h2>

                <p className="text-muted">
                    Welcome back, John Doe!
                </p>
            </div>

            <div className="card border-0 shadow-sm">
                <div className="card-body p-4">
                    <div className="d-flex align-items-center">
                        <div className="dashboard-icon me-3">
                            <i className="bi bi-person"></i>
                        </div>

                        <div>
                            <h5 className="mb-1">
                                Manage Your Account
                            </h5>

                            <p className="text-muted mb-0">
                                Update your profile, change your password,
                                and manage your account.
                            </p>
                        </div>
                    </div>
                </div>
            </div>

            <div className="row mt-4 g-3">
                <div className="col-md-4">
                    <div className="card border-0 shadow-sm h-100">
                        <div className="card-body">
                            <i className="bi bi-person-circle fs-2 text-primary"></i>

                            <h5 className="mt-3">
                                View Profile
                            </h5>

                            <p className="text-muted">
                                View your profile information.
                            </p>
                        </div>
                    </div>
                </div>

                <div className="col-md-4">
                    <div className="card border-0 shadow-sm h-100">
                        <div className="card-body">
                            <i className="bi bi-pencil-square fs-2 text-primary"></i>

                            <h5 className="mt-3">
                                Update Profile
                            </h5>

                            <p className="text-muted">
                                Keep your profile information up to date.
                            </p>
                        </div>
                    </div>
                </div>

                <div className="col-md-4">
                    <div className="card border-0 shadow-sm h-100">
                        <div className="card-body">
                            <i className="bi bi-shield-lock fs-2 text-primary"></i>

                            <h5 className="mt-3">
                                Reset Credentials
                            </h5>

                            <p className="text-muted">
                                Change your account password.
                            </p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default Dashboard;