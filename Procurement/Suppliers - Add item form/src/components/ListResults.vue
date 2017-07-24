<template>
	<div class="ui horizontal segment">
		<table id="existingSuppliersTable" class="ui selectable celled table">
			<thead>
				<tr>
					<th colspan="7">Suppliers</th>
				</tr>
				<tr>
					<th>Number</th>
					<th id="taxid">Tax ID</th>
					<th id="website">Company's website</th>
					<th id="country">Country</th>
					<th id="city">City</th>
					<th id="zipCode">Zip Code</th>
					<th id="street">Street</th>
				</tr>
			</thead>
			<tbody>
				<tr v-bind:key="supplier.taxId" v-for="(supplier, index) in suppliersArray">
					<td>{{index + 1}}</td>
					<td>{{supplier.taxId}}</td>
					<td>
						<a target="_blank" href="https://keenmate.sharepoint.com/sites/demo/procurement/{{supplier.taxid}}">{{supplier.company}}</a>
					</td>
					<td>{{supplier.country}}</td>
					<td>{{supplier.city}}</td>
					<td>{{supplier.zipCode}}</td>
					<td>{{supplier.Street}}</td>
				</tr>
			</tbody>
			<tfoot>
				<tr>
					<th colspan="7">
						<div class="ui right floated pagination menu">
							<a class="icon item">
								<i class="left chevron icon"></i>
							</a>
							<a class="item">1</a>
							<a class="item">2</a>
							<a class="item">3</a>
							<a class="icon item">
								<i class="right chevron icon"></i>
							</a>
						</div>
					</th>
				</tr>
			</tfoot>
		</table>
	</div>
</template>

<script>
export default {
	name: 'mylist',
	props: ['suppliersArray'],
	data() {
		return {
			pages: [[/*10 suppliers on each page*/]]
		};
	},
	methods: {
		SplitSuppliersBetweenPages: function (containable) {
			var currPage = 0;

			//init arrays for content
			for (var i = 0; i < (suppliersArray.lenght/10) + 1; i++) {
				pages[i] = [];
			}

			suppliersArray.forEach(function (supplier, index) {
				if ((index + 1) % (containable * (currPage + 1)) !== 0) {
					pages[currPage].push(supplier);
				}
				else {
					currPage++;
				}
			}, this);
		}
	}
}
</script>
