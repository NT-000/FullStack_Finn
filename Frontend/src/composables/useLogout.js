
import axios from "axios";
import router from "../router/index.js";
import store from "../composables/useStore";


export function useLogout() {
    
    const logOut = () => {
    
    localStorage.removeItem("jwt");
    delete axios.defaults.headers.common["Authorization"]; // sikrer at fremtidige kall ikke sendes med et gammelt token.
    store.clearUser();
        console.log("After logout, user:", store.user.value);
    router.push('/')
    };

    return { logOut };
}