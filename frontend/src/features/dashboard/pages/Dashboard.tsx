import { useAuth } from '../../../contexts/AuthContext';

const Dashboard = () => {
    const { user } = useAuth();


    return (
        <div>
            <div className="mb-4">
                <h2>Dashboard</h2>

                <p className="text-muted">
                    Welcome back, { user?.displayName }!
                </p>
            </div>


        </div>
    );
};

export default Dashboard;