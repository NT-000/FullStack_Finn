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
const message = ref('')
const error = ref('')

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
    message.value = "Annonsen ble opprettet!"
  } catch (err) {
    error.value = "Feil ved opplasting!"
  }
}

function handleFileChange(e) {
  files.value = e.target.files // oppdaterer files[] med valgte filer
}
</script>

<template>
  <div class="container">
    <h1>Lag en ny annonse</h1>
    <form @submit.prevent="handleSubmit">
      <div class="title">
        <label>Tittel<i class="fa-solid fa-scroll"></i></label>
        <br>
        <input v-model="title" type="text"/>
      </div>
      <div class="category">
        <label>Kategori<i class="fa-solid fa-tags"></i></label>
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
        <label>Pris<i class="fa-solid fa-coins"></i></label>
        <br>
        <input v-model="price" type="number"/>
        <br>
      </div>
      <div>
        <label>Beskrivelse<i class="fa-solid fa-pencil"></i></label>
        <br>
        <textarea v-model="description" placeholder="Beskriv varen..."></textarea>
      </div>
      <div>
        <label>Tilstand<i class="fa-solid fa-heart"></i></label>
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
        <button>
          <span>Legg til bilder<i class="fa-solid fa-circle-plus"></i></span>
          <input hidden="" multiple type="file" @change="handleFileChange"/>
        </button>
      </div>
      <button type="submit">Lag en ny annonse<i class="fa-solid fa-file-circle-plus"></i></button>
      <div>{{ error || message }}</div>
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

h1 {
  border-bottom: 5px solid black;
}

input {
  border: none;
  border-radius: 5px;
  max-width: 22vw;
  width: 22vw;
}

textarea {
  border: none;
  border-radius: 5px;
  max-width: 22vw;
  max-height: 15vh;
  width: 150vw;
  height: 150vh;
  text-align: center;
  align-items: center;
  justify-content: center;
  display: flex;
}

select {
  border: none;
  border-radius: 5px;
  max-width: 22vw;
  width: 22vw;

}

label {
  font-size: 1.2rem;
  font-weight: bolder;
  margin-top: 30px;
}

button {
  padding: 20px;
  margin: 10px;
  font-size: 1.2rem;
}

i {
  margin-left: 15px;
  font-size: 1.5rem;
}

</style>