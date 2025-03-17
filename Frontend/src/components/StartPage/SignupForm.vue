<script setup>
import {ref} from 'vue';

const newUser = ref({
  email: '',
  password: '',
  name: '',
})
</script>

<template>
<form @submit.prevent="registerUser" class="form">
  <label for="name" class="form-label">Name</label>
  <input type="text" placeholder="Your name..."  v-model="newUser.name" required/>
  <label for="password" class="form-label">Password</label>
  <input type="password" placeholder="Your password..." v-model="newUser.password" required />
  <label for="email" class="form-label">Email</label>
  <input type="text" placeholder="Your email..." v-model="newUser.email" required />
  <button type="submit" class="btn btn-primary">Submit</button>
</form>
</template>

<style scoped>

</style>