const EditProfile = () => {
    return (
        <div>
            <div className="mb-4">
                <h2>Edit Profile</h2>

                <p className="text-muted">
                    Update your personal information.
                </p>
            </div>

            <div className="card border-0 shadow-sm">
                <div className="card-body p-4">
                    <form>
                        <div className="mb-3">
                            <label className="form-label">
                                Full Name
                            </label>

                            <input
                                type="text"
                                className="form-control"
                                value="John Doe"
                                readOnly
                            />
                        </div>

                        <div className="mb-3">
                            <label className="form-label">
                                Email Address
                            </label>

                            <input
                                type="email"
                                className="form-control"
                                value="john.doe@example.com"
                                readOnly
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
                            />
                        </div>

                        <div className="d-flex gap-2">
                            <button
                                type="button"
                                className="btn btn-secondary"
                            >
                                Cancel
                            </button>

                            <button
                                type="submit"
                                className="btn btn-primary"
                            >
                                Save Changes
                            </button>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    );
};

export default EditProfile;