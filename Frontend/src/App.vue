<script setup>

import NavBar from "./components/NavBar.vue";
import {RouterView} from 'vue-router'
import {onMounted} from "vue";
import {useUserStore} from "./stores/useUserStore.js";
import '@fortawesome/fontawesome-free/css/all.css';
import Footer from "./components/Footer.vue";

const store = useUserStore();

onMounted(() => {
	store.initializeUser();
	console.log("running initializeUser()...");
});

</script>

<template>
	<div>
		<!--    <div class="headerNav">-->
		<!--    <Header/>-->
		<!--    </div>-->
		<NavBar v-if="store.isLoggedIn"/>
		<RouterView :key="$route.fullPath"/>
		<Footer/>
	</div>
</template>


<style>
.headerNav {
	display: flex;
}

.body {
	display: flex;
	flex-direction: column;
	justify-content: center;
	align-items: center;
}
</style>
