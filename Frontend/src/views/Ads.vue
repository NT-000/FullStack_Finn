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
const category = ref('')
const isAd = ref(true);
const isPerson = ref(true)
const isOpen = ref(false)
const filteredUsers = computed(() => {
  if(isPerson.value && search.value.length > 0){
    return users.value.filter(user =>
        user.name?.toLowerCase().includes(search.value.toLowerCase()))
  }

});
const filteredAds = computed(() => {
  if (!ads.value || ads.value.length === 0) return [];
  return ads.value.filter(ad => {
    const matchesSearch = ad.title.toLowerCase().includes(search.value.toLowerCase());
    const matchesCategory = category.value.toLowerCase() === '' || ad.category.toLowerCase() === category.value.toLowerCase();
    return matchesSearch && matchesCategory;
  });
});

</script>

<template>
  <h1>Søk</h1>
  <input v-model="search" placeholder="Søk..." type="search" />
  <div class="searchAlt" @click="isOpen = !isOpen">
  <span v-if="isOpen">Lukk</span>
  <span v-else>Søkealternativer</span>
  </div>
  <div v-if="isOpen">
    <label><input type="checkbox" v-model="isPerson" /> Personer</label>
    <label><input type="checkbox" v-model="isAd" /> Annonser</label>
    <select v-model="category">
      <option></option>
      <option>Sykler</option>
      <option>Leker</option>
      <option>Electronikk</option>
      <option>Møbler</option>
      <option>Klær</option>
      <option>Skytevåpen</option>
      <option>Hånd-våpen</option>
    </select>
  </div>

  <div class="search" v-if="search.length > 0">
    <h2>Søkeresultater</h2>
    <div class="users" v-if="filteredUsers.length > 0">
      <h3>Brukere<i class="fa-solid fa-person"></i><i class="fa-solid fa-person-dress"></i></h3>
        <div class="userClass" v-for="user in filteredUsers" :key="user.id">
          <RouterLink :to="{ name: 'UserProfile', params: { id: user.id } }">
          <img v-if="user.profileImageUrl" :src="user.profileImageUrl" alt="img">
            {{ user.name }}
          </RouterLink>
<!--          - <RouterLink :to="{name:'Chat', params:{id:user.id}}">Send Melding <i class="fa-solid fa-envelope fa-bounce"></i></RouterLink>-->
        </div>
      
    </div>
    
    <div class="ads" v-if="filteredAds.length > 0">
      <h3>Annonser<i class="fa-solid fa-rectangle-ad" title="Annonser"></i></h3>
      
        <div class="ad" v-for="ad in filteredAds" :key="ad.id">
          <RouterLink :to="{ name: 'AdDetails', params: { id: ad.id } }">
            {{ ad.title }} - {{ad.category}}
          </RouterLink>
        </div>
    </div>
    
    <div v-if="filteredUsers.length === 0 && filteredAds.length === 0">
      <p>Ingen treff funnet.</p>
    </div>
  </div>
</template>

<style scoped>
i:hover{
  border: white;
  color: black;
  transition: 0.3s;
}
.router-link-exact-active{
  color: white;
}
.searchAlt{
  text-align: center;
  border: 1px solid black;
  border-radius: 5px;
  display: inline-flex;
  align-items: center;
}
h1{
  
}

.users{
  
  border-radius: 5px;
  padding: 10px;
  
}
.userClass{
  border-bottom: 1px solid black;
  padding: 10px;
  border-radius: 5px;
  
}
.userClass:hover{
  transform: scale(1.2);
  background-color: ghostwhite;
}
.ads{
  padding: 10px;
  
  border-radius: 5px;
}
.ad{
  border-radius: 5px;
  padding: 10px;
}
.ad:hover{
  transform: scale(1.2);
  background-color: ghostwhite;
}
img{
  height: 50px;
  width: 50px;
  border-radius: 20px;
}
</style>