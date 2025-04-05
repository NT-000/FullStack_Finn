<script setup>
import {computed, onMounted, ref} from "vue";
import {useStarRating} from "../composables/useStarRating.js";
import {useAdStore} from "../stores/adStore.js";
import {getRoute} from "../composables/getRoute.js";
import {useDateFormat} from "../composables/useFormatDate.js";
import {useFavStore} from "../stores/favStore.js"
import AdDetails from "../views/AdDetails.vue";

const props = defineProps({
	user: Object,
	ads: Array,
})

const favStore = useFavStore();
const adStore = useAdStore();
const {getStars} = useStarRating();
const reviewsForUser = getRoute(`/reviews/${props.user.id}`);
const users = getRoute(`/users`);
const dateFormat = useDateFormat();
const isActiveReviews = ref(true)
const isActiveAds = ref(false);
const isActiveBoughtAds = ref(false);
const isFavoriteAds = ref(false);


const getReviewer = (fromUserId) => {

	return users.items.value.find(user => user.id === fromUserId)
}

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

onMounted(async () => {
	await adStore.fetchBoughtAds()
	await reviewsForUser.fetchData()
	await users.fetchData()
	await favStore.fetchFavorites()
})
</script>


<template>

	<div v-if="user && user.name" class="container">
		<h2>Min konto</h2>
		<img :src="user.profileImageUrl" alt="profilePic" class="profilePic"/>
		<p><strong>Navn:</strong> {{ user.name }}</p>
		<p><strong>Email:</strong> {{ user.email }}</p>
		<p><strong>Vurdering:</strong> <span v-html="getStars(calculateAverageRating)"></span></p>
	</div>
	<div v-else>
		<i class="fa-solid fa-circle-notch fa-spin"></i>
	</div>
	<h2 v-if="favStore" @click="isFavoriteAds = !isFavoriteAds">
		<span>Mine favoritter<i class="fa-solid fa-heart fa-beat"></i></span>
	</h2>
	<div class="favList">
		<div v-if="favStore && favStore.favorites.length > 0 && isFavoriteAds">
			<div v-for="fav in favStore.favorites" v-if="favStore && favStore.favorites.length > 0" :key="fav.id"
			     class="adImages">
				<RouterLink :to="{name: 'AdDetails', params:{id: fav.id}}">
					<div class="ad">
						<h6>{{ fav.title }}</h6>
						<div v-if="fav.images && fav.images.length > 0">
							<img :src="fav.images[0].imageUrl" alt="img"/>
						</div>
					</div>
				</RouterLink>
			</div>
			<div v-else>
				Ingen annonser
			</div>

		</div>
	</div>
	<h2 @click="isActiveAds=!isActiveAds">
    <span v-if="!isActiveAds" title="Trykk for å åpne">Aktive annonser<i class="fa-solid fa-bars"
                                                                         style="padding: 10px"></i></span>
		<span v-else>Aktive annonser</span>
	</h2>
	<div v-if="ads && ads.length && isActiveAds> 0" class="adImages">
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
	<h2 @click="isActiveBoughtAds=!isActiveBoughtAds">
    <span v-if="!isActiveBoughtAds" title="Trykk for å åpne">Mine Kjøp<i class="fa-solid fa-bars"
                                                                         style="padding: 10px"></i></span>
		<span v-else>Mine Kjøp</span>
	</h2>
	<div v-if="ads && ads.length > 0 && isActiveBoughtAds" class="myBoughtAds">
		<div v-for="ad in adStore.adsBought" v-if="adStore.adsBought.length > 0" :key="ad.id" class="adImages">
			<RouterLink :to="{name: 'AdDetails', params:{id: ad.id}}">
				<div class="ad">
					<h6>{{ ad.title }}</h6>
					<div v-if="ad.images && ad.images.length > 0">
						<img :src="ad.images[0].imageUrl" alt="img"/>
					</div>
				</div>
			</RouterLink>
		</div>
		<div v-else>
			Ingen annonser
		</div>
	</div>
	<h2 title="Trykk for å åpne" @click="isActiveReviews = !isActiveReviews">Anmeldelser av {{ user.name }}<i
			class="fa-solid fa-file-pen" style="padding: 5px"></i></h2>
	<div v-if="reviewsForUser.items.value.length > 0 && isActiveReviews" class="adReview">
		<div v-for="review in reviewsForUser.items.value" :key="review.id">
			<div class="innerReview">

				<RouterLink v-if="getReviewer(review.fromUserId)"
				            :to="{name: 'UserProfile', params:{id: getReviewer(review.fromUserId).id}}">
					<div v-if="getReviewer(review.fromUserId)" class="review"><img
							:src="getReviewer(review.fromUserId).profileImageUrl" alt="img"/>{{ getReviewer(review.fromUserId).name }}
					</div>
				</RouterLink>

				<RouterLink :to="{name:'AdDetails', params:{id: review.adId}}">
					<small>{{ review.comment }}</small>
					<br>
					<div><span v-html="getStars(review.rating)"></span></div>
					<small>{{ dateFormat.formatDate(review.createdAt) }}</small>
				</RouterLink>
			</div>
		</div>
	</div>


</template>

<style scoped>


.profilePic img:hover {
	transform: scale(1.2);
}

.favList {
	display: flex;
	justify-content: space-between;
	align-items: center;
}

.container h2 {
	margin-bottom: 10px;
	padding: 5px;
}

h2 {
	cursor: pointer;
	padding: 5px;
	margin-left: 10px;
}

.container p {
	margin: 5px 0;
	padding: 5px;
}


.adImages {
	display: flex;
	justify-content: center;
	align-items: center;
	gap: 20px;
	padding: 10px;
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

.myActiveAds {
	display: flex;
	justify-content: center;
	align-items: center;
	gap: 20px;
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

.innerReview:hover {
}

.innerReview img {
	height: 50px;
	width: 50px;
	border-radius: 25px;
	border: 3px solid #98cbe8;
	margin: 10px;
}

.myBoughtAds {
	display: flex;
	justify-content: center;
	align-items: center;
	gap: 20px;
}

h2:hover {
	background-color: #98cbe8;
	border-radius: 5px;
}

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


.profilePic {
	height: 150px;
	width: 150px;
	border-radius: 50%;
	border: 5px solid #98cbe8;
	object-fit: cover;
	margin-bottom: 20px;
}

.container h2 {
	margin-bottom: 10px;
	padding: 5px;
}

h2 {
	cursor: pointer;
}

.container p {
	margin: 5px 0;
	padding: 5px;
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

.innerReview img {
	height: 50px;
	width: 50px;
	border-radius: 25px;
	border: 3px solid #98cbe8;
	margin: 10px;
}

h2:hover {
	background-color: #98cbe8;
	border-radius: 5px;
}

</style>
