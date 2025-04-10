<script setup>
import NavBar from "./components/NavBar.vue";
import { RouterView } from 'vue-router';
import { onMounted } from "vue";
import { useUserStore } from "./stores/useUserStore.js";
import '@fortawesome/fontawesome-free/css/all.css';
import Footer from "./components/Footer.vue";
import Header from "./components/Header.vue";
import AdBanner from "./components/AdBanner.vue";

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

		<div class="content-layout">
			<AdBanner class="ad-banner" />
			<main class="main-content">
				<RouterView :key="$route.fullPath" />
			</main>
			<AdBanner class="ad-banner" />
		</div>

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
	grid-template-columns: 320px 1fr 320px;
	gap: 1rem;
	padding: 2rem;
	background-color: #e2e8f0;
	border-radius: 0.4rem;
}

.ad-banner {
	position: sticky;
	top: 1rem;
	align-self: start;
	height: 500px;
}


.main-content {
	background-color: white;
	padding: 1rem;
	border-radius: 8px;
	box-shadow: 0 4px 6px rgba(0,0,0,0.1);
	width: 500px;
}

</style>
