import {createApp} from 'vue'
import App from './App.vue'
import './style.css'
import router from './router'
import axios from "axios";
import {createPinia} from 'pinia';


axios.defaults.withCredentials = true;
const app = createApp(App)
const pinia = createPinia();

app.use(router)
app.use(pinia);
app.mount('#app')

