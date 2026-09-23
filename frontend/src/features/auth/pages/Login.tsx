import { useState } from "react";
import type { SubmitEvent } from "react";
import { Link, useNavigate } from "react-router-dom";

import AuthLayout from '../../../features/auth/components/AuthLayout';
import { useAuth } from '../../../contexts/AuthContext';

const Login = () => {
    const navigate = useNavigate();
    const { login } = useAuth();

    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");

    const [error, setError] = useState<string | null>(null);
    const [isSubmitting, setIsSubmitting] = useState(false);

    const handleSubmit = async (
        event: SubmitEvent<HTMLFormElement>
    ) => {
        event.preventDefault();

        setError(null);
        setIsSubmitting(true);

        try {
            await login(email, password);

            navigate("/dashboard", {
                replace: true,
            });
        } catch (error) {
            console.error(error);
            setError("Invalid email or password.");
        } finally {
            setIsSubmitting(false);
        }
    };

    return (
        <AuthLayout>

            <div className="auth-card">
                <div className="mb-4">
                    <h2>Welcome Back</h2>
                    <p className="text-muted mb-0">
                        Sign in to your Forge account
                    </p>
                </div>

                {error && (
                    <div className="alert alert-danger">
                        {error}
                    </div>
                )}

                <form onSubmit={handleSubmit}>
                    <div className="mb-3">
                        <label className="form-label">
                            Email address
                        </label>

                        <div className="input-group">
                            <span className="input-group-text">
                                <i className="bi bi-envelope"></i>
                            </span>

                            <input
                                type="email"
                                className="form-control"
                                placeholder="Enter your email"
                                value={email}
                                onChange={(e) => setEmail(e.target.value)}
                                required
                            />
                        </div>
                    </div>

                    <div className="mb-3">
                        <label className="form-label">
                            Password
                        </label>

                        <div className="input-group">
                            <span className="input-group-text">
                                <i className="bi bi-lock"></i>
                            </span>

                            <input
                                type="password"
                                className="form-control"
                                placeholder="Enter your password"
                                value={password}
                                onChange={(e) => setPassword(e.target.value)}
                                required
                            />
                        </div>
                    </div>

                    <div className="d-flex justify-content-between align-items-center mb-4">
                        <div className="form-check">
                            <input
                                type="checkbox"
                                className="form-check-input"
                                id="rememberMe"
                            />

                            <label
                                className="form-check-label"
                                htmlFor="rememberMe"
                            >
                                Remember me
                            </label>
                        </div>

                        <Link to="/forgot-password">
                            Forgot password?
                        </Link>
                    </div>

                    <button type="submit"
                            className="btn btn-primary w-100"
                            disabled={isSubmitting}>
                        {isSubmitting ? "Logging in..." : "Login"}
                    </button>
                </form>

                <div className="text-center mt-4">
                    <span className="text-muted">
                        Don't have an account?{' '}
                    </span>

                    <Link to="/register">
                        Register
                    </Link>
                </div>
            </div>
        </AuthLayout>
    );
};

export default Login;