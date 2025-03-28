<script setup>
import {ref} from "vue";
import LoginForm from "../components/StartPage/LoginForm.vue";
import SignupForm from "../components/StartPage/SignupForm.vue";

const isUser = ref(true)
</script>

<template>
  <div class="container">
  <div class="loginForm" v-if="isUser">
    <LoginForm :isUser="isUser"/>
    <button @click="isUser = !isUser">
      <span v-if="!isUser">Back</span>
      <span v-else>Register</span>
    </button>
  </div>
    
  <div v-else>
    <SignupForm/>
  </div>
  </div>
</template>

<style scoped>
.container {
  display: flex;
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
  background: deepskyblue;
}
button:hover {
  font-size: 1.2rem;
}
</style>