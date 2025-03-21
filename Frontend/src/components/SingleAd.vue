<script setup>
import {computed, onMounted, reactive, ref} from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { useUserStore } from "../stores/useUserStore.js";
import { useAdStore } from "../stores/adStore.js";

const route = useRoute();
const router = useRouter();
const adStore = useAdStore();
const userStore = useUserStore();

const isUpdating = ref(false)

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

const updateAd = async () => {
  if (!currentAd.value) {
    console.error("currentAd er ikke lastet enda!");
    return;
  }
    const adData ={
    Title: currentAd.title,
    Category: currentAd.category,
    Description: currentAd.description,
      Condition: currentAd.condition,
      Price:  Number(currentAd.value.price)
  };
  await adStore.updateAd(adId, adData);
  isUpdating.value = false;
}

</script>

<template>
  <br>
  <h1>Single ad</h1>
  <div class="container">
    <div class="header">
      <div class="info">
        <div v-if="currentAd && adId">
          <form @submit.prevent="updateAd" v-if="currentAd">
      <h1>{{ currentAd.title }}</h1>
          <input type="text" v-model="currentAd.title" placeholder="New title" v-if="isUpdating"/>
        <div v-if="currentAd?.images && currentAd.images.length > 0">
          <img :src="currentAd.images[0].imageUrl" alt=""/>
        </div>
        <div v-if="currentAd?.images && currentAd.images.length > 1">
          <div v-for="image in currentAd.images.slice(1)" :key="image.id">
            <img :src="image.imageUrl" alt="Ad image"/>
          </div>
        </div>
        <label>Category: {{currentAd.category }}</label>
            <select v-model="currentAd.category" v-if="isUpdating">
              <option>Bikes</option>
              <option>Electronics</option>
              <option>Furniture</option>
              <option>Clothing</option>
              <option>Guns</option>
              <option>Hand-weapons</option>
            </select>
        <label>Info: {{ currentAd.description }}</label>
          <textarea v-model="currentAd.description" placeholder="New description" v-if="isUpdating"/>
        <label>Price: {{ currentAd.price }} kr</label>
          <input type="number" v-model="currentAd.price" placeholder="new price" v-if="isUpdating"/>   
            <label>Condition: {{ currentAd.condition }}</label>
            <select v-model="currentAd.condition" v-if="isUpdating">
              <option>New</option>
              <option>Like new</option>
              <option>Used</option>
              <option>Well used</option>
            </select>
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