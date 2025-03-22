<script setup>
import {ref, onMounted, watchEffect} from "vue";
import L from 'leaflet'
import 'leaflet/dist/leaflet.css'
import iconRetinaUrl from 'leaflet/dist/images/marker-icon-2x.png'
import iconUrl from 'leaflet/dist/images/marker-icon.png'
import shadowUrl from 'leaflet/dist/images/marker-shadow.png'
import {useAdStore} from "../stores/adStore.js";
import axios from "axios";

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
const map =ref(null)

onMounted(async() => {
  // oppretter selve kartet
  // start-koordinater er satt til Oslo.
 await adStore.fetchAds();
map.value = L.map(mapContainer.value).setView([59.91, 10.74], 10)

  // layoyt
  L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
    maxZoom: 19,
    attribution: '© OpenStreetMap'
  }).addTo(map.value)

  for(const ad of adStore.ads)
  {
    if(ad.latitude && ad.longitude){
      console.log("location name:", ad.locationName)
      L.marker([ad.latitude, ad.longitude]).addTo(map.value)
          .bindPopup(`${ad.title} - ${ad.locationName}`)
    }
  }
  
})

watchEffect(async ()=>{
  
})

</script>

<template>
  <div class="container">
  <h1>Alle annonser</h1>
<div ref="mapContainer" style="width:150%; height:600px;">
</div>
  </div>
</template>

<style scoped>
.map{
  width: 100%;
  height: 400px;
}
</style>