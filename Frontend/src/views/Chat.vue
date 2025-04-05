<script setup>
import {useRoute} from "vue-router";
import {useChatStore} from "../stores/chatStore.js";
import {computed, onMounted, ref} from "vue";
import {getRoute} from "../composables/getRoute.js";
import AdChat from "../components/AdChat.vue";


const users = getRoute('/users');
const route = useRoute();
const chatStore = useChatStore();
const receiverId = ref(Number(route.params.id));
console.log("ReceiverId from route:", route.params.id)
const adId = ref(Number(route.query.adId));
const receiverUser = computed(() => {
	if (!users.items.value || users.items.value.length === 0) {
		return null;
	}
	return users.items.value.find(user => user.id === receiverId.value)
})
console.log("receiverUSer", receiverUser.value)

onMounted(async () => {
	await chatStore.startChat()
	await chatStore.loadConversation(receiverId.value)
	await users.fetchData()
	console.log("adId chat-vue", adId.value)
})

</script>

<template>

	<AdChat v-if="adId" :adId="adId" :receiverUser="receiverUser"/>
</template>

<style scoped>

</style>