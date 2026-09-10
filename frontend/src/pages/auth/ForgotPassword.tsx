import { Link } from 'react-router-dom';
import AuthLayout from '../../components/auth/AuthLayout';

const ForgotPassword = () => {
    return (
        <AuthLayout>
            <div className="auth-card">
                <div className="text-center mb-4">
                    <div className="auth-icon">
                        <i className="bi bi-key"></i>
                    </div>

                    <h2 className="mt-3">
                        Reset Password
                    </h2>

                    <p className="text-muted">
                        Enter your email address and we'll send
                        instructions to reset your password.
                    </p>
                </div>

                <form>
                    <div className="mb-4">
                        <label className="form-label">
                            Email Address
                        </label>

                        <input
                            type="email"
                            className="form-control"
                            placeholder="Enter your email"
                            required
                        />
                    </div>

                    <button
                        type="submit"
                        className="btn btn-primary w-100"
                    >
                        Send Reset Instructions
                    </button>
                </form>

                <div className="text-center mt-4">
                    <Link to="/login">
                        <i className="bi bi-arrow-left me-2"></i>
                        Back to Login
                    </Link>
                </div>
            </div>
        </AuthLayout>
    );
};

export default ForgotPassword;