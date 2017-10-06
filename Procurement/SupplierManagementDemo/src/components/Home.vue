<template>
	<div>
		<div class="row">
			<div class="col s3">
				<!-- <button class="waves-effect waves-light btn-large" v-on:click.prevent="redirectToForm()">Nový dodavatel </button> -->
				<router-link class="waves-effect waves-light btn-large light-blue darken-4 white-text" :to="{name: 'newsupplier'}">New Supplier</router-link>
			</div>
			<div class="col s6">
				<my-search-component :supplierName="searchExpression" @searchChanged="onSearchChanged"></my-search-component>
			</div>
		</div>
		<my-list :searchExpression="searchExpression"></my-list>
	</div>
</template>

<script>
import MyList from './SuppliersList.vue'
import MySearchComponent from './SearchForm.vue'

export default {
	name: 'HomeComp',
	components: {
		MyList,
		MySearchComponent
	},
	computed: {
		searchExpression() {
			return this.$route.query.supplierName ? this.$route.query.supplierName : '';
		}
	},
	watch: {
		'$route'(to, from) {
			if (to.params.pageSize%5 !== 0) {
				this.$router.push({
					name: 'paged',
					params: {
						pageSize: 5,
						pageNumber: 1
					},
					query: this.$route.query
				})
			}
		}
	},
	methods: {
		onSearchChanged(val) {
			console.log('search changed in home');
			this.$router.push({ name: 'paged', params: { pageSize: 5, pageNumber: 1 }, query: { supplierName: val } });
		}
	}
}
</script>

<style>
a i,
.input-field i {
	cursor: pointer;
}
</style>

