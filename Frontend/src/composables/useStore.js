import {ref} from "vue";

const user = ref(null)


    const setUser = (newUser) => user.value = newUser;
    const clearUser = () => user.value = null;
    
    const store = {user, setUser,clearUser};

export default store;