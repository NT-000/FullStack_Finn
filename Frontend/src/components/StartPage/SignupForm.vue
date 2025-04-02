<script setup>
import {ref} from 'vue';
import axios from "axios";
import router from "../../router/index.js";

const name = ref('');
const email = ref('');
const password = ref('');
const file = ref(null);

const props = defineProps({
  isUser: Boolean
})
const emit = defineEmits(['update:isUser'])

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
    const res = await axios.post('/api/users/register', formData, {withCredentials: true});
    console.log("response data reg user:", res.data);
    router.push('/');
  } catch (err) {
    console.log("Registration failed", err);
  }
}

const toggle = () => {
  emit('update:isUser', true)
}
</script>

<template>
  <div class="container">
    <form class="form" @submit.prevent="handleSubmit">
      <label class="form-label" for="name">Navn</label>
      <input v-model="name" placeholder="Navn..." required type="text"/>
      <label class="form-label" for="password">Passord</label>
      <input v-model="password" placeholder="Passord..." required type="new-password"/>
      <label class="form-label" for="email">Email</label>
      <input v-model="email" placeholder="Email..." required type="text"/>
      <label>Bilder</label>
      <button>
        <input hidden="" type="file" @change="handleFileChange"/>
        <span>Legg til <i class="fa-solid fa-image"></i></span>
      </button>
      <br>
      <button class="btn btn-primary" type="submit">Registrer</button>
      <button type="button" @click="toggle">Tilbake til innlogging</button>
    </form>
  </div>

</template>

<style scoped>
.container {
  background: lightskyblue;
  padding: 40px;
  border-radius: 10px;
  margin-left: 40vw;
  width: 100vw;
  height: 50vh;
}

.form {
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
}

input {
  border: none;
  background: ghostwhite;
  border-radius: 5px;
  padding: 20px;
  font-size: 20px;
  font-weight: bolder;
  color: black;
  width: 100%;
}

input:focus {
  outline: none;
  border: none;
}

input:hover {
  outline: none;
  border: blue;
}


label {
  color: black;
  font-weight: bold;
  font-size: 30px;
  padding: 20px;
}

button {
  border: none;
  margin-bottom: 5px;
  font-size: 1.2rem;
}

button:hover {
  transition: filter 300ms;
  filter: drop-shadow(0 0 2em #646cffaa);
}

.buttonRegister:hover {
  
}
</style>