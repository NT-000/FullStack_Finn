import {defineStore} from 'pinia';
import axios from 'axios';

const currentPath = window.location.pathname;

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
                if (err.response.status === 401) {
                    if (currentPath !== '/') {
                        console.error("bruker ikke logget inn");
                    }

                } else {
                    console.error("something went wrong", err.message);
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