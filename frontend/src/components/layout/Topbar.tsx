import { Link, useNavigate } from 'react-router-dom';
import { logout } from '../../services/authService';

const Topbar = () => {
    const navigate = useNavigate();
    const handleLogout = async () => {
        try {
            await logout();

            navigate('/login', { replace: true });
        } catch (err) {
            console.error('Logout failed:', err);

            navigate('/login', { replace: true });
        }
    };

    return (
        <header className="topbar">
            <div className="ms-auto">
                <div className="dropdown">
                    <button
                        className="btn btn-link text-decoration-none dropdown-toggle"
                        type="button"
                        data-bs-toggle="dropdown"
                    >
                        <i className="bi bi-person-circle me-2"></i>
                        John Doe
                    </button>

                    <ul className="dropdown-menu dropdown-menu-end">
                        <li>
                            <Link to="/profile" className="dropdown-item" >
                                <i className="bi bi-person me-2"></i>
                                Profile
                            </Link>
                        </li>

                        <li>
                            <Link to="/account/reset-password" className="dropdown-item" >
                                <i className="bi bi-gear me-2"></i>
                                Account Settings
                            </Link>
                        </li>

                        <li>
                            <hr className="dropdown-divider" />
                        </li>

                        <li>
                            <button
                                className="dropdown-item text-danger"
                                onClick={handleLogout}
                            >
                                <i className="bi bi-box-arrow-right me-2"></i>
                                Logout
                            </button>
                        </li>
                    </ul>
                </div>
            </div>
        </header>
    );
};

export default Topbar;