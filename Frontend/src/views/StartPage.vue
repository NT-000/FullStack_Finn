<script setup>
import {ref} from "vue";
import LoginForm from "../components/StartPage/LoginForm.vue";
import SignupForm from "../components/StartPage/SignupForm.vue";
import {useUserStore} from "@/stores/useUserStore.js";

const store = useUserStore();
const isUser = ref(true)
</script>

<template>
	<div class="container">
		<div v-if="isUser && !store.isLoggedIn" class="loginForm">
			<LoginForm v-model:isUser="isUser"/>
		</div>

		<div v-if="!isUser && !store.isLoggedIn" class="loginForm">
			<SignupForm v-model:isUser="isUser"/>
		</div>
	</div>
</template>

<style scoped>
.container {
	display: flex;
	justify-content: center;
	align-items: center;
	width: 400px;
}

h1 {
	font-size: 4rem;
	text-align: center;
	color: #00FF66FF;
	border: darkgreen solid 2px;
}

h1 {
	font-size: 5.8rem;
	background: black;
}

button:hover {
	font-size: 1.2rem;
}
</style>