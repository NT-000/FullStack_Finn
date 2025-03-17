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
            const res = await axios.get(url,{
                headers: {Authorization: `Bearer ${localStorage.getItem('jwt')}`,
                },
            });
            user.value = res.data;
        }
        catch(err){
            console.log(error)
            error.value = 'Something went wrong when loading userprofile'
        }
    };
    watchEffect(() => {
        const token = localStorage.getItem("jwt");
        if (token) {
            fetchUserProfile();
        } else {
            user.value = null;
        }
    });
    return {user, error, loading, fetchUserProfile}
}