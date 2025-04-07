<script setup>
import {ref} from "vue";
import {useLogin} from "../../composables/useLogin.js";

const email = ref('');
const password = ref('');

const {loading, error, logIn} = useLogin();

const props = defineProps({
	isUser: Boolean,
});
const emit = defineEmits(['update:isUser']);

const checkIn = async () => {
	await logIn(email.value, password.value);
};

const toggle = () => {
	emit('update:isUser', !props.isUser);
};
</script>

<template>
	<div class="login-container">
		<div class="login-card">
			<form @submit.prevent="checkIn">
				<h2>Velkommen tilbake 👋</h2>
				<div class="form-group">
					<label><i class="fa-solid fa-at"></i> Email</label>
					<input v-model="email" placeholder="din@email.com" required type="email"/>

					<label><i class="fa-solid fa-lock"></i> Passord</label>
					<input v-model="password" placeholder="••••••••" required type="password"/>
				</div>

				<div v-if="error" class="error">{{ error }}</div>

				<div class="btn-group">
					<button :disabled="loading" type="submit">
						{{ loading ? "Logger inn..." : "Logg inn" }}
					</button>
					<button class="ghost" type="button" @click="toggle">
						Registrer
					</button>
				</div>
			</form>
		</div>
	</div>
</template>

<style scoped>

.login-container {
	display: flex;
	align-items: center;
	justify-content: center;
	min-height: 100vh;
}

.login-card {
	background: rgba(255, 255, 255, 0.15);
	backdrop-filter: blur(10px);
	border-radius: 20px;
	padding: 40px;
	width: 30vw;
	max-width: 40vw;
	box-shadow: 0 8px 32px rgba(0, 0, 0, 0.25);
	color: white;
	text-align: center;
}

.form-group {
	display: flex;
	flex-direction: column;
	gap: 15px;
	margin-bottom: 20px;
}

label {
	font-size: 1rem;
	text-align: left;
}

input {
	padding: 12px;
	border: none;
	border-radius: 10px;
	background: rgba(255, 255, 255, 0.9);
	color: #333;
	font-size: 1rem;
	font-weight: 500;
	box-shadow: 0 3px 6px rgba(0, 0, 0, 0.1);
	transition: 0.2s;
}

input:focus {
	outline: none;
	box-shadow: 0 0 0 3px #00b4db;
}


.error {
	color: #ff6b6b;
	margin-bottom: 10px;
	font-weight: bold;
}

.btn-group {
	display: flex;
	justify-content: space-between;
	gap: 10px;
}

button {
	flex: 1;
	padding: 12px;
	border: none;
	border-radius: 10px;
	background: #00b4db;
	color: white;
	font-weight: bold;
	cursor: pointer;
	transition: background 0.3s ease;
}

button:hover {
	background: #0095c9;
}

button.ghost {
	background: transparent;
	border: 2px solid white;
	color: white;
}

button.ghost:hover {
	background: rgba(255, 255, 255, 0.2);
}
</style>