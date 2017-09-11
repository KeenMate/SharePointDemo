<template>
	<div>
		<div class="input-field valign-wrapper">
			<i class="material-icons medium prefix" v-on:click="RedirectSearch()">search</i>
			<input type="text" v-model="supplierName" @keyup.enter="RedirectSearch()" />
			<!-- <v-input label="Search Expression" v-model="supplierName" type="text" required></v-input> -->
			<label id="nameSearchLabel">Search Expression</label>
			<i v-if="$route.query.supplierName" class="material-icons small right" @click="onFilterCancel()">
				close
			</i>
		</div>
	</div>
</template>

<script>
import VInput from "./global-components/v-input.vue"

export default {
	name: 'SearchFormComponent',
	components: {
		"v-input": VInput
	},
	props: [
		'supplierName'
	],
	methods: {
		RedirectSearch() {
			if (this.supplierName !== '')
				this.$emit('searchChanged', this.supplierName);
		},
		onFilterCancel() {
			this.$router.push("/");
		}
	},
	mounted() {
		if (!this.$route.query.supplierName)
			$('#nameSearchLabel').addClass('active');
		else
			$('#nameSearchLabel').removeClass('active');
	}
}
</script>
