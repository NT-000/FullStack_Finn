<script setup>
import {computed, onMounted} from "vue";
import {useStarRating} from '../composables/useStarRating.js'

const props = defineProps({
  user: Object,
  ads: Array,
})

const userStars = computed(() =>{
  if (!props.user) {
    return;
  }
  return useStarRating(props.user.rating).value;
});
  
</script>

<template>
  <div class="container" v-if="user">
    <h2>{{ user.name }}'s page</h2>
    <div v-if="user && user.name">
      <img class="profilePic" :src="user.profileImageUrl" alt="profilePic"/>
      <br>
      <RouterLink :to="{name: 'Chat', params:{id:user.id}}"><button><i class="fa-solid fa-message">Send melding</i></button></RouterLink>
    
      <p><strong>Navn:</strong> {{ user.name }}</p>
      <p><strong>Rangering:</strong> <span v-html="userStars"></span></p>
      <p><strong>Email:</strong> {{ user.email }}</p>
      <p><strong>Vurdering:</strong> {{ user.rating }} </p>
  </div>
    </div>
  <h2>Aktive annonser</h2>
  <div class="adImages" v-if="ads && ads.length > 0">
    <div v-for="ad in ads" :key="ad.id" class="adContainer">
      <RouterLink :to="{name: 'AdDetails', params:{id: ad.id}}">
        <div class="ad">
          <h6>{{ ad.title }}</h6>
          <div v-if="ad.images && ad.images.length > 0">
            <img :src="ad.images[0].imageUrl" alt="img"/>
          </div>
        </div>
      </RouterLink>
    </div>
  </div>
  <div v-else>
    <i class="fa-solid fa-circle-notch fa-spin"></i>
  </div>
</template>

<style scoped>
.container {
  background-color: #6cb4da;
  padding: 20px;
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0, 0, 1, 0.1);
  max-width: 600px;
  margin: 20px auto;
  text-align: center;
  color: #00263b;
}


.profilePic {
  height: 150px;
  width: 150px;
  border-radius: 50%;
  border: 3px solid #98cbe8;
  object-fit: cover;
  margin-bottom: 20px;
}


.container h2 {
  margin-bottom: 10px;
}

.container p {
  margin: 5px 0;
}


.adImages {
  display: flex;
  gap: 20px;
  max-width: 200px;
  align-items: center;
  margin-top: 20px;
  max-lines: 15;
}


.ad {
  background-color: #98cbe8;
  color: #ffffff;
  border: 2px solid blue;
  border-radius: 8px;
  padding: 15px;
  width: 200px;
  max-width: 175px;
  max-height: 250px;
  transition: transform 0.3s, background-color 0.3s, color 0.3s;
}


.ad:hover {
  background-color: #ffffff;
  color: #00263b;
  transform: scale(1.05);
}

.ad img {
  width: 100px;
  height: 100px;
  max-width: 150px;
  max-height: 150px;
  border-radius: 5px;
  border: 1px solid #e6f2fa;
  margin-bottom: 10px;
}

</style>