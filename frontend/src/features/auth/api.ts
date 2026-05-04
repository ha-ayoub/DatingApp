import api from '@/lib/axios'
import type { AuthResponse } from '@/types'

export const authApi = {
  register: (data: {
    email: string
    password: string
    firstName: string
    lastName: string
    dateOfBirth: string
    gender: number
  }) => api.post<AuthResponse>('/auth/register', data),

  login: (email: string, password: string) =>
    api.post<AuthResponse>('/auth/login', { email, password }),

  refresh: (refreshToken: string) =>
    api.post<AuthResponse>('/auth/refresh', { refreshToken }),
}
