<script setup>
import {useRoute} from "vue-router";
import {useChatStore} from "../stores/chatStore.js";
import {ref, onMounted, defineProps, computed} from "vue";
import {getRoute} from "../composables/getRoute.js";
import {useUserStore} from "../stores/useUserStore.js";

const users = getRoute('/users');
const route = useRoute();
const chatStore = useChatStore();
const receiverId =ref(Number(route.params.id));
console.log("ReceiverId from route:", route.params.id)
const newMessage = ref('')
const userStore = useUserStore();

const receiverUser = computed(() => {
  if (!users.items.value || users.items.value.length === 0) {
    return null;
  }
  return users.items.value.find(user => user.id === receiverId.value)
})
console.log("receiverUSer",receiverUser.value)

onMounted(async() => {
  await chatStore.startChat()
  await chatStore.loadConversation(receiverId.value)
  await users.fetchData()
})

const sendMessage = () => {
  chatStore.sendMessage(receiverId.value, newMessage.value)
  newMessage.value = '';
}

</script>

<template>
  <div v-if="receiverUser">
    <h2>Chat with user {{ receiverUser.name }}</h2>
    <div v-for="msg in chatStore.messages" :key="msg.id">
      <strong>{{ msg.senderName }}:</strong> {{ msg.content}}
      <p>{{msg.timestamp.toLocaleString()}}</p>
    </div>
    <input v-model="newMessage" @keyup.enter="sendMessage" placeholder="Enter message"/>
    
  </div>
</template>

<style scoped>

</style>