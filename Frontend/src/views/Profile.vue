<script setup>
import {computed, onMounted, ref, watchEffect} from "vue";
import {useUser} from "../composables/useUser";
import {useRoute} from "vue-router";
import {useAdStore} from "../stores/adStore.js";
import MyProfile from "../components/MyProfile.vue";
import OtherProfile from "../components/OtherProfile.vue";
import {useUserStore} from '../stores/useUserStore.js';

const userStore = useUserStore();
const adStore = useAdStore();
const route = useRoute();
const userId = ref(route.params.id || null);
const {user, fetchUserProfile} = useUser();
const currentUser = computed(() => userStore.user);

const isOwnProfile = computed(() => {
	return !userId.value || Number(userId.value) === currentUser.value?.id;
});

onMounted(async () => {
	await fetchUserProfile(userId.value);
	await adStore.fetchAds();
});

const filteredAds = computed(() => {
	const routeId = userId.value;
	return adStore.getAdsByUser(routeId || currentUser.value?.id);
});

</script>

<template>

	<div v-if="isOwnProfile">
		<MyProfile :ads="filteredAds" :user="currentUser"/>
	</div>
	<div v-else>
		<OtherProfile :ads="filteredAds" :user="user"/>
	</div>

</template>

<style scoped>
img {
	width: 20%;
	height: 20%;
	border-radius: 5px;
	border: solid 1px white;
}
</style>