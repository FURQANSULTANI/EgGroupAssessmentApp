import axios, { AxiosError } from 'axios';
import { LoginRequest, LoginResponse, DashboardResponse, ApiResponse } from '../types/auth';

const API_BASE_URL = process.env.REACT_APP_API_BASE_URL || 'https://localhost:44341/api';

const apiClient = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

apiClient.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('auth_token');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

apiClient.interceptors.response.use(
  (response) => response,
  (error: AxiosError) => {
    if (error.response?.status === 401) {
      localStorage.removeItem('auth_token');
      localStorage.removeItem('user_data');
      window.location.href = '/login';
    }
    return Promise.reject(error);
  }
);

export const authService = {
  login: async (credentials: LoginRequest): Promise<LoginResponse> => {
    try {
      const response = await apiClient.post<LoginResponse>('/auth/login', credentials);
      return response.data;
    } catch (error) {
      if (axios.isAxiosError(error) && error.response) {
        return error.response.data as LoginResponse;
      }
      throw error;
    }
  },

  getDashboard: async (): Promise<DashboardResponse> => {
    try {
      const response = await apiClient.get<DashboardResponse>('/dashboard');
      return response.data;
    } catch (error) {
      if (axios.isAxiosError(error) && error.response) {
        return error.response.data as DashboardResponse;
      }
      throw error;
    }
  },

  getAdminDashboard: async (): Promise<DashboardResponse> => {
    try {
      const response = await apiClient.get<DashboardResponse>('/dashboard/admin');
      return response.data;
    } catch (error) {
      if (axios.isAxiosError(error) && error.response) {
        return error.response.data as DashboardResponse;
      }
      throw error;
    }
  },
};

export default apiClient;