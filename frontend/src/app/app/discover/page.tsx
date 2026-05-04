'use client'
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query'
import { discoverApi } from '@/features/discover/api'
import { motion, AnimatePresence, useMotionValue, useTransform } from 'framer-motion'
import { useState } from 'react'
import type { User } from '@/types'
import { Heart, X, Star } from 'lucide-react'
import Image from 'next/image'

export default function DiscoverPage() {
  const [currentIndex, setCurrentIndex] = useState(0)
  const qc = useQueryClient()

  const { data: users = [], isLoading } = useQuery({
    queryKey: ['discover'],
    queryFn: () => discoverApi.getUsers(1, 20).then((r) => r.data),
  })

  const swipeMutation = useMutation({
    mutationFn: ({ userId, dir }: { userId: string; dir: 'Left' | 'Right' | 'SuperLike' }) =>
      discoverApi.swipe(userId, dir),
    onSuccess: (data) => {
      if (data.data.isMatch) qc.invalidateQueries({ queryKey: ['matches'] })
      setCurrentIndex((i) => i + 1)
    },
  })

  if (isLoading) return <DiscoverSkeleton />
  const currentUser = users[currentIndex]
  if (!currentUser) return <EmptyDiscover />

  return (
    <div className="flex flex-col items-center justify-center min-h-[calc(100vh-5rem)] bg-gray-50 p-4">
      <div className="relative w-full max-w-sm h-[580px]">
        <AnimatePresence>
          {users.slice(currentIndex, currentIndex + 2).reverse().map((user, i, arr) => (
            <SwipeCard
              key={user.id}
              user={user}
              isTop={i === arr.length - 1}
              onSwipe={(dir) => swipeMutation.mutate({ userId: user.id, dir })}
            />
          ))}
        </AnimatePresence>
      </div>
      <div className="flex gap-6 mt-6">
        <ActionButton onClick={() => swipeMutation.mutate({ userId: currentUser.id, dir: 'Left' })} size="lg">
          <X className="text-gray-400" size={26} />
        </ActionButton>
        <ActionButton onClick={() => swipeMutation.mutate({ userId: currentUser.id, dir: 'SuperLike' })} size="sm">
          <Star className="text-blue-400" size={20} />
        </ActionButton>
        <ActionButton onClick={() => swipeMutation.mutate({ userId: currentUser.id, dir: 'Right' })} size="lg">
          <Heart className="text-rose-400" size={26} />
        </ActionButton>
      </div>
    </div>
  )
}

function ActionButton({ onClick, children, size }: { onClick: () => void; children: React.ReactNode; size: 'sm' | 'lg' }) {
  const s = size === 'lg' ? 'w-14 h-14' : 'w-12 h-12'
  return (
    <button onClick={onClick}
      className={`${s} rounded-full bg-white shadow-lg flex items-center justify-center hover:scale-110 active:scale-95 transition-transform`}>
      {children}
    </button>
  )
}

function SwipeCard({ user, isTop, onSwipe }: { user: User; isTop: boolean; onSwipe: (d: 'Left' | 'Right' | 'SuperLike') => void }) {
  const x = useMotionValue(0)
  const rotate = useTransform(x, [-200, 200], [-25, 25])
  const likeOpacity = useTransform(x, [50, 150], [0, 1])
  const nopeOpacity = useTransform(x, [-150, -50], [1, 0])

  return (
    <motion.div
      style={{ x, rotate, position: 'absolute', width: '100%', height: '100%' }}
      drag={isTop ? 'x' : false}
      dragConstraints={{ left: 0, right: 0 }}
      onDragEnd={(_, info) => {
        if (info.offset.x > 100) onSwipe('Right')
        else if (info.offset.x < -100) onSwipe('Left')
      }}
      initial={{ scale: isTop ? 1 : 0.95, y: isTop ? 0 : 12 }}
      exit={{ x: x.get() > 0 ? 500 : -500, opacity: 0, transition: { duration: 0.25 } }}
      className="rounded-3xl overflow-hidden shadow-2xl bg-white cursor-grab active:cursor-grabbing select-none"
    >
      {user.mainPhotoUrl ? (
        <div className="relative w-full h-full">
          <Image src={user.mainPhotoUrl} alt={user.firstName} fill className="object-cover" priority={isTop} />
          <motion.div style={{ opacity: likeOpacity }}
            className="absolute top-8 left-6 border-4 border-green-400 text-green-400 font-black text-3xl px-3 py-1 rounded-xl -rotate-12 pointer-events-none">
            LIKE
          </motion.div>
          <motion.div style={{ opacity: nopeOpacity }}
            className="absolute top-8 right-6 border-4 border-rose-400 text-rose-400 font-black text-3xl px-3 py-1 rounded-xl rotate-12 pointer-events-none">
            NOPE
          </motion.div>
          <div className="absolute bottom-0 inset-x-0 bg-gradient-to-t from-black/80 via-black/20 to-transparent p-6 text-white">
            <h2 className="text-2xl font-bold">{user.firstName}, {user.age}</h2>
            {user.city && <p className="text-sm opacity-80 mt-1">📍 {user.city}</p>}
            {user.bio && <p className="text-sm opacity-90 mt-2 line-clamp-2">{user.bio}</p>}
          </div>
        </div>
      ) : (
        <div className="w-full h-full flex items-center justify-center bg-gradient-to-br from-rose-100 to-pink-200">
          <span className="text-8xl">💕</span>
        </div>
      )}
    </motion.div>
  )
}

function DiscoverSkeleton() {
  return (
    <div className="flex flex-col items-center justify-center min-h-[calc(100vh-5rem)] p-4">
      <div className="w-full max-w-sm h-[580px] rounded-3xl bg-gray-200 animate-pulse" />
    </div>
  )
}

function EmptyDiscover() {
  return (
    <div className="flex flex-col items-center justify-center min-h-[calc(100vh-5rem)] gap-4">
      <span className="text-6xl">🌟</span>
      <h2 className="text-xl font-semibold text-gray-700">No more profiles nearby</h2>
      <p className="text-gray-500 text-center max-w-xs">Check back later for new people in your area!</p>
    </div>
  )
}
