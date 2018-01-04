<template>
	<div>
		<div class="row">
			<v-input v-model="myModel.taxId" id="taxId" label="Tax ID" type="number" required></v-input>
			<v-input v-model="myModel.supplierName" type="text" label="Supplier's name" id="supplierName" required></v-input>
			<v-input v-model="myModel.city" type="text" label="City" id="city" required></v-input>
		</div>
		<div class="row">
			<v-input v-model="myModel.street" type="text" class="col s6" id="street" label="Street" required></v-input>
			<v-input type="number" v-model="myModel.zipCode" class="col s6" id="zipCode" label="Zip Code" required></v-input>
		</div>
		<button class="btn waves-effect waves-light green btn-action" v-on:click.prevent="SubmitForm()">Add new Supplier
			<i class="material-icons right">add</i>
		</button>
		<button class="right btn waves-effect waves-light red btn-action" @click="$router.go(-1)">Cancel</button>
	</div>
</template>

<script>
import VInput from "./global-components/v-input.vue"

import ajaxService from './../services/ajaxServices'

export default {
	name: 'NewSupplierFormComp',
	components: {
		"v-input": VInput
	},
	data() {
		return {
			myModel: {
				supplierName: "",
				city: "",
				street: ""
			}
		}
	},
	methods: {
		IsValidFormData() {
			console.log('validation started');
			console.log(this.myModel);
			return this.myModel.taxId && !isNaN(this.myModel.taxId) && this.myModel.taxId.toString().length === 8
				&& this.myModel.supplierName
				&& this.myModel.city
				&& this.myModel.street
				&& this.myModel.zipCode && this.myModel.zipCode.toString().length === 5;
		},
		SubmitForm() {
			console.log(this.myModel);
			console.log('valid data? ' + this.IsValidFormData());
			if (!this.IsValidFormData()) {
				console.log('invalid data');
				return;
			}
			var digest = ajaxService.RequestNewDigestValue();
			var self = this;
			ajaxService.AddNewItemToSPList(digest, this.myModel)
				.done(function(response) {
					self.$router.push({ name: 'paged' });
				});
		}
	},
	mounted() {
		console.log('new supplier form mounted');
	},
	created() {
		console.log('new supplier form created');
	}
}
</script>

<style scoped>
	.btn-action {
		margin-top: 10px;
	}
</style>
