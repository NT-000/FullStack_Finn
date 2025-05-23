<script setup>
import {computed, onMounted, ref} from "vue";
import {useStarRating} from "../composables/useStarRating.js";
import {useAdStore} from "../stores/adStore.js";
import {getRoute} from "../composables/getRoute.js";
import {useDateFormat} from "../composables/useFormatDate.js";
import {useFavStore} from "../stores/favStore.js";
import FalloutTitleTheme from "../components/FalloutTitleTheme.vue";

const props = defineProps({
	user: Object,
	ads: Array,
});

const favStore = useFavStore();
const adStore = useAdStore();
const {getStars} = useStarRating();
const reviewsForUser = getRoute(`/reviews/${props.user.id}`);
const users = getRoute(`/users`);
const dateFormat = useDateFormat();

const isActiveReviews = ref(true);
const isActiveAds = ref(false);
const isActiveBoughtAds = ref(false);
const isFavoriteAds = ref(false);

const getReviewer = (fromUserId) => {
	return users.items.value.find((user) => user.id === fromUserId);
};

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
	await adStore.fetchBoughtAds();
	await reviewsForUser.fetchData();
	await users.fetchData();
	await favStore.fetchFavorites();
});
</script>

<template>
	<section v-if="user" class="profile-container">
		<div class="profile-card">
			<img :src="user.profileImageUrl" alt="Profilbilde" class="profile-image"/>
			<FalloutTitleTheme> <h2>{{ user.name }}</h2>
			<p><strong>E-post:</strong> {{ user.email }}</p>
			<p><strong>Vurdering:</strong> <span class="user-rating" v-html="getStars(calculateAverageRating)"></span></p></FalloutTitleTheme>
		</div>


		<h3 class="section-header" @click="isFavoriteAds = !isFavoriteAds">
			<FalloutTitleTheme><span>Mine favoritter</span></FalloutTitleTheme>
			<i class="fa-solid fa-heart fa-beat"></i>
		</h3>
		<transition name="fade-slide">
			<div v-if="isFavoriteAds && favStore.favorites.length" class="ad-list">
				<div v-for="fav in favStore.favorites"
				     :key="fav.id" class="ad-card">
					<RouterLink 
							:to="{ name: 'AdDetails', params: { id: fav.id } }">
						<img :src="fav.images[0]?.imageUrl" alt="img"/>
						<h4>{{ fav.title }}</h4>
					</RouterLink>
				</div>
			</div>
		</transition>


		<h3 class="section-header" @click="isActiveAds = !isActiveAds">
			<FalloutTitleTheme>	<span>Aktive annonser</span></FalloutTitleTheme>
			<i class="fa-solid fa-bars"></i>
		</h3>
		<transition name="fade-slide">
			<div v-if="isActiveAds && ads.length" class="ad-list">
				<div v-for="ad in ads" :key="ad.id" class="ad-card">
					<RouterLink :to="{ name: 'AdDetails', params: { id: ad.id } }">
						<img :src="ad.images[0]?.imageUrl" alt="img"/>
						<h4>{{ ad.title }}</h4>
					</RouterLink>
				</div>
			</div>
		</transition>


		<h3 class="section-header" @click="isActiveBoughtAds = !isActiveBoughtAds">
			<FalloutTitleTheme>	<span>Mine kjøp</span></FalloutTitleTheme>
			<i class="fa-solid fa-cart-shopping"></i>
		</h3>
		<transition name="fade-slide">
			<div v-if="isActiveBoughtAds && adStore.adsBought.length" class="ad-list">
				<div v-for="ad in adStore.adsBought" :key="ad.id" class="ad-card">
					<RouterLink :to="{ name: 'AdDetails', params: { id: ad.id } }">
						<img :src="ad.images[0].imageUrl" alt="img"/>
						<h4>{{ ad.title }}</h4>
					</RouterLink>
				</div>
			</div>
		</transition>

		<h3 class="section-header" @click="isActiveReviews = !isActiveReviews">
			<FalloutTitleTheme>	<span>Anmeldelser</span></FalloutTitleTheme>
			<i class="fa-solid fa-comment-dots"></i>
		</h3>
		<transition name="fade-slide">
			<div v-if="isActiveReviews && reviewsForUser.items.value.length" class="review-list">
				<div v-for="review in reviewsForUser.items.value" :key="review.id" class="review-card">
					<div v-if="getReviewer(review.fromUserId)" class="review-author">
						<img :src="getReviewer(review.fromUserId)?.profileImageUrl"/>
						<RouterLink :to="{ name: 'UserProfile', params: { id: getReviewer(review.fromUserId).id } }">
							{{ getReviewer(review.fromUserId).name }}
						</RouterLink>
					</div>
					<FalloutTitleTheme>	<div class="review-content">
						<RouterLink :to="{ name: 'AdDetails', params: { id: review.adId } }">
							<p>{{ review.comment }}</p>
							<div class="rating"><span v-html="getStars(review.rating)"></span></div>
							<small>{{ dateFormat.formatDate(review.createdAt) }}</small>
						</RouterLink>
					</div></FalloutTitleTheme>
				</div>
			</div>
		</transition>
	</section>

	<div v-else>
		<i class="fa-solid fa-circle-notch fa-spin"></i> Laster profil...
	</div>
