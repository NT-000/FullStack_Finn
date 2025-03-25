import { defineStore } from 'pinia'
import * as signalR from '@microsoft/signalr'
import { useUserStore } from './useUserStore.js'
import axios from "axios";
import {computed} from "vue";
import {getRoute} from "../composables/getRoute.js";

export const useChatStore = defineStore('chatStore', {
    state: () => ({
        connection: null,
        messages: []
    }),
    actions: {
        async startChat() {
            // Hvis tilkobling allerede finnes, ikke lag en ny:
            if (this.connection) return

            // 1) Bygg tilkobling
            this.connection = new signalR.HubConnectionBuilder()
                .withUrl('/chatHub', { withCredentials: true })
                .build()

            // 2) Lytt på “ReceiveMessage”
            this.connection.on('ReceiveMessage', async(senderId, receiverId, content) => {


                const users = getRoute('/users');
                await users.fetchData();
                const senderName = users.items.value.find(user => user.id === senderId).name;
                this.messages.push({
                    senderId,
                    receiverId,
                    senderName,
                    content,
                    timestamp: new Date()
                })
            })

            // 3) Start
            try {
                await this.connection.start()
                console.log('SignalR chat connection started')
            } catch (err) {
                console.error('SignalR connection error', err)
            }
        },
        async loadConversation(receiverId) {
            // 1) Finn min userId
            const userStore = useUserStore()
            const myId = userStore.user.id

            const users = getRoute('/users');
            await users.fetchData();

            const userList = users.items.value;
            const findUserName = (id) => userList.find(user => user.id === id).name;
            
            // 2) Hent meldinger fra backend
            try {
                const res = await axios.get(`/api/messages/conversation?userId1=${myId}&userId2=${receiverId}`,
                    { withCredentials: true })
                // 3) Tøm meldinger
                this.messages = []
                console.log("res.data laodConversation",res.data)
                res.data.forEach(msg => {
                    this.messages.push({
                        senderName: findUserName(msg.senderUserId),
                        senderId: msg.senderUserId,
                        receiverId: msg.receiverUserId,
                        content: msg.content,
                        timestamp: new Date(msg.timestamp),
                    })
                    console.log("message forEach loadConversation:",msg)
                })
            } catch (err) {
                console.error('Could not load conversation:', err)
            }
        },

        async sendMessage(receiverId, content) {
            const userStore = useUserStore()
            const senderId = userStore.user?.id
            if (!senderId || !receiverId) {
                console.error('Missing sender or receiver for chat message')
                return
            }
            if (!content) return

            try {
                await this.connection.invoke('SendMessage', senderId, receiverId, content)
                // melding vil ankomme “ReceiveMessage” og lagres i messages
            } catch (err) {
                console.error('Could not send message:', err)
            }
        }
    },
    getters: {
        // filtrerer meldinger for en bestemt samtale
        conversation: (state) => (receiverId) => {
            const userStore = useUserStore()
            const myId = userStore.user?.id
            return state.messages.filter(msg =>
                (msg.senderId === myId && msg.receiverId === receiverId) ||
                (msg.senderId === receiverId && msg.receiverId === myId)
            )
        }
    
        
    }
})