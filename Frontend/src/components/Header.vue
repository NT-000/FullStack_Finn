<script setup>
import LoginTitle from "@/components/LoginTitle.vue";
import CrtPanel from "@/components/CrtPanel.vue";

</script>

<template>
<CrtPanel>
	<div class="header">

		<img src="/header2.png" alt="Header logo" />
	
	</div>
</CrtPanel>

</template>
<style>
.header img {
	max-height: 25vh;
	width: 40vw;
	font-family: 'Overseer', 'Press Start 2P', monospace;
	font-size: 1.4rem;
	color: #00ff66;
	background-color: #000;
	padding: 0.5rem 1rem;
	border-radius: 10px;
	text-align: center;
	margin-bottom: 1.5rem;
	text-shadow:
			0 0 4px #00ff66,
			0 0 10px #00ff66;
	box-shadow: 0 0 20px rgba(0, 255, 102, 0.2);
	border: 1px solid #00ff66;
	letter-spacing: 1px;
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

</style>