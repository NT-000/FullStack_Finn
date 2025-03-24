<script setup>
const props = defineProps({
  user: Object,
  ads: Array,
})
</script>

<template>
  <div v-if="user && user.name" class="container">
    <h2>{{ user.name }}'s page</h2>
    <img class="profilePic" :src="`${user.profileImageUrl}`" alt="profilePic"/>
    <p><strong>Navn:</strong> {{ user.name }}</p>
    <p><strong>Email:</strong> {{ user.email }}</p>
    <p><strong>Rangering:</strong> {{ user.rating }}</p>
  </div>
  <div v-else>
    <p>Loading user...</p>
  </div>
    <div class="adImages" v-if="ads.length > 0">
      <div v-for="ad in ads" :key="ad.id">
          <RouterLink :to="{name: 'AdDetails', params:{id:ad.id}}"> {{ ad.title }}
        <div class="ad">
          <div v-if="ad.images.length > 0">
            <img :src="ad.images[0].imageUrl" alt="img"/>
          </div>
        </div>
      </RouterLink>
      </div>
    </div>
</template>

<style scoped>
.container {
  display: flex;
  flex-direction: column;
  padding: 10px;
}
.ad{
  padding: 10px;
  gap: 10px;
  text-align: center;
  background: lightskyblue;
  border: 3px solid blue;
  justify-content: space-between;
  border-radius: 10px;
  height: 15vh;
  width: 15vw;
}
.ad:hover{
  background: white;
  transform: scale(1.1);
}
.profilePic {
  height: 20vh;
  width: 10vw;
  position: absolute;
  left: 450px;
}

.adImages {
  display: flex;
  flex-direction: row;
}

img {
  width: 15vw;
  height: 15vh;
  border-radius: 5px;
  border: solid 1px white;
}
</style>