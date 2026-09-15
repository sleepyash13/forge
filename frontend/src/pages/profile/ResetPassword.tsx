import { useState, type SubmitEvent } from 'react';
import { useNavigate } from 'react-router-dom';
import { changePassword } from '../../services/userService';

const ResetPassword = () => {
    const navigate = useNavigate();

    const [currentPassword, setCurrentPassword] = useState('');
    const [newPassword, setNewPassword] = useState('');
    const [confirmPassword, setConfirmPassword] = useState('');

    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');

    const handleSubmit = async (event: SubmitEvent<HTMLFormElement>) => {
        event.preventDefault();

        setError('');
        setSuccess('');

        if (!currentPassword) {
            setError('Current password is required.');
            return;
        }

        if (!newPassword) {
            setError('New password is required.');
            return;
        }

        if (newPassword !== confirmPassword) {
            setError('New password and confirmation password do not match.');
            return;
        }

        if (newPassword.length < 12) {
            setError('Password must be at least 12 characters long.');
            return;
        }

        if (!/[A-Z]/.test(newPassword)) {
            setError('Password must contain at least one uppercase letter.');
            return;
        }

        if (!/[a-z]/.test(newPassword)) {
            setError('Password must contain at least one lowercase letter.');
            return;
        }

        if (!/[0-9]/.test(newPassword)) {
            setError('Password must contain at least one number.');
            return;
        }

        if (!/[^a-zA-Z0-9]/.test(newPassword)) {
            setError('Password must contain at least one special character.');
            return;
        }

        if (currentPassword === newPassword) {
            setError('New password must be different from your current password.');
            return;
        }

        setLoading(true);

        try {
            await changePassword({
                currentPassword,
                newPassword,
                confirmPassword
            });

            setSuccess('Password changed successfully. Redirecting to login...');

            setCurrentPassword('');
            setNewPassword('');
            setConfirmPassword('');

            setTimeout(() => {
                navigate('/login');
            }, 1500);
        }
        catch (error: any) {
            setError(error.response?.data?.message ?? 'Failed to change password.');
        }
        finally {
            setLoading(false);
        }
    };

    return (
        <div className="container py-4">
            <div className="row justify-content-center">
                <div className="col-md-7 col-lg-6">
                    <div className="card shadow-sm">
                        <div className="card-body p-4">
                            <h2 className="mb-2">
                                Reset Credentials
                            </h2>

                            <p className="text-muted mb-4">
                                Change your account password.
                            </p>

                            {error && (
                                <div className="alert alert-danger">
                                    {error}
                                </div>
                            )}

                            {success && (
                                <div className="alert alert-success">
                                    {success}
                                </div>
                            )}

                            <form onSubmit={handleSubmit}>
                                <div className="mb-3">
                                    <label className="form-label">
                                        Current Password
                                    </label>

                                    <input
                                        type="password"
                                        className="form-control"
                                        value={currentPassword}
                                        onChange={(e) => setCurrentPassword(e.target.value)}
                                        autoComplete="current-password"
                                    />
                                </div>

                                <div className="mb-3">
                                    <label className="form-label">
                                        New Password
                                    </label>

                                    <input
                                        type="password"
                                        className="form-control"
                                        value={newPassword}
                                        onChange={(e) => setNewPassword(e.target.value)}
                                        autoComplete="new-password"
                                    />

                                    <div className="form-text">
                                        Password must contain:
                                        <ul className="mb-0 mt-1">
                                            <li>
                                                At least 12 characters
                                            </li>
                                            <li>
                                                One uppercase letter
                                            </li>
                                            <li>
                                                One lowercase letter
                                            </li>
                                            <li>
                                                One number
                                            </li>
                                            <li>
                                                One special character
                                            </li>
                                        </ul>
                                    </div>
                                </div>

                                <div className="mb-4">
                                    <label className="form-label">
                                        Confirm New Password
                                    </label>

                                    <input
                                        type="password"
                                        className="form-control"
                                        value={confirmPassword}
                                        onChange={(e) => setConfirmPassword(e.target.value)}
                                        autoComplete="new-password" />
                                </div>

                                <div className="d-flex gap-2">
                                    <button type="submit"
                                        className="btn btn-primary"
                                        disabled={loading} >
                                        {loading
                                            ? "Changing Password..."
                                            : "Change Password"}
                                    </button>

                                    <button
                                        type="button"
                                        className="btn btn-secondary"
                                        onClick={() => navigate("/profile")}
                                        disabled={loading} >
                                        Cancel
                                    </button>
                                </div>
                            </form>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default ResetPassword;