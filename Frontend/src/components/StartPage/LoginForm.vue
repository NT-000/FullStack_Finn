<script setup>
import {ref} from "vue";
import {useLogin} from "../../composables/useLogin.js";

const email = ref('')
const password = ref('')

const {token, loading, error, logIn} = useLogin();

const checkIn = async () => {
 await logIn(email.value, password.value)
}
</script>

<template>
<form @submit.prevent="checkIn">
  <div class="form-group">
    <input type="email" v-model="email" required />
    <input type="password" v-model="password" required />
  </div>
  <button type="submit" :disabled="loading">
    {{ loading ? "Logging in" : "Log in" }}
  </button>
</form>
</template>

<style scoped>
.form-group {
  display: flex;
  flex-direction: column;
  gap: 10px;
}

.error {
  color: red;
  font-size: 14px;
}
</style>