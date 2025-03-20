<script setup>
import router from "../router/index.js";
import {useRoute} from 'vue-router';
import Search from "../components/Search.vue";

console.log(router.currentRoute.value)

const route = useRoute();
console.log("Current Route:", route.path);
</script>

<template>
  <h1>Main Page</h1>
  <Search/>
</template>

<style scoped>

</style>