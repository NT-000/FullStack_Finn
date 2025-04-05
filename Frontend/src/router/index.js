import {createRouter, createWebHistory} from 'vue-router'
import StartPage from "../views/StartPage.vue";
import PageNotFound from "../views/PageNotFound.vue";
import MainPage from "../views/MainPage.vue";
import Ads from "../views/Ads.vue";
import AdDetails from "../views/AdDetails.vue";
import Profile from "../views/Profile.vue";
import CreateNewAd from "../views/CreateNewAd.vue";
import Map from "../views/Map.vue";
import {useUserStore} from '../stores/useUserStore.js'
import Chat from "../views/Chat.vue";
import Inbox from "../views/Inbox.vue";

const router = createRouter({
    history: createWebHistory(),
    routes: [

        {
            path: '/',
            name: 'StartPage',
            component: StartPage
        },
        {
            path: "/users/profile",
            name: 'Profile',
            component: Profile,
            props: true,
            meta: {requiresAuth: true}
        },
        {
            path: "/users/:id",
            name: 'UserProfile',
            component: Profile,
            props: true,
            meta: {requiresAuth: true}
        },

        {
            path: '/ads/:id',
            name: 'AdDetails',
            component: AdDetails,
            meta: {requiresAuth: true}

        },
        {
            path: '/ads',
            name: 'Ads',
            component: Ads,
            meta: {requiresAuth: true}
        },
        {
            path: '/ads/create-with-files',
            name: 'CreateNewAd',
            component: CreateNewAd,
            meta: {requiresAuth: true}
        },
        {
            path: '/mainpage',
            name: 'MainPage',
            component: MainPage,
            meta: {requiresAuth: true}
        },
        {
            path: '/map',
            name: 'Map',
            component: Map,
            meta: {requiresAuth: true}
        },
        {
            path: '/chat/:id',
            name: 'Chat',
            component: Chat,
            props: true,
            meta: {requiresAuth: true}
        },
        {
            path: '/inbox',
            name: 'Inbox',
            component: Inbox,
            props: true,
            meta: {requiresAuth: true}
        },
        {
            path: '/:catchAll(.*)',
            name: 'PageNotFound',
            component: PageNotFound
        },
    ],


});

//nav blocker før hver navigering
router.beforeEach(async (to, from, next) => {
    const userStore = useUserStore();
    // sjekker om brukerdata allerede er lastet inn
    if (userStore.user === null) {
        await userStore.initializeUser();
    }

    const requiresAuth = to.matched.some(record => record.meta.requiresAuth);

    if (requiresAuth && !userStore.isLoggedIn) {
        next('/');
        console.log("you need to log in")
    } else {
        next();
    }
});
export default router