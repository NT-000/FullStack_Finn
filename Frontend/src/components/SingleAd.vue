<script setup>
import { computed, onMounted } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { useUserStore } from "../stores/useUserStore.js";
import { useAdStore } from "../stores/adStore.js";



const route = useRoute();
const router = useRouter();
const adStore = useAdStore();
const userStore = useUserStore();
console.log('route id', route.params.id)
const adId = route.params.id;

const currentAd = computed(() => {
  return adStore.ads.find(ad => ad.id == adId);
});

//sjekker om innlogget bruker
const isOwner = computed(() => {
  return userStore.user.id == currentAd.value?.userId;
});

const deleteAd = async () => {
  if(confirm("Are you sure you want to delete this ad?")) {
    await adStore.deleteAd(adId);
    router.push("/ads");
  }
}

onMounted(async () => {
  adStore.fetchAds();
})

function updateAd() {
 
}

</script>

<template>
  <br>
  <h1>Single ad</h1>
  <div class="container">
    <div class="header">
      <div class="info">
        <div v-if="currentAd">
      <h1>{{ currentAd.title }}</h1>
          <input type="text">
        <div v-if="currentAd?.images && currentAd.images.length > 0">
          <img :src="currentAd.images[0].imageUrl" alt=""/>
        </div>
        <div v-if="currentAd?.images && currentAd.images.length > 1">
          <div v-for="image in ad.images.slice(1)" :key="image.id">
            <img :src="image.imageUrl" alt="Ad image"/>
          </div>
        </div>
        <p>Category: {{ currentAd.category }}</p>
        <p>Info: {{ currentAd.description }}</p>
        <p>Price: {{ currentAd.price }} kr</p>
        </div>
        <div v-if="isOwner">
          
          <button @click="deleteAd">Slett annonse</button>
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