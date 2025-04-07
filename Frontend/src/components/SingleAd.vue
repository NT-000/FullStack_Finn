<script setup>
import {computed, onMounted, onUnmounted, ref} from 'vue';
import {RouterLink, useRoute, useRouter} from 'vue-router';
import {useUserStore} from "../stores/useUserStore.js";
import {useAdStore} from "../stores/adStore.js";
import {useDateFormat} from "../composables/useFormatDate.js";
import {useFavStore} from "../stores/favStore.js";
import L from 'leaflet'
import 'leaflet/dist/leaflet.css'
import {getRoute} from "../composables/getRoute.js";
import iconRetinaUrl from 'leaflet/dist/images/marker-icon-2x.png'
import iconUrl from 'leaflet/dist/images/marker-icon.png'
import shadowUrl from 'leaflet/dist/images/marker-shadow.png'
import axios from "axios";
import CreateReview from "./CreateReview.vue";
import {useStarRating} from "../composables/useStarRating.js";

const route = useRoute();
const adId = Number(route.params.id);
const users = getRoute('/users');
const reviews = getRoute(`/reviews/ad/${adId}`);
const router = useRouter();
const dateFormat = useDateFormat();
const adStore = useAdStore();
const userStore = useUserStore();
const favStore = useFavStore();
const starRating = useStarRating();

const isUpdating = ref(false)
const selectedBuyerId = ref(null)
const currentImageIndex = ref(0);
console.log('route id', route.params.id)


L.Marker.prototype.options.icon = L.icon({
	iconRetinaUrl,
	iconUrl,
	shadowUrl,
	iconSize: [25, 41],
	iconAnchor: [12, 41]
})
const map = ref(null)
const mapContainer = ref(null)
const marker = ref(null)
const lat = ref(null)
const lng = ref(null)
const error = ref(null)


const currentAd = computed(() => {
	return adStore.ads.find(ad => ad.id === adId);
});


//sjekker om innlogget bruker
const isOwner = computed(() => {
	return userStore.user.id === currentAd.value?.userId;
});

const adReview = computed(() => {
	return reviews.items.value.find(review => review.adId === currentAd.value.id)
})
const reviewer = computed(() => {
	return users.items.value.find(user => user.id === adReview.value?.fromUserId)
})


console.log('adReview:', adReview)
const deleteAd = async () => {
	if (confirm("Slette annonsen?")) {
		await adStore.deleteAd(adId);
		router.push("/ads");
	}
}
const markAdAsSold = async () => {
	if (!selectedBuyerId.value) {
		error.value = "Velg en kjøper først"
		return;
	}
	await adStore.markAsSold(currentAd.value.id, selectedBuyerId.value);
	error.value = "Annonsen markert som solgt!"
};

onMounted(async () => {
	await adStore.fetchAds();
	await reviews.fetchData();
	console.log("All reviews:", reviews.items.value);
	console.log("Current ad id:", currentAd.value?.id);
	console.log("Current user id:", userStore.user.id);
	await users.fetchData();
	await adStore.getInterestedUsers(adId);
	console.log("buyer:", reviewer)
	await favStore.fetchFavorites(adId);


	map.value = L.map(mapContainer.value)
	map.value.on('click', onMapClick)

	L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
		maxZoom: 19,
		attribution: '© OpenStreetMap'
	}).addTo(map.value)

	if (currentAd.value && currentAd.value.latitude && currentAd.value.longitude) {
		map.value.setView([currentAd.value.latitude, currentAd.value.longitude], 12)
		marker.value = L.marker([currentAd.value.latitude, currentAd.value.longitude])
				.addTo(map.value)
				.bindPopup(`
    <h3>${currentAd.value.title ?? ''}</h3>`)

	} else {
		map.value.setView([59.91, 10.74], 6) // fallback-posisjon
	}

})

onUnmounted(() => {
	if (map.value) {
		map.value.remove()
	}
})

