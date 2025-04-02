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

const route = useRoute();
const adId = Number(route.params.id);
const users = getRoute('/users');
const reviews = getRoute(`/reviews/ad/${adId}`);
const router = useRouter();
const dateFormat = useDateFormat();
const adStore = useAdStore();
const userStore = useUserStore();
const favStore = useFavStore();

const isUpdating = ref(false)
const selectedBuyerId = ref(null)
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


</script>

<template>
  <div class="outerContainer">
    <div class="container">
      <div class="info">
        <div></div>
        <div v-if="currentAd && favStore">
          <div @click="favStore.toggleFav(currentAd.id)">
            <i :class="favStore.isFavorite(currentAd.id) ? 'fa-solid fa-beat': 'fa-regular'" class="fa-heart"
               style="color: red; cursor: pointer;"></i>
          </div>
        </div>
        <div v-if="currentAd && adId">
          <form v-if="currentAd && seller" @submit.prevent="updateAd">
            <div class="header">
              <div class="title"><h1>{{ currentAd.title }}</h1></div>
              <div v-if="currentAd && currentAd.isSold">
                <div class="sold">SOLGT</div>
              </div>
              <div>
                <RouterLink v-if="seller && userStore.$state.user.id !== seller.id"
                            :to="{name: 'UserProfile', params:{id:seller.id}}"><h3 v-if="seller">{{ seller.name }} <img
                    :src="seller.profileImageUrl"></h3></RouterLink>
              </div>
              <div>
                <RouterLink v-if="seller && userStore.$state.user.id === seller.id" :to="{name: 'Profile'}"><h3
                    v-if="seller">{{ seller.name }}</h3></RouterLink>
              </div>
              <div class="userMap">
                <div v-if="seller && userStore.$state.user.id !== seller.id && !currentAd.isSold">
                  <RouterLink :to="{name:'Chat', params:{id:seller.id}, query:{adId:currentAd.id}}" class="messageRL"><i
                      class="fa-solid fa-envelope fa-bounce">Send melding</i></RouterLink>
                </div>
              </div>
              <div class="map">
                <div ref="mapContainer" style="height: 200px; width: 200px; border-radius: 10px;"></div>
              </div>
            </div>
            <label>Sted:</label>
            <div>{{ currentAd.locationName }}</div>
            <br>
            <input v-if="isUpdating" v-model="currentAd.title" placeholder="New title" type="text"/>
            <div v-if="currentAd?.images && currentAd.images.length > 0">
              <img :src="currentAd.images[0].imageUrl" alt=""/>
            </div>
            <div v-if="currentAd?.images && currentAd.images.length > 1" class="imagesReel">
              <div v-for="image in currentAd.images.slice(1)" :key="image.id">
                <img :src="image.imageUrl" alt="Ad image"/>
              </div>
            </div>
            <br>
            <div class="category">
              <label>Kategori: {{ currentAd.category }}</label>
              <select v-if="isUpdating" v-model="currentAd.category">
                <option>Sykler</option>
                <option>Elektronikk</option>
                <option>Møbler</option>
                <option>Klær</option>
                <option>Våpen</option>
                <option>Hånd-våpen</option>
                <option>Treningsutstyr</option>
              </select>
            </div>
            <div class="price">
              <label>Pris: {{ currentAd.price }} kr</label>
              <input v-if="isUpdating" v-model="currentAd.price" placeholder="new price" type="number"/>
            </div>
            <div class="condition">
              <label>Tilstand: {{ currentAd.condition }}</label>
              <br>
              <select v-if="isUpdating" v-model="currentAd.condition">
                <option>Ny</option>
                <option>Nesten ny</option>
                <option>Brukt</option>
                <option>Godt brukt</option>
              </select>
            </div>
            <label>Beskrivelse:</label>
            <div>{{ currentAd.description }}</div>
            <textarea v-if="isUpdating" v-model="currentAd.description" placeholder="New description"/>
            <br>

          </form>
          <div v-if="isOwner && adStore.interestedUsers.length > 0" class="info">
            <h3>Kjøper</h3>
            <select v-if="currentAd.isSold === false" v-model="selectedBuyerId">
              <option disabled value="">Velg kjøper</option>
              <option v-for="user in adStore.interestedUsers" :key="user.Id" :value="user.Id">
                {{ user.Name }}
              </option>
            </select>
            <div v-if="reviewer">{{ error }} Kjøper - {{ reviewer.name }}</div>
            <button v-if="currentAd.isSold === false" @click="markAdAsSold">Selg til {{ buyer }}</button>
          </div>
        </div>
        <div v-if="isOwner && currentAd.isSold === false">
          <button @click="deleteAd">Slett annonse</button>
          <button @click="isUpdating = !isUpdating">
            <span v-if="!isUpdating"> Endre annonse </span>
            <span v-if="isUpdating" @click="updateAd"> Lagre annonse </span>
          </button>
        </div>
        <div v-if="adReview" class="adReview">
          <RouterLink :to="{name:'UserProfile', params:{id:reviewer.id}}">
            <div v-if="reviewer">{{ reviewer.name }}</div>
          </RouterLink>
          <div>{{ adReview.comment }}</div>
          <div>{{ adReview.rating }}</div>
          <div>{{ dateFormat.formatDate(adReview.createdAt) }}</div>
        </div>
      </div>
      <div>
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

.title {
  background-color: blue;
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
}

.fa-heart {
  font-size: 100px;

}

.userMap {
  display: inline-flex;
  flex-direction: row;


}

.imagesReel {
  display: flex;
  gap: 10px;
  flex-wrap: wrap;
  justify-content: center;
}

.imageReel img {
  height: 100px;
  width: 100px;
}

img:hover {
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
  font-size: 2rem;
  display: flex;
  justify-content: center;
}

.price {
  font-weight: bold;
  font-size: 2rem;
  display: flex;
  justify-content: center;
}

img {
  height: 100px;
  width: 100px;
  border: 2px solid black;
  align-items: center;
  transition: transform 0.3s ease;
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
}

.messageRL {
  color: white;
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
}

button:hover {
  background-color: #004080;
  color: white;
  transition: all 0.3s ease;
}

.adReview {
  font-weight: bolder;
  padding: 5px;
  display: flex;
  justify-content: center;
  align-items: center;
  border-bottom: 3px solid #e6f2fa;

}

.review {
  font-weight: bolder;
  padding: 5px;
  display: flex;
  justify-content: center;
  align-items: center;
  border-bottom: 3px solid #e6f2fa;
}

</style>