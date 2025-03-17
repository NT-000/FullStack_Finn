import {ref} from "vue";

const user = ref(null)

export const useStore = () => {
    const setUser = (userData) => user.value = userData;
    const clearUser = () => user.value = null;
    return { user, setUser: (newUser) => user.value = newUser, clearUser: () => user.value = null };
}

export default useStore;