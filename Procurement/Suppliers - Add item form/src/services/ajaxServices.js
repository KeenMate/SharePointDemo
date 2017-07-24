const ajaxService = {
	SendAjaxRequestToSharepoint: function (url = '/sites/demo/procurement/_api/web/lists/getbytitle("suppliers")/items', json, requestDigest) {
		return $.ajax({
			url: url,
			method: 'POST',
			headers: {
				'Accept': 'application/json; odata=verbose',
				'X-Request-Digest': requestDigest
			},
			data: json,
		});
	},
	RequestExistingSuppliers: function (url, query) {
		return $.ajax({
			url: url + '?' + query,
			method: 'GET',
			headers: {
				'Accept': 'application/json; odata=verbose'
			}
		});
	},
	GetSuppliersWebSites: function (url, query) {
		return $.ajax({
			url: url + '?' + query,
			method: 'GET',
			headers: {

			}
		});
	}
}
export default ajaxService;

// '/sites/demo/procurement/_api/web/lists/getbytitle("suppliers")/items'
