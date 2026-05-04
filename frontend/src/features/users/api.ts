import api from '@/lib/axios'

export const usersApi = {
  getMyProfile: () => api.get('/users/me'),

  updateProfile: (data: { bio?: string; city?: string; country?: string }) =>
    api.put('/users/profile', data),

  updatePreferences: (data: {
    genderPreferences: number[]
    minAge: number
    maxAge: number
    maxDistanceKm: number
  }) => api.put('/users/preferences', data),

  addPhoto: (file: File) => {
    const form = new FormData()
    form.append('file', file)
    return api.post('/photos', form, {
      headers: { 'Content-Type': 'multipart/form-data' },
    })
  },

  deletePhoto: (photoId: string) => api.delete(`/photos/${photoId}`),

  setMainPhoto: (photoId: string) => api.put(`/photos/${photoId}/main`, {}),
}