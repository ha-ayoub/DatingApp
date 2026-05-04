import api from '@/lib/axios'
import type { Match, Message } from '@/types'

export const matchesApi = {
  getMatches: (page = 1, pageSize = 20) =>
    api.get<Match[]>('/matches', { params: { page, pageSize } }),

  getMessages: (matchId: string, page = 1, pageSize = 30) =>
    api.get<Message[]>(`/matches/${matchId}/messages`, { params: { page, pageSize } }),
}
