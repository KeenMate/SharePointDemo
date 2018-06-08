export default function () {
	return {
		amount: 0,
		itemUnitPrice: 0,
		Name: "",
		MaterialType: "",
		MeasuringUnit: "",
		PricePerUnit: 0,
		ItemsInStock: 0,

		isActive: {
			amount: false,
			saveButton: false,
		},

		IsLoading: true,

		eTag: null,
		ID: null,
		EditedID: null,
		ReloadLog: "",
	}
}
