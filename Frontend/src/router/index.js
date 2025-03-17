import {createRouter, createWebHistory} from 'vue-router'

import StartPage from "../views/StartPage.vue";
import PageNotFound from "../views/PageNotFound.vue";

import MainPage from "../views/MainPage.vue";
import Ads from "../views/Ads.vue";
import AdDetails from "../views/AdDetails.vue";
import Profile from "../views/Profile.vue";
import CreateNewAd from "../views/CreateNewAd.vue";

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
            component: Profile },
        { 
            path: "/users/:id",
            name: 'UserProfile',
            component: Profile },

        {
            path: '/ads/:id',
            name: 'AdDetails',
            component: AdDetails
        },
        {
            path: '/ads',
            name: 'Ads',
            component: Ads
        },
        {
            path: '/ads/create-with-files',
            name: 'CreateNewAd',
            component: CreateNewAd
        },
        {
            path: '/mainpage',
            name: 'MainPage',
            component: MainPage
        },
        {
            path: '/:catchAll(.*)',
            name: 'PageNotFound',
            component: PageNotFound
        },
    ]
})

export default router