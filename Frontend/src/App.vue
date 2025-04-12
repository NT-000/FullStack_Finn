<script setup>
import NavBar from "./components/NavBar.vue";
import { RouterView } from 'vue-router';
import { onMounted } from "vue";
import { useUserStore } from "./stores/useUserStore.js";
import '@fortawesome/fontawesome-free/css/all.css';
import Footer from "./components/Footer.vue";
import Header from "./components/Header.vue";
import AdBanner from "./components/AdBanner.vue";
import CrtPanel from "@/components/CrtPanel.vue";

const store = useUserStore();

onMounted(() => {
	store.initializeUser();
	console.log("running initializeUser()...");
});
</script>

<template>
	<div class="app-layout">
		<Header />
		<NavBar v-if="store.isLoggedIn" />

	<CrtPanel>
		<div :class="['content-layout', { active: !store.isLoggedIn  }]">
			<AdBanner v-if="store.user" :class="['ad-banner',{ active: !store.isLoggedIn}]"/>
			<main :class="['main-content',{ active: !store.isLoggedIn }]">
				<RouterView :key="$route.fullPath" />
			</main>
			<AdBanner v-if="store.user" :class="['ad-banner',{ active: !store.isLoggedIn}]"/>
		</div>
	</CrtPanel>

		<Footer />
	</div>
</template>

<style scoped>
.app-layout {
	display: flex;
	flex-direction: column;
	min-height: 400px;
	
}

.content-layout {
	flex: 1;
	display: grid;
	grid-template-columns: 370px 1fr 370px;
	gap: 1rem;
	padding: 3rem;
	background-color: black;
	border-radius: 0.4rem;
	font-family: 'Overseer', 'Press Start 2P', monospace;
}

.content-layout.active{
	display: flex;
	flex-direction: column;
	justify-content: center;
	align-items: center;
	background-color: transparent;
}

.ad-banner {
	position: sticky;
	align-self: start;
	height: 100%;
}

.main-content {
	background-color: black;
	padding: 1rem;
	border-radius: 8px;
	box-shadow: 0 4px 6px rgba(0,0,0,0.1);
	width: 600px;
}

.main-content.active {
	display: flex;
	justify-content: center;
	align-items: center;
	background-color: transparent;
	border: none;
	box-shadow: none;
}

</style>
