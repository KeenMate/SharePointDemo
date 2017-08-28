<template>
	<div>
		<div class="input-field valign-wrapper">
			<i class="material-icons medium prefix" v-on:click="RedirectSearch()">search</i>
			<input type="text" v-model="supplierName" @keyup.enter="RedirectSearch()" />
			<label id="nameSearchLabel">Supplier's name</label>
			<i v-if="$route.query.supplierName" class="material-icons small right" @click="onFilterCancel()">
				close
			</i>
		</div>
	</div>
</template>

<script>
export default {
	props: [
		'supplierName'
	],
	methods: {
		RedirectSearch() {
			if (this.supplierName !== '')
				this.$emit('searchChanged', this.supplierName);
		},
		onFilterCancel() {
			this.$router.push({ name: 'paged', params: { pageSize: this.$route.params.pageSize, pageNumber: 1 }, query: undefined });
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
