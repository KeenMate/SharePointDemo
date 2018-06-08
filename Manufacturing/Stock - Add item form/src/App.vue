<template>
  <div id="app">
    <div class="row">
      <div class="input-field col s6">
        <input v-bind:disabled="!isActive.amount" v-model="amount" v-on:change="testSend" id="amount" min="0" type="number" class="validate">
        <label id="lAmount" for="amount">Select an Item to type amount</label>
      </div>
      <loading v-bind:isLoading="IsLoading"></loading>
      <div class="col s6" id="ItemInfo">
        <item-info class="item-info" v-bind:data="{pricePerUnit: PricePerUnit, itemsInStock: ItemsInStock, materialType: MaterialType, measuringUnit: MeasuringUnit}"></item-info>
      </div>
    </div>
    <div class="row removeMargin">
      <div class="input-field col s6">
        <input disabled id="totalPrice" class="removeMargin validate" :value="CalcTotalPrice" type="text">
        <label for="totalPrice">Total price</label>
      </div>
      <div class="input-field col s6">
        <input disabled id="totalAmount" class="removeMargin validate" :value="CalcTotalAmount" type="text">
        <label for="totalAmount">Total amount</label>
      </div>
    </div>
    <div class="row removeMargin">
      <button v-bind:disabled="!isActive.saveButton" class="btn waves-effect waves-light" id="sendBtn" @click.prevent="send">
        Add
        <i class="material-icons right ">playlist_add</i>
      </button>
    </div>
  
    <div class="row ">
      <transaction-log v-bind:name="Name" @loadingChanged="isLoading($event)"></transaction-log>
    </div>
</template>

<script>
import { Observable } from "rxjs/Observable";
import "rxjs/add/observable/fromEvent";
import service from "./services/stockItemsService";
import moment from "moment";
import dataFunc from "./viewModels/appViewModel";
import transactionLog from "./components/transaction-log.vue";
import itemInfo from "./components/item-info.vue";
import loading from "./components/loading.vue";

