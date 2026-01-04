const TOKEN_KEY = 'auth_token';
const USER_KEY = 'user_data';

export const saveAuthToken = (token: string): void => {
  localStorage.setItem(TOKEN_KEY, token);
};

export const getAuthToken = (): string | null => {
  return localStorage.getItem(TOKEN_KEY);
};

export const saveUserData = (userData: { username: string; role: string; email: string }): void => {
  localStorage.setItem(USER_KEY, JSON.stringify(userData));
};

export const getUserData = (): { username: string; role: string; email: string } | null => {
  const userData = localStorage.getItem(USER_KEY);
  return userData ? JSON.parse(userData) : null;
};

export const isAuthenticated = (): boolean => {
  return !!getAuthToken();
};

export const isAdmin = (): boolean => {
  const userData = getUserData();
  return userData?.role === 'Admin';
};

export const logout = (): void => {
  localStorage.removeItem(TOKEN_KEY);
  localStorage.removeItem(USER_KEY);
  window.location.href = '/login';
};

export const getAuthHeader = (): { Authorization: string } | {} => {
  const token = getAuthToken();
  return token ? { Authorization: `Bearer ${token}` } : {};
};