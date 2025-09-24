import axios, { AxiosResponse } from 'axios';
import Cookies from 'js-cookie';
import { 
  ApiResponse, 
  LoginRequest, 
  LoginResponse, 
  RegisterRequest, 
  User
} from '@/types/api';

const API_BASE_URL = process.env.NEXT_PUBLIC_API_URL || 'https://localhost:5189/api';

// Create axios instance
const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
  withCredentials: true
});

// Request interceptor to add auth token
api.interceptors.request.use(
  (config) => {
    const token = Cookies.get('accessToken');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// Response interceptor to handle token expiration
api.interceptors.response.use(
  (response) => {
    return response;
  },
  (error) => {
    if (error.response?.status === 401) {
      // Token expired or invalid
      Cookies.remove('accessToken');
      Cookies.remove('refreshToken');
      window.location.href = '/login';
    }
    return Promise.reject(error);
  }
);

// Auth API
export const authAPI = {
  login: async (data: LoginRequest): Promise<ApiResponse<LoginResponse>> => {
    const response: AxiosResponse<ApiResponse<LoginResponse>> = await api.post('/auth/login', data);
    console.log(response.data);
    return response.data;
  },

  register: async (data: RegisterRequest): Promise<ApiResponse<User>> => {
    const response: AxiosResponse<ApiResponse<User>> = await api.post('/auth/register', data);
    return response.data;
  },
};

// User API
export const userAPI = {
  searchByEmail: async (email: string): Promise<ApiResponse<User>> => {
    const response: AxiosResponse<ApiResponse<User>> = await api.get(`/users/search?email=${email}`);
    return response.data;
  },

  getCurrentUser: async (): Promise<ApiResponse<User>> => {
    const response: AxiosResponse<ApiResponse<User>> = await api.get('/users/@me');
    return response.data;
  },
};

export default api;
