import {defineStore} from 'pinia';
import axios from 'axios';

export const useAdStore = defineStore('adStore', {
    state: () => ({
        ads: [],
        interestedUsers: [],
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
        
        async markAsSold(adId,buyerId) {
            if(!buyerId) {
                console.error("no user selected for ad:", adId);
                return
            }
            try{
                await axios.put(`/api/ads/${adId}/sold`,  buyerId, 
                    {
                    withCredentials: true,
                });
                await this.fetchAds();
            }
            catch(error) {
                console.error("could not mark as sold, ad:", error);
            }
        },

        async getInterestedUsers(adId) {
            try {
                const response = await axios.get(`/api/messages/interested-users/${adId}`, {
                    withCredentials: true
                });
                this.interestedUsers = response.data;
                console.log("interested Users:", this.interestedUsers);
            } catch (error) {
                console.error("could not fetch interested users:", error);
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