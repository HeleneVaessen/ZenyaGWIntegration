import Vue from 'vue'
import App from './App.vue'
import router from "./router";
import ajax from "vuejs-ajax"
Vue.use(ajax)
Vue.config.productionTip = false;

new Vue({
  router,
  render: h => h(App)
}).$mount("#app");
