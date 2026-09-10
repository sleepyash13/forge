import type { ReactNode } from 'react';

interface AuthLayoutProps {
    children: ReactNode;
}

const AuthLayout = ({ children }: AuthLayoutProps) => {
    return (
        <div className="auth-page">
            <div className="auth-brand-panel">
                <div className="auth-brand">
                    <div className="forge-logo">
                        <i className="bi bi-box"></i>
                    </div>

                    <h1>Forge</h1>
                </div>

                <p className="auth-tagline">
                    Build. Deploy. Grow.
                </p>

                <p className="auth-description">
                    Your developer platform for building and deploying applications.
                </p>
            </div>

            <div className="auth-form-panel">
                {children}
            </div>
        </div>
    );
};

export default AuthLayout;