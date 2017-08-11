<template>
  <div id="app">
    <loading v-bind:isLoading="isLoading"></loading>
    <div class="row">
      <mat-autocomplete @onAutocompleted="addItem($event)" @loadingChanged="isLoading = $event"></mat-autocomplete>
    </div>
    <div class="row">
      <div class="input-field inline col s6">
        <input :disabled="DateDisabled" id="datepicker" type="text" class="datepicker" @blur="parseDatePicker()">
        <label class="active" for="datepicker">Delivered On</label>
      </div>
      <div class="input-field inline col s6">
        <input :disabled="TimeDisabled" id="timepicker" type="text" class="timepicker" @blur="parseTimePicker(false)">
        <label for="timepicker">Delivered At</label>
      </div>
    </div>
    <item :item="item" v-bind:key="item.OrderID" v-for="item in stockItems" @lockButton="testBtn(true)" @amountChanged="testBtn" @itemRemove="removeItem($event)"></item>
    <div class="row">
      <button :disabled="!ButtonEnabled" class="btn waves-effect waves-light" @click.prevent="send" type="submit" name="action">
        <span v-if="stockItems.length === 0">Select an item</span>
        <span v-else-if="stockItems.length === 1">Request 1 item
        </span>
        <span v-else>Request {{stockItems.length}} items
        </span>
      </button>
    </div>
  </div>
</template>

<script>
import service from "./services/stockItemsService"
import loading from "./components/loading.vue"
import matAutocomplete from "./components/mat-autocomplete.vue"
import item from "./components/item.vue"
import debounce from "./services/debounce"
import moment from "moment"
import flash from "./services/flash"

export default {
  name: 'app',
  components: {
    "loading": loading,
    "mat-autocomplete": matAutocomplete,
    "item": item
  },
  data() {
    return {
      //searchText: "",
      isLoading: false,
      stockItems: [],
      orderID: 0,
      TimeDisabled: true,
      DateDisabled: true,
      DeliveredOn: new Date(2000, 1, 1),
      ButtonEnabled: false
    }
  },
  created() {

  },
  methods: {
    testBtn: function (lock = false) {
      if (lock || this.DeliveredOn < Date.now() || $(".datepicker").val() === "" || $(".timepicker").val() === "") {
        this.ButtonEnabled = false;
        return;
      }
      if (this.stockItems.filter(function (e) { return e.Amount > 0 }).length === this.stockItems.length
        && this.stockItems.length !== 0
        && this.stockItems.filter(function (e) { return e.Amount <= e.MaxAmount }).length === this.stockItems.length) {
        this.ButtonEnabled = true;
      }
      else {
        this.ButtonEnabled = false;
      }
    },
    initializePicker: function () {
      $('.datepicker').pickadate({
        selectMonths: true, // Creates a dropdown to control month
        selectYears: 1, // Creates a dropdown of 15 years to control year,
        today: 'Today',
        clear: 'Clear',
        close: 'OK',
        closeOnSelect: true // Close upon selecting a date,
      });

      $('.timepicker').pickatime({
        default: '12:00', // Set default time: 'now', '1:30AM', '16:30'
        twelvehour: false, // Use AM/PM or 24-hour format
        donetext: 'OK', // text for done-button
        cleartext: 'Clear', // text for clear-button
        canceltext: 'Cancel', // Text for cancel-button
        autoclose: true, // automatic close timepicker
      });
      this.DateDisabled = false;
    },
    parseDatePicker: function () {
      var self = this;
      self.testBtn(true);
      setTimeout(function () {
        var fn = function () {
          if ($("#datepicker").prev().attr("aria-hidden") === "true") {
            clearInterval(x);
            self.DeliveredOn = moment($("#datepicker").val()).utcOffset('+0200');
            if ($("#datepicker").val() === "")
              self.TimeDisabled = true;
            else
              self.TimeDisabled = false;
            if ($("#timepicker").val() !== "")
              self.parseTimePicker(true);
            self.testBtn();
          }
        };
        var x = setInterval(fn, 1000);
      }, 1000);
    },
    parseTimePicker: function (fast) {
      var self = this;
      if (fast === true) {
        self.DeliveredOn = moment(moment(self.DeliveredOn).format("YYYY-MM-DD") + " " + $("#timepicker").val()).utcOffset('+0200');
        return
      }
      self.testBtn(true);
      setTimeout(function () {
        var fn = function () {
          if ($("#timepicker").next().hasClass("picker--opened") === false) {
            clearInterval(x);
            self.DeliveredOn = moment(moment(self.DeliveredOn).format("YYYY-MM-DD") + " " + $("#timepicker").val()).utcOffset('+0200');
            self.testBtn();
          }
        };

        var x = setInterval(fn, 1000);
      }, 1000);
    },
    removeItem: function (title) {
      this.stockItems = this.stockItems.filter(x => x.Title !== title);
      this.testBtn();
    },
    addItem: function (item) {
      var items = this.stockItems.filter(x => x.Title === item.Title);
      if (items.length > 0) {
        flash("#order" + items[0].OrderID);
        return;
      }
      if (this.DateDisabled === true)
        this.initializePicker();
      item.OrderID = this.orderID++;
      this.stockItems.push(item);
      this.testBtn();
    },
    send: function () {
      var self = this;
      this.testBtn(true);
      this.isLoading = true;
      var finalJSON = {
        __metadata: {
          type: 'SP.Data.StockrequestListItem'
        },
        DeliveredOn: moment(this.DeliveredOn).toDate()
      };
      var DataJSON = [];
      this.stockItems.forEach(function (element) {
        var json = {
          Amount: element.Amount,
          Title: element.Title,
          EditedID: element.EditedID,
          TotalPrice: element.Amount * element.UnitPrice,
          MaterialType: element.MaterialType
        };
        DataJSON.push(json);
      });
      console.dir(DataJSON);
      finalJSON.Data = JSON.stringify(DataJSON);
      console.dir(finalJSON);
      service.sendNewItem(finalJSON).done(function () {
        self.stockItems = [];
        self.isLoading = false;
        Materialize.toast("Items requested", 7500);
      }).fail(function (result) {
        Materialize.toast("Operation failed. See console for more details.", 15000);
        console.dir(result);
        self.isLoading = false;
      });
    }
  }
}
</script>

<style>
#app {
  font-family: 'Avenir', Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
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
</style>
