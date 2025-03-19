import {ref, watchEffect} from 'vue'
import axios from 'axios'
import useStore from "./useStore";

export function useUser(){
    const { user, setUser } = useStore();
    const error = ref(null)
    const loading = ref(true);
    
    const fetchUserProfile = async (userId = null) => {
        try{
            const url = userId ? `/api/users/${userId}` : `/api/users/profile`;
            console.log(`Fetching user profile from: ${url}`);
            const res = await axios.get(url,{ withCredentials: true })
            setUser(res.data);
            }
        
        catch(err){
            console.log(error)
            error.value = 'Something went wrong when loading userprofile'
            setUser(null);
        }
        finally{
            loading.value = false
        }
    };

    return {user, error, loading, fetchUserProfile}
}