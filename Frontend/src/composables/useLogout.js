import axios from "axios";
import {useRouter} from "vue-router";
import {useUserStore} from "../stores/useUserStore.js";

export function useLogout() {
    const userStore = useUserStore();


    const router = useRouter();
    const logOut = async () => {
        try {
            await axios.post('/api/users/logout', {withCredentials: true});
            userStore.clearUser();
         await router.push('/')
        } catch (err) {
            console.error(err.message);
        }
    };
    return {logOut};
}