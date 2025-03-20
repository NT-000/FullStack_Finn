<script setup>

import NavBar from "./components/NavBar.vue";
import {RouterView} from 'vue-router'
import {onMounted} from "vue";
import {useUserStore} from "./stores/useUserStore.js";

const store = useUserStore();

onMounted(() => {
  store.initializeUser();
  console.log("running initializeUser()...");
});

</script>

<template>
  <div>
    <NavBar/>
    <RouterView :key="$route.fullPath"/>
  </div>

</template>

<style scoped>
.logo {
  height: 6em;
  padding: 1.5em;
  will-change: filter;
  transition: filter 300ms;
}

.logo:hover {
  filter: drop-shadow(0 0 2em #646cffaa);
}

.logo.vue:hover {
  filter: drop-shadow(0 0 2em #42b883aa);
}
</style>
