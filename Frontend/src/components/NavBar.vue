<script setup>
import {computed, onMounted, watchEffect} from 'vue'
import { useLogout } from "../composables/useLogout";
import store from "../composables/useStore";

const {user} = store;
const { logOut } = useLogout();

const currentUser = computed(() => store.user.value)

watchEffect(()=>{
  console.log("currentUser changed: ", currentUser.value)
})

</script>

<template>
  <nav>
    <p v-if="currentUser  && currentUser.email">
      Logged in user: {{ currentUser.email }}
    </p>
    <p v-else></p>
    <button v-if="currentUser && currentUser.email" @click="logOut">Logg ut</button>
    <div v-if="currentUser  && currentUser.email">
      <RouterLink to="/mainpage"><button>Main Page</button></RouterLink>
      <RouterLink to="/ads"><button>Annonser</button></RouterLink>
      <RouterLink to="/users/profile"><button>Profile</button></RouterLink>
      <RouterLink to="/ads/create-with-files"><button>Create new ad</button></RouterLink>
    </div>
  </nav>
</template>