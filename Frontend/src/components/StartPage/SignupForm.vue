<script setup>
import {ref} from 'vue';
import axios from "axios";
import router from "../../router/index.js";

const name = ref('');
const email = ref('');
const password = ref('');
const file = ref(null);

function handleFileChange(e) {
  file.value = e.target.files[0];
}

async function handleSubmit() {
  const formData = new FormData();
  formData.append('Name', name.value);
  formData.append('Email', email.value);
  formData.append('Password', password.value);
  formData.append('file', file.value);

  try {
    const res = await axios.post('/api/users/register', formData, { withCredentials: true });
    console.log("response data reg user:",res.data);
    router.push('/login');
  } catch (err) {
    console.log("Registration failed",err);
  }
}

</script>

<template>
<form @submit.prevent="handleSubmit" class="form">
  <label for="name" class="form-label">Name</label>
  <input type="text" placeholder="Your name..."  v-model="name" required/>
  <label for="password" class="form-label">Password</label>
  <input type="password" placeholder="Your password..." v-model="password" required />
  <label for="email" class="form-label">Email</label>
  <input type="text" placeholder="Your email..." v-model="email" required />
  <label>Images</label>
  <input type="file" @change="handleFileChange" />
  <button type="submit" class="btn btn-primary">Submit</button>
</form>
</template>

<style scoped>

</style>