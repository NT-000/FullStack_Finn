<script setup>
import {useChatStore} from "../stores/chatStore.js";
import {computed, onMounted, ref} from "vue";

const chatStore = useChatStore();
const newMessage = ref('');
const props = defineProps({
	receiverUser: Object,
	adId: [String, Number],
})

const conversation = computed(() => chatStore.conversation(props.receiverUser.id, props.adId))

onMounted(() => {
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
		<div v-for="msg in conversation" :key="msg.id">
			<strong>{{ msg.senderName }}, <small>{{ msg.timestamp.toLocaleString() }}</small>:</strong> {{ msg.content }}
		</div>
		<input v-model="newMessage" placeholder="Enter message" @keyup.enter="sendMessage"/>
	</div>
</template>

<style scoped>

</style>