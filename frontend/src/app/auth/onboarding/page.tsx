'use client'
import { useRouter } from 'next/navigation'
import { motion } from 'framer-motion'

export default function OnboardingPage() {
  const router = useRouter()
  return (
    <div className="min-h-screen flex items-center justify-center bg-gradient-to-br from-pink-50 to-rose-100 p-4">
      <motion.div
        initial={{ opacity: 0, scale: 0.9 }}
        animate={{ opacity: 1, scale: 1 }}
        transition={{ duration: 0.4 }}
        className="w-full max-w-md bg-white rounded-2xl shadow-xl p-8 text-center"
      >
        <span className="text-6xl"></span>
        <h1 className="text-2xl font-bold text-gray-900 mt-4">Welcome aboard!</h1>
        <p className="text-gray-500 mt-2">Your account is ready. Let&apos;s find your match!</p>
        <button
          onClick={() => router.push('/app/discover')}
          className="mt-6 w-full py-3 bg-gradient-to-r from-rose-400 to-pink-500 text-white font-semibold rounded-xl hover:opacity-90 transition"
        >
          Start discovering 💕
        </button>
      </motion.div>
    </div>
  )
}
