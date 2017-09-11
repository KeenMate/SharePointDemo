<template>
	<div>
		<div v-if="isLoading" class="progress">
			<div class="indeterminate"></div>
		</div>
		<div v-else id="baseList">
			<div class="row">
				<!-- PRE-HEADER Filters and pagination here -->
				<div class="col s4">
					<filters :pageSize="PageSize" @pageSizeChanged="onPageSizeChanged"></filters>
				</div>
				<div class="col s8">
					<pagination class="right" :page-count="PageCount" @pageChanged="onPageChanged"></pagination>
				</div>
			</div>
			<div class="row">
				<!-- HEADER -->
				<div class="col s1">
					<strong>Number</strong>
				</div>
				<div class="col s1">
					<strong>Tax Id</strong>
				</div>
				<div class="col s3">
					<strong>Suppliers website</strong>
				</div>
				<div class="col s2">
					<strong>City</strong>
				</div>
				<div class="col s1">
					<strong>Zip Code</strong>
				</div>
				<div class="col s2">
					<strong>Street</strong>
				</div>
				<div class="col s1">Edit/Delete</div>
			</div>
			<div class="row divider"></div>
			<div v-if="!suppliers.length" class="row">
				<!-- NODATA BODY -->
				<div class="col s12 center">
					No data to be displayed
				</div>
			</div>
			<div v-else class="row" :key="index" v-for="(supplier, index) in CurrentPageData">
				<!-- BODY -->
				<div class="col s1">{{supplier.Id}}</div>
				<div class="col s1">{{supplier.TaxId}}</div>
				<div class="col s3">
					<a target="_blank" :href="'/sites/demo/procurement/' + supplier.TaxId">
						{{supplier.CompanyName}}
					</a>
				</div>
				<div class="col s2">{{supplier.Address.City}}</div>
				<div class="col s1">{{supplier.Address.ZipCode}}</div>
				<div class="col s2">{{supplier.Address.Street}}</div>
				<div class="col s1">
					<a @click="editSupplier(supplier.TaxId)">
						<i class="material-icons">
							edit
						</i>
					</a>
					<a @click="deleteSupplier(supplier.TaxId)">
						<i class="material-icons">
							delete
						</i>
					</a>
				</div>
			</div>
			<div class="row divider"></div>
			<div class="row">
				<!-- FOOTER -->
				<div class="col s12">
					<pagination class="right" :page-count="PageCount" @pageChanged="onPageChanged"></pagination>
				</div>
			</div>
		</div>
		<modal-edit id="modaledit" :supplier="edittedSupplier" @dataChanged="onDataChanged()"></modal-edit>
		<delete-confirmation id="modaldelete" @deleted="onDeleteConfirm()"></delete-confirmation>
	</div>
</template>

<script>
import ajaxService from './../services/ajaxServices'
import mapService from './../services/mapServices'

import Pagination from './PaginationForList.vue'
import Filters from './ListFilters.vue'
import ModalEdit from './EditSupplier.vue'
import DeleteConfirmation from './DeleteSupplierConfirm.vue'

export default {
	name: 'MyListComp',
	components: {
		Pagination,
		Filters,
		ModalEdit,
		DeleteConfirmation
	},
	props: [
		'searchExpression'
	],
	data() {
		return {
			suppliers: [],
			suppliersBackup: [],
			edittedSupplier: {
				Address: {}
			},
			deletitionKey: '',
			isLoading: false
		};
	},
	watch: {
		'searchExpression'(val) {
			this.filterList();
		}
	},
	computed: {
		PageCount() {
			console.log('PageCount computed called');
			return (Math.ceil(this.suppliers.length / this.$route.params.pageSize) === 0 ? 1 : Math.ceil(this.suppliers.length / this.$route.params.pageSize));
		},
		CurrentPageData() {
			return this.suppliers.slice((this.$route.params.pageNumber - 1) * this.$route.params.pageSize, this.$route.params.pageNumber * this.$route.params.pageSize);
		},
		PageSize() {
			return this.$route.params.pageSize % 5 === 0 ? this.$route.params.pageSize : 5;
		}
	},
	methods: {
		deleteSupplier(key) {
			this.deletitionKey = key;
			$('#modaldelete').modal('open');
		},
		onDeleteConfirm() {
			var self = this;
			ajaxService.DeleteSupplier(
				self.suppliers.filter(function(supplier) {
					return (supplier.TaxId === self.deletitionKey);
				})[0].SharepointInnerId,
				ajaxService.RequestNewDigestValue()
			).done(function(response) {
				self.UpdateList();
			});
		},
		editSupplier(key) {
			this.edittedSupplier = this.suppliers.filter(function(supplier) {
				return (supplier.TaxId === key);
			})[0];
			$('#modaledit').modal('open');
		},
		onPageChanged(newPage) {
			if (newPage > 0 && newPage < this.PageCount)
				this.$router.push({ name: 'paged', params: { pageNumber: newPage, pageSize: this.$route.params.pageSize }, query: this.$route.query });
		},
		onPageSizeChanged(size) {
			if (size != this.$route.params.pageSize && size % 5 === 0 && size > 0 && size <= 200) {
				this.$router.push({ name: 'paged', params: { pageNumber: 1, pageSize: size }, query: this.$route.query });
			} else {
				console.log('pageSize Changed handler');
				this.$router.push({ name: 'paged', params: { pageNumber: 1, pageSize: 5 }, query: this.$route.query });
			}
		},
		onDataChanged() {
			this.UpdateList();
		},
		filterList() {
			if (this.searchExpression) {
				this.isLoading = true;
				var self = this;
				ajaxService.filterSuppliers(
					this.searchExpression,
					ajaxService.RequestNewDigestValue()
				).done(function(response) {
					self.isLoading = false;
					if (response.d.results.length) {
						console.log(response.d.results);
						self.suppliers = mapService.MapSPResponseToSuppliers(response.d.results);
						self.suppliers.forEach(function(supplier, index) {
							supplier.Id = index + 1;
						});
					}
					else {
						self.suppliers = [];
						console.log('no matched result ... .length = 0');
					}
				});
			} else {
				console.log('supplierName is empty .. refreshing list');
				this.UpdateList();
			}
		},
		UpdateList() {
			if (Number(this.$route.params.pageNumber) > this.PageCount || Number(this.$route.params.pageNumber) < 1) {
				this.$router.push({
					name: 'paged',
					params: {
						pageSize: this.$route.params.pageSize,
						pageNumber: 1,
					},
					query: this.$route.query
				});
			}
			if (this.searchExpression)
				this.filterList();
			else {
				this.isLoading = true;
				var self = this;
				ajaxService.RequestAllItems().done(function(response) {
						self.isLoading = false;
						self.suppliers = mapService.MapSPResponseToSuppliers(response.d.results).reverse();
						self.suppliers.forEach(function(supplier, index) {
							supplier.Id = index + 1;
						});
						self.suppliersBackup = self.suppliers.slice();
					});
			}
		}
	},
	created() {
		this.UpdateList();
	},
	mounted() {
		$('.modal').modal();
	}
}
</script>
