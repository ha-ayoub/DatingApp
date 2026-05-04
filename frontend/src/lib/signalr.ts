import * as signalR from '@microsoft/signalr'
import { useAuthStore } from '@/store/authStore'
import { useChatStore } from '@/store/chatStore'
import type { Message } from '@/types'

let connection: signalR.HubConnection | null = null

export function getConnection(): signalR.HubConnection {
  if (!connection) {
    connection = new signalR.HubConnectionBuilder()
      .withUrl(`${process.env.NEXT_PUBLIC_WS_URL ?? 'http://localhost:5000'}/hubs/chat`, {
        accessTokenFactory: () => useAuthStore.getState().accessToken ?? '',
      })
      .withAutomaticReconnect()
      .configureLogging(signalR.LogLevel.Warning)
      .build()

    connection.on('ReceiveMessage', (message: Message) => {
      useChatStore.getState().addMessage(message.matchId, message)
    })

    connection.on('NewMatch', (data: { matchId: string }) => {
      console.log('New match!', data.matchId)
    })
  }
  return connection
}

export async function startConnection() {
  const conn = getConnection()
  if (conn.state === signalR.HubConnectionState.Disconnected) {
    await conn.start()
  }
}

export async function stopConnection() {
  await connection?.stop()
}

export async function joinMatch(matchId: string) {
  await getConnection().invoke('JoinMatch', matchId)
}

export async function sendMessage(matchId: string, content: string) {
  await getConnection().invoke('SendMessage', matchId, content)
}
