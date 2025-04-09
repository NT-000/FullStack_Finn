<script setup>
import {onMounted, ref} from "vue";
import L from 'leaflet'
import 'leaflet/dist/leaflet.css'
import {useAdStore} from "../stores/adStore.js";

import {useCategories} from '../composables/useCategories.js'

const {categories} = useCategories();

const adStore = useAdStore()
const mapContainer = ref(null)
const map = ref(null)
const categoryMarkers = ref({});
const activeCategories = ref([])


onMounted(async () => {
	// oppretter selve kartet
	// start-koordinater er satt til Oslo.
	await adStore.fetchAds();
	map.value = L.map(mapContainer.value, {
		zoomAnimation: false
	}).setView([59.91, 10.74], 8)


	// layoyt
	L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
		maxZoom: 19,
		attribution: '© OpenStreetMap'
	}).addTo(map.value)

})

const handleCat = (cat) => {
	if (categoryMarkers.value[cat]) {
		categoryMarkers.value[cat].forEach(marker => marker.remove()); // kunne brukt L.layerGroup
		delete categoryMarkers.value[cat];
		activeCategories.value = activeCategories.value.filter(category => category !== cat);
	} else {
		const markers = [];
		for (const ad of adStore.ads) {
			if (ad.latitude && ad.longitude && ad.category === cat) {
				const marker = L.marker([ad.latitude, ad.longitude])
						.addTo(map.value)
						.bindPopup(`<img src="${ad.images.length ? ad.images[0].imageUrl : ""}" style="width: 100px"> <div class="ad-title">${ad.title} - ${ad.locationName}</div>`)

				markers.push(marker);
			}
		}
		categoryMarkers.value[cat] = markers;
		console.log("active cats", activeCategories.value);
		activeCategories.value.push(cat);
	}
};


</script>

<template>
	<div class="container">
		<h1>Kart over annonser</h1>

		<div class="buttons">
			<button
					v-for="cat in categories"
					:key="cat"
					:class="{ active: activeCategories.includes(cat) }"
					@click="handleCat(cat)"
			>
				{{ cat }}
			</button>

		</div>

		<div ref="mapContainer" class="mapbox"></div>
	</div>
</template>


<style scoped>
.container {
	display: flex;
	flex-direction: column;
	align-items: center;
	padding: 30px;
}

h1 {
	font-family: "Comic Sans MS", cursive;
	font-size: 3rem;
	margin-bottom: 20px;
	text-align: center;
	color: #00263b;
}

.mapbox {
	width: 90vw;
	height: 60vh;
	border-radius: 20px;
	box-shadow: 0 6px 18px rgba(0, 0, 0, 0.2);
	margin-top: 20px;
}

.buttons {
	display: flex;
	flex-wrap: wrap;
	justify-content: center;
	gap: 10px;
	margin-bottom: 20px;
}

button {
	padding: 10px 20px;
	font-size: 1rem;
	font-weight: bold;
	border-radius: 10px;
	cursor: pointer;
	border: none;
	background-color: #d4e8f4;
	transition: all 0.2s ease;
}

button:hover {
	transform: scale(1.05);
	background-color: #b6def5;
}

button.active {
	background-color: #4a90e2;
	color: white;
	box-shadow: 0 0 10px rgba(0, 0, 0, 0.15);
}

button.clear {
	background-color: crimson;
	color: white;
}

.ad-title {
	font-family: "Comic Sans MS", cursive;
}

</style>