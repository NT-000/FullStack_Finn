<script setup>

import {RouterLink} from "vue-router";
import {computed, onMounted, ref} from "vue";
import {getRoute} from "../composables/getRoute.js";
import {useCategories} from '../composables/useCategories.js'

const {categories} = useCategories();

//Henter både brukere og annonser for søkefunksjon
const {items: users, fetchData: fetchUsers} = getRoute('/users');

const {items: ads, fetchData: fetchAds} = getRoute('/ads');

const search = ref('');
const category = ref('')
const isAd = ref(true);
const isPerson = ref(false)
const isOpen = ref(false)
const price = ref(0)
const minPrice = ref(0)
const maxPrice = ref(100000)
const showCount = ref(10)


const filteredUsers = computed(() => {
	if (!isPerson.value && search.value.length > 0) return [];
	return users.value.filter(user => {
		const username = user.name?.toLowerCase().includes(search.value.toLowerCase());
		const email = user.email?.toLowerCase().includes(search.value.toLowerCase())
		return username || email;
	})
})

const filteredAds = computed(() => {
	if (!ads.value || ads.value.length === 0) return [];
	return ads.value.filter(ad => {
		const matchesSearch = ad.title.toLowerCase().includes(search.value.toLowerCase());
		const matchesCategory = category.value.toLowerCase() === '' || ad.category.toLowerCase() === category.value.toLowerCase();
		const priceRange = minPrice.value <= ad.price && ad.price <= maxPrice.value;
		const calculatedPrice = ad.price > price.value;
		return matchesSearch && matchesCategory && priceRange && calculatedPrice;
	});
})


onMounted(async () => {
	try {
		await fetchUsers();
		await fetchAds();
	} catch (err) {
		console.log("Error fetching data:", err.message);
	}
	console.log("Users:", users.value);
	console.log("Ads:", ads.value);
})

</script>

<template>
	<h1>Søk</h1>
	<div class="search-box">
		<input v-model="search" placeholder="⌕ Søk..." type="search"/>
		<button class="searchAlt" @click="isOpen = !isOpen">
			<span v-if="isOpen">Lukk søkealternativer</span>
			<span v-else>Flere søkealternativer</span>
		</button>

		<div v-if="isOpen">
			<label><input v-model="isPerson" type="checkbox"/> Personer</label>
			<label><input v-model="isAd" type="checkbox"/> Annonser</label>
			<br>
			<div class="inputNum">
				Min pris:
				<input v-model="minPrice" placeholder="Min pris" type="number"/>
				<br>
				Maks pris:
				<input v-model="maxPrice" placeholder="Maks pris" type="number"/>
			</div>
			<input v-model="price" :max="maxPrice" :min="minPrice" step="100" type="range"/>
			<p>Nåværende pris: {{ price }} kr</p>
		</div>
		<select v-model="category">
			<option disabled value="">Velg en kategori</option>
			<option value="">Ingen valgt</option>
			<option v-for="cat in categories" :key="cat">{{ cat }}</option>
		</select>
	</div>

	<div v-if="search.length >= 0" class="search">
		<h3>Søkeresultater <i class="fa-solid fa-magnifying-glass"></i></h3>
		<br>
		<h2 @click="isPerson = !isPerson">Brukere<i class="fa-solid fa-person"></i><i class="fa-solid fa-person-dress"></i>
		</h2>
		<div v-if="filteredUsers.length > 0 && isPerson" class="users">
			<div v-for="user in filteredUsers.slice(0,5)" :key="user.id" class="userClass">
				<RouterLink :to="{ name: 'UserProfile', params: { id: user.id } }">
					<img v-if="user.profileImageUrl" :src="user.profileImageUrl" alt="img">
					{{ user.name }} - {{ user.email }}
				</RouterLink>
			</div>

		</div>

		<h2 @click="isAd=!isAd">Annonser<i class="fa-solid fa-rectangle-ad" title="Annonser"></i></h2>
		<div v-if="filteredAds.length && isAd" class="ads">
			<div v-for="ad in filteredAds.slice(0, showCount)" :key="ad.id" class="ad">
				<RouterLink :to="{ name: 'AdDetails', params: { id: ad.id } }">
					<img :src="ad.images[0].imageUrl"> {{ ad.title }} - {{ ad.category }} - {{ ad.price }} Kr
				</RouterLink>
			</div>

			<div class="moreLess">
				<div class="more" @click="showCount += 5"><i class="fa-solid fa-square-plus"></i>Vis mer</div>
				<div v-if="showCount > 5">
					<div class="less" @click="showCount -= 5"><i class="fa-solid fa-square-minus"></i>Vis mindre</div>
				</div>
			</div>
		</div>

		<div v-if="filteredUsers.length === 0 && filteredAds.length === 0">
			<p>Ingen treff funnet.</p>
		</div>
	</div>
