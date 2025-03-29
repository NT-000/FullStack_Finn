import {ref} from 'vue'
import axios from 'axios'

// Get-composable for å hente fra API avhengig av route

export function getRoute(apiRoute) {
    const items = ref([])
    const loading = ref(false)
    const error = ref(null)

    async function fetchData() {
        loading.value = true
        error.value = ref(null)
        try {
            const res = await axios.get(`/api${apiRoute}`, {withCredentials: true})
            items.value = res.data;
            console.log('items array', items.value)
        } catch (err) {
            error.value = err.message
        } finally {
            loading.value = false
        }
    }

    return {items, loading, error, fetchData}
}