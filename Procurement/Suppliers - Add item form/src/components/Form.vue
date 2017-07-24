<template>
	<div id="myBlock" class="ui form">
		<div class="ui input">
			<!-- <label for="taxId">Tax ID</label><br/> -->
			<input type="text" id="taxId" v-model="inputObj.taxId" name="taxId" placeholder="Tax ID" />
		</div>
	
		<div class="ui input">
			<!-- <label for="supplierName">Supplier's Name</label><br/> -->
			<input type="text" id="supplierName" v-model="inputObj.supplierName" name="supplierName" placeholder="Supplier's Name" />
		</div>
	
		<div class="ui input">
			<!-- <label for="country">Country</label><br/> -->
			<input type="text" id="country" v-model="inputObj.country" name="country" placeholder="Country" />
		</div>
	
		<div class="ui input">
			<!-- <label for="city">City</label><br/> -->
			<input type="text" id="city" v-model="inputObj.city" name="city" placeholder="City" />
		</div>
	
		<div class="ui input">
			<!-- <label for="street">Street</label><br/> -->
			<input type="text" id="street" v-model="inputObj.street" name="street" placeholder="Street" />
		</div>
	
		<div class="ui input">
			<!-- <label for="zipCode">Zip Code</label><br/> -->
			<input type="text" id="zipCode" v-model="inputObj.zipCode" name="zipCode" placeholder="Zip Code" />
		</div>
	
		<div class="ui submit button" @click="SubmitForm()">Add New Supplier</div>
		<div class="ui error message"></div>
	</div>
</template>

<script>
export default {
	name: 'myform',
	props: ['inputObj'],
	methods: {
		SubmitForm: function() {
			console.log(this.inputObj);
			this.$emit('submitted', this.inputObj);
		}
	}
}
</script>
