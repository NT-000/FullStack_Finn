import {ref} from 'vue'
import axios from 'axios'

export function useAd() {
    const error = ref(null)
    const loading = ref(false)
    const message = ref(null)

    const createAd = async (adData) => {
        try {
            loading.value = true
            error.value = null
            message.value = null

            const res = await axios.post('/api/ads', adData, {
                withCredentials: true
            })
            message.value = res.data
        } catch (err) {
            console.log('Could not create ad', err)
            error.value = err.response?.data
        } finally {
            loading.value = false
        }
    }
    return {createAd, error, loading, message}
}