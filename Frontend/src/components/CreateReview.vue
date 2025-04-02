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
const message = ref(null)

const handleSubmit = async () => {
  const review = {
    adId: adId.value,
    toUserId: userId.value,
    rating: rating.value,
    comment: reviewComment.value,
  }
  try {
    await axios.post("/api/reviews", review, {headers: {"Content-Type": "application/json"}, withCredentials: true});
    router.push(`/ads/${adId.value}`);
    message.value = "Anmeldelse sendt inn!";
  } catch (err) {
    console.log("couldn't submit review", err.message)
  }
}
</script>

<template>
  <form class="formComment" @submit.prevent="handleSubmit">
    <div>
      <label for="comment">Legg igjen en kommentar</label>
      <br>
      <textarea id="comment" v-model="reviewComment" placeholder="" required/>
      <br>
      <label for="rating">Legg igjen en vurdering</label>
      <br>
      <div class="star-rating">
        <i
            v-for="star in 5"
            :key="star"
            :class="rating >= star ? 'fa-solid fa-beat' : 'fa-regular'"
            class="fa-star "
            style="color: #FFD43B; cursor: pointer;"
            @click="rating = star"
        ></i>
      </div>
      <br>
      <button class="submit" type="submit">Send inn tilbakemelding</button>
    </div>

  </form>
</template>

<style scoped>

.formComment {
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  border-radius: 10px;
  border: 5px solid blue;
  background-color: lightskyblue;
}

.star-rating i {
  font-size: 2rem;
  margin-right: 0.25rem;
  transition: transform 0.2s, color 0.2s;
}

.fa-star:hover {
  transform: scale(1.2);
}

button:hover {
  transform: scale(1.2);
}

button {
  padding: 1rem;
}
</style>