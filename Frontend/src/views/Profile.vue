<script setup>
import {computed, watchEffect, onMounted, ref} from "vue";
import { useStore } from "../composables/useStore";
import { useUser } from "../composables/useUser";
import {useRoute} from "vue-router";
import {useAdStore} from "../stores/adStore.js";

const adStore = useAdStore();
const route = useRoute();
const userId = ref(route.params.id || null);
const store = useStore();
const { fetchUserProfile } = useUser();
const computedUser = computed(() => store.user.value);

onMounted(async () => {
  await fetchUserProfile();
  await adStore.fetchAds();
});

watchEffect(() => {
  console.log("change in store.user:", computedUser.value);
});

const filteredAds = computed(() => {
  const routeId = userId.value;
  console.log("Decoded user ID:", computedUser.value?.id, "user email:", computedUser.value?.email);
  return adStore.getAdsByUser(routeId || computedUser.value?.id);
});
</script>

<template>
  <h2>{{ userId ? "Other profile" : "My profile" }}</h2>
  <div v-if="computedUser && computedUser.name">
    <p><strong>Name:</strong> {{ computedUser.name }}</p>
    <p><strong>Email:</strong> {{ computedUser.email }}</p>
    <p><strong>Rating:</strong> {{ computedUser.rating }}</p>
    <div v-if="filteredAds.length > 0">
      <div v-for="ad in filteredAds" :key="ad.id">
        
        <div class="ad">
          <RouterLink :to="{name: 'AdDetails', params:{id:ad.id}}"> {{ad.title}}</RouterLink>
        <div v-if="ad.images.length > 0" class="image">
          <div v-for="image in ad.images" :key="image.id">
            <img :src="image.imageUrl" alt="x" />
          </div>
        </div>
        </div>
        </div>
    </div>
  </div>
  <div v-else>
    <p>Loading user...</p>
  </div>
</template>

<style scoped>
img{
  width: 20%;
  height: 20%;
  border-radius: 5px;
  border: solid 1px white;
}
</style>