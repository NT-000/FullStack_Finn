import {defineStore} from 'pinia';
import axios from 'axios';

export const useAdStore = defineStore('adStore', {
    state: () => ({
        ads: [],
    }),
    actions: {
        async fetchAds() {
            try {
                const response = await axios.get('/api/ads', {withCredentials: true});
                this.ads = response.data;
                console.log("Ads:", response.data);
            } catch (error) {
                console.error("Error fetching ads:", error);
            }
        }
    },
    getters: {
        getAdsByUser: (state) => (userId) => {
            if (!userId) {
                return [];
            }
            return state.ads.filter(ad => Number(ad.userId) === Number(userId));
        }
    }
});