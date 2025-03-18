import {ref} from 'vue'
import axios from 'axios'
import {useRouter} from 'vue-router'
import store from "../composables/useStore";

export function useLogin() {
    const router = useRouter();
    const loading = ref(false)
    const error = ref(null)
    
    const logIn = async (email,password) => {
        loading.value = true
        error.value = null;
        
        try{
            await axios.post('/api/users/login', {email, password});
            const userRes = await axios.get('/api/users/profile')                                                                                                                       
            store.setUser(userRes.data)
            console.log("Logged-in user:", userRes.data);
            console.log("Logged-in user store.user.value:", store.user.value);
            router.push('/mainpage')
        }
        catch(error){
            console.log('error login',error)
            error.value = 'Login failed'
        }
        finally {
            loading.value = false;
        }
    }
        return {logIn, loading, error }
}