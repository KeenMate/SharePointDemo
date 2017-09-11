<template>
	<div class="input-field">
		<label :for="localId">{{localLabel}}</label>
		<input
			:type="localType"
			:id="localId"
			:value="value"
			@input="onInput($event.target.value)"
			class="validate"
			:required="localRequired"
			@keyup="onKeyup"/>
	</div>
</template>

<script>
export default {
	props: [
		"label",
		"value", // Used for v-model attribute
		"id",
		"type",
		"required"
	],
	computed: {
		// Symbolises Method for setting up default values
		localId() {
			return this.id != "" ? this.id : "_" + Math.random().toString(39).substring(2, 9)
		},
		localLabel() {
			return this.label != "" ? this.label : "Label text"
		},
		localType() {
			return this.type != "" ? this.type : typeof (this.value) != undefined ? typeof(this.value).toString() : "text"
		},
		localRequired() {
			return this.required === "" || this.required === "required" ? this.required : null;
		}
	},
	methods: {
		onInput(val) {
			// Used for v-model attribute
			this.$emit("input", val);
		},
		onKeyup(event) {
			this.$emit("keyup", event);
		}
	}
}
</script>
