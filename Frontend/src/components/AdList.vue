<script setup>
import {onMounted} from "vue";
import {getRoute} from "../composables/getRoute.js";

const {items, loading, error, fetchData} = getRoute('/ads');

onMounted(async()=>{
await fetchData();
})


</script>

<template>
<div class="container">
<h1>Active ads</h1>
  <p v-if="loading">Loading...</p>
  <p v-else-if="error">{{ error }}</p>
  <div v-if="items.length">
  <div v-for="ad in items" :key="ad.id" class="innerContainer">
    <div class="miniContainer_header">
      <div>{{ad.title}}</div>
      <div class="miniContainer">
      <div>{{ad.category}}</div>
      <div>{{ad.price}}</div>
      <div>{{ad.condition}}</div>
        <div>{{ad.createdAt}}</div>
        </div>
    </div>
    
  </div>
      </div>
    
  </div>
  
  
</template>

<style scoped>

</style>