import {ref} from 'vue'
import axios from 'axios'
import {useRouter} from 'vue-router'
import {useStore} from "./useStore.js";
export function useLogin() {
    const router = useRouter();
    const loading = ref(false)
    const error = ref(null)
    const store = useStore();
    const logIn = async (email,password) => {
        loading.value = true
        error.value = null;
        
        try{
            await axios.post('/api/users/login', {email, password}, {withCredentials: true});
            const userRes = await axios.get('/api/users/profile', {withCredentials: true});                                                                                                                       
            store.setUser(userRes.data)
            console.log("Logged-in user:", userRes.data);
            console.log("Logged-in user store.user.value:", store.user.value);
            router.push('/users/profile')
        }
        catch(err){
            console.log('error login',err)
            error.value = 'Login failed'
        }
        finally {
            loading.value = false;
        }
    }
        return {logIn, loading, error }
}