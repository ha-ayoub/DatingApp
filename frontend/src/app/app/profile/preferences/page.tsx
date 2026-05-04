'use client'
import { useState, useEffect } from 'react'
import { useRouter } from 'next/navigation'
import { usersApi } from '@/features/users/api'
import { motion } from 'framer-motion'

const GENDER_OPTIONS = [
  { label: '👨 Hommes', value: 0 },
  { label: '👩 Femmes', value: 1 },
]

export default function PreferencesPage() {
  const router = useRouter()
  const [selected, setSelected] = useState<number[]>([])
  const [minAge, setMinAge] = useState(18)
  const [maxAge, setMaxAge] = useState(40)
  const [distance, setDistance] = useState(50)
  const [saving, setSaving] = useState(false)
  const [loading, setLoading] = useState(true)

  // Charger les préférences existantes au montage
  useEffect(() => {
    const load = async () => {
      try {
        const res = await usersApi.getMyProfile()
        const p = res.data
        setSelected(p.genderPreferences ?? [])
        setMinAge(p.minAgePreference ?? 18)
        setMaxAge(p.maxAgePreference ?? 40)
        setDistance(p.maxDistanceKm ?? 50)
      } catch {
        // valeurs par défaut déjà définies
      } finally {
        setLoading(false)
      }
    }
    load()
  }, [])

  const toggle = (val: number) =>
    setSelected(prev =>
      prev.includes(val) ? prev.filter(v => v !== val) : [...prev, val]
    )

  const save = async () => {
    if (selected.length === 0) return
    setSaving(true)
    try {
      await usersApi.updatePreferences({
        genderPreferences: selected,
        minAge,
        maxAge,
        maxDistanceKm: distance,
      })
      router.push('/app/discover')
    } finally {
      setSaving(false)
    }
  }

  if (loading) {
    return (
      <div className="min-h-screen flex items-center justify-center">
        <div className="w-8 h-8 border-4 border-rose-400 border-t-transparent rounded-full animate-spin" />
      </div>
    )
  }

  return (
    <div className="min-h-screen bg-gray-50 dark:bg-gray-900 p-6 max-w-md mx-auto">
      <motion.div initial={{ opacity: 0, y: 16 }} animate={{ opacity: 1, y: 0 }} className="space-y-8">
        <div>
          <h1 className="text-2xl font-bold text-gray-900 dark:text-white">Mes préférences</h1>
          <p className="text-gray-500 mt-1 text-sm">Personnalise qui tu veux découvrir</p>
        </div>

        {/* Genre */}
        <div className="space-y-3">
          <h2 className="font-semibold text-gray-700 dark:text-gray-300">Je cherche</h2>
          <div className="flex gap-3">
            {GENDER_OPTIONS.map(opt => (
              <button key={opt.value} onClick={() => toggle(opt.value)}
                className={`flex-1 py-4 rounded-2xl border-2 font-medium transition-all text-sm ${
                  selected.includes(opt.value)
                    ? 'border-rose-400 bg-rose-50 dark:bg-rose-900/20 text-rose-600 dark:text-rose-400'
                    : 'border-gray-200 dark:border-gray-700 text-gray-500 dark:text-gray-400 bg-white dark:bg-gray-800'
                }`}>
                {opt.label}
              </button>
            ))}
          </div>
          {selected.length === 0 && (
            <p className="text-rose-500 text-xs">Sélectionne au moins un genre</p>
          )}
        </div>

        {/* Tranche d'âge */}
        <div className="space-y-3">
          <h2 className="font-semibold text-gray-700 dark:text-gray-300">
            Tranche d&apos;âge : <span className="text-rose-500">{minAge} – {maxAge} ans</span>
          </h2>
          <div className="space-y-2">
            <div className="flex items-center gap-3">
              <span className="text-xs text-gray-400 w-6">Min</span>
              <input type="range" min={18} max={maxAge - 1} value={minAge}
                onChange={e => setMinAge(Number(e.target.value))}
                className="flex-1 accent-rose-400" />
              <span className="text-sm font-medium text-gray-700 dark:text-gray-300 w-8">{minAge}</span>
            </div>
            <div className="flex items-center gap-3">
              <span className="text-xs text-gray-400 w-6">Max</span>
              <input type="range" min={minAge + 1} max={99} value={maxAge}
                onChange={e => setMaxAge(Number(e.target.value))}
                className="flex-1 accent-rose-400" />
              <span className="text-sm font-medium text-gray-700 dark:text-gray-300 w-8">{maxAge}</span>
            </div>
          </div>
        </div>

        {/* Distance */}
        <div className="space-y-3">
          <h2 className="font-semibold text-gray-700 dark:text-gray-300">
            Distance max : <span className="text-rose-500">{distance} km</span>
          </h2>
          <div className="flex items-center gap-3">
            <input type="range" min={5} max={300} step={5} value={distance}
              onChange={e => setDistance(Number(e.target.value))}
              className="flex-1 accent-rose-400" />
            <span className="text-sm font-medium text-gray-700 dark:text-gray-300 w-16">{distance} km</span>
          </div>
        </div>

        <button onClick={save} disabled={saving || selected.length === 0}
          className="w-full py-4 bg-gradient-to-r from-rose-400 to-pink-500 text-white font-semibold rounded-2xl hover:opacity-90 transition disabled:opacity-50">
          {saving ? 'Sauvegarde...' : 'Sauvegarder mes préférences'}
        </button>
      </motion.div>
    </div>
  )
}