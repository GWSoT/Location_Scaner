import Vue from 'vue';
import Vuex from 'vuex';
import auth from './modules/auth';
import user from './modules/user';
import profile from './modules/profile';

Vue.use(Vuex);

export default new Vuex.Store({
  state: {

  },
  mutations: {

  },
  actions: {

  },
  modules: {
    auth: {
      namespaced: true,
      state: auth.state,
      getters: auth.getters,
      actions: auth.actions,
      mutations: auth.mutations,
    },
    user: { 
      namespaced: true,
      state: user.state,
      getters: user.getters,
      actions: user.actions,
      mutations: user.mutations,
    },
    profile: {
      namespaced: true,
      state: profile.state,
      getters: profile.getters,
      actions: profile.actions,
      mutations: profile.mutations,
    },
  },
});
