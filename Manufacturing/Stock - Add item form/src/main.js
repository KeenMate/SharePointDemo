import $ from 'jquery'
import Vue from 'vue'
import App from './App.vue'
import Rx from 'rxjs/Rx'
import VueRx from 'vue-rx'
import 'materialize-css'

// tada!
Vue.use(VueRx, Rx)

var vm = new Vue({
  el: '#app',
  render: h => h(App)
})