</template>

<style scoped>

.fade-slide-enter-active,
.fade-slide-leave-active {
	transition: all 0.4s ease;
}

.fade-slide-enter-from {
	opacity: 0;
	transform: translateY(-10px);
}

.fade-slide-leave-to {
	opacity: 0;
	transform: translateY(10px);
}

.profile-container {
	max-width: 800px;
	margin: 40px auto;
	padding: 20px;
	background: #e0f4ff;
	border-radius: 16px;
	box-shadow: 0 8px 24px rgba(0, 0, 0, 0.1);
	color: #000000;

}

.profile-card {
	text-align: center;
	margin-bottom: 30px;
}

.profile-image {
	width: 120px;
	height: 120px;
	object-fit: cover;
	border-radius: 50%;
	border: 4px solid #00ff66;;
	box-shadow: 0 4px 12px rgba(0, 0, 0, 0.2);
	margin-bottom: 10px;
}

.profile-image:hover{
	transform:scale(1.2);
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


.section-header {
	cursor: pointer;
	font-size: 1.4rem;
	background-color: darkgreen;
	color: #00ff66;
	border-radius: 8px;
	padding: 10px 15px;
	margin: 20px 0 10px;
	display: flex;
	align-items: center;
	justify-content: space-between;
	box-shadow: 0 3px 8px rgba(0, 0, 0, 0.1);
}

.section-header:hover {
	animation: flicker 1.8s infinite ease-in-out;
}

@keyframes flicker {
	0%, 19%, 21%, 23%, 25%, 54%, 56%, 100% {
		opacity: 1;
	}
	20%, 24%, 55% {
		opacity: 0.85;
	}
}

.ad-list {
	display: flex;
	flex-wrap: wrap;
	justify-content: center;
	gap: 16px;
	margin-top: 10px;
}

.ad-card {
	background: black;
	border-radius: 10px;
	box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
	width: 150px;
	text-align: center;
	padding: 10px;
	transition: transform 0.2s ease;
}

.ad-card:hover {
	transform: scale(1.05);
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

	
.ad-card img {
	width: 100%;
	height: 100px;
	object-fit: cover;
	border-radius: 8px;
}

.review-list {
	display: flex;
	flex-direction: column;
	gap: 16px;
}

.review-card {
	background-color: black;
	border-left: 5px solid #00ff66;
	padding: 12px;
	border-radius: 8px;
	box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
}

.review-author {
	display: flex;
	align-items: center;
	gap: 10px;
	margin-bottom: 6px;
	font-weight: bold;
}

.review-author img {
	width: 40px;
	height: 40px;
	object-fit: cover;
	border-radius: 50px;
	border: 2px solid #00ff66;
}

.user-rating img{
	height: 20px;
	width: 20px;
}
</style>