</template>

<style scoped>

.search-box {
	background-color: black;
	padding: 2rem;
	border-radius: 1rem;
	margin: 2rem auto;
	width: 80%;
	box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
	color: #00ff66;
	border: 5px solid #00ff66;
}

input[type="range"] {
	width: 60%;
	accent-color: darkgreen;
}

.inputNum input[type="number"] {
	width: 20%;
	height: 5vh;
	color: #00ff66;
}

input {
	border: 2px solid #00ff66;;
	border-radius: 1rem;
	padding: 0.3rem;
	text-align: center;
	color: #00ff66;
}

input[type=search] {
	border: 5px solid #00ff66;
	border-radius: 1rem;
	padding: 0.3rem;
	text-align: center;
	width: 70%;
	height: 30%;
	background-color: black;
	color: #00ff66;
	font-family: 'Overseer', 'Press Start 2P', monospace;
	font-size: 2rem;
}

select {
	padding: 1rem;
	border-radius: 1rem;
	color: #00ff66;
	font-family: 'Overseer', 'Press Start 2P', monospace;
}
select:hover{
	padding: 1rem;
	border-radius: 1rem;
	color: #00ff66;
	background-color: darkgreen;
}
option:hover {
	background-color: #00ff66;
}

input:focus {
	border: darkgreen;
}

.router-link-exact-active {
	color: darkgreen;
}

.searchAlt button {
	text-align: center;
	border: 1px solid #00ff66;
	height: 1.5rem;
	padding: 0.5rem;
	display: inline-flex;
	align-items: center;
	border-radius: 0.5rem;
	font-family: 'Overseer', 'Press Start 2P', monospace;
}

.searchAlt:hover {
	background-color: darkgreen;
	cursor: pointer;
	
}

.users {
	color: darkgreen;
	border-radius: 5px;
	padding: 10px;

}

.userClass {
	border-bottom: 1px solid black;
	padding: 10px;
	border-radius: 5px;

}

.userClass:hover {
	transform: scale(1.2);
	background-color: darkgreen;
}

.ads {
	padding: 10px;
	border-radius: 5px;
	text-align: center;
	font-family: 'Overseer', 'Press Start 2P', monospace;
	max-height: 50vh;
	overflow-x: hidden;
}

.ad {
	border-radius: 5px;
	padding: 10px;
	font-family: 'Overseer', 'Press Start 2P', monospace;
	font-size: 1.2rem;
	color: #00ff66;
}

.ad:hover {
	
	background-color: darkgreen;
	font-weight: bold;
	border-radius: 1rem;
	color: #00ff66;
	z-index: 15;
}


img {
	height: 50px;
	width: 50px;
	border-radius: 20px;
}

.more {
	border-radius: 1rem;
	margin-top: 25px;
}

.more:hover {
	border-radius: 1rem;
	transition: 0.5s;
	cursor: pointer;
	transform: scale(1.2);
	color: green;
}

.less {
	border-radius: 1rem;
	margin-bottom: 25px;
	margin-top: 25px;
}

.less:hover {
	border-radius: 1rem;
	transition: 0.5s;
	cursor: pointer;
	transform: scale(1.2);
	color: red;
	
}

h2 {
	font-family: 'Overseer', 'Press Start 2P', monospace;
}

h2:hover {
	cursor: pointer;
	background-color: transparent;
	border-radius: 1rem;
}
</style>