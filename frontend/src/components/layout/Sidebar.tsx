import { NavLink } from 'react-router-dom';

const Sidebar = () => {
    return (
        <aside className="sidebar">
            <div className="sidebar-brand">
                <div className="forge-logo small">
                    <i className="bi bi-box"></i>
                </div>

                <span>Forge</span>
            </div>

            <nav className="sidebar-nav">
                <NavLink
                    to="/dashboard"
                    className="sidebar-link"
                >
                    <i className="bi bi-house"></i>
                    <span>Dashboard</span>
                </NavLink>

                <NavLink
                    to="/profile"
                    className="sidebar-link"
                >
                    <i className="bi bi-person"></i>
                    <span>Profile</span>
                </NavLink>

                <NavLink
                    to="/account/reset-password"
                    className="sidebar-link"
                >
                    <i className="bi bi-gear"></i>
                    <span>Account Settings</span>
                </NavLink>
            </nav>
        </aside>
    );
};

export default Sidebar;