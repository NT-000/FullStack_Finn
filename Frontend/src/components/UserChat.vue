<script setup>
import {useChatStore} from "../stores/chatStore.js";
import {ref} from "vue";

const chatStore = useChatStore();
const props = defineProps({
  receiverUser: Object,
})

const newMessage = ref('')
const sendMessage = () => {
  chatStore.sendMessage(props.receiverUser.id, newMessage.value)
  newMessage.value = '';
}

</script>

<template>
  <div v-if="receiverUser">
    <h2>Chat with user {{ receiverUser.name }}</h2>
    <div v-for="msg in chatStore.messages" :key="msg.id">
      <strong>{{ msg.senderName }}, <small>{{msg.timestamp.toLocaleString()}}</small>:</strong> {{ msg.content}}
    </div>
    <input v-model="newMessage" @keyup.enter="sendMessage" placeholder="Enter message"/>
  </div>
</template>

<style scoped>

</style>