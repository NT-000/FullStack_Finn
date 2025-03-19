<script setup>
import { computed, watchEffect, onMounted } from "vue";
import { useStore } from "../composables/useStore";
import { useUser } from "../composables/useUser";

const store = useStore();
const { fetchUserProfile } = useUser();

const computedUser = computed(() => store.user.value);

onMounted(async () => {
  await fetchUserProfile();
});

watchEffect(() => {
  console.log("change in store.user:", computedUser.value);
});
</script>

<template>
  <h2>My profile</h2>
  <div v-if="computedUser && computedUser.name">
    <p><strong>Name:</strong> {{ computedUser.name }}</p>
    <p><strong>Email:</strong> {{ computedUser.email }}</p>
  </div>
  <div v-else>
    <p>Loading user...</p>
  </div>
</template>
