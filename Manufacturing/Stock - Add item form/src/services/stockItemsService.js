const service = {
	searchByTitle: function (text) {
		return $.ajax({
			url: Config.SiteAddressFromRoot + "_vti_bin/listdata.svc/StockItems()?$filter=startswith(" +
				Config.SI_FieldName_Title + ", '" +
				text + "')&$select=" +
				Config.SI_FieldName_Title + "," +
				Config.SI_FieldName_Id + "," +
				Config.SI_FieldName_MaterialType + "," +
				Config.SI_FieldName_MeasuringUnit + "," +
				Config.SI_FieldName_UnitPrice + "&$expand=" +
				Config.SI_FieldName_MaterialType + "," +
				Config.SI_FieldName_MeasuringUnit + "&$top=" +
				Config.Stock.AutocompleteLimit,
			dataType: "json"
		});
	},
	loadStockItem: function (params) {
		return $.ajax({
			url: Config.SiteAddressFromRoot + "_vti_bin/listdata.svc/Stock()?$filter=(" +
				Config.St_FieldName_Item + "/" + Config.SI_FieldName_Title +
				" eq '" +
				params +
				"')&$expand=" +
				Config.St_FieldName_Item + "&$select=" +
				Config.St_FieldName_Item + "/" + Config.SI_FieldName_Title + "," +
				Config.St_FieldName_Item + "/" + Config.SI_FieldName_Id + "," +
				Config.St_FieldName_Price + "," +
				Config.St_FieldName_Amount + "," +
				Config.St_FieldName_Id,
			dataType: "json"
		});
	},


	parseAutocompleteData: function (result) {
		var obj = result.d;
		var toReturn = {};
		obj.forEach(function (value) {
			toReturn[value.Title] = null;
		});
		return toReturn;
	},
	sendNewItem: function (JSONObj) {
		return $.ajax({
			url: Config.SiteAddressFromRoot + "_api/web/lists/GetByTitle('" + Config.ListName_Stock + "')/items",
			type: "POST",
			data: JSON.stringify(JSONObj),
			headers: {
				"accept": "application/json;odata=verbose",
				"content-type": "application/json;odata=verbose",
				"X-RequestDigest": $("#__REQUESTDIGEST").val()
			}
		});
	},
	sendEditedItem: function (JSONObj, editedItemID, eTag) {
		return $.ajax({
			url: Config.SiteAddressFromRoot + "_api/web/lists/GetByTitle('" + Config.ListName_Stock + "')/items(" + editedItemID + ")",
			type: "PATCH",
			data: JSON.stringify(JSONObj),
			headers: {
				"IF-MATCH": "\"" + eTag + "\"",
				"content-type": "application/json;odata=verbose",
				"X-RequestDigest": $("#__REQUESTDIGEST").val()
			}
		});
	},
	sendLog: function (JSONObj) {
		return $.ajax({
			url: Config.SiteAddressFromRoot + "_api/web/lists/GetByTitle('" + Config.ListName_StockTransactionLog + "')/items",
			type: "POST",
			data: JSON.stringify(JSONObj),
			headers: {
				"accept": "application/json;odata=verbose",
				"content-type": "application/json;odata=verbose",
				"X-RequestDigest": $("#__REQUESTDIGEST").val()
			}
		});
	},
	loadLog: function (itemName) {
		return $.ajax({
			url: Config.SiteAddressFromRoot + "_vti_bin/listdata.svc/StockTransactionLog()?&$select=" +
				Config.STL_FieldName_Created + "," +
				Config.STL_FieldName_Operation + "," +
				Config.STL_FieldName_Title + "," +
				Config.STL_FieldName_Amount + "," +
				Config.STL_FieldName_TotalPrice + "," +
				Config.STL_FieldName_CreatedBy + "/" + Config.STL_FieldName_CreatedBy_Name +
				"&$filter=(" + Config.STL_FieldName_Title + " eq '"
				+ itemName +
				"')&$expand=" +
				Config.STL_FieldName_Operation + "," +
				Config.STL_FieldName_CreatedBy + "," +
				Config.STL_FieldName_Created + "&$orderby=" +
				Config.STL_FieldName_Created + " desc&$top=" +
				Config.Stock.TransactionLogLimit,
			dataType: "json"
		});
	}
}
export default service