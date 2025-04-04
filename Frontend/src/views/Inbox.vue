<script setup>
import {useAdStore} from "../stores/adStore.js";
import {useUserStore} from "../stores/useUserStore.js";
import {computed, onMounted, ref} from "vue";
import {RouterLink, useRouter} from "vue-router";


const adStore = useAdStore();
const userStore = useUserStore();
const interestedPerAd = ref([]);
const router = useRouter()

const goToChat = (userId, adId) => {
  router.push({ name: 'Chat', params: { id: userId }, query: { adId } })
}

const findOwnedAds = computed(()=>{
  return adStore.ads.filter(ad => ad.userId == userStore.user.id)
})

onMounted(async()=>{
 await adStore.fetchAds()
console.log("find owned ads:",findOwnedAds.value)

console.log("interestedPerAd:",interestedPerAd.value);
  
  for (const ad of findOwnedAds.value) {
    interestedPerAd.value[ad.id] = await adStore.getInterestedUsers(ad.id)
  }
 
})
</script>

<template>
  
    <h1>Innboks</h1>
    <div class="ad" v-if="findOwnedAds.length > 0">
      <div v-for="ad in findOwnedAds" :key="ad.id" class="ownedAds">
        <h2>{{ ad.title }}</h2>
        <p>{{ad.description}}</p>
       <div v-if="ad.id && ad.isSold">
        <div  class="sold">Solgt</div>
       </div>
         <div>
          <div>
          <img :src="ad.images[0]" alt="" />
        </div>
        </div>
        <select v-if="interestedPerAd && interestedPerAd.length > 0" @change="goToChat($event.target.value, ad.id)">
          <option value="">Velg chat å åpne</option>
          <option v-for="user in interestedPerAd[ad.id]" :key="user.id" :value="user.id">
             {{ user.name }}
          </option>
        </select>
        <p v-else>Ingen interesserte</p>
      </div>
    </div>
 
</template>

<style scoped>

h1{
	border-bottom: 5px solid blue;
}

option {
	background: #e2f1fc;
	border: none;
	border-radius: 10px;
	text-align: center;
	padding: 5px;
	font-weight: 600;
}

.sold {
	font-family: "Comic Sans MS", cursive;
	color: red;
	font-size: 4rem;
	transform: rotateZ(8deg);
	background-color: rgba(0, 0, 0, 0.8);
	padding: 10px 20px;
	border-radius: 8px;
	box-shadow: 0 4px 12px rgba(0, 0, 0, 0.3);
	width: fit-content;
}

.outerSold {
	background: linear-gradient(90deg, yellow, gold);
	border-radius: 12px;
	padding: 10px;
	display: flex;
	justify-content: center;
}

select {
	border-radius: 10px;
	padding: 10px;
	width: 60%;
	margin-top: 10px;
	font-size: 1rem;
	font-weight: bold;
	background-color: #f0faff;
	box-shadow: 0 3px 10px rgba(0, 0, 0, 0.1);
}

.profile-img {
	width: 60px;
	height: 60px;
	border-radius: 50%;
	object-fit: cover;
	border: 2px solid #007bff;
}

.ad {
	display: flex;
	flex-direction: column;
	gap: 20px;
	padding: 20px;
}

.ownedAds {
	display: flex;
	flex-direction: column;
	padding: 30px;
	border-radius: 16px;
	gap: 15px;
	border: none;
	background: linear-gradient(145deg, #d0e9ff, #f0faff);
	box-shadow: 0 6px 25px rgba(0, 0, 0, 0.1);
	transition: transform 0.2s ease, box-shadow 0.2s ease;
}

.ownedAds:hover {
	transform: translateY(-4px);
	box-shadow: 0 12px 30px rgba(0, 0, 0, 0.15);
}

.ownedAds img {
	max-width: 300px;
	border-radius: 12px;
	box-shadow: 0 4px 15px rgba(0, 0, 0, 0.2);
	border: 3px solid #007bff;
}

h2 {
	background-color: #007bff;
	color: white;
	border-radius: 10px;
	padding: 10px 15px;
	font-size: 1.8rem;
	box-shadow: 0 4px 10px rgba(0, 0, 0, 0.2);
}

p {
	font-size: 1.2rem;
	line-height: 1.6;
	background-color: rgba(255, 255, 255, 0.75);
	padding: 10px;
	border-radius: 8px;
}

</style>