<script setup>
import {useChatStore} from "../stores/chatStore.js";
import {onMounted, ref} from "vue";
import {useRoute} from 'vue-router'

const chatStore = useChatStore();
const newMessage = ref('');
const props = defineProps({
  receiverUser: Object,
  adId: String,
})

onMounted(()=>{
  console.log("adId:", props.adId);
})
const sendMessage = () => {
  chatStore.sendMessage(props.receiverUser.id, newMessage.value, props.adId)
  newMessage.value = '';
}
</script>

<template>
<h1>AdChat</h1>
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