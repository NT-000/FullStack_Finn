<script setup>
import {computed, ref} from 'vue'
import axios from 'axios'
import { useUserStore } from '../stores/useUserStore.js';

const userStore = useUserStore();

const title = ref('')
const description = ref('')
const category = ref('')
const condition = ref('')
const price = ref(0)
const userId = computed(() => userStore.user.id || null);
const files = ref([]) // filer brukeren laster opp

async function handleSubmit(e) {
  e.preventDefault()

  // lager formdata
  const formData = new FormData()
  formData.append('Title', title.value)
  formData.append('Description', description.value)
  formData.append('Category', category.value)
  formData.append('Condition', condition.value)
  formData.append('Price', price.value)
  formData.append('UserId', userId.value)

  // legger til alle bildene(filene)
  for (let i = 0; i < files.value.length; i++) {
    formData.append('Files', files.value[i])
  }

  try {
    const res = await axios.post('/api/ads/create-with-files', formData, {
      withCredentials: true
    })
    console.log('Annonsen ble opprettet:', res.data)
  } catch (err) {
    console.error('Feil ved opplasting:', err)
  }
}

function handleFileChange(e) {
  files.value = e.target.files // oppdaterer files[] med valgte filer
}
</script>

<template>
  <h2>Create new ad(w files)</h2>
  <form @submit.prevent="handleSubmit">
    <div>
      <label>Title</label>
      <input v-model="title" type="text"/>
    </div>
    <div>
      <label>Category</label>
      <select v-model="category">
        <option>Bikes</option>
        <option>Electronics</option>
        <option>Furniture</option>
        <option>Clothing</option>
        <option>Guns</option>
        <option>Hand-weapons</option>
      </select>
    </div>
    <div>
      <label>Price</label>
      <input v-model="price" type="number"/>
    </div>
    <div>
      <label>Description</label>
      <textarea v-model="description"></textarea>
    </div>
    <div>
      <label>Condition</label>
      <select v-model="condition">
        <option>New</option>
        <option>Like new</option>
        <option>Used</option>
        <option>Well used</option>
      </select>
    </div>
    <br>
    <div>
      <label>Images</label>
      <input multiple type="file" @change="handleFileChange"/>
    </div>
    <button class="btn waves-effect waves-light" name="action" type="submit">Create new ad
    </button>
  </form>
</template>