import {ref, watchEffect} from 'vue'
import axios from 'axios'

export function useUser(){
    const user = ref(null)
    const error = ref(null)
    const loading = ref(false);
    
    const fetchUserProfile = async (userId = null) => {
        try{
            const url = userId ? `/api/users/${userId}` : `/api/users/profile`;
            console.log(`Fetching user profile from: ${url}`);
            const res = await axios.get(url)
            user.value = res.data;
            }
        
        catch(err){
            console.log(error)
            error.value = 'Something went wrong when loading userprofile'
        }
        finally{
            loading.value = false
        }
    };
    return {user, error, loading, fetchUserProfile}
}