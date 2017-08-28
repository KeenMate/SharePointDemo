const mapService = {
	MapSPResponseToSuppliers(SPsuppliersArray) {
		var array = SPsuppliersArray.map(function (item) {
			var newItem = {
				Address: {
					City: '',
					Street: '',
					ZipCode: ''
				},
				CompanyName: '',
				TaxId: '',
				Id: -1
			};
			newItem.CompanyName = item.Title ? item.Title : item.SupplierName ? item.SupplierName : "Name not found";
			newItem.TaxId = item.TaxID;
			newItem.Address.City = item.City;
			newItem.Address.Street = item.Street;
			newItem.Address.ZipCode = item.ZIP_x0020_Code ? item.ZIP_x0020_Code : item.ZIPCode ? item.ZIPCode : "Zip Code not found";
			newItem.SharepointInnerId = item.Id;

			return newItem;
		});
		return array;
	}
}

export default mapService;