import Vue from 'vue';
import Router from 'vue-router';
import Home from './views/Home.vue';
import About from './views/About.vue';
import RegistrationForm from './views/account/Register.vue';
import LoginForm from './views/account/Login.vue';
import Dashboard from './views/dashboard/Dashboard.vue';

import DashboardRoot from './views/dashboard/DashboardRoot.vue';


import Friends from './views/dashboard/Friends/Friends.vue';
import ReceievedFriends from './views/dashboard/Friends/ReceievedFriends.vue';
import SentFriendRequests from './views/dashboard/Friends/SentFriendRequests.vue';
import FriendsRoot from './views/dashboard/Friends/FriendsRoot.vue';
import FriendMap from './views/dashboard/FriendMap.vue';
import Messages from './views/dashboard/Messages.vue';
import store from './store/store';

Vue.use(Router);

const router = new Router({
  routes: [
    {
      path: '/register',
      name: 'registerForm',
      component: RegistrationForm,
    },
    {
      path: '/login',
      name: 'loginForm',
      component: LoginForm
    },
    {
      path: '/dashboard',
      component: DashboardRoot,
      children: [
        {
          path: '/dashboard/:id',
          component: Dashboard,
          name: 'dashboard',
          meta: { requiresAuth: true },
        },
        {
          path: '/dashboard/friends',
          component: FriendsRoot,
          children: [
            {
              path: '/dashboard/friends/friends',
              component: Friends,
              name: 'friends',
              meta: { requiresAuth: true },
            },
            {
              path: '/dashboard/friends/receievedFriends',
              component: ReceievedFriends,
              name: 'receievedFriends',
              meta: { requiresAuth: true },
            },
            {
              path: '/dashboard/friends/sentRequests',
              component: SentFriendRequests,
              name: 'sentFriendRequests',
              meta: { requiresAuth: true },
            },
          ],
        },
        {
          path: '/dashboard/friendsMap',
          component: FriendMap,
          name: 'friendMap',
          meta: { requiresAuth: true },
        },
        {
          path: '/dashboard/messages',
          component: Messages,
          name: 'messages',
          meta: { requiresAuth: true },
        },
      ],
    },
  ],
});

router.beforeEach((to: any, from: any, next: any) => {
  if (to.matched.some((record: any) => record.meta.requiresAuth)) {
    if (!store.getters['auth/isAuthenticated']) {
      next({
        path: '/login',
        query: { redirect: to.fullPath },
      })
    } else {
      next();
    }
  } else {
    next();
  }
})

export default router;