const onMapClick = async (e) => {
	if (!isOwner.value) {
		return
	}

	//plasserer en ny markør på kartet
	if (isUpdating.value) {
		if (marker.value) {
			marker.value.remove()
		}
		lat.value = e.latlng.lat
		lng.value = e.latlng.lng

		marker.value = L.marker([lat.value, lng.value]).addTo(map.value)

		const locationData = await reverseGeocode(lat.value, lng.value)

		const locationName = locationData
				? `${locationData.city}, ${locationData.county}, ${locationData.country}`
				: 'Ukjent sted'

		updateCoordinates(adId, lat.value, lng.value, locationName)
	}
}

async function reverseGeocode(latValue, longValue) {
	try {
		const url = `https://nominatim.openstreetmap.org/reverse?lat=${latValue}&lon=${longValue}&format=json`
		const res = await axios.get(url, {withCredentials: false})
		const {address} = res.data
		console.log("res.data reverseCode", res.data)
		const city = address.city || address.town || address.village || ''
		const county = address.county || ''
		const country = address.country || ''
		console.log('reverse geocode result:', "address city", address.city, "nation:", address.country)
		return {city, county, country}
	} catch (err) {
		console.error('reverse geocoding failed:', err)
		return null
	}
}

const updateCoordinates = async (adId, lat, lng, location) => {
	error.value = null
	try {
		const res = await axios.put(`/api/ads/${adId}/location`, {
			latitude: lat,
			longitude: lng,
			locationName: location
		}, {withCredentials: true})
		console.log("position updated", res.data)

	} catch (err) {
		console.log("error when updating position", err)
		error.value = 'Location could not be updated'
	}
}
const updateAd = async () => {
	if (!currentAd.value) {
		console.error("currentAd er ikke lastet enda!");
		return;
	}
	const adData = {
		Title: currentAd.value.title,
		Category: currentAd.value.category,
		Description: currentAd.value.description,
		Condition: currentAd.value.condition,
		Price: currentAd.value.price
	};
	await adStore.updateAd(adId, adData);
	isUpdating.value = false;
}
const seller = computed(() => {
	return users.items.value.find(user => user.id === currentAd.value?.userId)
})

const handleClickImg = (arrow) => {
	if (!currentAd.value || !currentAd.value.images.length) return;

	const totalImages = currentAd.value.images.length;

	if (arrow === 'right') {
		currentImageIndex.value = (currentImageIndex.value + 1) % totalImages;
	} else if (arrow === 'left') {
		currentImageIndex.value = (currentImageIndex.value - 1 + totalImages) % totalImages;
	}

};

</script>

