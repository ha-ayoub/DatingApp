'use client'
import { useForm } from 'react-hook-form'
import { zodResolver } from '@hookform/resolvers/zod'
import { z } from 'zod'
import { useRouter } from 'next/navigation'
import { authApi } from '@/features/auth/api'
import { useAuthStore } from '@/store/authStore'
import { motion } from 'framer-motion'

const schema = z.object({
  firstName: z.string().min(1).max(50),
  lastName: z.string().min(1).max(50),
  email: z.string().email(),
  password: z.string().min(8).regex(/[A-Z]/, 'Needs uppercase').regex(/[0-9]/, 'Needs digit'),
  dateOfBirth: z.string().min(1, 'Date required'),
  gender: z.coerce.number().min(0).max(3),
})
type FormData = z.infer<typeof schema>

export default function RegisterPage() {
  const router = useRouter()
  const { setTokens, setUser } = useAuthStore()
  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
    setError,
  } = useForm<FormData>({ resolver: zodResolver(schema) })

  const onSubmit = async (data: FormData) => {
    try {
      const res = await authApi.register(data)
      setTokens(res.data.accessToken, res.data.refreshToken)
      setUser(res.data.user)
      router.push('/auth/onboarding')
    } catch {
      setError('root', { message: 'Registration failed. Try again.' })
    }
  }

  return (
    <div className="min-h-screen flex items-center justify-center bg-gradient-to-br from-pink-50 to-rose-100 p-4">
      <motion.div
        initial={{ opacity: 0, y: 20 }}
        animate={{ opacity: 1, y: 0 }}
        className="w-full max-w-md bg-white rounded-2xl shadow-xl p-8"
      >
        <h1 className="text-3xl font-bold text-center text-gray-900 mb-8">Create account </h1>
        <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">First name</label>
              <input {...register('firstName')} className="w-full px-3 py-2 border border-gray-200 rounded-xl focus:outline-none focus:ring-2 focus:ring-rose-400" />
              {errors.firstName && <p className="text-rose-500 text-xs mt-1">{errors.firstName.message}</p>}
            </div>
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">Last name</label>
              <input {...register('lastName')} className="w-full px-3 py-2 border border-gray-200 rounded-xl focus:outline-none focus:ring-2 focus:ring-rose-400" />
            </div>
          </div>
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">Email</label>
            <input {...register('email')} type="email" className="w-full px-4 py-3 border border-gray-200 rounded-xl focus:outline-none focus:ring-2 focus:ring-rose-400" />
            {errors.email && <p className="text-rose-500 text-xs mt-1">{errors.email.message}</p>}
          </div>
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">Password</label>
            <input {...register('password')} type="password" className="w-full px-4 py-3 border border-gray-200 rounded-xl focus:outline-none focus:ring-2 focus:ring-rose-400" />
            {errors.password && <p className="text-rose-500 text-xs mt-1">{errors.password.message}</p>}
          </div>
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">Date of birth</label>
            <input {...register('dateOfBirth')} type="date" className="w-full px-4 py-3 border border-gray-200 rounded-xl focus:outline-none focus:ring-2 focus:ring-rose-400" />
          </div>
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">Gender</label>
            <select {...register('gender')} className="w-full px-4 py-3 border border-gray-200 rounded-xl focus:outline-none focus:ring-2 focus:ring-rose-400">
              <option value={0}>Male</option>
              <option value={1}>Female</option>
            </select>
          </div>
          {errors.root && <p className="text-rose-500 text-sm text-center">{errors.root.message}</p>}
          <button type="submit" disabled={isSubmitting}
            className="w-full py-3 bg-gradient-to-r from-rose-400 to-pink-500 text-white font-semibold rounded-xl hover:opacity-90 transition disabled:opacity-50">
            {isSubmitting ? 'Creating...' : 'Create account'}
          </button>
        </form>
        <p className="text-center text-sm text-gray-500 mt-6">
          Already have an account?{' '}
          <a href="/auth/login" className="text-rose-500 font-medium hover:underline">Sign in</a>
        </p>
      </motion.div>
    </div>
  )
}
