<template>
	<div class="modal">
		<div class="modal-content">
			<div class="centered row">
				<div class="input-field">
					<label class="active">mySupplier Name (original: {{supplierCopy.CompanyName}})</label>
					<input type="text" v-model="mySupplier.CompanyName" />
				</div>
				<div class="input-field">
					<label class="active">City (original: {{supplierCopy.Address.City}})</label>
					<input type="text" v-model="mySupplier.Address.City" />
				</div>
				<div class="input-field">
					<label class="active">Zip Code (original: {{supplierCopy.Address.ZipCode}})</label>
					<input type="text" v-model="mySupplier.Address.ZipCode" />
				</div>
				<div class="input-field">
					<label class="active">Street (original: {{supplierCopy.Address.Street}})</label>
					<input type="text" v-model="mySupplier.Address.Street" />
				</div>
				<a class="waves-effect waves-light btn green modal-action" type="button" @click="onEditSubmit()">Edit mySupplier</a>
				<a class="waves-effect waves-light right btn red modal-close" type="button">Cancel</a>
			</div>
		</div>
	</div>
</template>

<script>
import ajaxService from './../services/ajaxServices';

export default {
	props: [
		'supplier'
	],
	data() {
		return {
			supplierCopy: {
				Address: {}
			},
			mySupplier: {
				Address: {}
			}
		}
	},
	watch: {
		'supplier'(obj) {
			this.mySupplier = this.DeepObjCopy(obj);
			this.supplierCopy = this.DeepObjCopy(obj);

			console.log('result of deep obj copy: ');
			console.log(this.supplierCopy);

			// making label active for fancier UI
			$('.modal label').addClass('active');
		}
	},
	methods: {
		onEditSubmit() {
			var payload = {
				__metadata: {
					type: 'SP.Data.SuppliersListItem'
				}
			}
			if (this.mySupplier.CompanyName !== this.supplierCopy.CompanyName)
				payload.Title = this.mySupplier.CompanyName;
			if (this.mySupplier.Address.City !== this.supplierCopy.Address.City)
				payload.City = this.mySupplier.Address.City;
			if (this.mySupplier.Address.ZipCode !== this.supplierCopy.Address.ZipCode)
				payload.ZIP_x0020_Code = this.mySupplier.Address.ZipCode;
			if (this.mySupplier.Address.Street !== this.supplierCopy.Address.Street)
				payload.Street = this.mySupplier.Address.Street;

			var self = this;
			ajaxService.EditSupplier(
				this.mySupplier.SharepointInnerId,
				payload,
				ajaxService.RequestNewDigestValue()
			).done(function(response) {
				self.$emit('dataChanged');
			});
			$('.modal').modal('close');
		},
		DeepObjCopy(source) {
			var target = {};
			for (var key in source) {
				if (typeof (source[key]) === 'object') {
					console.log(source[key]);
					target[key] = this.DeepObjCopy(source[key]);
				} else
					target[key] = source[key];
			}
			return target;
		}
	},
	mounted() {
		console.log('modal mounted: taxID: ' + this.mySupplier.TaxId);
		if (!this.mySupplier.TaxId || !this.mySupplier.TaxId) return;
		for (var key in this.mySupplier) {
			this.supplierCopy[key] = this.mySupplier[key];
		}
		Materialize.updateTextFields();
	}
}
</script>
