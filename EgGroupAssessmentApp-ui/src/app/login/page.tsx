import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { saveAuthToken, saveUserData } from '../../utils/auth';
import '../../../src/App.css';
import { authService } from '../../services/authServices';

const LoginPage: React.FC = () => {
  const navigate = useNavigate();
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');

    if (!username.trim() || !password.trim()) {
      setError('Please enter both username and password');
      return;
    }

    setLoading(true);

    try {
      const response = await authService.login({ username, password });

      if (response.success && response.data) {
        saveAuthToken(response.data.token);
        saveUserData({
          username: response.data.username,
          role: response.data.role,
          email: response.data.email
        });

        navigate('/dashboard');
      } else {
        setError(response.message || 'Login failed. Please check your credentials.');
      }
    } catch (err) {
      setError('An error occurred. Please try again.');
      console.error('Login error:', err);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="login-container">
      <div className="login-card">
        <h1 className="login-title">EgGroup Assessment</h1>
        <h2 className="login-subtitle">Sign In</h2>

        <form onSubmit={handleSubmit} className="login-form">
          <div className="form-group">
            <label htmlFor="username">Username</label>
            <input
              type="text"
              id="username"
              value={username}
              onChange={(e) => setUsername(e.target.value)}
              placeholder="Enter your username"
              disabled={loading}
              required
            />
          </div>

          <div className="form-group">
            <label htmlFor="password">Password</label>
            <input
              type="password"
              id="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              placeholder="Enter your password"
              disabled={loading}
              required
            />
          </div>

          {error && (
            <div className="error-message">
              {error}
            </div>
          )}

          <button
            type="submit"
            className="login-button"
            disabled={loading}
          >
            {loading ? 'Signing In...' : 'Sign In'}
          </button>
        </form>

        <div className="login-info">
          <p>Demo Credentials:</p>
          <div className="credentials">
            <div>
              <strong>Admin:</strong> admin / Admin@123
            </div>
            <div>
              <strong>User:</strong> user / User@123
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default LoginPage;