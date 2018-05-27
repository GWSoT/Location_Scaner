import Vue from 'vue';
import App from './App.vue';
import router from './router';
import store from './store/store';
import './assets/sass/main.scss';
import axios from 'axios';

Vue.config.productionTip = false;

new Vue({
  router,
  store,
  render: (h) => h(App),
}).$mount('#app');

axios.interceptors.request.use((axiosCfg: any) => {
  const authToken = store.getters['auth/authToken'];
  if (authToken) {
    axiosCfg.headers.Authorization = `Bearer ${authToken}`;
  }
  return axiosCfg;
}, (err: any) => {
  return Promise.reject(err);
})