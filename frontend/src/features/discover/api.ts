import api from '@/lib/axios'
import type { User } from '@/types'

export const discoverApi = {
  getUsers: (page = 1, pageSize = 10) =>
    api.get<User[]>('/users/discover', { params: { page, pageSize } }),

  swipe: (swipedUserId: string, direction: 'Left' | 'Right' | 'SuperLike') =>
    api.post<{ isMatch: boolean; matchId?: string }>('/matches/swipe', {
      swipedUserId,
      direction, // ← envoyer la string directement "Left", "Right", "SuperLike"
    }),
}
