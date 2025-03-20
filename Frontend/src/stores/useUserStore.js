import {defineStore} from 'pinia';
import axios from 'axios';

export const useUserStore = defineStore('user', {
    state: () => ({
        user: null
    }),
    getters: {
        isLoggedIn: (state) => !!state.user
    },
    actions: {
        async initializeUser() {
            try {
                const res = await axios.get("/api/users/profile", {withCredentials: true});
                console.log("user from backend:", res.data);
                this.user = res.data;
            } catch (err) {
                if(err.response && err.response.status === 401) {
                console.error("user not logged in");
                }
                else{
                    console.error("something went wrong", err);
                }
                this.user = null;
            }
        },
        setUser(newUser) {
            console.log("updated user", newUser);
            this.user = newUser;
        },
        clearUser() {
            console.log("removes user from store");
            this.user = null;
        }
    }
});