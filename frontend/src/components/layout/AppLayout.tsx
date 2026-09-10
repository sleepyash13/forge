import { Outlet } from 'react-router-dom';
import Sidebar from './Sidebar';
import Topbar from './Topbar';

const AppLayout = () => {
    return (
        <div className="app-layout">
            <Sidebar />

            <div className="app-content">
                <Topbar />
                <main className="content-area">
                    <Outlet />
                </main>
            </div>
        </div>
    );
};

export default AppLayout;