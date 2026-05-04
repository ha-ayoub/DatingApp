'use client'
import { useEffect, useState } from 'react'
import { useRouter } from 'next/navigation'
import { usersApi } from '@/features/users/api'
import { useCallback } from 'react'
import { useDropzone } from 'react-dropzone'
import { motion, AnimatePresence } from 'framer-motion'
import Image from 'next/image'
import { Trash2, Star, Plus, ArrowLeft } from 'lucide-react'

interface Photo { id: string; url: string; isMain: boolean }

export default function EditProfilePage() {
  const router = useRouter()
  const [bio, setBio] = useState('')
  const [city, setCity] = useState('')
  const [country, setCountry] = useState('')
  const [photos, setPhotos] = useState<Photo[]>([])
  const [loading, setLoading] = useState(true)
  const [saving, setSaving] = useState(false)
  const [uploading, setUploading] = useState(false)

useEffect(() => {
  usersApi.getMyProfile().then(res => {
    const p = res.data
    setBio(p.bio ?? '')
    setCity(p.city ?? '')
    setCountry(p.country ?? '')
    setPhotos(p.photos?.map((photo: { id: string; url: string; isMain: boolean }) => ({
      id: photo.id,
      url: photo.url,
      isMain: photo.isMain,
    })) ?? [])
  }).finally(() => setLoading(false))
}, [])

  const onDrop = useCallback(async (files: File[]) => {
    const file = files[0]
    if (!file) return
    setUploading(true)
    try {
      const res = await usersApi.addPhoto(file)
      setPhotos(prev => [...prev, res.data])
    } finally {
      setUploading(false)
    }
  }, [])

  const { getRootProps, getInputProps, isDragActive } = useDropzone({
    onDrop, accept: { 'image/*': [] }, maxFiles: 1, maxSize: 5 * 1024 * 1024,
  })

  const handleDelete = async (photoId: string) => {
    await usersApi.deletePhoto(photoId)
    setPhotos(prev => prev.filter(p => p.id !== photoId))
  }

  const handleSetMain = async (photoId: string) => {
    await usersApi.setMainPhoto(photoId)
    setPhotos(prev => prev.map(p => ({ ...p, isMain: p.id === photoId })))
  }

  const handleSave = async () => {
    setSaving(true)
    try {
      await usersApi.updateProfile({ bio, city, country })
      router.push('/app/profile')
    } finally {
      setSaving(false)
    }
  }

  if (loading) return (
    <div className="min-h-screen flex items-center justify-center">
      <div className="w-8 h-8 border-4 border-rose-400 border-t-transparent rounded-full animate-spin" />
    </div>
  )

  return (
    <div className="min-h-screen bg-gray-50 dark:bg-gray-900 p-6 max-w-md mx-auto">
      <motion.div initial={{ opacity: 0, y: 16 }} animate={{ opacity: 1, y: 0 }} className="space-y-8">

        {/* Header */}
        <div className="flex items-center gap-3">
          <button onClick={() => router.back()}
            className="p-2 rounded-full bg-white dark:bg-gray-800 shadow">
            <ArrowLeft size={20} className="text-gray-600 dark:text-gray-300" />
          </button>
          <h1 className="text-2xl font-bold text-gray-900 dark:text-white">Modifier mon profil</h1>
        </div>

        {/* Photos */}
        <div className="space-y-3">
        <h2 className="font-semibold text-gray-700 dark:text-gray-300">📸 Mes photos</h2>
        <div className="grid grid-cols-3 gap-2">
            <AnimatePresence>
            {photos.map(photo => (
                <motion.div key={photo.id}
                initial={{ opacity: 0, scale: 0.8 }}
                animate={{ opacity: 1, scale: 1 }}
                exit={{ opacity: 0, scale: 0.8 }}
                className="relative aspect-square rounded-xl overflow-hidden">

                <Image src={photo.url} alt="photo" fill className="object-cover" />

                {/* Badge photo principale */}
                {photo.isMain && (
                    <div className="absolute top-1 left-1 bg-yellow-400 rounded-full px-2 py-0.5 flex items-center gap-1">
                    <Star size={10} className="text-white" fill="white" />
                    <span className="text-white text-xs font-bold">Main</span>
                    </div>
                )}

                {/* Boutons toujours visibles en bas */}
                {!photo.isMain && (
                    <div className="absolute bottom-0 left-0 right-0 flex">
                    {/* Définir comme principale */}
                    <button
                        onClick={() => handleSetMain(photo.id)}
                        className="flex-1 py-2 bg-yellow-400/90 flex items-center justify-center gap-1 hover:bg-yellow-500 transition"
                        title="Définir comme principale">
                        <Star size={14} className="text-white" fill="white" />
                    </button>

                    {/* Supprimer */}
                    <button
                        onClick={() => handleDelete(photo.id)}
                        className="flex-1 py-2 bg-red-500/90 flex items-center justify-center gap-1 hover:bg-red-600 transition"
                        title="Supprimer">
                        <Trash2 size={14} className="text-white" />
                    </button>
                    </div>
                )}
                </motion.div>
            ))}

            {/* Zone upload */}
            {photos.length < 6 && (
                <div {...getRootProps()}
                className={`aspect-square rounded-xl border-2 border-dashed flex flex-col items-center justify-center cursor-pointer transition-colors gap-1 ${
                    isDragActive
                    ? 'border-rose-400 bg-rose-50 dark:bg-rose-900/20'
                    : 'border-gray-300 dark:border-gray-600 bg-white dark:bg-gray-800'
                }`}>
                <input {...getInputProps()} />
                {uploading
                    ? <div className="w-5 h-5 border-2 border-rose-400 border-t-transparent rounded-full animate-spin" />
                    : <>
                        <Plus size={24} className="text-gray-400" />
                        <span className="text-xs text-gray-400">Ajouter</span>
                    </>
                }
                </div>
            )}
            </AnimatePresence>
        </div>
        <p className="text-xs text-gray-400">⭐ = photo principale · Max 6 photos · 5MB max</p>
        </div>

        {/* Bio */}
        <div className="space-y-2">
          <label className="font-semibold text-gray-700 dark:text-gray-300">✍️ Bio</label>
          <textarea value={bio} onChange={e => setBio(e.target.value)}
            maxLength={500} rows={4} placeholder="Parle de toi..."
            className="w-full px-4 py-3 rounded-2xl border border-gray-200 dark:border-gray-700 bg-white dark:bg-gray-800 text-gray-900 dark:text-white resize-none focus:outline-none focus:ring-2 focus:ring-rose-400 text-sm" />
          <p className="text-xs text-gray-400 text-right">{bio.length}/500</p>
        </div>

        {/* Ville */}
        <div className="space-y-2">
          <label className="font-semibold text-gray-700 dark:text-gray-300"> Ville</label>
          <input value={city} onChange={e => setCity(e.target.value)}
            placeholder="Paris, Lyon..."
            className="w-full px-4 py-3 rounded-2xl border border-gray-200 dark:border-gray-700 bg-white dark:bg-gray-800 text-gray-900 dark:text-white focus:outline-none focus:ring-2 focus:ring-rose-400 text-sm" />
        </div>

        {/* Pays */}
        <div className="space-y-2">
          <label className="font-semibold text-gray-700 dark:text-gray-300"> Pays</label>
          <input value={country} onChange={e => setCountry(e.target.value)}
            placeholder="France, Belgique..."
            className="w-full px-4 py-3 rounded-2xl border border-gray-200 dark:border-gray-700 bg-white dark:bg-gray-800 text-gray-900 dark:text-white focus:outline-none focus:ring-2 focus:ring-rose-400 text-sm" />
        </div>

        {/* Boutons */}
        <div className="flex gap-3">
          <button onClick={() => router.back()}
            className="flex-1 py-4 border border-gray-200 dark:border-gray-700 text-gray-600 dark:text-gray-300 font-semibold rounded-2xl hover:bg-gray-100 dark:hover:bg-gray-800 transition">
            Annuler
          </button>
          <button onClick={handleSave} disabled={saving}
            className="flex-1 py-4 bg-gradient-to-r from-rose-400 to-pink-500 text-white font-semibold rounded-2xl hover:opacity-90 transition disabled:opacity-50">
            {saving ? 'Sauvegarde...' : 'Sauvegarder'}
          </button>
        </div>

      </motion.div>
    </div>
  )
}