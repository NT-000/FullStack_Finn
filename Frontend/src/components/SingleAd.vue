<script setup>
import {computed, onMounted, onUnmounted, ref} from 'vue';
import {useRoute, useRouter} from 'vue-router';
import {useUserStore} from "../stores/useUserStore.js";
import {useAdStore} from "../stores/adStore.js";
import L from 'leaflet'
import 'leaflet/dist/leaflet.css'
import {getRoute} from "../composables/getRoute.js";
import iconRetinaUrl from 'leaflet/dist/images/marker-icon-2x.png'
import iconUrl from 'leaflet/dist/images/marker-icon.png'
import shadowUrl from 'leaflet/dist/images/marker-shadow.png'
import axios from "axios";


L.Marker.prototype.options.icon = L.icon({
  iconRetinaUrl,
  iconUrl,
  shadowUrl,
  iconSize: [25, 41],
  iconAnchor: [12, 41]
})
const users = getRoute('/users');
const route = useRoute();
const router = useRouter();
const adStore = useAdStore();
const userStore = useUserStore();

const isUpdating = ref(false)

console.log('route id', route.params.id)
const adId = route.params.id;
const map = ref(null)
const mapContainer = ref(null)
const marker = ref(null)

const lat = ref(null)
const lng = ref(null)
const error = ref(null)

const currentAd = computed(() => {
  return adStore.ads.find(ad => ad.id == adId);
});

//sjekker om innlogget bruker
const isOwner = computed(() => {
  return userStore.user.id == currentAd.value?.userId;
});

const deleteAd = async () => {
  if (confirm("Are you sure you want to delete this ad?")) {
    await adStore.deleteAd(adId);
    router.push("/ads");
  }
}

onMounted(async () => {
  await adStore.fetchAds();
  await users.fetchData();

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
    <h3>${currentAd.value.title ?? ''}</h3>
    ${currentAd.value?.images?.length ? `<img src="${currentAd.value.images[0].imageUrl}" alt="Ad image" style="max-width:50px; max" />` : ''}`)
        .openPopup()
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

    const locationName = await reverseGeocode(lat.value, lng.value)
    
    updateCoordinates(adId, lat.value, lng.value, locationName)
  }
}

async function reverseGeocode(latValue, longValue) {
  try {
    const url = `https://nominatim.openstreetmap.org/reverse?lat=${latValue}&lon=${longValue}&format=json`
    const res = await axios.get(url, {withCredentials: false})
    const displayName = res.data?.display_name
    console.log('reverse geocode result:', displayName)
    return displayName
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
    await adStore.fetchAds();
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
    Price: Number(currentAd.value.price)
  };
  await adStore.updateAd(adId, adData);
  isUpdating.value = false;
}

const seller = computed(() =>{
 return users.items.value.find(user => user.id === currentAd.value?.userId)
})

</script>

<template>
  <br>
  <h1>Single ad</h1>
  <div class="container">
    <div class="header">
      <div class="info">
        <div v-if="currentAd && adId">
          <form v-if="currentAd" @submit.prevent="updateAd">
            <h1>{{ currentAd.title }}</h1>
            <h3 v-if="seller">Seller:{{seller.name}}</h3>
            <input v-if="isUpdating" v-model="currentAd.title" placeholder="New title" type="text"/>
            <div v-if="currentAd?.images && currentAd.images.length > 0">
              <img :src="currentAd.images[0].imageUrl" alt=""/>
            </div>
            <div v-if="currentAd?.images && currentAd.images.length > 1">
              <div v-for="image in currentAd.images.slice(1)" :key="image.id">
                <img :src="image.imageUrl" alt="Ad image"/>
              </div>
            </div>
            <label>Category: {{ currentAd.category }}</label>
            <select v-if="isUpdating" v-model="currentAd.category">
              <option>Bikes</option>
              <option>Electronics</option>
              <option>Furniture</option>
              <option>Clothing</option>
              <option>Guns</option>
              <option>Hand-weapons</option>
            </select>
            <label>Info: {{ currentAd.description }}</label>
            <textarea v-if="isUpdating" v-model="currentAd.description" placeholder="New description"/>
            <label>Price: {{ currentAd.price }} kr</label>
            <input v-if="isUpdating" v-model="currentAd.price" placeholder="new price" type="number"/>
            <label>Condition: {{ currentAd.condition }}</label>
            <select v-if="isUpdating" v-model="currentAd.condition">
              <option>New</option>
              <option>Like new</option>
              <option>Used</option>
              <option>Well used</option>
            </select>
          <label>Location:{{currentAd.locationName}}</label>
          
            <div ref="mapContainer" style="height: 300px; width: 50%;"></div>
          </form>
        </div>
        <div v-if="isOwner">

          <button @click="deleteAd">Slett annonse</button>

          <button @click="isUpdating = !isUpdating">
            <span v-if="!isUpdating"> Endre annonse </span>
            <span v-if="isUpdating" @click="updateAd"> Lagre annonse </span>
          </button>
        </div>

      </div>
    </div>
  </div>
</template>

<style scoped>
img {
  height: 20%;
  width: 20%;
}
</style>