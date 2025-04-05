<script setup>
import {computed, onMounted, ref} from 'vue'
import axios from 'axios'
import {useUserStore} from '../stores/useUserStore.js';
import {useRouter} from "vue-router";

const userStore = useUserStore();

const title = ref('')
const description = ref('')
const category = ref('')
const condition = ref('')
const price = ref(0)
const userId = computed(() => userStore.user.id || null);
const files = ref([]) // filer brukeren laster opp
const message = ref('')
const error = ref('')
const latitude = ref(null);
const longitude = ref(null);
const locationError = ref(null);
const location = ref(null);
const router = useRouter();


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

async function handleSubmit() {
	// lager formdata
	const formData = new FormData()
	formData.append('Title', title.value)
	formData.append('Description', description.value)
	formData.append('Category', category.value)
	formData.append('Condition', condition.value)
	formData.append('Price', price.value)
	formData.append('UserId', userId.value)
	formData.append('Latitude', latitude.value);
	formData.append('Longitude', longitude.value);
	formData.append('LocationName', `${location.value.city}, ${location.value.county}, ${location.value.country}`);
	console.log("formdata:", latitude.value, longitude.value)

	// legger til alle bildene(filene)
	for (let i = 0; i < files.value.length; i++) {
		formData.append('Files', files.value[i])
	}

	try {
		await axios.post('/api/ads/create-with-files', formData, {
			withCredentials: true
		})
		message.value = "Annonsen ble opprettet!"
		router.push({name: 'Profile'})

	} catch (err) {
		error.value = "Feil ved opplasting!"
	}
}

function handleFileChange(e) {
	files.value = e.target.files // oppdaterer files[] med valgte filer
}
</script>

<template>
	<div class="container">
		<h1>Lag en ny annonse</h1>
		<form @submit.prevent="handleSubmit">
			<div class="title">
				<label>Tittel<i class="fa-solid fa-scroll"></i></label>
				<br>
				<input v-model="title" type="text"/>
			</div>
			<div class="category">
				<label>Kategori<i class="fa-solid fa-tags"></i></label>
				<br>
				<select v-model="category">
					<option>Sykler</option>
					<option>Leker</option>
					<option>Electronikk</option>
					<option>Møbler</option>
					<option>Klær</option>
					<option>Skytevåpen</option>
					<option>Hånd-våpen</option>
				</select>

			</div>
			<div>
				<label>Pris<i class="fa-solid fa-coins"></i></label>
				<br>
				<input v-model="price" type="number"/>
				<br>
			</div>
			<div>
				<label>Beskrivelse<i class="fa-solid fa-pencil"></i></label>
				<br>
				<textarea v-model="description" placeholder="Beskriv varen..."></textarea>
			</div>
			<div>
				<label>Tilstand<i class="fa-solid fa-heart"></i></label>
				<br>
				<select v-model="condition">
					<option>Ny</option>
					<option>Nesten ny</option>
					<option>Brukt</option>
					<option>Godt brukt</option>
				</select>
			</div>
			<br>
			<div>
				<label>Bilder</label>
				<br>
				<label>
					<span>Legg til bilder<i class="fa-solid fa-circle-plus"></i></span>
					<input hidden="" multiple type="file" @change.prevent="handleFileChange"/>
				</label>
			</div>
			<button type="submit">Lag en ny annonse<i class="fa-solid fa-file-circle-plus"></i></button>
			<div>{{ error || message }}</div>
		</form>
	</div>
</template>

<style scoped>

.container {
	display: flex;
	flex-direction: column;
	align-items: center;
	justify-content: center;
	background-color: lightblue;
	width: 60vw;
	height: 80vh;
	padding: 30px;
	margin-top: 1vh;
	border-radius: 10px;
}

h1 {
	border-bottom: 5px solid black;
}

input {
	border: none;
	border-radius: 5px;
	max-width: 22vw;
	width: 22vw;
}

textarea {
	border: none;
	border-radius: 5px;
	max-width: 22vw;
	max-height: 15vh;
	width: 150vw;
	height: 150vh;
	text-align: center;
	align-items: center;
	justify-content: center;
	display: flex;
}

select {
	border: none;
	border-radius: 5px;
	max-width: 22vw;
	width: 22vw;

}

label {
	font-size: 1.2rem;
	font-weight: bolder;
	margin-top: 30px;
}

button {
	padding: 20px;
	margin: 10px;
	font-size: 1.2rem;
}

i {
	margin-left: 15px;
	font-size: 1.5rem;
}

</style>