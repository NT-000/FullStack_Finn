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
		router.push(`/users/${userId.value}`);
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
				<div
						v-for="star in 5"
						:key="star"
						style ="cursor: pointer;"
						@click="rating = star">
				<img v-if="rating>= star" alt=""
				     src="/f1.png"
				style="height: 40px; width: 40px;"/>
			<div v-else>
     <img src="/f4.png"
      style="height: 40px; width: 40px;"
          />
			</div>
				</div>
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
	border: 5px solid darkgreen;
	
	color: #00ff66;
}

.star-rating i {
	font-size: 2rem;
	margin-right: 0.25rem;
	transition: transform 0.2s, color 0.2s;
}

.star-rating img:hover {
	animation: pulse 1.8s infinite ease-in-out;
}
@keyframes pulse {
	0%, 100% {
		box-shadow: 0 0 10px rgba(0, 255, 102, 0.5);
	}
	50% {
		box-shadow: 0 0 30px rgba(0, 255, 102, 0.9);
	}
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