export interface LoginRequest {
  username: string;
  password: string;
}

export interface LoginResponse {
  success: boolean;
  message: string;
  data?: {
    token: string;
    username: string;
    role: string;
    email: string;
    expiresAt: string;
  };
  errors?: string[];
}

export interface User {
  id: number;
  username: string;
  role: string;
  email: string;
}

export interface DashboardResponse {
  success: boolean;
  message: string;
  data?: string;
  errors?: string[];
}

export interface ApiResponse<T> {
  success: boolean;
  message: string;
  data?: T;
  errors?: string[];
}