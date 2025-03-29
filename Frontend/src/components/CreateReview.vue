<script setup>
import {ref} from "vue";
import axios from "axios";
import {useRouter} from "vue-router";

const props = defineProps({
  currentAd: Object,
  
})

const router = useRouter();

const adId = ref(props.currentAd.id);
const userId = ref(props.currentAd.userId);
const rating = ref(null)
const reviewComment = ref(null)

const handleSubmit = async () => {
  const review = {
    adId: adId.value,
    toUserId: userId.value,
    rating: rating.value,
    comment: reviewComment.value,
  }
  try{
    await axios.post("/api/reviews", review, {headers: {"Content-Type": "application/json"}, withCredentials: true});
    router.push(`/ads/${adId.value}`);
  }catch(err){
    console.log("couldn't submit review",err.message)
  }
}
</script>

<template>
<form @submit.prevent="handleSubmit">
  <div>
    <label for="comment">Legg igjen en kommentar</label>
    <br>
    <textarea id="comment" v-model="reviewComment" placeholder="" required />
    <br>
    <label for="rating">Legg igjen en vurdering</label>
    <br>
    
      <div class="star-rating">
        <i
            v-for="star in 5"
            :key="star"
            class="fa-star"
            :class="rating >= star ? 'fa-solid' : 'fa-regular'"
            @click="rating = star"
            style="color: #FFD43B; cursor: pointer;"
        ></i>
      </div>
    <button class="submit" type="submit">Send inn tilbakemelding</button>
  </div>
  
</form>
</template>

<style scoped>
.star-rating i {
  font-size: 2rem;
  margin-right: 0.25rem;
}
</style>