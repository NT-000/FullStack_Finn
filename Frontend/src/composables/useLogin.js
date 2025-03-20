import {ref} from 'vue'
import axios from 'axios'
import {useRouter} from 'vue-router'
import { useUserStore } from '../stores/useUserStore.js';

export function useLogin() {
const userStore = useUserStore();
    const router = useRouter();
    const loading = ref(false)
    const error = ref(null)
    const logIn = async (email, password) => {
        loading.value = true
        error.value = null;

        try {
            await axios.post('/api/users/login', {email, password}, {withCredentials: true});
            const userRes = await axios.get('/api/users/profile', {withCredentials: true});
            userStore.setUser(userRes.data)
            console.log("Logged-in user:", userRes.data);
            console.log("Logged-in user userstore.user:", userStore.user);
            router.push('/users/profile')
        } catch (err) {
            console.log('error login', err)
            error.value = 'Login failed'
        } finally {
            loading.value = false;
        }
    }
    return {logIn, loading, error}
}