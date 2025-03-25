<script setup>
const props = defineProps({
  user: Object,
  ads: Array,
})
</script>

<template>
  <div v-if="user && user.name" class="container">
    <h2>{{ user.name }}'s page</h2>
    <img class="profilePic" :src="user.profileImageUrl" alt="profilePic"/>
    <p><strong>Navn:</strong> {{ user.name }}</p>
    <p><strong>Email:</strong> {{ user.email }}</p>
    <p><strong>Rangering:</strong> {{ user.rating }}</p>
  </div>
  <div v-else>
    <i class="fa-solid fa-circle-notch fa-spin"></i>
  </div>
  <div class="adImages" v-if="ads && ads.length > 0">
    <div v-for="ad in ads" :key="ad.id" class="adContainer">
      <RouterLink :to="{name: 'AdDetails', params:{id: ad.id}}">
        <div class="ad">
          <h3>{{ ad.title }}</h3>
          <div v-if="ad.images && ad.images.length > 0">
            <img :src="ad.images[0].imageUrl" alt="img"/>
          </div>
        </div>
      </RouterLink>
    </div>
  </div>
</template>

<style scoped>
.container {
  background-color: #ffffff; 
  padding: 20px;
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
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
  flex-wrap: wrap;
  gap: 20px;
  justify-content: center;
  margin-top: 20px;
}


.ad {
  background-color: #98cbe8; 
  color: #ffffff; 
  border: 2px solid blue;
  border-radius: 8px;
  padding: 15px;
  width: 200px;
  text-align: center;
  transition: transform 0.3s, background-color 0.3s, color 0.3s;
}


.ad:hover {
  background-color: #ffffff;
  color: #00263b;
  transform: scale(1.05);
}

.ad img {
  width: 100%;
  height: auto;
  border-radius: 5px;
  border: 1px solid #e6f2fa;
  margin-bottom: 10px;
}
</style>
