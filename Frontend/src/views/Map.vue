<script setup>
import {ref, onMounted} from "vue";
import L from 'leaflet'
import 'leaflet/dist/leaflet.css'
import iconRetinaUrl from 'leaflet/dist/images/marker-icon-2x.png'
import iconUrl from 'leaflet/dist/images/marker-icon.png'
import shadowUrl from 'leaflet/dist/images/marker-shadow.png'

L.Marker.prototype.options.icon = L.icon({
  iconRetinaUrl,
  iconUrl,
  shadowUrl,
  iconSize: [25, 41],
  iconAnchor: [12, 41],
  popupAnchor: [1, -34],
  tooltipAnchor: [16, -28]
})

const mapContainer = ref(null)
const map =ref(null)

onMounted(() => {
  // oppretter selve kartet
  // start-koordinater er satt til Oslo.
map.value = L.map(mapContainer.value).setView([59.91, 10.74], 10)

  // layoyt
  L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
    maxZoom: 19,
    attribution: '© OpenStreetMap'
  }).addTo(map.value)
})

</script>

<template>
<div ref="mapContainer" style="width:100%; height:400px;">
</div>
</template>

<style scoped>
.map{
  width: 100%;
  height: 400px;
}
</style>