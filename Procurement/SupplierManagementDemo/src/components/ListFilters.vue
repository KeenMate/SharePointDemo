<template>
	<div>
		<div class="col s4">
			<select v-model="pageSize" @change="onPageSizeChanged" class="browser-default">
				<option value="5" selected>5</option>
				<option value="10">10</option>
				<option value="15">15</option>
				<option value="30">30</option>
				<option value="60">60</option>
				<option value="100">100</option>
				<option value="200">200</option>
			</select>
		</div>
	</div>
</template>

<script>
export default {
	name: 'myListFiltersComp',
	props: ['pageSize'],
	methods: {
		onPageSizeChanged(newVal) {
			console.log('new val of pageSize is: ');
			console.log(newVal.currentTarget.value);
			if (newVal.currentTarget.value != this.$route.params.pageSize)
				this.$emit('pageSizeChanged', newVal.currentTarget.value);
		}
	},
	mounted() {
		console.log('filter mounted with prop: ' + this.pageSize);
	}
}
</script>