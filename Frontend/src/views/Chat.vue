<script setup>
import {useRoute} from "vue-router";
import {useChatStore} from "../stores/chatStore.js";
import {ref, onMounted, defineProps} from "vue";

const props = defineProps({
  Id: String,
})

const route = useRoute();
const chatStore = useChatStore();
const receiverId =ref(Number(route.params.id));
console.log("ReceiverId from route:", route.params.id)
const newMessage = ref('')

onMounted(async() => {
  await chatStore.startChat()
  await chatStore.loadConversation(receiverId.value)
})

const sendMessage = () => {
  chatStore.sendMessage(receiverId.value, newMessage.value)
  newMessage.value = '';
}

</script>

<template>
  <div>
    <h2>Chat with user {{ receiverId }}</h2>
    <div v-for="msg in chatStore.messages" :key="msg.id">
      <strong>{{ msg.senderId }}:</strong> {{ msg.content}}
      <p>{{msg.timestamp.toLocaleString()}}</p>
    </div>
    <input v-model="newMessage" @keyup.enter="sendMessage" />
    <button @click="sendMessage">Send</button>
  </div>
</template>

<style scoped>

</style>