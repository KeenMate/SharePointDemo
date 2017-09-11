const ajaxService = {
	defaultUrl: _spPageContextInfo.webAbsoluteUrl,
	defaultListName: "Suppliers",
	UrlToList(listName = defaultListName) {
		return `/_api/web/lists/getbytitle('${listName}')/items`;
	},

	RequestAllItems(
		url = this.defaultUrl,
		listName = this.defaultListName
	) {
		return $.ajax({
			url: url + this.UrlToList(listName),
			method: 'GET',
			headers: {
				'Accept': 'application/json; odata=verbose'
			}
		})
	},
	RequestNewDigestValue(
		url = _spPageContextInfo.siteAbsoluteUrl
	) {
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
		url = this.defaultUrl,
		listName = this.defaultListName
	) {
		return $.ajax({
			url: url + this.UrlToList(listName),
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
		url = this.defaultUrl,
		listName = this.defaultListName
	) {
		return $.ajax({
			url: url + this.UrlToList(listName) + `(${itemId})`,
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
		url = this.defaultUrl,
		listName = this.defaultListName
	) {
		return $.ajax({
			url: url + this.UrlToList(listName) + `(${itemId})`,
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
		url = this.defaultUrl,
		listName = this.defaultListName
	) {
		var buildedQuery = encodeURI("/_vti_bin/listdata.svc/" + listName + "?" +
			"$filter=substringof('" + expression + "',SupplierName) or " +
			"substringof('" + expression + "',Street) or " +
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
