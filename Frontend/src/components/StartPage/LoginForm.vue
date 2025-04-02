<script setup>
import {ref} from "vue";
import {useLogin} from "../../composables/useLogin.js";

const email = ref('')
const password = ref('')

const {loading, error, logIn} = useLogin();

const props = defineProps({
  isUser: Boolean,
})
const emit = defineEmits(['update:isUser'])
const checkIn = async () => {
  await logIn(email.value, password.value)
}

const toggle = () => {
  emit('update:isUser', !props.isUser)
}

</script>

<template>
  <div class="loginCard">
    <form @submit.prevent="checkIn">
      <div class="form-group">
        <label><i class="fa-solid fa-at">Email</i></label>
        <input v-model="email" required type="email"/>
        <label><i class="fa-solid fa-lock">Passord</i></label>
        <input v-model="password" required type="password"/>
      </div>
      <div class="error">{{ error }}</div>
      <button :disabled="loading" type="submit">
        {{ loading ? "Logger inn" : "Logg inn" }}
      </button>
      <button @click="toggle">
        <span>Register</span>
      </button>
    </form>
  </div>
</template>

<style scoped>
.loginCard {
  height: 50vh;
  width: 50vw;
  align-content: center;
  justify-content: center;
  margin-left: 39vw;
  padding: 20px;
  border-radius: 10px;
  background: blue;
}

.form-group {
  display: flex;
  flex-direction: column;

  align-items: center;
  gap: 10px;
}

input {
  border-radius: 10px;
  height: 40px;
  font-weight: bold;
  padding-left: 10px;
  align-items: center;
  background: #fff;
}

input text {
  font-weight: bold;
}

label {
  font-weight: bold;
  font-size: 1.5rem;
}

.error {
  color: red;
  font-size: 16px;
  font-weight: bold;
}
</style>