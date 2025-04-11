<template>
	<div class="crt">
		<div class="crt-inner">
			<slot />
		</div>
	</div>
</template>

<script setup>
</script>

<style scoped>
.crt {
	position: relative;
	background-color: #000;
	color: #00ff66;
	padding: 2rem;
	border-radius: 10px;
	text-shadow: 0 0 2px #00ff66, 0 0 10px #00ff66;
	font-family: 'Overseer', 'Press Start 2P', monospace;
	animation: flicker 2s infinite alternate;
	box-shadow: 0 0 15px rgba(0, 255, 102, 0.3);
	overflow: hidden;
}


.crt::before {
	content: '';
	position: absolute;
	top: 0;
	left: 0;
	width: 100%;
	height: 100%;
	pointer-events: none;
	background: repeating-linear-gradient(
			to bottom,
			rgba(0, 255, 102, 0.05),
			rgba(0, 255, 102, 0.05) 1px,
			transparent 1px,
			transparent 2px
	);
	z-index: 2;
}

@keyframes flicker {
	0%, 19%, 21%, 23%, 25%, 54%, 56%, 100% {
		opacity: 1;
	}
	20%, 24%, 55% {
		opacity: 0.85;
	}
}

.crt-inner {
	position: relative;
	z-index: 3;
	text-align: center;
}
</style>