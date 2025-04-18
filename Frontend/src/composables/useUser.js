import {ref} from 'vue'
import axios from 'axios'

export function useUser() {
    const error = ref(null)
    const loading = ref(true);
    const user = ref(null)

    const fetchUserProfile = async (userId = null) => {
        try {
            const url = userId ? `/api/users/profile/${userId}` : `/api/users/profile`;
            console.log(`Fetching user profile from: ${url}`);
            const res = await axios.get(url, {withCredentials: true})
            user.value = res.data;
        } catch (err) {
            console.log(error)
            error.value = 'Something went wrong when loading userprofile'
        } finally {
            loading.value = false
        }
    };

    return {user, error, loading, fetchUserProfile}
}