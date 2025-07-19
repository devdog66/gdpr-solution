import { createRouter, createWebHistory, type RouteRecordRaw } from 'vue-router'
import NotFound from '../views/NotFound.vue';
import ManageCookies from '../views/ManageCookies.vue';

const routes: Array<RouteRecordRaw> = [
  {
    path: "/app/cookies",
    name: 'Manage Cookies',
    component: ManageCookies
  },
  {
    path: "/:catchAll(.*)",
    name: 'NotFound',
    component: NotFound
  }
];

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router;