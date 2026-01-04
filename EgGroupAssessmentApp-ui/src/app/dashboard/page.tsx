import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { authService } from './../../services/authServices';
import { getUserData, isAdmin, logout } from '../../utils/auth';
import '../../../src/App.css';

const DashboardPage: React.FC = () => {
  const navigate = useNavigate();
  const [dashboardMessage, setDashboardMessage] = useState('');
  const [adminMessage, setAdminMessage] = useState('');
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const userData = getUserData();

  useEffect(() => {
    fetchDashboardData();
  }, []);

  const fetchDashboardData = async () => {
    try {
      setLoading(true);
      setError('');

      const dashboardResponse = await authService.getDashboard();
      if (dashboardResponse.success && dashboardResponse.data) {
        setDashboardMessage(dashboardResponse.data);
      } else {
        setError(dashboardResponse.message || 'Failed to load dashboard');
      }

      if (isAdmin()) {
        const adminResponse = await authService.getAdminDashboard();
        if (adminResponse.success && adminResponse.data) {
          setAdminMessage(adminResponse.data);
        }
      }
    } catch (err) {
      setError('An error occurred while loading dashboard data');
      console.error('Dashboard error:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleLogout = () => {
    logout();
  };

  if (loading) {
    return (
      <div className="dashboard-container">
        <div className="loading-spinner">
          <div className="spinner"></div>
          <p>Loading dashboard...</p>
        </div>
      </div>
    );
  }

  return (
    <div className="dashboard-container">
      <nav className="dashboard-nav">
        <div className="nav-brand">EgGroup Dashboard</div>
        <div className="nav-user">
          <span className="user-info">
            {userData?.username} ({userData?.role})
          </span>
          <button onClick={handleLogout} className="logout-button">
            Logout
          </button>
        </div>
      </nav>

      <main className="dashboard-content">
        <div className="welcome-section">
          <h1>{dashboardMessage}</h1>
          <p className="user-email">
            Logged in as: {userData?.email}
          </p>
        </div>

        {error && (
          <div className="error-alert">
            <strong>Error:</strong> {error}
          </div>
        )}

        {isAdmin() && (
          <div className="admin-section">
            <h2>Administrator Panel</h2>
            <div className="admin-card">
              <p className="admin-message">{adminMessage}</p>
              <div className="admin-features">
                <h3>Admin Features:</h3>
                <ul>
                  <li>Manage all users</li>
                  <li>View system statistics</li>
                  <li>Configure application settings</li>
                  <li>Access audit logs</li>
                </ul>
              </div>
            </div>
          </div>
        )}

        {!isAdmin() && (
          <div className="user-section">
            <div className="user-card">
              <h3>User Features:</h3>
              <ul>
                <li>View personal dashboard</li>
                <li>Update profile information</li>
                <li>View assigned tasks</li>
                <li>Access basic reports</li>
              </ul>
            </div>
          </div>
        )}

        <div className="info-section">
          <h3>Application Information</h3>
          <div className="info-grid">
            <div className="info-card">
              <h4>Technology Stack</h4>
              <p>React.js frontend with .NET Core Web API backend</p>
            </div>
            <div className="info-card">
              <h4>Authentication</h4>
              <p>JWT-based authentication with role-based authorization</p>
            </div>
            <div className="info-card">
              <h4>Database</h4>
              <p>SQL Server with Entity Framework Core</p>
            </div>
            <div className="info-card">
              <h4>Security</h4>
              <p>Password hashing, CORS protection, input validation</p>
            </div>
          </div>
        </div>
      </main>

      <footer className="dashboard-footer">
        <p>© 2024 EgGroup Assessment Application. All rights reserved.</p>
        <p className="footer-info">
          This is a demonstration application for technical assessment purposes.
        </p>
      </footer>
    </div>
  );
};

export default DashboardPage;