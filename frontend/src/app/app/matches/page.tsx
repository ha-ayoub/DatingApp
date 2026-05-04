'use client'
import { useQuery } from '@tanstack/react-query'
import { matchesApi } from '@/features/matches/api'
import Link from 'next/link'
import Image from 'next/image'
import { formatDistanceToNow } from 'date-fns'

export default function MatchesPage() {
  const { data: matches = [], isLoading } = useQuery({
    queryKey: ['matches'],
    queryFn: () => matchesApi.getMatches().then((r) => r.data),
  })

  if (isLoading)
    return (
      <div className="p-4 space-y-3">
        {Array.from({ length: 5 }).map((_, i) => (
          <div key={i} className="flex items-center gap-3 p-3 bg-white rounded-2xl animate-pulse">
            <div className="w-14 h-14 rounded-full bg-gray-200" />
            <div className="flex-1 space-y-2">
              <div className="h-4 bg-gray-200 rounded w-1/3" />
              <div className="h-3 bg-gray-200 rounded w-2/3" />
            </div>
          </div>
        ))}
      </div>
    )

  if (matches.length === 0)
    return (
      <div className="flex flex-col items-center justify-center min-h-[calc(100vh-5rem)] gap-3 text-center px-4">
        <span className="text-5xl">💌</span>
        <h2 className="text-xl font-semibold text-gray-700">No matches yet</h2>
        <p className="text-gray-500">Start swiping to find your match!</p>
        <Link href="/app/discover" className="mt-2 px-6 py-3 bg-rose-500 text-white rounded-full font-medium hover:bg-rose-600 transition">
          Discover people
        </Link>
      </div>
    )

  return (
    <div className="p-4">
      <h1 className="text-2xl font-bold text-gray-900 mb-4">Matches 💕</h1>
      <div className="space-y-3">
        {matches.map((match) => (
          <Link key={match.id} href={`/app/messages/${match.id}`}
            className="flex items-center gap-3 p-3 bg-white rounded-2xl shadow-sm hover:shadow-md transition-shadow">
            <div className="relative w-14 h-14 rounded-full overflow-hidden bg-gray-100 flex-shrink-0">
              {match.otherUser.mainPhotoUrl ? (
                <Image src={match.otherUser.mainPhotoUrl} alt={match.otherUser.firstName} fill className="object-cover" />
              ) : (
                <span className="text-2xl flex items-center justify-center h-full">💕</span>
              )}
            </div>
            <div className="flex-1 min-w-0">
              <p className="font-semibold text-gray-900 truncate">
                {match.otherUser.firstName}, {match.otherUser.age}
              </p>
              <p className="text-sm text-gray-500 truncate">
                {match.lastMessage?.content ?? 'Say hello! 👋'}
              </p>
            </div>
            {match.lastMessage && (
              <span className="text-xs text-gray-400 flex-shrink-0">
                {formatDistanceToNow(new Date(match.lastMessage.createdAt), { addSuffix: true })}
              </span>
            )}
          </Link>
        ))}
      </div>
    </div>
  )
}
