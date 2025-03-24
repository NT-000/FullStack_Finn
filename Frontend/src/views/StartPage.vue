<script setup>
import {ref} from "vue";
import LoginForm from "../components/StartPage/LoginForm.vue";
import SignupForm from "../components/StartPage/SignupForm.vue";

const isUser = ref(true)
</script>

<template>
  <div class="container">
 <div v-if="isUser"> <h1>Finn</h1></div>
    <div v-else>
      <h1>Registrer ny bruker</h1>
    </div>
  <div v-if="isUser">
    <LoginForm/>
  </div>
  <div v-else>
    <SignupForm/>
  </div>
  <button @click="isUser = !isUser">
    <span v-if="!isUser">Back</span>
    <span v-else>Register</span>
  </button>
  </div>
</template>

<style scoped>
.container {
  display: inline-block;
  justify-content: center;
  align-items: center;
  width: 400px;
}
h1 {
  font-size: 4rem;
  text-align: center;
  color: white;
  border: blue solid 2px;
}
h1{
  font-size: 5.8rem;
}
button:hover {
  font-size: 1.2rem;
}
</style>