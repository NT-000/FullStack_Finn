<template>
	<div class="fallout-title">
		<slot />
	</div>
</template>

<script setup>
</script>

<style scoped>
.fallout-title {
	font-family: 'Overseer', 'Press Start 2P', monospace;
	color: #00ff66;
	background-color: #000;
	padding: 1rem 1.5rem;
	font-size: 1.2rem;
	border-radius: 8px;
	text-align: center;
	text-transform: uppercase;
	letter-spacing: 1px;
	text-shadow:
			0 0 5px #00ff66,
			0 0 10px #00ff66,
			0 0 20px #00ff66;
	box-shadow: 0 0 10px rgba(0, 255, 100, 0.4);
}
</style>
