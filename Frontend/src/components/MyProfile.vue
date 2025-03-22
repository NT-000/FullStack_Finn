<script setup>
const props = defineProps({
  user: Object,
  ads: Array,
})
</script>

<template>
  <div v-if="user && user.name" class="container">
    <h2>{{ user.name }}'s page</h2>
    <img :src="`${user.profileImageUrl}`" alt="profilePic"/>
    <p><strong>Name:</strong> {{ user.name }}</p>
    <p><strong>Email:</strong> {{ user.email }}</p>
    <p><strong>Rating:</strong> {{ user.rating }}</p>
    <div v-if="ads.length > 0">
      <div v-for="ad in ads" :key="ad.id">
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
.container {
  display: inline-block;

}

img {
  width: 10%;
  height: 30%;
  border-radius: 5px;
  border: solid 1px white;
}
</style>