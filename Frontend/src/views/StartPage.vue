<script setup>
import {ref} from "vue";
import LoginForm from "../components/StartPage/LoginForm.vue";
import SignupForm from "../components/StartPage/SignupForm.vue";

const isUser = ref(true)
</script>

<template>
  <h1>Start Page</h1>
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
</template>

<style scoped>

</style>