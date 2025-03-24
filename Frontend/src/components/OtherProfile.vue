<script setup>
const props = defineProps({
  user: Object,
  ads: Array,
})

const snippet = ad.images
</script>

<template>
  <div class="container" v-if="user">
    <h2>{{ user.name }}'s page</h2>
    <div v-if="user && user.name">
      <img class="profilePic" :src="`${user.profileImageUrl}`" alt="profilePic"/>
      <br>
    <RouterLink :to="{name: 'Chat', params:{id:user.id}}"><button>Send melding</button></RouterLink>
      <p><strong>Navn:</strong> {{ user.name }}</p>
      <p><strong>Email:</strong> {{ user.email }}</p>
      <p><strong>Rangering:</strong> {{ user.rating }}</p>
  </div>
      <div v-if="ads.length > 0">
        <div class="adImgs" v-for="ad in ads" :key="ad.id">
          <div class="ad">
            <RouterLink :to="{name: 'AdDetails', params:{id:ad.id}}"> {{ ad.title }}</RouterLink>
            <div v-if="ad.images.length > 0">
              <img :src="ad.images[0].imageUrl" alt="img"/>
            </div>
          </div>
        </div>
      </div>
    </div>
  <div v-else>
    <p>Loading user...</p>
  </div>
</template>

<style scoped>
img {
  width: 10vh;
  height: 10vw;
  border-radius: 5px;
  border: solid 1px white;
}
.container{
  display: flex;
  flex-direction: column;
}
.profilePic{
  width: 10vw;
  height: 10vw;
}
.adImgs{
  width: 10vw;
  height: 10vw;
  display: flex;
  flex-direction: row;
  align-items: center;

  justify-content: center;
}

</style>