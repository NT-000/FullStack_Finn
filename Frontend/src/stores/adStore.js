import { defineStore } from 'pinia';
import axios from 'axios';

export const useAdStore = defineStore('adStore', {
    state: () => ({
        ads: [],
    }),
    actions: {
        async fetchAds() {
            try {
                const response = await axios.get('https://localhost:5205/api/ads');
                this.ads = response.data;
            } catch (error) {
                console.error("Error fetching ads:", error);
            }
        }
    },
    getters: {
        getAdsByUser: (state) => (userId) => {
            return state.ads.filter(ad => ad.userId === userId);
        }
    }
});