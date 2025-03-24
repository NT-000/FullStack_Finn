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
                console.error("error fetching ads:", error);
            }
        },

        async updateAd(adId, adData) {
            try {
                await axios.put(`/api/ads/${adId}`, adData, {withCredentials: true});
                await this.fetchAds();
            } catch (error) {
                console.error("could not update ad:", error);
            }
        },

        async deleteAd(adId) {
            try {
                await axios.delete(`/api/ads/${adId}`, {withCredentials: true});
                this.ads = this.ads.filter(ad => ad.id !== adId);
            } catch (error) {
                console.log("error deleting ad: ", error);
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