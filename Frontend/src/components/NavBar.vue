<script setup>
import {computed, watchEffect} from 'vue'
import {useLogout} from "../composables/useLogout";
import {useUserStore} from '../stores/useUserStore.js';

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
    <div >
    <div v-if="currentUser && currentUser.name">
      <i class="fa-solid fa-lock-open"></i>{{ currentUser.name }}
    </div>
    <p v-else></p>
    <div class="nav-bar" v-if="currentUser  && currentUser.email">
      <div>
    <button v-if="currentUser && currentUser.email" @click="logOut"><i class="fa-solid fa-arrow-right-from-bracket"></i></button>
      </div>
<!--      <div>-->
      <RouterLink to="/mainpage">
        <button><i class="fa-solid fa-house"></i></button>
      </RouterLink>
<!--      </div>-->
<!--      <div>-->
      <RouterLink to="/ads">
        <button><svg xmlns="http://www.w3.org/2000/svg" enable-background="new 0 0 20 20" height="20px" viewBox="0 0 20 20" width="20px" fill="#e8eaed"><g><rect fill="none" height="20" width="20" x="0"/></g><g><g/><g><path d="M14,15H5V6h6V5H5C4.45,5,4,5.45,4,6V15c0,0.55,0.45,1,1,1H14c0.55,0,1-0.45,1-1V9h-1V15z"/><polygon points="7,8 7,9 11,9 12,9 12,8"/><rect height="1" width="5" x="7" y="10"/><rect height="1" width="5" x="7" y="12"/><polygon points="15,3 14,3 14,5 14,5 12,5 12,6 14,6 14,8 15,8 15,6 15,6 17,6 17,5 15,5"/></g></g></svg></button>
      </RouterLink>
<!--      </div>-->
<!--      <div>-->
      <RouterLink to="/users/profile">
        <button><i class="fa-solid fa-circle-user"></i></button>
      </RouterLink>
<!--    </div>-->
<!--      <div>-->
      <RouterLink to="/ads/create-with-files">
        <button><i class="fa-solid fa-file-circle-plus"></i></button>
      </RouterLink>
<!--    </div>-->
<!--      <div>-->
      <RouterLink to="/inbox">
        <button><i class="fa-solid fa-inbox"></i></button>
      </RouterLink>
<!--    </div>-->
<!--      <div>-->
      <RouterLink to="/map">
        <button><i class="fa-solid fa-map-location-dot"></i></button>
      </RouterLink>
<!--    </div>-->
    </div>
  </div>
  </nav>
</template>

<style scoped>
.nav-bar{
  display: flex;
  gap: 30px;
  padding: 30px;
  margin-right: 5px;
  max-width: 700px;
  justify-content: center;
}
nav {
  display: flex;
  align-items: center;
}
nav a{
  padding: 10px;
}
nav a.Router-link-exact-active {
  color: gray;
}
button{
  gap: 10px;

  font-size: 1.5vw;
  
}
</style>