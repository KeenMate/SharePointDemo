<template>
	<div>
		<div class="col s4">
			<select v-model.number="pageSize" class="browser-default">
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
	props: ['pageSize'],
	watch: {
		'pageSize'(val) {
			this.$emit('pageSizeChanged', val);
		}
	}
}
</script>