export default {
  name: "app",
  components: {
    transactionLog,
    itemInfo,
    loading
  },
  data: function() {
    return dataFunc();
  },
  subscriptions() {
    return {
      filterText: Observable.fromEvent(
        document.querySelector("#autocomplete-input"),
        "input"
      )
        .filter(event => {
          if (this.Name !== "") {
            this.resetItem();
            this.isLoading(false);
          }
          return event.target.value.length > 0;
        })
        .debounceTime(250)
        .map(event => event.target.value)
    };
  },
  computed: {
    CalcTotalPrice: function() {
      return (
        parseInt(this.itemUnitPrice) *
        (parseInt(this.amount) + parseInt(this.ItemsInStock))
      );
    },
    CalcTotalAmount: function() {
      return parseInt(this.amount) + parseInt(this.ItemsInStock);
    },
    CalcPriceForSelectedAmount: function() {
      return parseInt(this.itemUnitPrice) * parseInt(this.amount);
    }
  },
  created() {
    var self = this;

    this.$observables.filterText.subscribe(searchText => {
      service.searchByTitle(searchText).done(function(result) {
        $(".autocomplete-content").off();
        $("#autocomplete-input").autocomplete({
          data: service.parseAutocompleteData(result),
          limit: Config.Stock.AutocompleteLimit,
          onAutocomplete: function(params) {
            self.autocomplete(params, result);
          },
          minLength: 1
        });
        if (searchText in service.parseAutocompleteData(result)) {
          self.autocomplete(searchText, result);
        }
        $("#autocomplete-input").trigger("focus");
      });
    });
    this.isLoading(false);
  },
  methods: {
    autocomplete: function(params, result) {
      var self = this;
      self.isLoading(true);
      var stockItem = result.d.filter(x => x.Title === params)[0];
      service.loadStockItem(params).done(function(stockAmount) {
        var stock = stockAmount.d.results;
        if (stock.length > 0) {
          stock = stock[0];
          self.ItemsInStock = stock.Amount;
          self.eTag = stock.__metadata.etag;
          self.EditedID = stock.Id;
        } else {
          self.ItemsInStock = 0;
          self.eTag = null;
        }
        self.ID = stockItem.Id;
        self.MaterialType = stockItem.MaterialType.Value;
        self.MeasuringUnit = stockItem.MeasuringUnit.Value;
        self.itemUnitPrice = stockItem.UnitPrice;

        self.Name = stockItem.Title;
        self.PricePerUnit = stockItem.UnitPrice;
        self.amount = 0;

        self.isActive.amount = true;
        self.autocompleteEnable();
        $("#lAmount").text("Amount");
      });
    },
    send: function(event) {
      var self = this;
      this.isLoading(true);
      var JSONToSend = {
        __metadata: {
          type: "SP.Data.StockListItem"
        },
        Price: this.CalcTotalPrice,
        Amount: this.CalcTotalAmount
      };
      if (this.eTag === null) {
        JSONToSend.ItemId = this.ID;
        service
          .sendNewItem(JSONToSend)
          .done(function() {
            self.sendLog();
          })
          .fail(function(data) {
            Materialize.toast(
              "Operation failed, see console for more details.",
              7500
            );
            console.dir(data);
            self.isLoading(false);
          });
      } else {
        var ieTag = this.eTag.split('"');
        ieTag = parseInt(ieTag[1]);
        service
          .sendEditedItem(JSONToSend, this.EditedID, ieTag)
          .done(function() {
            self.sendLog();
          })
          .fail(function(data) {
            if (data.responseText.includes("request ETag value")) {
              Materialize.toast(
                "Ooops! Seems like someone was faster than you. See what happend in the log below and try it again.",
                20000
              );
            } else if (data.responseText.includes("does not exist")) {
              Materialize.toast(
                "Ooops! Seems like " +
                  self.Name +
                  " no longer exists. It was probably deleted while you were editing. Simply try to create it again.",
                20000
              );
            } else {
              Materialize.toast(
                "Operation failed, see console for more details.",
                10000
              );
              console.dir(data);
            }
            self.resetItem();
            self.autocompleteReset();
            self.isLoading(false);
          });
      }
    },
    sendLog: function() {
      var self = this;
      var toSend = {
        __metadata: {
          type: "SP.Data.StockTransactionLogListItem"
        },
        Amount: parseInt(this.amount),
        TotalPrice: this.CalcPriceForSelectedAmount,
        Title: this.Name,
        Operation: "Stock-In",
        WhatId: this.ID
      };
      service
        .sendLog(toSend)
        .done(function() {
          self.autocompleteReset();
          Materialize.toast(
            self.amount +
              " of " +
              self.Name +
              " was successfully added to stock",
            15000
          );
          self.resetItem();
          self.isLoading(false);
        })
        .fail(function(data) {
          alert("Operation failed, see console for more details.");
          console.dir(data);
          self.isLoading(false);
        });
    },
    autocompleteReset: function() {
      $("#autocomplete-input").val("");
      this.autocompleteEnable();
      $("#autocomplete-input").trigger("focus");
    },
    autocompleteEnable: function() {
      $("#autocomplete-input").removeAttr("disabled");
    },
    autocompleteDisable: function() {
      $("#autocomplete-input").attr("disabled", true);
    },
    isLoading: function(bool) {
      if (bool) {
        this.IsLoading = true;
        this.autocompleteDisable();
        this.isActive.amount = false;
        this.isActive.saveButton = false;
      } else {
        this.IsLoading = false;
        this.autocompleteEnable();
        this.testAmount();
      }
    },
    resetItem: function() {
      this.Name = "";
      this.amount = 0;
      this.MaterialType = "";
      this.MeasuringUnit = "";
      this.PricePerUnit = 0;
      this.ItemsInStock = 0;
      this.eTag = null;
      this.ID = null;
      this.EditedID = null;
    },
    testAmount: function() {
      if (this.Name !== "") {
        this.isActive.amount = true;
      } else {
        this.isActive.amount = false;
      }
      this.testSend();
    },
    testSend: function() {
      if (this.amount > 0) {
        this.isActive.saveButton = true;
      } else {
        this.isActive.saveButton = false;
      }
    },
    parseDate: function(date) {
      return moment(date).format("YYYY-MM-DD HH:mm:ss");
    }
  }
};
</script>

<style>
#app {
  font-family: "Avenir", Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 20px;
}

h1,
h2 {
  font-weight: normal;
}

ul {
  list-style-type: none;
  padding: 0;
}

li {
  display: inline-block;
  margin: 0 10px;
}

a {
  color: #42b983;
}

.item-info {
  margin-top: 10px;
}

.removeMargin {
  margin-bottom: 0px !important;
}

ul.autocomplete-content {
  overflow: hidden;
}
</style>
