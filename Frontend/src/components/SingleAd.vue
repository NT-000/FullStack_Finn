<script setup>
import {getPost} from "../composables/getPost.js";
import {useRoute} from "vue-router";
import {onMounted} from "vue";


const route = useRoute();
console.log('route id',route.params.id)

const { item: ad, loading, error, fetchData } = getPost(`/ads/${route.params.id}`);

onMounted( async()=>{
  if(route.params.id){
    await fetchData();
  }
})
</script>

<template>
<h1>Single ad</h1>
  <div class="container">
    <div class="header">
      <h1 v-if="!loading">{{ ad.title }}</h1>
      <div class="info">
        
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

</style>