<script setup>
import {computed, watchEffect, onMounted, ref} from "vue";
import { useStore } from "../composables/useStore";
import { useUser } from "../composables/useUser";
import {useRoute} from "vue-router";
import {useAdStore} from "../stores/adStore.js";
import MyProfile from "../components/MyProfile.vue";
import OtherProfile from "../components/OtherProfile.vue";


const adStore = useAdStore();
const route = useRoute();
const userId = ref(route.params.id || null);
const store = useStore();
const {user,fetchUserProfile } = useUser();
const computedUser = computed(() => store.user.value);

const isOwnProfile = computed(() => {
  return !userId.value || Number(userId.value) === computedUser.value?.id;
});

onMounted(async () => {
  await fetchUserProfile(userId.value);
  await adStore.fetchAds();
  console.log("computed user",computedUser.value);
  console.log("computed user profileImage path",computedUser.value.profileImageUrl);
});

const filteredAds = computed(() => {
  const routeId = userId.value;
  console.log("Decoded user ID:", computedUser.value?.id, "user email:", computedUser.value?.email);
  return adStore.getAdsByUser(routeId || computedUser.value?.id);
});
</script>

<template>
  <div v-if="isOwnProfile">
<MyProfile :user="computedUser" :ads="filteredAds"/>
  </div>
  <div v-else> <OtherProfile :user="user" :ads="filteredAds" />
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