<template>
	<div class="outerContainer">
		<div>
			<div class="user">
				<RouterLink v-if="seller && userStore.$state.user.id !== seller.id"
				            :to="{name: 'UserProfile', params:{id:seller.id}}"><h3 v-if="seller">{{ seller.name }}
					<img :src="seller.profileImageUrl"></h3>
				</RouterLink>
			</div>
			<div>
				<RouterLink v-if="seller && userStore.$state.user.id === seller.id" :to="{name: 'Profile'}"><h3
						v-if="seller"><img :src="seller.profileImageUrl">{{ seller.name }}</h3>
				</RouterLink>
			</div>
			<div class="userMap">
				<div v-if="seller && userStore.$state.user.id !== seller.id && !currentAd.isSold">
					<RouterLink :to="{name:'Chat', params:{id:seller.id}, query:{adId:currentAd.id}}"
					            class="messageRL"><i
							class="fa-solid fa-envelope">Send melding</i>
					</RouterLink>
				</div>
			</div>
		</div>
		<div class="container">

			<div class="info">

				<div v-if="currentAd && adId">
					<form v-if="currentAd && seller" @submit.prevent="updateAd">
						<div class="main-display">
							<div class="title"><h3>{{ currentAd.title }}</h3></div>
							<div v-if="currentAd && currentAd.isSold">
								<div class="sold">SOLGT</div>
							</div>
							<div v-if="currentAd?.images && currentAd.images.length > 0" class="image-box">
								<img :src="currentAd.images[currentImageIndex].imageUrl" alt="" class="mainImage"/>
								<div v-if="currentAd && favStore && !isOwner && !currentAd.isSold" class="favorite">
									<div @click="favStore.toggleFav(currentAd.id)">
										<i :class="favStore.isFavorite(currentAd.id) ? 'fa-solid fa-beat': 'fa-regular'" class="fa-heart"
										   style="color: red; cursor: pointer;"></i>
									</div>
								</div>
								<div v-if="currentAd.images.length > 1" class="arrow-left" @click="handleClickImg('left')"><i
										class="fa-solid fa-angles-left"></i></div>
								<div v-if="currentAd.images.length > 1" class="arrow-right" @click="handleClickImg('right')"><i
										class="fa-solid fa-angles-right"></i></div>
								<div v-if="currentAd?.images && currentAd.images.length > 1" class="imagesReel">
									<div v-for="(image, index) in currentAd.images" :key="image.id" class="img">
										<img :src="image.imageUrl" alt="Ad image" @click="currentImageIndex = index"/>
									</div>
									<div>
									</div>
									<br>

								</div>
							</div>
							<div class="map">
								<div ref="mapContainer" style="height: 250px; width: 400px; border-radius: 10px;"></div>
							</div>
							<label>Sted:</label>
							<div>{{ currentAd.locationName }}</div>
							<br>
							<label v-if="isUpdating">Tittel</label>
							<input v-if="isUpdating" v-model="currentAd.title" placeholder="New title" type="text"/>

						</div>
						<br>
						<div class="category">
							<label>Kategori:</label>
							<div v-if="!isUpdating">{{ currentAd.category }}</div>
							<select v-if="isUpdating" v-model="currentAd.category">
								<option>Bøker</option>
								<option>Elektronikk</option>
								<option>Annet</option>
								<option>Klesplagg</option>
								<option>Våpen</option>
								<option>Leker</option>
								<option>Instrumenter</option>
								<option>Bolig</option>
								<option>Verktøy</option>
								<option>Næring</option>
							</select>
						</div>
						<br>
						<div class="price">
							<label>Pris:</label>
							<div v-if="!isUpdating">{{ currentAd.price }} kr</div>
							<input v-if="isUpdating" v-model="currentAd.price" placeholder="new price" type="number"/>
						</div>
						<br>
						<div class="condition">
							<label>Tilstand: </label>
							<div v-if="!isUpdating">{{ currentAd.condition }}</div>
							<select v-if="isUpdating" v-model="currentAd.condition">
								<option>Ny</option>
								<option>Nesten ny</option>
								<option>Brukt</option>
								<option>Godt brukt</option>
							</select>
						</div>
						<br>
						<label>Beskrivelse:</label>
						<div class="info">{{ currentAd.description }}</div>
						<textarea v-if="isUpdating" v-model="currentAd.description" placeholder="New description"/>
						<br>

					</form>
					<div v-if="isOwner && adStore.interestedUsers.length > 0" class="info">
						<h3 v-if="reviewer">Kjøper: {{ reviewer.name }}</h3>
						<select v-if="!currentAd.isSold && userStore.user.id" v-model="selectedBuyerId">
							<option disabled value="">Velg kjøper</option>
							<option v-for="user in adStore.interestedUsers.filter(u => u.id !== userStore.user.id)" :key="user.id"
							        :value="user.id">
								{{ user.name }}
							</option>
						</select>
						<div v-if="reviewer">{{ error }}</div>
						<button v-if="!currentAd.isSold" @click="markAdAsSold">Selg til</button>
					</div>
				</div>
				<div v-if="isOwner && !currentAd.isSold">
					<button @click="deleteAd">Slett annonse</button>
					<button @click="isUpdating = !isUpdating">
						<span v-if="!isUpdating"> Endre annonse </span>
						<span v-if="isUpdating" @click="updateAd"> Lagre annonse </span>
					</button>
				</div>
				<div v-if="adReview && reviewer" class="adReview">
					<RouterLink :to="{name:'UserProfile', params:{id:reviewer.id}}">
						<div v-if="reviewer"><img :src="reviewer.profileImageUrl">{{ reviewer.name }}</div>
					</RouterLink>
					<div>{{ adReview.comment }}</div>
					<div v-html="starRating.getStars(adReview.rating)"></div>
					<div>{{ dateFormat.formatDate(adReview.createdAt) }}</div>
				</div>
			</div>
			<div class="createReview">
				<CreateReview
						v-if="currentAd && currentAd.isSold && userStore.$state.user.id === currentAd.buyerId && !adReview"
						:currentAd="currentAd" class="innerReview"/>
			</div>
		</div>
	</div>
