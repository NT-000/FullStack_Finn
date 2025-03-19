import {computed, ref} from "vue";
import axios from "axios";

    const user = ref({});
export function useStore() {

    const setUser = (newUser) => {
        console.log("updated user",newUser);
        user.value = newUser;
    };

    const clearUser = () => {
        console.log("removes user from store");
        user.value = {};
    };
    const initializeUser = async () => {
        try {
            const res = await axios.get("/api/users/profile", { withCredentials: true });
            console.log("user from backend:", res.data);
            setUser(res.data);
        } catch (err) {
            console.error("Something went wrong:", err);
            setUser(null);
        }
    };

    return { user, setUser, clearUser, initializeUser };
}

export default useStore;
