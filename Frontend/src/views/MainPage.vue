<script setup>
import AdList from "../components/AdList.vue";
import router from "../router/index.js";
console.log(router.currentRoute.value)
import { useRoute } from 'vue-router';
import Search from "../components/Search.vue";
const route = useRoute();
console.log("Current Route:", route.path);
</script>

<template>
<h1>Main Page</h1>
  <Search/>
</template>

<style scoped>

</style>