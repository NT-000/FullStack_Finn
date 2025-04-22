<script setup>
import { useChatStore } from "../stores/chatStore.js";
import { computed, onMounted, ref } from "vue";
import { useUserStore } from "../stores/useUserStore.js";
import {useDateFormat} from "../composables/useFormatDate.js";
import {getRoute} from "../composables/getRoute.js";

const chatStore = useChatStore();
const userStore = useUserStore();
const dateFormat = useDateFormat();
const users = getRoute("/users");

const newMessage = ref('');
const props = defineProps({
	receiverUser: Object,
	adId: [String, Number],
});

// Henter samtalen for bruker og annonse
const conversation = computed(() => chatStore.conversation(props.receiverUser.id, props.adId));

const sender = computed(() => {
	
});
// Funksjon for å sende melding
const sendMessage = () => {
	if (newMessage.value.trim() === '') return;
	chatStore.sendMessage(props.receiverUser.id, newMessage.value, props.adId);
	newMessage.value = '';
};

onMounted(async() => {
	console.log("adId:", props.adId);
	try{
		await users.fetchData()
	}catch(e){
		console.log(e);
	}
});
</script>

<template>
	<div class="chat-wrapper" v-if="receiverUser">
		<h2>Chat med {{ receiverUser.name }}</h2>

		<div class="chat-messages">
			<div
					v-for="msg in conversation" 
					:key="msg.id"
					:class="['chat-msg', msg.senderId === userStore.user.id ? 'sent' : 'received']"
			>
			<img v-if="msg.senderId === userStore.user.id"
			:src="userStore.user.profileImageUrl"
			class="chatProfilePic">
				
				<img v-else
				:src="receiverUser.profileImageUrl"
				     class="chatProfilePic">
				<strong>{{ msg.senderId === userStore.user.id ? 'Deg' : msg.senderName }}</strong>
				<small>{{ dateFormat.formatDate(msg.timestamp) }}</small>
				<p>{{ msg.content }}</p>
			</div>
		</div>

		<div class="chat-input">
			<input
					v-model="newMessage"
					placeholder="Skriv melding..."
					@keyup.enter="sendMessage"
			/>
			<button @click="sendMessage">Send</button>
		</div>
	</div>
</template>

<style scoped>
.chat-wrapper {
	max-width: 35vw;
	width: 120vw;
	margin: 5px auto;
	padding: 1rem;
	background-color: #f5f5f5;
	border-radius: 8px;
	box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
}

.chat-messages {
	max-height: 900px;
	width: 35vw;
	overflow-y: auto;
	margin-bottom: 1rem;
	display: flex;
	flex-direction: column;
	gap: 0.5rem;
}

.chat-msg {
	padding: 0.5rem 1rem;
	border-radius: 10px;
	background-color: #ddd;
	align-self: flex-start;
	max-width: 70%;
	word-break: break-word;
}
.chatProfilePic{
	height: 5rem;
	width: 5rem;
	border-radius: 50%;
	vertical-align: middle;
}

.chat-msg.sent {
	background-color: darkgreen;
	align-self: flex-end;
}

.chat-msg.received {
	background-color: #e6e6e6;
}

.chat-msg strong {
	display: block;
	font-weight: bold;
}

.chat-msg small {
	font-size: 0.8rem;
	color: #666;
}

.chat-input {
	display: flex;
	gap: 0.5rem;
	
}

.chat-input input {
	flex: 1;
	padding: 0.5rem;
	border-radius: 6px;
	border: 1px solid #ccc;
	color: springgreen;
}

.chat-input button {
	padding: 0.5rem 1rem;
	border: none;
	background-color: gray;
	color: springgreen;
	border-radius: 6px;
	cursor: pointer;
}
</style>
