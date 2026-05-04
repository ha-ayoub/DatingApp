import { create } from 'zustand'
import type { Message } from '@/types'

interface ChatState {
  messages: Record<string, Message[]>
  addMessage: (matchId: string, message: Message) => void
  setMessages: (matchId: string, messages: Message[]) => void
}

export const useChatStore = create<ChatState>((set) => ({
  messages: {},
  addMessage: (matchId, message) =>
    set((s) => ({
      messages: {
        ...s.messages,
        [matchId]: [...(s.messages[matchId] ?? []), message],
      },
    })),
  setMessages: (matchId, messages) =>
    set((s) => ({ messages: { ...s.messages, [matchId]: messages } })),
}))
