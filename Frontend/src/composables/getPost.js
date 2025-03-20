import {ref} from 'vue';
import axios from 'axios';

export function getPost(apiRoute) {
    const item = ref({});
    const loading = ref(false);
    const error = ref(null);

    async function fetchData() {
        loading.value = true;
        error.value = null;
        try {
            const {data} = await axios.get(`/api${apiRoute}`, {withCredentials: true});
            console.log('data from:', apiRoute);
            item.value = data;
        } catch (err) {
            error.value = err.message;
            console.error("couldn't get ad:", err);
        } finally {
            loading.value = false;
        }
    }

    return {item, loading, error, fetchData};
}