<script setup>
import {computed, watchEffect} from 'vue'
import {useLogout} from "../composables/useLogout";
import { useUserStore } from '../stores/useUserStore.js';

const userStore = useUserStore();
console.log(" Vue-component:", userStore.user);
const {logOut} = useLogout();

const currentUser = computed(() => userStore.user)

watchEffect(() => {
  console.log("currentUser changed: ", currentUser.value)
})

</script>

<template>
  <nav>
    <p v-if="currentUser && currentUser.name">
      Logged in user: {{ currentUser.name }}
    </p>
    <p v-else></p>
    <button v-if="currentUser && currentUser.email" @click="logOut">Logg ut</button>
    <div v-if="currentUser  && currentUser.email">
      <RouterLink to="/mainpage">
        <button>Main Page</button>
      </RouterLink>
      <RouterLink to="/ads">
        <button>Annonser</button>
      </RouterLink>
      <RouterLink to="/users/profile">
        <button>Profile</button>
      </RouterLink>
      <RouterLink to="/ads/create-with-files">
        <button>Create new ad</button>
      </RouterLink>
    </div>
  </nav>
</template>