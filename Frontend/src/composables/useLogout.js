
import axios from "axios";
import router from "../router/index.js";
import useStore from "../composables/useStore";

const store = useStore()

export function useLogout() {

    const { user } = useStore();
    const logOut = () => {
    
    localStorage.removeItem("jwt");
    delete axios.defaults.headers.common["Authorization"];
    store.clearUser();
    console.log(`logged out, jwt:`,localStorage.getItem("jwt"));
    router.push('/')
    };

    return { logOut };
}