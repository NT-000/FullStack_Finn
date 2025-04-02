<script setup>
import {computed, watchEffect} from 'vue'
import {useLogout} from "../composables/useLogout";
import {useUserStore} from '../stores/useUserStore.js';

const userStore = useUserStore();
const {logOut} = useLogout();
const currentUser = computed(() => userStore.user)

watchEffect(() => {
  console.log("currentUser changed: ", currentUser.value)
})
</script>

<template>
  <nav>

    <div class="nav-bar">
      <div v-if="currentUser && currentUser.name" class="user-display">
        <img src="/public/frog.png"/>
      </div>
      <p v-else>Ingen bruker innlogget</p>


      <div v-if="currentUser && currentUser.email" class="nav-bar">
        <div v-if="currentUser && currentUser.email" @click="logOut">
          <i :title="currentUser.name" class="fa-solid fa-lock-open"></i>
        </div>

        <RouterLink to="/mainpage">
          <i class="fa-solid fa-house" title="Hovedside"></i>
        </RouterLink>

        <RouterLink to="/ads">
          <i class="fa-solid fa-rectangle-ad" title="Annonser"></i>
        </RouterLink>

        <RouterLink to="/users/profile">
          <i class="fa-solid fa-circle-user" title="Min profil"></i>
        </RouterLink>

        <RouterLink to="/ads/create-with-files">
          <i class="fa-solid fa-file-circle-plus" title="Lag ny annonse"></i>
        </RouterLink>

        <RouterLink to="/inbox">
          <i class="fa-solid fa-inbox" title="Innboks"></i>
        </RouterLink>

        <RouterLink to="/map">
          <i class="fa-solid fa-map-location-dot" title="Kart"></i>
        </RouterLink>
      </div>
    </div>
  </nav>
</template>

<style scoped>

nav {
  display: flex;
  justify-content: space-between;
  align-items: center;
  border-radius: 5px;
  padding: 20px 20px;
  border-bottom: 6px solid #b3d4e0;
  width: 50vw;
}

.user-display {
  font-size: 1.2rem;
  font-weight: 500;
  color: #00263b;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 8px;
}

nav {
  padding: 20px;
  background-color: #98b3b2;
  width: 50vw;
  border-radius: 15px;
}

.nav-bar {
  display: flex;
  margin-left: 1vw;
  gap: 3vw;
  padding: 10px;
  border-radius: 8px;
  width: 50vw;
}

.nav-bar img {
  height: 7vh;
  width: 5vw;
  border-radius: 10px;
}

i {
  color: black;
  font-size: 4rem;
}

button {
  background-color: #98cbe8;
  border: none;
  border-radius: 4px;
  padding: 6px 10px;
  color: #ffffff;
  font-size: 1rem;
  cursor: pointer;
  transition: background-color 0.3s ease;
}

button:hover {
  background-color: #6cb4da;
}

button i {
  font-size: 2rem;
  margin-right: 4px;
}

nav a {
  text-decoration: none;
}

nav a.Router-link-exact-active button {
  background-color: blue;
}

i:hover {
  transform: scale(1.2);
}

</style>