const service = {
    searchByTitle: function (text) {
        return $.ajax({
            url: "/sites/demo/manufacturingfacility/_vti_bin/listdata.svc/StockItems()?$filter=startswith(Title, '"
            + text
            + "')&$select=Title,Id,MaterialType,MeasuringUnit,UnitPrice&$expand=MaterialType,MeasuringUnit&$top=10",
            dataType: "json"
        });
    },
    loadStockItem: function (params) {
        return $.ajax({
            url: "/sites/demo/manufacturingfacility/_vti_bin/listdata.svc/Stock()?$filter=(Item/Title eq '"
            + params
            + "')&$expand=Item&$select=Item/Title,Item/Id,Price,Amount,Id",
            dataType: "json"
        });
    },
    parseAutocompleteData: function (result) {
        var obj = result.d;
        var toReturn = {};
        obj.forEach((value) => {
            toReturn[value.Title] = null;
        });
        return toReturn;
    },
    sendNewItem: function (JSONObj) {
        return $.ajax({
            url: "/sites/demo/manufacturingfacility/_api/web/lists/GetByTitle('Stock')/items",
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
            url: "/sites/demo/manufacturingfacility/_api/web/lists/GetByTitle('Stock')/items(" + editedItemID + ")",
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
            url: "/sites/demo/manufacturingfacility/_api/web/lists/GetByTitle('Stock - Transaction Log')/items",
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
            url: "/sites/demo/manufacturingfacility/_vti_bin/listdata.svc/StockTransactionLog()?&$select=Created,Operation,Title,Amount,TotalPrice,CreatedBy/Jméno&$filter=(Title eq '"
            + itemName +
            "')&$expand=Operation,CreatedBy,Created&$orderby=Created desc&$top=10",
            dataType: "json"
        });
    }
}
export default service