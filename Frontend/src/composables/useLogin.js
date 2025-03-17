import {ref, watchEffect} from 'vue'
import axios from 'axios'
import {useRouter} from 'vue-router'
import useStore from './useStore'

//For å sjekke innlogget bruker
const decodeJwt = (token) => {
    if (!token) return {};
    try{
        const base64Url = token.split(".")[1]; // Henter payload-delen av JWT
        const base64 = base64Url.replace(/-/g, "+").replace(/_/g, "/");
        return JSON.parse(atob(base64)); // Dekoder base64 til JSON
    }
    catch(err) {
        return {}
    }
};

export function useLogin() {
    const token = ref(localStorage.getItem('jwt'))
    const userInfo = ref(decodeJwt(token.value));
    const loading = ref(false)
    const error = ref(null)
    const router = useRouter();
    const {user} = useStore();
    const store = useStore();
    
    watchEffect( () => {
        userInfo.value = decodeJwt(token.value);
        store.setUser(userInfo.value);
        console.log("Debug: updated userInfo:", userInfo.value);
    });
    const logIn = async (email,password) => {
        loading.value = true
        error.value = ''
        
        try{
            const response = await axios.post('/api/users/login', {email, password});
            token.value = response.data.token;
            localStorage.setItem('jwt', token.value);
            axios.defaults.headers.common['Authorization'] = 'Bearer ' + token.value;
            user.value = decodeJwt(token.value);
            store.setUser(userInfo.value)
            console.log("Logged-in user:", userInfo.value);
            
            router.push('/mainpage')
        }
        catch(error){
            console.log('error login',error)
            error.value = 'Login failed'
        }
        finally {
            loading.value = false
        }
    }
        return {logIn, token, userInfo, loading, error }
}