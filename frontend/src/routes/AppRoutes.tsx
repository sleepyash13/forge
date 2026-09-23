import { Routes, Route, Navigate } from 'react-router-dom';

import Login from '../features/auth/pages/Login';
import Register from '../features/auth/pages/Register';
import ForgotPassword from '../features/auth/pages/ForgotPassword';

import Dashboard from '../features/dashboard/pages/Dashboard';

import Profile from '../features/profile/pages/Profile';
import EditProfile from '../features/profile/pages/EditProfile';
import ResetPassword from '../features/profile/pages/ResetPassword';

import AppLayout from '../components/layout/AppLayout';
import ProtectedRoute from '../routes/ProtectedRoute';

import Projects from '../features/project/pages/List'
import CreateProject from '../features/project/pages/Create'
import ProjectDetails from '../features/project/pages/EditDetails'

const AppRoutes = () => {
    return (
        <Routes>

            {/* Authentication */}
            <Route path="/login" element={<Login />} />
            <Route path="/register" element={<Register />} />
            <Route path="/forgot-password" element={<ForgotPassword />} />

            {/* Application */}
            <Route element={<ProtectedRoute />}>
                <Route element={<AppLayout />}>
                    <Route path="/dashboard" element={<Dashboard />} /> 
                    <Route path="/profile" element={<Profile />} />
                    <Route path="/profile/edit" element={<EditProfile />} />
                    <Route path="/account/reset-password" element={<ResetPassword />} />
                    
                    <Route path="/projects" element={<Projects />} />
                    <Route path="/projects/create" element={<CreateProject />} />
                    <Route path="/projects/:projectId" element={<ProjectDetails />} />
                </Route>
            </Route>

            {/* Default */}
            <Route path="/" element={<Navigate to="/login" replace />} />

            {/* 404 */}
            <Route path="*" element={<Navigate to="/dashboard" replace />} />

        </Routes>
    );
};

export default AppRoutes;