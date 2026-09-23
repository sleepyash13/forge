import { useState } from 'react';
import type { SubmitEvent } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import AuthLayout from '../../../features/auth/components/AuthLayout';
import { register } from '../../../features/auth/services/authService';

const Register = () => {
    const navigate = useNavigate();

    // declare variables state
    const [firstName, setFirstName] = useState('');
    const [middleName, setMiddleName] = useState('');
    const [lastName, setLastName] = useState('');
    const [displayName, setDisplayName] = useState('');
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [confirmPassword, setConfirmPassword] = useState('');

    const handleSubmit = async (event: SubmitEvent<HTMLFormElement>) => {
        event.preventDefault();

        try {
            await register({
                email,
                password, 
                confirmPassword,
                displayName,
                firstName,
                middleName,
                lastName
            });

            navigate('/login');
        }
        catch (error) {
            console.error(error);
        }
    };

    return (
        <AuthLayout>
            <div className="auth-card">
                <div className="mb-4">
                    <h2 className="fw-semibold mb-1">Create Account</h2>

                    <p className="text-muted mb-0">
                        Get started with Forge
                    </p>
                </div>

                <form onSubmit={handleSubmit}>
                    <div className="row g-2 mb-3">
                        <div className="col-md-4">
                            <label className="form-label">
                                First Name
                            </label>

                            <input
                                type="text"
                                className="form-control"
                                placeholder="First name"
                                value={firstName}
                                onChange={(e) => setFirstName(e.target.value)}
                                required
                            />
                        </div>

                        <div className="col-md-4">
                            <label className="form-label">
                                Middle Name
                            </label>

                            <input
                                type="text"
                                className="form-control"
                                placeholder="Middle name"
                                value={middleName}
                                onChange={(e) => setMiddleName(e.target.value)}
                            />
                        </div>

                        <div className="col-md-4">
                            <label className="form-label">
                                Last Name
                            </label>

                            <input
                                type="text"
                                className="form-control"
                                placeholder="Last name"
                                value={lastName}
                                onChange={(e) => setLastName(e.target.value)}
                                required
                            />
                        </div>
                    </div>

                    {/* Display Name */}
                    <div className="mb-3">
                        <label className="form-label">
                            Display Name
                        </label>

                        <input
                            type="text"
                            className="form-control"
                            placeholder="Enter your display name"
                            value={displayName}
                            onChange={(e) => setDisplayName(e.target.value)}
                            required
                        />
                    </div>

                    {/* Email */}
                    <div className="mb-3">
                        <label className="form-label">
                            Email address
                        </label>

                        <input
                            type="email"
                            className="form-control"
                            placeholder="Enter your email"
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                            required
                        />
                    </div>

                    <div className="row g-2 mb-4">
                        <div className="col-md-6">
                            <label className="form-label">
                                Password
                            </label>

                            <input
                                type="password"
                                className="form-control"
                                placeholder="Create a password"
                                value={password}
                                onChange={(e) => setPassword(e.target.value)}
                                required
                            />
                        </div>

                        <div className="col-md-6">
                            <label className="form-label">
                                Confirm Password
                            </label>

                            <input
                                type="password"
                                className="form-control"
                                placeholder="Confirm password"
                                value={confirmPassword}
                                onChange={(e) => setConfirmPassword(e.target.value)}
                                required
                            />
                        </div>
                    </div>

                    <button
                        type="submit"
                        className="btn btn-primary w-100"
                    >
                        Register
                    </button>
                </form>

                <div className="text-center mt-4">
                    <span className="text-muted">
                        Already have an account?{' '}
                    </span>

                    <Link to="/login">
                        Login
                    </Link>
                </div>
            </div>
        </AuthLayout>
    );
};

export default Register;