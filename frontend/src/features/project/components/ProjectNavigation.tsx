import { NavLink, useParams } from 'react-router-dom';

const ProjectNavigation = () => {
    const { projectId } = useParams<{ projectId: string }>();

    if (!projectId) {
        return null;
    }

    const links = [
        {
            label: 'Overview',
            to: `/projects/${projectId}`
        },
        {
            label: 'Members',
            to: `/projects/${projectId}/members`
        },
        {
            label: 'Repository',
            to: `/projects/${projectId}/repository`
        },
        {
            label: 'Builds',
            to: `/projects/${projectId}/builds`
        },
        {
            label: 'Deployments',
            to: `/projects/${projectId}/deployments`
        },
        {
            label: 'Settings',
            to: `/projects/${projectId}/settings`
        }
    ];

    return (
        <ul className="nav nav-tabs mb-4">
            {links.map((link) => (
                <li className="nav-item" key={link.to}>
                    <NavLink
                        to={link.to}
                        end={link.label === 'Overview'}
                        className={({ isActive }) =>
                            `nav-link ${isActive ? 'active' : ''}`
                        }
                    >
                        {link.label}
                    </NavLink>
                </li>
            ))}
        </ul>
    );
};

export default ProjectNavigation;
