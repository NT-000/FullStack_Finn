<script setup>
const props = defineProps({
  user: Object,
  ads: Array,
})
</script>

<template>
  <div v-if="user && user.name">
  <h2>{{ user.name }}'s page</h2>
    <img :src="`${user.profileImageUrl}`" alt="profilePic"/>
    <p><strong>Name:</strong> {{ user.name }}</p>
    <p><strong>Email:</strong> {{ user.email }}</p>
    <p><strong>Rating:</strong> {{ user.rating }}</p>
    <div v-if="ads.length > 0">
      <div v-for="ad in ads" :key="ad.id">
        <div class="ad">
          <RouterLink :to="{name: 'AdDetails', params:{id:ad.id}}"> {{ ad.title }}</RouterLink>
          <div v-if="ad.images.length > 0" class="image">
            <div v-for="image in ad.images" :key="image.id">
              <img :src="image.imageUrl" alt="x"/>
            </div>
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
  width: 20%;
  height: 20%;
  border-radius: 5px;
  border: solid 1px white;
}
</style>