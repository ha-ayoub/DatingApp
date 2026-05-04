'use client'
import { useEffect, useState } from 'react'
import { useAuthStore } from '@/store/authStore'
import { usersApi } from '@/features/users/api'
import Image from 'next/image'
import { motion } from 'framer-motion'
import { Settings, Heart, MessageCircle, MapPin } from 'lucide-react'
import Link from 'next/link'

export default function ProfilePage() {
  const { user } = useAuthStore()
  const [profile, setProfile] = useState<any>(null)
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    usersApi.getMyProfile()
      .then(res => setProfile(res.data))
      .finally(() => setLoading(false))
  }, [])

  if (loading) return (
    <div className="min-h-screen flex items-center justify-center bg-gray-50 dark:bg-gray-900">
      <div className="w-8 h-8 border-4 border-rose-400 border-t-transparent rounded-full animate-spin" />
    </div>
  )

  return (
    <div className="min-h-screen bg-gray-50 dark:bg-gray-900 pb-24">

      {/* Bandeau couleur haut */}
      <div className="h-40 bg-gradient-to-br from-rose-400 to-pink-500" />

      {/* Contenu principal */}
      <div className="px-6 -mt-20 space-y-6">
        <motion.div initial={{ opacity: 0, y: 16 }} animate={{ opacity: 1, y: 0 }} className="space-y-5">

          {/* Avatar + bouton settings */}
          <div className="flex items-end justify-between">
            {/* Photo de profil en cercle */}
            <div className="relative w-28 h-28 rounded-full border-4 border-white dark:border-gray-900 shadow-lg overflow-hidden bg-gradient-to-br from-rose-200 to-pink-300">
              {profile?.mainPhotoUrl ? (
                <Image
                  src={profile.mainPhotoUrl}
                  alt="Profile"
                  fill
                  className="object-cover"
                />
              ) : (
                <div className="w-full h-full flex items-center justify-center text-4xl font-bold text-white">
                  {user?.firstName?.charAt(0).toUpperCase()}
                </div>
              )}
            </div>
          </div>

          {/* Nom + localisation — toujours visibles */}
          <div>
            <h1 className="text-2xl font-bold text-gray-900 dark:text-white">
              {user?.firstName} {user?.lastName}
            </h1>
            {profile?.city && (
              <div className="flex items-center gap-1 mt-1">
                <MapPin size={14} className="text-gray-400" />
                <p className="text-gray-500 text-sm">{profile.city}{profile.country ? `, ${profile.country}` : ''}</p>
              </div>
            )}
          </div>

          {/* Stats */}
          <div className="grid grid-cols-2 gap-3">
            <div className="bg-white dark:bg-gray-800 rounded-2xl p-4 text-center shadow-sm">
              <Heart size={22} className="text-rose-400 mx-auto mb-1" fill="currentColor" />
              <p className="text-lg font-bold text-gray-900 dark:text-white">—</p>
              <p className="text-xs text-gray-500">Matches</p>
            </div>
            <div className="bg-white dark:bg-gray-800 rounded-2xl p-4 text-center shadow-sm">
              <MessageCircle size={22} className="text-blue-400 mx-auto mb-1" />
              <p className="text-lg font-bold text-gray-900 dark:text-white">—</p>
              <p className="text-xs text-gray-500">Messages</p>
            </div>
          </div>

          {/* Actions */}
          <div className="space-y-3">
            <Link href="/app/profile/edit"
              className="flex items-center justify-between p-4 bg-white dark:bg-gray-800 rounded-2xl shadow-sm">
              <span className="font-medium text-gray-700 dark:text-gray-300">Modifier mon profil</span>
              <span className="text-gray-400">›</span>
            </Link>
            <Link href="/app/profile/preferences"
              className="flex items-center justify-between p-4 bg-white dark:bg-gray-800 rounded-2xl shadow-sm">
              <span className="font-medium text-gray-700 dark:text-gray-300">Mes préférences</span>
              <span className="text-gray-400">›</span>
            </Link>
          </div>

        </motion.div>
      </div>
    </div>
  )
}