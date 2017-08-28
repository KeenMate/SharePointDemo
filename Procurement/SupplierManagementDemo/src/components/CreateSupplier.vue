<template>
	<div>
		<div class="row">
			<div class="input-field">
				<label for="taxId">Tax ID</label>
				<input type="number" id="taxId" v-model="myModel.taxId" class="validate" />
			</div>
			<div class="input-field">
				<label for="supplierName">Supplier's Name</label>
				<input type="text" id="supplierName" v-model="myModel.supplierName" class="validate" />
			</div>
			<div class="input-field">
				<label for="city">City</label>
				<input type="text" id="city" v-model="myModel.city" class="validate" />
			</div>
		</div>
		<div class="row">
			<div class="input-field col s6">
				<label for="street">Street</label>
				<input type="text" id="street" v-model="myModel.street" class="validate" />
			</div>
			<div class="input-field col s6">
				<label for="zipCode">Zip Code</label>
				<input type="number" id="zipCode" v-model="myModel.zipCode" class="validate" />
			</div>
		</div>
		<button class="btn waves-effect waves-light green" v-on:click.prevent="SubmitForm()">Add new Supplier
			<i class="material-icons right">add</i>
		</button>
		<button class="right btn waves-effect waves-light red" @click="$router.go(-1)">Cancel</button>
	</div>
</template>

<script>
import ajaxService from './../services/ajaxServices'

export default {
	name: 'NewSupplierForm',
	data() {
		return {
			myModel: {}
		}
	},
	methods: {
		IsValidFormData() {
			console.log('validation started');
			console.log(this.myModel);
			// if (!this.myModel.taxId) {
			// 	console.log('taxid empty or undefined');
			// 	return false;
			// }
			// if (this.myModel.taxId.toString().length !== 8) {
			// 	console.log('taxid is not 8 chars long');
			// 	return false;
			// }
			// if (!this.myModel.supplierName) {
			// 	console.log('supplier empty or undefined');
			// 	return false;
			// }
			// if (!this.myModel.city) {
			// 	console.log('city empty or undefined');
			// 	return false;
			// }
			// if (!this.myModel.street) {
			// 	console.log('street empty or undefined');
			// 	return false;
			// }
			// if (!this.myModel.zipCode) {
			// 	console.log('zipCode empty or undefined');
			// 	return false;
			// }
			// if (this.myModel.zipCode.toString().length !== 5) {
			// 	console.log('zipcode is not 5 chars long');
			// 	return false;
			// }
			// console.log('form is valid');
			// return true;
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
