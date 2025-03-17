<script setup>
import { computed, onMounted } from 'vue'
import { useLogout } from "../composables/useLogout";
import useStore from "../composables/useStore";

const store = useStore();
const { logOut } = useLogout();

const currentUser = computed(() => store.user.value)

</script>

<template>
  <nav>
    <p v-if="currentUser">
      Logged in user: {{ currentUser.email }}

    </p>
    <button v-if="currentUser" @click="logOut">Logg ut</button>
    <div v-if="currentUser">
      <RouterLink to="/mainpage"><button>Main Page</button></RouterLink>
      <RouterLink to="/ads"><button>Annonser</button></RouterLink>
      <RouterLink to="/users/profile"><button>Profile</button></RouterLink>
      <RouterLink to="/ads/create-with-files"><button>Create new ad</button></RouterLink>
    </div>
    <div v-else>
      <RouterLink to="/"><button>Log in / Register</button></RouterLink>
    </div>
  </nav>
</template>