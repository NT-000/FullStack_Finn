<script setup>
import AdList from "../components/AdList.vue";
import router from "../router/index.js";
console.log(router.currentRoute.value)
import { useRoute } from 'vue-router';
const route = useRoute();
console.log("Current Route:", route.path);
</script>

<template>
<h1>Main Page</h1>
</template>

<style scoped>

</style>