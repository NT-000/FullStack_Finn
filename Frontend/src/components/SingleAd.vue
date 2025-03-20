<script setup>
import {getPost} from "../composables/getPost.js";
import {useRoute} from "vue-router";
import {onMounted} from "vue";


const route = useRoute();
console.log('route id', route.params.id)

const {item: ad, loading, error, fetchData} = getPost(`/ads/${route.params.id}`);

onMounted(async () => {
  if (route.params.id) {
    await fetchData();
  }
})
</script>

<template>
  <br>
  <h1>Single ad</h1>
  <div class="container">
    <div class="header">
      <h1 v-if="!loading">{{ ad.title }}</h1>
      <div class="info">
        <div v-if="ad?.images && ad.images.length > 0">
          <img :src="ad.images[0].imageUrl" alt=""/>
        </div>
        <div v-if="ad?.images && ad.images.length > 1">
          <div v-for="image in ad.images.slice(1)" :key="image.id">
            <img :src="image.imageUrl" alt="Ad image"/>
          </div>
        </div>
        <p v-if="!loading">Category: {{ ad.category }}</p>
        <p v-if="!loading">Info: {{ ad.description }}</p>
        <p v-if="!loading">Price: {{ ad.price }} kr</p>
        <p v-if="loading">Loading ad...</p>
        <p v-if="error">{{ error }}</p>

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