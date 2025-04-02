<script setup>
import {computed, onMounted, ref} from "vue";
import {useStarRating} from '../composables/useStarRating.js'
import {useDateFormat} from "../composables/useFormatDate.js";
import {getRoute} from "../composables/getRoute.js";
import {useRoute} from "vue-router"
import {useFavStore} from "../stores/favStore.js";

const props = defineProps({
  user: Object,
  ads: Array,
})

const route = useRoute()
const reviewsForUser = getRoute(`/reviews/${route.params.id}`);
const users = getRoute(`/users`);
const favStore = useFavStore();
const {getStars} = useStarRating();
const dateFormat = useDateFormat();
const isActiveReviews = ref(true)
const isActiveAd = ref(false)
const getReviewer = (fromUserId) => {
  return users.items.value.find(user => user.id === fromUserId)
}
console.log("user", props.user)

onMounted(async () => {
  await reviewsForUser.fetchData()
  await users.fetchData()
})

const calculateAverageRating = computed(() => {
  const reviews = reviewsForUser.items.value;
  if (reviews.length === 0) {
    return 0;
  }
  let total = 0;
  for (let i = 0; i < reviews.length; i++) {
    total += reviews[i].rating;
  }
  return total / reviews.length;
})

</script>

<template>
  <div v-if="user" class="container">
    <h2>{{ user.name }} sin side</h2>
    <div v-if="user && user.name">
      <img :src="user.profileImageUrl" alt="profilePic" class="profilePic"/>
      <br>
      <!--      <RouterLink :to="{name: 'Chat', params:{id:user.id}}"><button><i class="fa-solid fa-message">Send melding</i></button></RouterLink>-->
      <p><strong>Navn:</strong> {{ user.name }}</p>
      <p><strong>Rangering:</strong> <span v-html="getStars(calculateAverageRating)"></span></p>
      <p><strong>Email:</strong> {{ user.email }}</p>
    </div>
  </div>
  <h2 @click="isActiveAd = !isActiveAd">Aktive annonser<i class="fa-solid fa-rectangle-ad" title="Annonser"></i></h2>
  <div v-if="ads && ads.length > 0 && isActiveAd" class="adImages">
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

  <h2 v-if="user" title="Trykk for å åpne" @click="isActiveReviews = !isActiveReviews">Anmeldelser av
    {{ user.name }}</h2>
  <div v-if="isActiveReviews && reviewsForUser?.items?.value?.length > 0" class="adReview">
    <div v-for="review in reviewsForUser.items.value" :key="review.id">
      <div class="innerReview">
        <RouterLink v-if="getReviewer(review.fromUserId)"
                    :to="{name: 'UserProfile', params:{id: getReviewer(review.fromUserId).id}}" class="reviewLink">
          <div v-if="getReviewer(review.fromUserId)" class="review"><img
              :src="getReviewer(review.fromUserId).profileImageUrl" alt="img"/>{{ getReviewer(review.fromUserId).name }}
          </div>
        </RouterLink>

        <RouterLink :to="{name:'AdDetails', params:{id: review.adId}}" class="reviewLink">
          <small>{{ review.comment }}</small>
          <br>
          <div><span v-html="getStars(review.rating)"></span></div>
          <small>{{ dateFormat.formatDate(review.createdAt) }}</small>
        </RouterLink>
      </div>
    </div>
  </div>
  <div v-else-if="isActiveReviews">
    Ingen vurderinger...
  </div>

</template>

<style scoped>
.container {
  background-color: #6cb4da;
  padding: 20px;
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(2, 3, 2, 0.1);
  max-width: 600px;
  margin: 20px auto;
  text-align: center;
  color: #00263b;
  border: 5px solid #00263b;
}

.container h2,
h2 {
  cursor: pointer;
  margin-bottom: 10px;
  padding: 5px;
  margin-left: 10px;
}

h2:hover {
  background-color: #98cbe8;
  border-radius: 5px;
}

.container p {
  margin: 5px 0;
  padding: 5px;
}

.profilePic {
  height: 150px;
  width: 150px;
  border-radius: 50%;
  border: 5px solid #98cbe8;
  object-fit: cover;
  margin-bottom: 20px;
}

.adImages {
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 20px;
  padding: 10px;
}

.adContainer {

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

.adReview {
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 20px;
}

.innerReview {
  border: 5px solid #98cbe8;
  display: inline-block;
  font-family: "Comic Sans MS", cursive;
  padding: 10px;
  border-radius: 10px;
}

.review {
  font-weight: bolder;
  padding: 5px;
  display: flex;
  justify-content: center;
  align-items: center;
  border-bottom: 3px solid #e6f2fa;
}

.innerReview img {
  height: 50px;
  width: 50px;
  border-radius: 25px;
  border: 3px solid #98cbe8;
  margin: 10px;
}

i {
  padding: 10px;
  margin-right: 10px;
}

.reviewLink:hover {
  color: black;

  border-radius: 5px;
}

.reviewLink {
  color: white;
}

</style>