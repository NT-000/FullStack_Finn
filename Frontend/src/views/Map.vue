<script setup>
import {onMounted, ref, watchEffect} from "vue";
import L from 'leaflet'
import 'leaflet/dist/leaflet.css'
import iconRetinaUrl from 'leaflet/dist/images/marker-icon-2x.png'
import iconUrl from 'leaflet/dist/images/marker-icon.png'
import shadowUrl from 'leaflet/dist/images/marker-shadow.png'
import {useAdStore} from "../stores/adStore.js";

L.Marker.prototype.options.icon = L.icon({
	iconRetinaUrl,
	iconUrl,
	shadowUrl,
	iconSize: [25, 41],
	iconAnchor: [12, 41],
	popupAnchor: [1, -34],
	tooltipAnchor: [16, -28]
})

const adStore = useAdStore()
const mapContainer = ref(null)
const map = ref(null)
const categoryMarkers = ref({});
const activeCategories = ref([])


onMounted(async () => {
	// oppretter selve kartet
	// start-koordinater er satt til Oslo.
	await adStore.fetchAds();
	map.value = L.map(mapContainer.value).setView([59.91, 10.74], 10)

	// layoyt
	L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
		maxZoom: 19,
		attribution: '© OpenStreetMap'
	}).addTo(map.value)

})

const handleCat = (cat) => {
	if (categoryMarkers.value[cat]) {
		categoryMarkers.value[cat].forEach(marker => marker.remove());
		delete categoryMarkers.value[cat];
		activeCategories.value = activeCategories.value.filter(category => category !== cat);
	} else {
		const markers = [];
		for (const ad of adStore.ads) {
			if (ad.latitude && ad.longitude && ad.category === cat) {
				const marker = L.marker([ad.latitude, ad.longitude])
						.addTo(map.value)
						.bindPopup(`${ad.title} - ${ad.locationName}`);
				markers.push(marker);
			}
		}
		categoryMarkers.value[cat] = markers;
		activeCategories.value.push(cat);
	}
};

watchEffect(async () => {

})

</script>

<template>
	<div class="container">
		<h1>Alle annonser</h1>
		<h3>
			<button :class="{active: activeCategories.includes('Sykler')}" @click="handleCat('Sykler')">Sykler</button>
			<button :class="{active: activeCategories.includes('Elektronikk')}" @click="handleCat('Elektronikk')">
				Elektronikk
			</button>
			<button :class="{active: activeCategories.includes('Møbler')}" @click="handleCat('Møbler')">Møbler</button>
			<button :class="{active: activeCategories.includes('Klær')}" @click="handleCat('Klær')">Klær</button>
			<button :class="{active: activeCategories.includes('Våpen')}" @click="handleCat('Våpen')">Våpen</button>
			<button :class="{active: activeCategories.includes('Hånd-våpen')}" @click="handleCat('Hånd-våpen')">Hånd-våpen
			</button>
			<button :class="{active: activeCategories.includes('Treningsutstyr')}" @click="handleCat('Treningsutstyr')">
				Treningsutstyr
			</button>
		</h3>
		<div ref="mapContainer" style="width:45vw; border-radius: 10px; height:45vh;">
		</div>
	</div>
</template>

<style scoped>
button {
	display: inline-flex;
	justify-content: center;
	align-items: center;
	gap: 1rem;
	margin: 0.3rem;
	padding: 0.5rem 1rem;
	border: 2px solid transparent;
	transition: transform 0.2s, border 0.2s, background-color 0.2s;
	border-radius: 10px;
}

button:hover {
	transform: scale(1.1);
}

button.active {
	border: 2px solid #004080;
	background-color: #4a90e2;
	color: white;
}

h1 {
	font-family: "Comic Sans MS", cursive;
	font-size: 6rem;
}

h3 {
	background-color: lightskyblue;
	padding: 20px;
	text-align: center;
	border-radius: 10px;
	display: flex;
	justify-content: space-evenly;
}
</style>