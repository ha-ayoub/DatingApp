'use client'
import { useEffect } from 'react'
import { useRouter } from 'next/navigation'
import { useAuthStore } from '@/store/authStore'
import AppNav from '@/components/layout/AppNav'

export default function AppLayout({ children }: { children: React.ReactNode }) {
  const router = useRouter()
  const { isAuthenticated, accessToken, _hasHydrated } = useAuthStore()

  useEffect(() => {
    // Attendre que Zustand ait lu localStorage avant de rediriger
    if (!_hasHydrated) return
    if (!isAuthenticated || !accessToken) {
      router.push('/auth/login')
    }
  }, [_hasHydrated, isAuthenticated, accessToken, router])

  // Afficher un loader pendant la rehydratation
  if (!_hasHydrated) {
    return (
      <div className="min-h-screen flex items-center justify-center bg-gray-50 dark:bg-gray-900">
        <div className="w-8 h-8 border-4 border-rose-400 border-t-transparent rounded-full animate-spin" />
      </div>
    )
  }

  if (!isAuthenticated || !accessToken) return null

  return (
    <div className="min-h-screen bg-gray-50 dark:bg-gray-900">
      <main className="pb-20">{children}</main>
      <AppNav />
    </div>
  )
}