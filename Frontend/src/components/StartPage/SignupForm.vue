<script setup>
import {ref} from 'vue';
import axios from "axios";
import {useRouter} from "vue-router";

const name = ref('');
const email = ref('');
const password = ref('');
const file = ref(null);
const router = useRouter();

const props = defineProps({
	isUser: Boolean
});
const emit = defineEmits(['update:isUser']);
const error = ref(null);
function handleFileChange(e) {
	file.value = e.target.files[0];
}

async function handleSubmit() {
	const formData = new FormData();
	formData.append('Name', name.value);
	formData.append('Email', email.value);
	formData.append('Password', password.value);
	formData.append('file', file.value);

	try {
		const res = await axios.post('/api/users/register', formData, {withCredentials: true});
		console.log("response data reg user:", res.data);
		toggle()
		router.push('/');
	} catch (err) {
		if(formData.file == null){
		error.value = "Last opp profilbilde."
		}
		console.log("Registration failed", err);
	}
}

const toggle = () => {
	emit('update:isUser', true);
};
</script>

<template>
	<div class="signup-container">
		<form class="signup-form" @submit.prevent="handleSubmit">
			<h2>Lag ny bruker</h2>

			<label for="name">Navn</label>
			<input v-model="name" placeholder="Fornavn Etternavn" required type="text"/>

			<label for="email">E-post</label>
			<input v-model="email" placeholder="epost@domene.no" required type="email"/>

			<label for="password">Passord</label>
			<input v-model="password" placeholder="••••••••" required type="new-password"/>

			<label for="file">Profilbilde</label>
			<div class="file-upload">
				<input id="fileInput" hidden type="file" @change="handleFileChange"/>
				<label class="file-label" for="fileInput">
					<i class="fa-solid fa-image"></i> Last opp bilde
				</label>
			</div>

			<button class="submit-btn" type="submit">
				Registrer <i class="fa-solid fa-paper-plane"></i>
			</button>
			<button class="ghost-btn" type="button" @click="toggle">Tilbake til innlogging</button>
		</form>
		<div>{{error}}</div>
	</div>
</template>

<style scoped>

.signup-container {
	display: flex;
	align-items: center;
	justify-content: center;
	min-height: 100vh;
}

.signup-form {
	background: rgba(255, 255, 255, 0.15);
	backdrop-filter: blur(12px);
	padding: 40px;
	border-radius: 20px;
	width: 20vw;
	max-width: 30vw;
	color: white;
	box-shadow: 0 8px 32px rgba(0, 0, 0, 0.3);
	display: flex;
	flex-direction: column;
	gap: 20px;
}

h2 {
	text-align: center;
	font-size: 1.8rem;
	margin-bottom: 10px;
}

label {
	font-weight: bold;
	font-size: 1rem;
	margin-top: 10px;
}

input {
	padding: 12px;
	border: none;
	border-radius: 10px;
	background: rgba(255, 255, 255, 0.9);
	color: #333;
	font-size: 1rem;
	transition: box-shadow 0.2s ease;
}

input:focus {
	outline: none;
	box-shadow: 0 0 0 3px #00b4db;
}

.file-upload {
	display: flex;
	justify-content: center;
}

.file-label {
	background-color: rgba(255, 255, 255, 0.9);
	color: #333;
	padding: 10px 20px;
	border-radius: 8px;
	cursor: pointer;
	font-weight: bold;
	display: flex;
	align-items: center;
	gap: 10px;
	transition: 0.3s;
}

.file-label:hover {
	background-color: #e0f4ff;
}


button {
	padding: 12px;
	border-radius: 10px;
	border: none;
	cursor: pointer;
	font-weight: bold;
	font-size: 1rem;
	transition: 0.3s ease;
}

.submit-btn {
	background-color: #00b4db;
	color: white;
}

.submit-btn:hover {
	background-color: #008bb5;
}

.ghost-btn {
	background-color: transparent;
	border: 2px solid white;
	color: white;
}

.ghost-btn:hover {
	background-color: rgba(255, 255, 255, 0.2);
}
</style>
