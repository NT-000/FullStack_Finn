<script setup>
import {computed, onMounted, onUnmounted, ref} from 'vue';
import {RouterLink, useRoute, useRouter} from 'vue-router';
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
const selectedBuyer = ref('')
console.log('route id', route.params.id)
const adId = Number(route.params.id);
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

const deleteAd = async () => {
  if (confirm("Slette annonsen?")) {
    await adStore.deleteAd(adId);
    router.push("/ads");
  }
}
const markAdAsSold = async () => {
  if (!selectedBuyer.value) {
    error.value = "Velg en kjøper først"
    return;
  }
  await adStore.markAsSold(currentAd.value.id, selectedBuyer.value);
  error.value = "Annonsen markert som solgt!"
};

onMounted(async () => {
  await adStore.fetchAds();
  await users.fetchData();
  await adStore.getInterestedUsers(adId);

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
    const country = address.country ||''
    console.log('reverse geocode result:', "address city", address.city, "nation:", address.country)
    return {city, county,country}
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
const seller = computed(() =>{
 return users.items.value.find(user => user.id === currentAd.value?.userId)
})

</script>

<template>
  <br>
  <h1>Single ad</h1>
  <div class="container">
    <div class="info">
        <div v-if="currentAd && adId">
                <form v-if="currentAd" @submit.prevent="updateAd">
                  <div class="header">
                    <h1>{{ currentAd.title }}</h1>
                    <h3 v-if="seller">Selger:{{seller.name}}</h3>
                    <div v-if="seller && userStore.$state.user.id !== seller.id">
                      <RouterLink :to="{name:'Chat', params:{id:seller.id}, query:{adId:currentAd.id}}"><i class="fa-solid fa-envelope"></i> </RouterLink>
                    </div>
                  </div>
                  <br>
            <input v-if="isUpdating" v-model="currentAd.title" placeholder="New title" type="text"/>
            <div v-if="currentAd?.images && currentAd.images.length > 0">
              <img :src="currentAd.images[0].imageUrl" alt=""/>
            </div>
            <div class="imagesReel" v-if="currentAd?.images && currentAd.images.length > 1">
              <div v-for="image in currentAd.images.slice(1)" :key="image.id">
                <img :src="image.imageUrl" alt="Ad image"/>
              </div>
            </div>
                  <br>
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
                  <br>
            <label>Beskrivelse:</label>
                  <div>{{ currentAd.description }}</div>
            <textarea v-if="isUpdating" v-model="currentAd.description" placeholder="New description"/>
                  <br>
            <label>Pris: {{ currentAd.price }} kr</label>
            <input v-if="isUpdating" v-model="currentAd.price" placeholder="new price" type="number"/>
                  <br>
            <label>Tilstand: {{ currentAd.condition }}</label>
                  <br>
            <select v-if="isUpdating" v-model="currentAd.condition">
              <option>Ny</option>
              <option>Nesten ny</option>
              <option>Brukt</option>
              <option>Godt brukt</option>
            </select>
          <label>Sted:{{currentAd.locationName}}</label>
                  <br>
          <div class="map">
            <div ref="mapContainer" style="height: 300px; width: 50%;"></div>
          </div>
          </form>
          <div class="info" v-if="isOwner && adStore.interestedUsers.length > 0">
            <h3>...</h3>
            <select v-model="selectedBuyer">
              <option disabled value="">Velg kjøper</option>
              <option v-for="user in adStore.interestedUsers" :key="user.id">
                {{ user.Name }}
              </option>
            </select>
      <div>{{error}}</div>
            <button @click="markAdAsSold">Selg til {{selectedBuyer}}</button>
          </div>
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
</template>

<style scoped>
.container {
  height: 600px;
  width: 100%;
  display: inline-block;
  justify-content: center;
  align-items: center;
  padding: 20px;
  background-color: deepskyblue;
  border-radius: 15px;
}
.imagesReel{
  background: #fff;
  display: flex;
  height: 100px;
  width: 100%;
  justify-content: center;
}

.imageReel img{
  height: 100px;
  width: 100px;
}
label{
  position: page;
  
}
img{
  height: 100px;
  width: 100px;
  border: 2px solid black;
  align-items: center;
}
.map{
  background: deepskyblue;
  top: 10px;
}
</style>