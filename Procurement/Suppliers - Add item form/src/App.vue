<template>
	<div id="app">
		<my-form :inputObj="userInput" @submitted="AddNewSupplier(newObj)"></my-form>
		<div>
			<p v-bind="userInput.taxId"></p>
		</div>
		<my-list v-bind:suppliers-array="existingSuppliers"></my-list>
	</div>
</template>

<script>
import myForm from './components/Form.vue'
import myList from './components/ListResults.vue'
import services from './services/ajaxservices'

export default {
	name: 'app',
	components: {
		myForm,
		myList
	},
	data: function () {
		return {
			userInput: {
				taxId: 'asd',
				supplierName: 'dds',
				country: 'ffasd',
				city: '123123',
				street: 'asdzxc',
				zipCode: 'zxczxc'
			},
			existingSuppliers: []
			/*
				{ V -- Dont forget for this obj
					website: {
						url,
						name
					}}
					website format: in url is tax id, website name = companyname
					*/
		};
	},
	methods: {
		AddNewSupplier: function (newObj) {
			this.userInput = newObj;
			console.log($);
			services.SendAjaxRequestToSharepoint(JSON.stringify(this.userInput), $('__REQUESTDIGEST').val())
				.done(function (response) {
					console.log(response);
				})
				.fail(function(err) {
					console.log(err.statusCode);
				})
				.always(function () {
					console.log('Aajx request finished');
				});
		}
	},
	created: function () {
		services.RequestExistingSuppliers(
			'/sites/demo/procurement/_vti_bin/listdata.svc/Suppliers()',
			'&$select=Title,Taxid,Country,city,ZIP_x0020_Code,street,modified,created&$orderby=Title desc'
		).done(function (response) {
			console.log(response);
		});

		services.GetSuppliersWebSites(
			'url',
			'query'
		).done(function (response) {
			existingSuppliers.forEach(function (supplier) {
				// match each site.taxid with suppliers taxid
			}, this);
		});
	}
}
</script>

<style scoped>
#app {
	font-family: 'Avenir', Helvetica, Arial, sans-serif;
	-webkit-font-smoothing: antialiased;
	-moz-osx-font-smoothing: grayscale;
	text-align: center;
	color: #2c3e50;
	margin-top: 60px;
}
</style>
