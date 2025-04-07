<script setup>
import {useAdStore} from "../stores/adStore.js";
import {useUserStore} from "../stores/useUserStore.js";
import {computed, onMounted, ref} from "vue";
import {useRouter} from "vue-router";


const adStore = useAdStore();
const userStore = useUserStore();
const interestedPerAd = ref({});
const selectedBuyerIds = ref({});
const selectedChatUsers = ref({});
const router = useRouter()
const error = ref(null)

const goToChat = (userId, adId) => {
	router.push({name: 'Chat', params: {id: userId}, query: {adId}})
}

const findOwnedAds = computed(() => {
	return adStore.ads.filter(ad => ad.userId == userStore.user.id)
})

const markAdAsSold = async (adId) => {
	const buyerId = selectedBuyerIds.value[adId];
	if (!buyerId) {
		error.value = "Velg en kjøper først";
		return;
	}
	try {
		await adStore.markAsSold(adId, buyerId);
		error.value = "Annonsen markert som solgt!";
		await adStore.fetchAds();
	} catch (err) {
		console.log(err.message);
		error.value = "Noe gikk galt under lagring.";
	}
};

onMounted(async () => {
	await adStore.fetchAds()
	console.log("find owned ads:", findOwnedAds.value)

	console.log("interestedPerAd:", interestedPerAd.value);

	for (const ad of findOwnedAds.value) {
		interestedPerAd.value[ad.id] = await adStore.getInterestedUsers(ad.id)
	}

})
</script>

<template>
	<div class="inbox-wrapper">
		<h1>Innboks</h1>

		<div v-if="findOwnedAds.length > 0" class="inbox-grid">
			<div v-for="ad in findOwnedAds" :key="ad.id" class="inbox-card">
				<h2>{{ ad.title }}</h2>

				<img v-if="ad.images?.[0]" :src="ad.images[0].imageUrl" alt="annonsebilde"/>

				<p>{{ ad.description }}</p>

				<div v-if="ad.isSold" class="sold-label">Solgt</div>

				<div v-if="!ad.isSold && interestedPerAd[ad.id]?.length > 0">
					<label for="buyer">Velg kjøper:</label>
					<select v-model="selectedBuyerIds[ad.id]">
						<option disabled value="">Velg kjøper</option>
						<option
								v-for="buyer in interestedPerAd[ad.id].filter(u => u.id !== userStore.user.id)"
								:key="buyer.id"
								:value="buyer.id"
						>
							{{ buyer.name }}
						</option>
					</select>
					<button @click="markAdAsSold(ad.id)">Selg til</button>
				</div>

				<div v-if="interestedPerAd[ad.id]?.length > 0">
					<label for="chat">Åpne chat med:</label>
					<select v-model="selectedChatUsers[ad.id]">
						<option disabled value="">Velg bruker</option>
						<option
								v-for="user in interestedPerAd[ad.id].filter(u => u.id !== userStore.user.id)"
								:key="user.id"
								:value="user.id"
						>
							{{ user.name }}
						</option>
					</select>
					<button @click="goToChat(selectedChatUsers[ad.id], ad.id)">Åpne chat</button>
				</div>

				<p v-else-if="!ad.isSold">Ingen interesserte enda</p>

				<p>{{ error }}</p>
			</div>
		</div>

		<div v-else class="no-ads">Du har ingen aktive annonser.</div>
	</div>
</template>


<style scoped>

.inbox-wrapper {
	max-width: 1000px;
	margin: 0 auto;
	padding: 20px;
}

h1 {
	border-bottom: 4px solid #007bff;
	margin-bottom: 30px;
	color: #00263b;
}

.inbox-grid {
	display: flex;
	flex-direction: column;
	gap: 25px;
}

.inbox-card {
	background: linear-gradient(145deg, #d0e9ff, #f0faff);
	border-radius: 16px;
	box-shadow: 0 6px 25px rgba(0, 0, 0, 0.1);
	padding: 20px;
	transition: 0.2s;
}

.inbox-card:hover {
	transform: translateY(-3px);
	box-shadow: 0 10px 30px rgba(0, 0, 0, 0.15);
}

h2 {
	color: white;
	background-color: #007bff;
	border-radius: 10px;
	padding: 10px 15px;
	font-size: 1.5rem;
	box-shadow: 0 4px 10px rgba(0, 0, 0, 0.2);
	margin-bottom: 10px;
}

img {
	max-width: 100%;
	height: auto;
	border-radius: 12px;
	border: 3px solid #007bff;
	margin-bottom: 10px;
}

p {
	background-color: rgba(255, 255, 255, 0.8);
	padding: 10px;
	border-radius: 8px;
}

.no-interest {
	color: #555;
	font-style: italic;
	margin-top: 5px;
}

.sold-label {
	font-family: "Comic Sans MS", cursive;
	color: red;
	font-size: 1.8rem;
	background-color: rgba(0, 0, 0, 0.8);
	padding: 8px 16px;
	border-radius: 10px;
	width: fit-content;
	margin-top: 10px;
}

select {
	width: 100%;
	padding: 10px;
	font-size: 1rem;
	font-weight: bold;
	border-radius: 10px;
	background-color: #f0faff;
	margin-top: 10px;
	box-shadow: 0 3px 10px rgba(0, 0, 0, 0.1);
}

</style>