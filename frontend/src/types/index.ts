export type Gender = 'Male' | 'Female'
export type SwipeDirection = 'Left' | 'Right' | 'SuperLike'

export interface User {
  id: string
  firstName: string
  lastName: string
  age: number
  bio?: string
  city?: string
  country?: string
  gender: Gender
  mainPhotoUrl?: string
  photos: Photo[]
  lastActiveAt?: string
}

export interface Photo {
  id: string
  url: string
  isMain: boolean
  order: number
}

export interface Match {
  id: string
  otherUser: User
  lastMessage?: Message
  createdAt: string
}

export interface Message {
  id: string
  matchId: string
  senderId: string
  content: string
  isRead: boolean
  readAt?: string
  createdAt: string
}

export interface AuthResponse {
  accessToken: string
  refreshToken: string
  user: AuthUser
}

export interface AuthUser {
  id: string
  email: string
  firstName: string
  lastName: string
  mainPhotoUrl?: string
}

export interface ApiError {
  status: number
  title: string
  errors?: Record<string, string[]>
}
