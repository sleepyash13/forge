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
                <NavLink to="/dashboard" className="sidebar-link">
                    <i className="bi bi-house"></i>
                    <span>Dashboard</span>
                </NavLink>
                
                <NavLink to="/projects" className="sidebar-link">
                    <i className="bi bi-folder"></i>
                    <span>Projects</span>
                </NavLink>
            </nav>
        </aside>
    );
};

export default Sidebar;