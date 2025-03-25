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

    <div>
      <div v-if="currentUser && currentUser.name" class="user-display">
        <i class="fa-solid fa-lock-open"></i>
        {{ currentUser.name }}
      </div>
      <p v-else></p>
    </div>
    
    <div class="nav-bar" v-if="currentUser && currentUser.email">
      <button v-if="currentUser && currentUser.email" @click="logOut">
        <i class="fa-solid fa-arrow-right-from-bracket"></i>
      </button>

      <RouterLink to="/mainpage">
        <button><i class="fa-solid fa-house"></i></button>
      </RouterLink>

      <RouterLink to="/ads">
        <button>
          <svg xmlns="http://www.w3.org/2000/svg" enable-background="new 0 0 20 20" height="20px"
               viewBox="0 0 20 20" width="20px" fill="#e8eaed">
            <g>
              <rect fill="none" height="20" width="20" x="0"/>
            </g>
            <g>
              <g/>
              <g>
                <path d="M14,15H5V6h6V5H5C4.45,5,4,5.45,4,6V15c0,0.55,0.45,1,1,1H14c0.55,0,1-0.45,1-1V9h-1V15z"/>
                <polygon points="7,8 7,9 11,9 12,9 12,8"/>
                <rect height="1" width="5" x="7" y="10"/>
                <rect height="1" width="5" x="7" y="12"/>
                <polygon points="15,3 14,3 14,5 14,5 12,5 12,6 14,6 14,8 15,8 15,6 15,6 17,6 17,5 15,5"/>
              </g>
            </g>
          </svg>
        </button>
      </RouterLink>

      <RouterLink to="/users/profile">
        <button><i class="fa-solid fa-circle-user"></i></button>
      </RouterLink>

      <RouterLink to="/ads/create-with-files">
        <button><i class="fa-solid fa-file-circle-plus"></i></button>
      </RouterLink>

      <RouterLink to="/inbox">
        <button><i class="fa-solid fa-inbox"></i></button>
      </RouterLink>

      <RouterLink to="/map">
        <button><i class="fa-solid fa-map-location-dot"></i></button>
      </RouterLink>
    </div>
  </nav>
</template>

<style scoped>

nav {
  display: flex;
  justify-content: space-between;
  align-items: center;
  border-radius: 5px;
  background-color: #e6f2fa; 
  padding: 20px 20px;
  border-bottom: 2px solid #b3d4e0; 
}

.user-display {
  font-size: 1.2rem;
  font-weight: 500;
  color: #00263b; 
  display: flex;
  align-items: center;
  gap: 8px;
}

.nav-bar {
  display: flex;
  gap: 15px;
  background-color: #ffffff; 
  padding: 10px;
  border-radius: 8px; 
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1); 
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

@media (max-width: 600px) {
  nav {
    flex-direction: column;
  }

  .nav-bar {
    flex-wrap: wrap;
    justify-content: center;
    gap: 10px;
  }

  button {
    font-size: 0.9rem;
    padding: 5px 8px;
  }
}
</style>