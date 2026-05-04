'use client'
import Link from 'next/link'
import { usePathname } from 'next/navigation'
import { Flame, MessageCircle, Heart, User } from 'lucide-react'
import { cn } from '@/lib/utils'

const navItems = [
  { href: '/app/discover', icon: Flame, label: 'Discover' },
  { href: '/app/matches', icon: Heart, label: 'Matches' },
  { href: '/app/messages', icon: MessageCircle, label: 'Messages' },
  { href: '/app/profile', icon: User, label: 'Profile' },
]

export default function AppNav() {
  const pathname = usePathname()
  return (
    <nav className="fixed bottom-0 left-0 right-0 bg-white border-t border-gray-100 px-4 pb-safe">
      <div className="flex items-center justify-around h-16 max-w-md mx-auto">
        {navItems.map(({ href, icon: Icon, label }) => {
          const active = pathname.startsWith(href)
          return (
            <Link key={href} href={href} className="flex flex-col items-center gap-1 min-w-[44px] min-h-[44px] justify-center">
              <Icon size={22} className={cn('transition-colors', active ? 'text-rose-500' : 'text-gray-400')} />
              <span className={cn('text-xs', active ? 'text-rose-500 font-medium' : 'text-gray-400')}>{label}</span>
            </Link>
          )
        })}
      </div>
    </nav>
  )
}
