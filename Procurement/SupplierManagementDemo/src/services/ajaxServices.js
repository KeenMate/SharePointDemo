const ajaxService = {
	RequestExistingSuppliers (
		url = "https://keenmate.sharepoint.com/sites/demo/procurement/_api/web/lists/getbytitle('Suppliers')/items"
	) {
		return $.ajax({
			url: url,
			method: 'GET',
			headers: {
				'Accept': 'application/json; odata=verbose'
			}
		})
	},
	RequestNewDigestValue(
		url = 'https://keenmate.sharepoint.com/sites/demo') {
		var digest;
		$.ajax({
			url: url + '/_api/contextinfo',
			method: 'POST',
			headers: { 'Accept': 'application/json; odata=verbose' },
			async: false
		}).done(function (response) {
			digest = response.d.GetContextWebInformation.FormDigestValue;
		});
		return digest;
	},
	AddNewItemToSPList(
		digest,
		item,
		url = 'https://keenmate.sharepoint.com/sites/demo/procurement',
		listName = 'Suppliers'
	) {
		return $.ajax({
			url: url + "/_api/web/lists/getbytitle('" + listName + "')/items",
			data: JSON.stringify({
				'__metadata': {
					type: 'SP.Data.' + listName + 'ListItem'
				},
				'Title': item.supplierName,
				'TaxID': item.taxId,
				'City': item.city,
				'ZIP_x0020_Code': item.zipCode,
				'Street': item.street
			}),
			type: 'POST',
			headers:
			{
				"Accept": "application/json;odata=verbose",
				"Content-Type": "application/json;odata=verbose",
				"X-RequestDigest": digest,
				"X-HTTP-Method": "POST"
			}
		})
	},
	EditSupplier(
		itemId,
		payload,
		digest,
		url = 'https://keenmate.sharepoint.com/sites/demo/procurement',
		listName = 'Suppliers'
	) {
		return $.ajax({
			url: url + "/_api/web/lists/getbytitle('" + listName + "')/Items(" + itemId + ")",
			data: JSON.stringify(payload),
			type: 'MERGE',
			contentType: 'application/json; odata=verbose',
			headers: {
				'Accept': 'application/json; odata=verbose',
				'X-RequestDigest': digest,
				'If-Match': '*'
			}
		});
	},
	DeleteSupplier(
		itemId,
		digest,
		url = 'https://keenmate.sharepoint.com/sites/demo/procurement',
		listName = 'suppliers'
	) {
		console.log('delete called with item id: ' + itemId);
		return $.ajax({
			url: url + "/_api/web/lists/getbytitle('" + listName + "')/Items(" + itemId + ")",
			type: 'DELETE',
			headers: {
				'Accept': 'application/json; odata=verbose',
				'X-RequestDigest': digest,
				'If-Match': '*'
			}
		});
	},
	filterSuppliers(
		expression,
		digestValue,
		listName = 'Suppliers',
		url = 'https://keenmate.sharepoint.com/sites/demo/procurement'
	) {
		console.log('from filter suppliers function in ajax service: expression is: ' + expression);
		var buildedQuery = encodeURI("/_vti_bin/listdata.svc/" + listName + "?" +
			"$filter=substringof('" + expression + "',SupplierName) or " +
			//"TaxID eq '" + expression + "' or" +
			"substringof('" + expression + "',Street) or " +
			//"ZIPCode eq '" + expression + "' or " +
			"substringof('" + expression + "',City)&$orderby=Created");
		console.log('builded query: ' + buildedQuery);
		return $.ajax({
			url: url + buildedQuery,
			type: 'GET',
			headers: {
				'Accept': 'application/json; odata=verbose'
			}
		});
	}
}
export default ajaxService;

// '/sites/demo/procurement/_api/web/lists/getbytitle("suppliers")/items'
