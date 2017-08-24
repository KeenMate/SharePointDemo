<template>
    <div class="input-field col s12">
        <input v-model="searchText" type="text" id="autocomplete-input" class="autocomplete">
        <label for="autocomplete-input">Stock Item</label>
    </div>
</template>

<script>

import debounce from "../services/debounce"
import service from "../services/stockItemsService"

export default {
    name: "mat-autocomplete",
    props: [
        "searchText",
    ],
    data() {
        return {
            Item: {}
        }
    },
    watch: {
        searchText: debounce(function (value) {
            var self = this;
            service.searchByTitle(value)
                .done(function (result) {
                    $('#autocomplete-input').off();
                    $('#autocomplete-input').autocomplete({
                        data: service.parseAutocompleteData(result),
                        limit: 10,
                        onAutocomplete: function (params) {
                            self.autocomplete(params, result);
                        },
                        minLength: 1
                    });
                    if (value in service.parseAutocompleteData(result)) {
                        self.autocomplete(value, result);
                    }
                    $("#autocomplete-input").trigger("focus");
                });
        }, 250)
    },
    methods: {
        autocomplete: function (params, result) {
            var self = this;
            this.$emit("loadingChanged", true);
            var stockItem = result.d.filter(x => x.Title === params)[0];
            $("#autocomplete-input").text("");
            service.loadStockItem(params)
                .done(function (stockAmount) {
                    var stock = stockAmount.d.results;
                    if (stock.length > 0) {
                        stock = stock[0];
                        self.Item = {
                            Title: stockItem.Title,
                            UnitPrice: stockItem.UnitPrice,
                            MaxAmount: stock.Amount,
                            Amount: 0,
                            EditedID: stock.Id,
                            MaterialType: stock.Item.MaterialType.Value
                        };
                        self.$emit("onAutocompleted", self.Item);
                    }
                    else if (stock.length === 0) {
                        Materialize.toast("There are no items to withdraw.", 10000);
                    }
                    else {
                        Materialize.toast("Unable to load item", 10000);
                        console.dir(stockAmount);
                    }
                    self.$emit("loadingChanged", false);
                });
        }
    }
}
</script>
