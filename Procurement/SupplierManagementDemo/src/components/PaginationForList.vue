<template>
	<ul class="ui pagination">
		<li :class="{'waves-effect': true, disabled: (Number($route.params.pageNumber) === 1)}">
			<router-link :to="{ name: 'paged', params: { pageSize: $route.params.pageSize, pageNumber: 1}, query: $route.query}">
				<i class="material-icons">skip_previous</i>
			</router-link>
		</li>
		<li :class="{'waves-effect': true, disabled: (Number($route.params.pageNumber) === 1)}">
			<router-link :to="{ name: 'paged', params: { pageSize: $route.params.pageSize, pageNumber: $route.params.pageNumber > 1 ? $route.params.pageNumber - 1 : 1}, query: $route.query}">
				<i class="material-icons">navigate_before</i>
			</router-link>
		</li>
		<li :id="number" v-for="(number, index) in pageCount" :key="index" :class="{'waves-effect': (number !== $route.params.pageNumber), active: (number === Number($route.params.pageNumber))}">
			<router-link :to="{ name: 'paged', params: { pageSize: $route.params.pageSize, pageNumber: number}, query: $route.query}">
				{{number}}
			</router-link>
		</li>
		<li :class="{'waves-effect': true, disabled: (Number($route.params.pageNumber) === pageCount)}">
			<router-link :to="{ name: 'paged', params: { pageSize: $route.params.pageSize, pageNumber: $route.params.pageNumber < pageCount ? Number($route.params.pageNumber) + 1 : pageCount}, query: $route.query}">
				<i class="material-icons">navigate_next</i>
			</router-link>
		</li>
		<li :class="{'waves-effect': true, disabled: (Number($route.params.pageNumber) === pageCount)}">
			<router-link :to="{ name: 'paged', params: { pageSize: $route.params.pageSize, pageNumber: pageCount}, query: $route.query}">
				<i class="material-icons">skip_next</i>
			</router-link>
		</li>
	</ul>
</template>

<script>
export default {
	props: [
		'pageCount'
	]
}
</script>
