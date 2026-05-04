'use client'
import { use, useEffect, useRef, useState } from 'react'
import { useQuery } from '@tanstack/react-query'
import { matchesApi } from '@/features/matches/api'
import { useChatStore } from '@/store/chatStore'
import { useAuthStore } from '@/store/authStore'
import { joinMatch, sendMessage as sendSignalRMessage, startConnection } from '@/lib/signalr'
import { motion } from 'framer-motion'
import { Send, ArrowLeft } from 'lucide-react'
import Link from 'next/link'

export default function ChatPage({ params }: { params: Promise<{ matchId: string }> }) {
  const { matchId } = use(params)
  const [input, setInput] = useState('')
  const bottomRef = useRef<HTMLDivElement>(null)
  const { user } = useAuthStore()
  const { messages, setMessages } = useChatStore()
  const chatMessages = messages[matchId] ?? []

  useQuery({
    queryKey: ['messages', matchId],
    queryFn: async () => {
      const res = await matchesApi.getMessages(matchId)
      setMessages(matchId, [...res.data].reverse())
      return res.data
    },
  })

  useEffect(() => {
    startConnection().then(() => joinMatch(matchId))
  }, [matchId])

  useEffect(() => {
    bottomRef.current?.scrollIntoView({ behavior: 'smooth' })
  }, [chatMessages.length])

  const handleSend = async () => {
    const content = input.trim()
    if (!content) return
    setInput('')
    await sendSignalRMessage(matchId, content)
  }

  return (
    <div className="flex flex-col h-[100dvh] bg-gray-50">
      <header className="flex items-center gap-3 px-4 py-3 bg-white border-b border-gray-100 sticky top-0 z-10">
        <Link href="/app/matches" className="p-2 -ml-2 rounded-full hover:bg-gray-100 transition">
          <ArrowLeft size={20} className="text-gray-600" />
        </Link>
        <h1 className="font-semibold text-gray-900">Chat</h1>
      </header>

      <div className="flex-1 overflow-y-auto px-4 py-4 space-y-3">
        {chatMessages.length === 0 && (
          <div className="text-center text-gray-400 py-8">
            <p className="text-4xl mb-2">👋</p>
            <p>Say hello to start the conversation!</p>
          </div>
        )}
        {chatMessages.map((msg) => (
          <motion.div
            key={msg.id}
            initial={{ opacity: 0, y: 8 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.2 }}
            className={`flex ${msg.senderId === user?.id ? 'justify-end' : 'justify-start'}`}
          >
            <div className={`max-w-[72%] px-4 py-2.5 rounded-2xl text-sm leading-relaxed ${
              msg.senderId === user?.id
                ? 'bg-gradient-to-r from-rose-400 to-pink-500 text-white rounded-br-sm'
                : 'bg-white text-gray-900 shadow-sm rounded-bl-sm'
            }`}>
              {msg.content}
            </div>
          </motion.div>
        ))}
        <div ref={bottomRef} />
      </div>

      <div className="px-4 py-3 bg-white border-t border-gray-100 safe-area-bottom">
        <div className="flex gap-3 items-center max-w-2xl mx-auto">
          <input
            value={input}
            onChange={(e) => setInput(e.target.value)}
            onKeyDown={(e) => e.key === 'Enter' && !e.shiftKey && handleSend()}
            placeholder="Type a message..."
            className="flex-1 px-4 py-3 rounded-full bg-gray-100 text-sm outline-none focus:ring-2 focus:ring-rose-300"
          />
          <button
            onClick={handleSend}
            className="w-11 h-11 flex-shrink-0 rounded-full bg-gradient-to-r from-rose-400 to-pink-500 flex items-center justify-center text-white hover:opacity-90 active:scale-95 transition"
          >
            <Send size={16} />
          </button>
        </div>
      </div>
    </div>
  )
}
