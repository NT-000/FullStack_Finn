<script setup>
import {computed, onMounted, ref} from "vue";
import Profile from "../views/Profile.vue";
import {getRoute} from "../composables/getRoute.js";

const search= ref('');
const {items : users, loading, error, fetchData} = getRoute('/users');

const filteredSearch = computed(() =>{
  const searchTerm = search.value.toLowerCase();
  return users.value.filter(user => user.name?.toLowerCase().includes(searchTerm))
  
})
onMounted(()=>{
  fetchData()
})
</script>

<template>

      <div class="input-field">
        <input id="search" v-model="search" type="search">
        <label>Search</label>
      </div>

    <div v-if="filteredSearch.length > 0">
      <div class="search">
        <div v-for="user in filteredSearch" :key="user.id">
        <RouterLink :to="{name: 'UserProfile', params: {id:user.id}}">{{user.name}}</RouterLink>
        </div>
      </div>
    </div>
  
</template>

<style scoped>

</style>