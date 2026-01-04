import React from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import { isAuthenticated } from './utils/auth';
import './App.css';
import LoginPage from './app/login/page';
import DashboardPage from './app/dashboard/page';

const ProtectedRoute: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  return isAuthenticated() ? <>{children}</> : <Navigate to="/login" />;
};

function App() {
  return (
    <Router>
      <div className="App">
        <Routes>
          <Route path="/" element={
            isAuthenticated() ?
              <Navigate to="/dashboard" /> :
              <Navigate to="/login" />
          } />

          <Route path="/login" element={
            !isAuthenticated() ?
              <LoginPage /> :
              <Navigate to="/dashboard" />
          } />

          <Route path="/dashboard" element={
            <ProtectedRoute>
              <DashboardPage />
            </ProtectedRoute>
          } />

          <Route path="*" element={<Navigate to="/" />} />
        </Routes>
      </div>
    </Router>
  );
}

export default App;