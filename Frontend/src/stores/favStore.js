import {defineStore} from 'pinia'
import axios from 'axios'
import {computed, ref} from "vue";

export const useFavStore = defineStore('favStore', {
    state: () => ({
        favorites: [],
        error: ref(null),
        message: ref(null),
    }),
    actions: {
        async fetchFavorites() {
            const res = await axios.get('/api/favorites', {withCredentials: true});
            this.favorites = res.data;
            console.log("fetch favs:", res.data);
        },
        async addFavorite(adId) {
            try {
                await axios.post(`/api/favorites/${adId}`, {}, {withCredentials: true});
                await this.fetchFavorites();
            } catch (error) {
                console.log("error when adding to fav", error)
            }
        },
        async deleteFavorite(adId) {
            await axios.delete(`/api/favorites/${adId}`, {withCredentials: true});
            await this.fetchFavorites();
        },

        async toggleFav(adId) {
            const isFav = this.favorites.some(adFav => adFav.id === adId);
            if (isFav) {
                await this.deleteFavorite(adId)
            } else {
                await this.addFavorite(adId);
            }
        },

    },
    getters: {
        isFavorite: (state) => (adId) => {
            return state.favorites.some(adFav => adFav.id === adId);
        },
    },
});