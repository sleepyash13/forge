import { Outlet } from 'react-router-dom';
import ProjectNavigation from './ProjectNavigation';

const ProjectLayout = () => {
    return (
        <div className="container-fluid">
            <ProjectNavigation />

            <Outlet />
        </div>
    );
};

export default ProjectLayout;
