<script setup>
import LoginTitle from "@/components/LoginTitle.vue"
const ads = [
	{ text: '🔥 Kjøp 1, få 3 – i dag bare!', image: 'https://localhost:5205/adImages/1.png' },
	{ text: '💰 Lån 200 kr, betal 0 tilbake!', image: 'https://localhost:5205/adImages/7.png' },
	{ text: '👠 Nye importsko rett fra Romania, laget av barnekakerlakker slik at prisene holdes lave for deg!🫵', image: 'https://localhost:5205/adImages/3.png' },
	{ text: '👃🏼 Helt nye Snusekoppen! Prøv vår helt nye "Swap", bytt inn din gamle og få en "nesten ikke brukt" i retur!', image: 'https://localhost:5205/adImages/6.png' },
	{ text: '🪳☢️ Nyeste Roach-boy, helt rå! 25% Avslag!!!💥💥💥', image: 'https://localhost:5205/adImages/9.png' },
	{ text: '👃🏼Snusekoppen 2000, for deg som gir litt f.', image: 'https://localhost:5205/adImages/10.png' },
	{ text: '🪄Vi tryller vekk høye priser!!! Få tidenes deal på brukte Snusekopper!🫵', image: 'https://localhost:5205/adImages/11.png' },
	{ text: '💵Vi trenger deg, testpersoner søkes!🫵', image: 'https://localhost:5205/adImages/13.png' },
	{ text: '🚂Jeg har tatt tog før, har du!?🤷‍♂️', image: 'https://localhost:5205/adImages/14.png' },
]

const randomAd = ads[Math.floor(Math.random() * ads.length)]
</script>

<template>
	
	<div class="ad-box">
	<LoginTitle>
		<img :src="randomAd.image" alt="Annonse" />
		<p>{{ randomAd.text }}</p>
	</LoginTitle>
	</div>
</template>

<style scoped>


.ad-box {
	background-color: black;
	
	
	text-align: center;
	border-radius: 8px;
	margin: 10px auto;
	font-size: 1rem;
	width: 100%;
	box-sizing: border-box;
	font-family: 'Overseer', 'Press Start 2P', monospace;
	height: 100%;
	max-height: 60vh;
}

p{
	font-family: 'Overseer', 'Press Start 2P', monospace;
	font-size: 1.5rem;
	
	background-color: lightslategray;
	border-radius: 15px;
}
.ad-box img {
	width: 100%;
	height: 100%;
	max-height: 500px;
	margin-bottom: 8px;
	border: 8px solid darkgreen;
}


</style>