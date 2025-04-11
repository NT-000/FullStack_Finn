<script setup>
import {computed, watchEffect} from 'vue'
import {useLogout} from "../composables/useLogout";
import {useUserStore} from '../stores/useUserStore.js';

const userStore = useUserStore();
const {logOut} = useLogout();
const currentUser = computed(() => userStore.user)

watchEffect(() => {
	console.log("currentUser changed: ", currentUser.value)
})
</script>

<template>
	<nav>

		<div class="nav-bar">
			<div v-if="currentUser && currentUser.name" class="user-display">
				<div class="logo-container">Innlogget bruker: {{ currentUser.email }}</div>
			</div>
			<p v-else><i class="fa-solid fa-lock"></i></p>


			<div v-if="currentUser && currentUser.email" class="nav-bar">
				<div v-if="currentUser && currentUser.email" @click="logOut">
					<i :title="currentUser.name" class="fa-solid fa-lock-open"></i>
				</div>

				<RouterLink to="/mainpage">
					<i class="fa-solid fa-house" title="Hovedside"></i>
				</RouterLink>

				<RouterLink to="/ads">
					<i class="fa-solid fa-rectangle-ad" title="Annonser"></i>
				</RouterLink>

				<RouterLink to="/users/profile">
					<i class="fa-solid fa-circle-user" title="Min profil"></i>
				</RouterLink>

				<RouterLink to="/ads/create-with-files">
					<i class="fa-solid fa-file-circle-plus" title="Lag ny annonse"></i>
				</RouterLink>

				<RouterLink to="/inbox">
					<i class="fa-solid fa-inbox" title="Innboks"></i>
				</RouterLink>

				<RouterLink to="/map">
					<i class="fa-solid fa-map-location-dot" title="Kart"></i>
				</RouterLink>
			</div>
		</div>
	</nav>
</template>

<style scoped>

nav {
	position: sticky;
	top: 0;
	z-index: 100;
	background: #000;
	backdrop-filter: blur(8px);
	border-radius: 0 0 16px 16px;
	padding: 16px 24px;
	box-shadow: 0 6px 16px rgba(0, 0, 0, 0.2);
	margin: 0 auto 20px;
	max-width: 90vw;
	display: flex;
	justify-content: space-between;
	align-items: center;
	color: #00ff66;
}


.user-display {
	font-size: 1rem;
	font-weight: 500;
	color: #00ff66;
	background: rgba(255, 255, 255, 0.1);
	padding: 8px 16px;
	border-radius: 12px;
	box-shadow: inset 0 0 4px rgba(255, 255, 255, 0.1);
	display: flex;
	align-items: center;
	gap: 8px;
	
}


.nav-bar {
	display: flex;
	gap: 2rem;
	align-items: center;
	
}

nav a {
	color: #00ff66;
	text-decoration: none;
	transition: transform 0.2s ease, color 0.2s ease;
}

i {
	font-size: 2rem;
	padding: 10px;
	transition: all 0.3s ease;
	border-radius: 50px;
}

i:hover {
	transform: scale(1.2);
	background-color: rgba(255, 255, 255, 0.2);
	animation: pulse 1.8s infinite ease-in-out;
}
@keyframes pulse {
	0%, 100% {
		box-shadow: 0 0 10px rgba(0, 255, 102, 0.5);
		border: 3px solid;
		background-color: #00ff66;
		color: darkgreen;
	}
	50% {
		box-shadow: 0 0 30px rgba(0, 255, 102, 0.9);
	}
}


.router-link-active i {
	background-color: rgba(255, 255, 255, 0.25);
}


.nav-bar > div:hover i.fa-lock-open {
	color: #ff4d4d;
}

</style>