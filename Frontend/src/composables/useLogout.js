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
            console.log("After logout, user:", userStore.user);
            router.push('/')
        } catch (err) {
            console.log(err);
        }
    };
    return {logOut};
}