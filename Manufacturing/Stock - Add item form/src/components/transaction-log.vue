<template>
	<div>
		<h6 v-if='name !== ""'>All transactions of {{name}}:</h6>
		<h6 v-else>Please select an item to display a transaction log</h6>
		<table class="striped bordered ">
			<thead>
				<tr>
					<td>Created</td>
					<td>Operation</td>
					<td class="center-align ">Title</td>
					<td class="right-align ">Amount</td>
					<td class="right-align ">Total price</td>
					<td class="center-align ">Created by</td>
				</tr>
			</thead>
			<tbody>
				<tr v-bind:key="l.Created " v-for="l in Log ">
					<td>{{parseDate(l.Created)}}</td>				
					<template v-if="l.Operation !== null">
						<td>{{l.Operation.Value}}</td>
						<td class="center-align">{{l.Title}}</td>
						<td class="right-align">{{l.Amount}}</td>
						<td class="right-align">{{l.TotalPrice}} €</td>
						<td class="center-align">{{l.CreatedBy.Jméno}}</td>
					</template>
					<template v-else>
						<td colspan="5">Cannot display this transaction</td>
					</template>
				</tr>
			</tbody>
		</table>
	</div>
</template>

<script>
import moment from "moment";
// import service from "../services/stockItemsService";

export default {
  name: "transaction-log",
  props: ["name", "forceLoad"],
  data() {
    return {
      Log: null
    };
  },
  watch: {
    name: function(newName) {
      if (newName !== "") this.loadLog(newName);
      else {
        this.Log = null;
      }
    },
    forceLoad: function(newVal) {
      this.loadLog(newVal);
      this.forceLoaded();
      this.forceLoaded = "";
    }
  },
  methods: {
    parseDate: function(date) {
      return moment(date).format("YYYY-MM-DD HH:mm:ss");
    },
    loadLog: function(name) {
      var self = this;
      this.isLoading(true);
      service.loadLog(name).done(function(data) {
        self.Log = data.d;
        self.isLoading(false);
      });
    },
    isLoading: function(bool) {
      this.$emit("loadingChanged", bool);
    },
    forceLoaded: function() {
      this.$emit("forceLoaded", "");
    }
  }
};
</script>
