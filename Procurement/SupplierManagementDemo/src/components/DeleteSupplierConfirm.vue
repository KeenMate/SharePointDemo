<template>
	<div class="modal">
		<div class="modal-content center-align flow-text">
			Are you sure, you want to delete this item?
		</div>
		<div class="modal-fixed-footer row">
			<div class="green btn center col s3 offset-s2" @click="deleteConfirmed()">
				<div class="waves-effect modal-action">
					<div class="valign-wrapper">
						Yes
						<i class="material-icons">
							delete
						</i>
					</div>
				</div>
			</div>
			<div class="btn red center col s3 offset-s2">
				<div class="waves-effect modal-action modal-close" @click="deleteCanceled()">
					<div class="valign-wrapper">
						No
						<i class="material-icons">
							close
						</i>
					</div>
				</div>
			</div>
		</div>
	</div>
</template>

<script>
export default {
	name: 'DeleteSupplierConfirmComp',
	methods: {
		deleteConfirmed() {
			$('.modal').modal('close');
			this.$emit('deleted');
		},
		deleteCanceled() {
			this.$emit('canceled');
		}
	}
}
</script>
