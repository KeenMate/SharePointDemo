import $ from 'jquery'
import Vue from 'vue'
import App from './App.vue'
import VueRouter from 'vue-router'
import Materialize from 'materialize-css'

// Import components for routing
import CreateSupplierForm from './components/CreateSupplier.vue'
import Home from './components/Home.vue'

const router = new VueRouter({
	routes: [
		{
			path: '/page/:pageSize/:pageNumber',
			name: 'paged',
			component: Home
		},
		{ path: '/newsupplier', name: 'newsupplier', component: CreateSupplierForm },
		{ path: '/', redirect: { name: 'paged', params: { pageSize: 5, pageNumber: 1 } } }
	]
});

Vue.use(VueRouter);

new Vue({
	el: '#supplierPageVue',
	router,
	render: h => h(App)
})