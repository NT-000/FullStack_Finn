<script setup>
import {computed, onMounted, ref,} from 'vue'
import {useRoute} from 'vue-router'
import {useUser} from "../composables/useUser.js"
import {getRoute} from "../composables/getRoute.js";
import store from "../composables/useStore"

const {items : ads, loading,fetchData} = getRoute("/ads")

const {user, error, fetchUserProfile} = useUser();
const route = useRoute()
const userId = ref(route.params.id || null);

onMounted(()=>{
  fetchUserProfile(userId.value);
  fetchData();
  console.log("Current user ID:", store.user.value?.id);
});

const filteredAds = computed(() => {
  // route.params.id er enten en streng eller undefined
  const routeId = userId.value;  // reaktiv ID (evt. null)
  console.log("routeId:", routeId, "storeUserId:", store.user.value?.id, "ads:", ads.value);
  console.log("All ads:", ads.value);
  console.log("Decoded user ID:", store.user.value?.id, "user email:", store.user.value?.email);


  if (routeId) {
    // "other profile"
    return ads.value.filter(ad => ad.userId === Number(routeId))
  } else {
    // "my profile"
    // Hvis store.user er en ref: store.user.value?.id
    const loggedInId = store.user.value?.id;
    if (!loggedInId) {
      return []; // bruker er ikke logget inn eller data er ikke lastet
    }
    return ads.value.filter(ad => ad.userId === loggedInId);
  }
});
</script>

<template>
  <h2>{{ userId ? "Other profile" : "My profile" }}</h2>
  <p v-if="error" class="error">{{ error }}</p>
  <div v-if="user">
    <p><strong>Name:</strong> {{ user.name }}</p>
    <p><strong>Rating:</strong> {{ user.rating }}</p>
    <p><strong>Active ads</strong></p>
    <div v-if="userId" v-for="ad in filteredAds">
      {{ad.title}}
    </div>
    <div v-else v-for="ad in filteredAds" :key="ad.id">
      {{ad.title}}
    </div>
  </div>
</template>

<style scoped>

</style>