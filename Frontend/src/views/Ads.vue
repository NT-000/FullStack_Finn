<script setup>

import {RouterLink} from "vue-router";
import {computed, onMounted, ref} from "vue";
import {getRoute} from "../composables/getRoute.js";

//Henter både brukere og annonser for søkefunksjon
const {items: users, loading: usersLoading, error: errorUsers, fetchData: fetchUsers} = getRoute('/users');

const {items: ads, loading: adsLoading, error: adsError, fetchData: fetchAds} = getRoute('/ads');

onMounted(async () => {
  try {
    await fetchUsers();
    await fetchAds();
  } catch (err) {
    console.log("Error fetching data:", err.message);
  }
  console.log("Users:", users.value);
  console.log("Ads:", ads.value);
})

const search = ref('');

const filteredSearch = computed(() => {
  const searchTerm = search.value.toLowerCase();
  console.log(ads.value[0]);
  return [
    ...users.value.filter(user => user.name?.toLowerCase().includes(searchTerm)),
    ...ads.value.filter(ad => ad.title?.toLowerCase().includes(searchTerm)),
  ];
})
</script>

<template>
  <h1>Ads</h1>

  <input v-model="search" placeholder="Search" type="search"/>
  <div v-if="search.length > 0" class="search">
    <h2>Search results</h2>
    <ul>
      <li v-for="item in filteredSearch" :key="item.id">
        <strong v-if="item.title">
          <RouterLink :to="{name: 'AdDetails', params: {id:item.id}}">{{ item.title }}</RouterLink>
        </strong>
        <strong v-else="item.name">{{ item.name }}</strong>
      </li>
    </ul>
  </div>
  <div v-else-if="filteredSearch.length === 0" class="search">
    No ads found...
  </div>
</template>

<style scoped>

</style>