</template>

<style scoped>

.outerContainer {

}

.container {
	height: 200px;
	width: 100%;
	display: flex;
	justify-content: center;
	padding: 20px;
	border-radius: 15px;
}

.main-display {

}

a {
	color: white;
}

.img :hover {
	border: 5px solid #4a90e2;
	opacity: 1.7;
	transition: 0.3s;
	transform: scale(1.2);
}

.arrow-left {
	position: absolute;
	top: 25vh;
	left: -5vw;

}

.arrow-right {
	position: absolute;
	top: 25vh;
	right: -5vw;
}

.image-box {
	display: inline-block;
	position: relative;

}

.favorite {
	position: absolute;
	top: 15px;
	left: 15px;
}

.title {
	background-color: lightskyblue;
	border-radius: 10px;
	padding: 10px;
	margin-bottom: 10px;
	box-shadow: 0 4px 8px rgba(0, 0, 0, 3);
	text-shadow: 2px 2px 4px rgba(0, 0, 0, 0.6);
}

h1 {
	font-family: "Shadows Into Light Two", cursive;
	font-weight: bolder;
	font-style: italic;
	padding: 10px;
	text-shadow: 2px 2px 4px rgba(0, 0, 0, 0.6);
}

.fa-heart {
	font-size: 100px;

}

.user {
	position: relative;
	display: inline-flex;
	bottom: 0px;
	right: 0px;
}

.userMap {
	display: flex;
	position: relative;
	flex-direction: column;
	bottom: 0px;
}

.mainImage {
	height: 35vh;
	width: auto;
	background-color: white;
	border-radius: 10px;
	border: 25px solid #4a90e2;
}

.imagesReel {
	display: flex;
	gap: 20px;
	flex-wrap: wrap;
	justify-content: center;
	padding: 10px;

}

.imageReel img {
	height: 100px;
	width: 100px;
	border-radius: 20px;
}

.imageReel img:hover {
	transform: scale(1.2);
}

label {
	font-weight: bold;
	font-size: 1rem;
	display: flex;
	justify-content: center;

}

.category {
	font-weight: bold;
	font-size: 1rem;
	display: flex;
	justify-content: center;
}

.price {
	font-weight: bold;
	font-size: 1rem;
	display: flex;
	justify-content: center;
}

.info {
	font-style: italic;
}

img {
	height: 100px;
	width: 100px;
	border: 2px solid black;
	align-items: center;
	transition: transform 0.3s ease;
	border-radius: 10px;
}

h3 img {
	width: 50px;
	height: 50px;
	border-radius: 30px;
	border: 1px solid blue;
}

.sold {
	max-width: 100%;
	text-align: center;
	font-size: 2rem;
	padding: 10px;
	background-color: black;
	color: yellow;
	border-radius: 10px;
}

.map {
	font-weight: bold;
	font-size: 2rem;
	display: flex;
	justify-content: center;
	padding: 1rem;
}

.messageRL {
	color: white;
	font-size: 0.5rem;
}

.messageRL:hover {
	color: white;
}

body {
	background: deepskyblue;
	margin: 0;
	font-family: Arial, sans-serif;
	color: #333;
}

i {
	padding: 10px;
	font-size: 1rem;
	color: black;
}

i:hover {
	color: white;
	transition: 0.3s;
	transform: scale(1.5);
}

button:hover {
	background-color: #004080;
	color: white;
	transition: all 0.3s ease;
}

input {
	padding: 5px;
	border-radius: 5px;
}

textarea {
	padding: 10px;
	border-radius: 5px;
	min-height: 200px;
	min-width: 60vw;
	text-align: center;

}

.adReview {
	border: 5px solid #98cbe8;
	display: inline-block;
	font-family: "Comic Sans MS", cursive;
	padding: 10px;
	border-radius: 10px;
}

.adReview img {
	border-radius: 10rem;
	height: 2rem;
	width: 2rem;
}

.createReview {
	position: relative;
}
</style>