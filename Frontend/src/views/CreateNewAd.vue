<script setup>
import {computed, ref} from 'vue'
import axios from 'axios'
import {useUserStore} from '../stores/useUserStore.js';

const userStore = useUserStore();

const title = ref('')
const description = ref('')
const category = ref('')
const condition = ref('')
const price = ref(0)
const userId = computed(() => userStore.user.id || null);
const files = ref([]) // filer brukeren laster opp

async function handleSubmit() {
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
  <div class="container">
  <h2>Lag en ny annonse</h2>
  <form @submit.prevent="handleSubmit">
    <div class="title">
      <label>Tittel</label>
      <br>
      <input v-model="title" type="text"/>
    </div>
    <div class="category">
      <label>Kategori</label>
      <br>
      <select v-model="category">
        <option>Sykler</option>
        <option>Leker</option>
        <option>Electronikk</option>
        <option>Møbler</option>
        <option>Klær</option>
        <option>Skytevåpen</option>
        <option>Hånd-våpen</option>
      </select>
      
    </div>
    <div>
      <label>Pris</label>
      <input v-model="price" type="number"/>
      <br>
    </div>
    <div>
      <label>Beskrivelse</label>
      <br>
      <textarea v-model="description"></textarea>
    </div>
    <div>
      <label>Tilstand</label>
      <br>
      <select v-model="condition">
        <option>Ny</option>
        <option>Nesten ny</option>
        <option>Brukt</option>
        <option>Godt brukt</option>
      </select>
    </div>
    <br>
    <div>
      <label>Images</label>
      <br>
      <input multiple type="file" @change="handleFileChange"/>
    </div>
    <button type="submit">Lag en ny annonse</button>
  </form>
  </div>
</template>

<style scoped>

.container {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  background-color: lightblue;
  width: 60vw;
  height: 80vh;
  padding: 30px;
  margin-top: 1vh;
  border-radius: 10px;
}
input {
  border: none;
  border-radius: 5px;
  
}
textarea {
  border: none;
  border-radius: 5px;
  max-width: 250px;
  max-height: 250px;
  width: 250px;
  height: 250px;
}
select {
  border: none;
  border-radius: 5px;
  width: 20vw;
  
}
label{
font-size: 1.2rem;
  font-weight: bold;
}

</style>