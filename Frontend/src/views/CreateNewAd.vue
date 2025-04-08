<script setup>
import {computed, onMounted, ref} from 'vue';
import axios from 'axios';
import {useUserStore} from '../stores/useUserStore.js';
import {useRouter} from 'vue-router';

import { useCategories } from '../composables/useCategories.js'

const {categories} = useCategories();

const userStore = useUserStore();
const router = useRouter();

const title = ref('');
const description = ref('');
const category = ref('');
const condition = ref('');
const price = ref(0);
const files = ref([]);
const message = ref('');
const error = ref('');

const latitude = ref(null);
const longitude = ref(null);
const locationError = ref(null);
const location = ref(null);

const userId = computed(() => userStore.user.id || null);

onMounted(() => {
	if (navigator.geolocation) {
		navigator.geolocation.getCurrentPosition(async (pos) => {
			latitude.value = pos.coords.latitude;
			longitude.value = pos.coords.longitude;
			location.value = await reverseGeocode(latitude.value, longitude.value);
		}, (err) => {
			locationError.value = err.message;
		});
	} else {
		locationError.value = 'Fant ikke posisjonen din.';
	}
});

async function reverseGeocode(lat, lon) {
	try {
		const url = `https://nominatim.openstreetmap.org/reverse?lat=${lat}&lon=${lon}&format=json`;
		const res = await axios.get(url, {withCredentials: false});
		const address = res.data.address;
		const city = address.city || address.town || address.village || '';
		const county = address.county || '';
		const country = address.country || '';
		return {city, county, country};
	} catch (err) {
		console.error('reverse geocoding failed:', err);
		return null;
	}
}

function handleFileChange(e) {
	files.value = e.target.files;
}

async function handleSubmit() {
	const formData = new FormData();
	formData.append('Title', title.value);
	formData.append('Description', description.value);
	formData.append('Category', category.value);
	formData.append('Condition', condition.value);
	formData.append('Price', price.value);
	formData.append('UserId', userId.value);
	formData.append('Latitude', latitude.value);
	formData.append('Longitude', longitude.value);
	formData.append('LocationName', `${location.value.city}, ${location.value.county}, ${location.value.country}`);

	for (let i = 0; i < files.value.length; i++) {
		formData.append('Files', files.value[i])
	}

	try {
		await axios.post('/api/ads/create-with-files', formData, {
			withCredentials: true
		});
		message.value = "Annonsen ble opprettet!";
		router.push({name: 'Profile'});
	} catch (err) {
		error.value = "Feil ved opplasting!";
	}
}
</script>

<template>
	<div class="ad-container">
		<h1>Ny annonse</h1>
		<form class="ad-form" @submit.prevent="handleSubmit">
			<label>Tittel</label>
			<input v-model="title" placeholder="F.eks. Glock med diamanter" required/>

			<label>Kategori</label>
			<select v-model="category" required>
				<option disabled value="">Velg kategori</option>
				<option v-for="cat in categories">{{cat}}</option>
			</select>

			<label>Pris (kr)</label>
			<input v-model="price" required type="number"/>

			<label>Beskrivelse</label>
			<textarea v-model="description" placeholder="Beskriv produktet..." required></textarea>

			<label>Tilstand</label>
			<select v-model="condition" required>
				<option disabled value="">Velg tilstand</option>
				<option>Ny</option>
				<option>Nesten ny</option>
				<option>Brukt</option>
				<option>Godt brukt</option>
			</select>

			<label>Bilder</label>
			<input multiple type="file" @change="handleFileChange"/>
			<button type="submit">Opprett annonse</button>
			<p v-if="message" class="success">{{ message }}</p>
			<p v-if="error" class="error">{{ error }}</p>
		</form>
	</div>
</template>

<style scoped>
.ad-container {
	max-width: 600px;
	margin: 40px auto;
	padding: 30px;
	background: rgba(255, 255, 255, 0.15);
	backdrop-filter: blur(10px);
	border-radius: 20px;
	box-shadow: 0 8px 20px rgba(0, 0, 0, 0.2);
	color: #00263b;
	font-family: 'Segoe UI', sans-serif;
}

h1 {
	text-align: center;
	margin-bottom: 20px;
}

.ad-form {
	display: flex;
	flex-direction: column;
	gap: 15px;
}

label {
	font-weight: bold;
}

input,
select,
textarea {
	padding: 10px;
	border-radius: 8px;
	border: none;
	font-size: 1rem;
}

input[type="file"] {
	background: white;
}

textarea {
	min-height: 100px;
	resize: vertical;
}

button {
	background-color: #007bff;
	color: white;
	padding: 12px;
	font-weight: bold;
	border: none;
	border-radius: 8px;
	cursor: pointer;
	transition: background-color 0.3s ease;
}

button:hover {
	background-color: #0056b3;
}

.success {
	color: green;
	font-weight: bold;
}

.error {
	color: red;
	font-weight: bold;
}
</style>
