
import axios from "axios";
import store from "../composables/useStore";
import {useRouter} from "vue-router";


export function useLogout() {
    
    const router = useRouter();
    const logOut = async ()  => {
        try{
            await axios.post('/api/users/logout');
            store.clearUser();
            console.log("After logout, user:", store.user.value);
            router.push('/')
        }
        catch(err){
            console.log(err);
        }
    };
    return { logOut };
}