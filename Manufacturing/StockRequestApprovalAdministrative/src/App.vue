<template>
  <div id="app">  
    <a class="waves-effect waves-light btn" href="Report/GenerateReport">Get Stock Report</a>
    <h3 v-if="currentUser != ''">Welcome {{ currentUser }}</h3> 
    <loader v-else-if="!isLoading" size="small" centered="true"></loader>    
    <div class="row">
      <div class="col s12">
        <p>
          <input v-model="onlyUnresolved" :disabled="isLoading || itemProcessing" type="checkbox" class="filled-in" id="filled-in-box" />
          <label for="filled-in-box">Show only unresolved orders.</label>
        </p>
      </div>
    </div>
    <div class="row">
      <div class="col s1">
        <page-prev @prevclick="PrevPage()" :selected="pageSelected" :all-disable="!canClick"></page-prev>
      </div>
      <div class="col s10">
       
      </div>
      <div class="col s1">
        <page-next :count="totalItems" :items-per-page="itemsPerPage" @nextclick="NextPage()" :selected="pageSelected" :all-disable="!canClick"></page-next>
      </div>
    </div>
    <div class="row">
      <div class="col s12">
        <table v-if="!isLoading" class="bordered highlight">
          <thead>
            <tr>
              <th scope="col"></th>
              <th scope="col">Created</th>
              <th scope="col">Created By</th>
              <th scope="col">Status</th>
            </tr>
          </thead>
        	<tbody>
          	<row @modalapprove="ModalApprove" @modalreject="ModalReject" v-for="item in data" :user="currentUser" :processing="itemProcessing" :key="item.Created" :item="item"></row>
        	</tbody>
        </table>
        <loader v-else size="big" centered="true"></loader>
      </div>
    </div>
		<div class="row">
      <div class="col s1">
        <page-prev @prevclick="PrevPage()" :selected="pageSelected" :all-disable="!canClick"></page-prev>
      </div>
      <div class="col s10">
       
      </div>
      <div class="col s1">
        <page-next :count="totalItems" :items-per-page="itemsPerPage" @nextclick="NextPage()" :selected="pageSelected" :all-disable="!canClick"></page-next>
      </div>
    </div>
    <pagination @pageclick="Page($event)" @nextclick="NextPage()" @prevclick="PrevPage()" :all-disable="!canClick" :count="totalItems" :selected="pageSelected" :items-per-page="itemsPerPage"></pagination>

    <modal ref="modal1" :buttons="modalButtons" :error-message="modalError" @buttonclick="ModalClick">
      <template slot="header">
        {{modalHeader}}
      </template>  
      <template slot="body">
        {{modalText}}
        <modal-table v-if="modalTable != null" :rows="modalTable"></modal-table>
      </template>    
      </modal>
  </div>
</template>

<script>
import $ from "jquery";
import loader from "./components/loader.vue";
import extensions from "./extensions";
import provider from "./providers/restProvider";
import responser from "./responser";
import modal from "./components/modal.vue";
import row from "./components/row.vue";
import modalTable from "./components/modalTable.vue";
import pagination from "./components/pagination.vue";
import pagePrev from "./components/pagePrev.vue";
import pageNext from "./components/pageNext.vue";
import moment from "moment";

export default {
  name: "app",
  components: {
    loader,
    modal,
    row,
    modalTable,
    pagination,
    pagePrev,
    pageNext
  },
  data() {
    return {
      data: [],
      isLoading: true,
      currentUser: "",
      canClick: false,
      modalHeader: "",
      modalText: "",
      modalButtons: [],
      modalTable: false,
      modalError: "something",
      currentItem: null,
      itemProcessing: false,
      totalItems: null,
      onlyUnresolved: false,
      itemsPerPage: 5,
      pageSelected: 1,
      paginationString: "",
      num: require("numeral")
    };
  },
  mounted: function() {
    extensions.registerExtensions();
    this.GetData();
  },
  methods: {
    NextPage: function() {
      this.pageSelected++;
      var last = this.data[this.data.length - 1];
      this.GetPageOnlyData();
    },
    PrevPage: function() {
      this.pageSelected--;
      var last = this.data[0].Created;
      console.dir(last);
      this.paginationString =
        "PagedPrev=TRUE&Paged=TRUE&p_Created=" +
        moment(last, "DD.MM.YYYY HH:mm:ss").format(
          "YYYYMMDD[%20]HH[%3a]mm[%3a]ss"
        ) +
        "&p_ID=" +
        this.data[0].ID;
      this.GetPageOnlyData();
    },
    PostProcess: function(data, approve) {
      if (approve === undefined) approve = true;
      var err;
      if (approve == true) {
        err = responser.PostApprove(this.currentItem, data, this.currentUser);
      } else {
        err = responser.PostReject(this.currentItem, data, this.currentUser);
      }
      if (err) {
        this.PrepareModal(
          "Error occurred.",
          "There is some error",
          [{ text: "OK" }],
          null,
          data
        );
        this.$refs.modal1.ShowModal();
      }
      this.itemProcessing = false;
    },
    ModalClick: function(button) {
      var self = this;
      console.dir(button);
      if (button == "Approve") {
        this.itemProcessing = true;
        var res = provider.GetData(
          "Response/Approve",
          "guid=" + this.currentItem.RequestID
        );
        this.currentItem.Status = "Processing...";
        res.done(function(data) {
          console.dir(data);
          self.PostProcess(data);
        });
      } else if (button == "Reject") {
        this.itemProcessing = true;
        var res = provider.GetData(
          "Response/Reject",
          "guid=" + this.currentItem.RequestID
        );
        this.currentItem.Status = "Processing...";
        res.done(function(data) {
          console.dir(data);
          self.PostProcess(data, false);
        });
      }
    },
    ModalApprove: function(item) {
      var self = this;
      this.currentItem = item;
      this.PrepareModal(
        "Confirm approval",
        "Are you sure you want to approve following items?",
        [{ text: "Approve" }, { text: "Cancel" }],
        item.Items,
        null
      );
    },
    ModalReject: function(item) {
      this.currentItem = item;
      this.PrepareModal(
        "Confirm reject",
        "Are you sure you want to reject this request?",
        [{ text: "Reject" }, { text: "Cancel" }],
        null,
        null
      );
    },
    GetPageOnlyData: function() {
      this.isLoading = true;
      this.canClick = false;
      var self = this;

      var getDataResult = provider.GetData(
        null,
        "count=" +
          this.itemsPerPage +
          "&position=" +
          self.paginationString.replace(/&/g, "_AMP_") +
          (this.onlyUnresolved
            ? "&onlyUnresolved=true"
            : "&onlyUnresolved=false")
      );
      console.dir(self.paginationString);
      getDataResult
        .done(function(data) {
          if (data) {
            self.data = data.Data;
            if (data.Pos != null) self.paginationString = data.Pos;
          }
          self.canClick = true;
          self.isLoading = false;
        })
        .fail(function(response) {
          self.isLoading = false;
          self.canClick = false;
          console.dir(response);
          self.PrepareModal(
            response.statusText,
            null,
            [{ text: "Well, OK then..." }],
            null,
            response.responseText
          );
          self.$refs.modal1.ShowModal();
        });
    },
    GetData: function() {
      this.isLoading = true;
      this.canClick = false;
      var self = this;

      var getDataResult = provider.GetData(null, "count=" + this.itemsPerPage);
      var userResult = provider.GetData("RequestsOverview/GetCurrentUser");
      var totalItems = provider.GetData("RequestsOverview/GetItemCount");
      var isAuthenticated = provider.GetData(
        "RequestsOverview/IsAuthenticated"
      );

      isAuthenticated
        .done(function(response) {
          $.when(getDataResult, userResult, totalItems)
            .done(function(data, user, count) {
              if (data) {
                self.data = data.Data;
                self.paginationString = data.Pos;
              }
              if (user) {
                self.currentUser = user;
              }
              if (count) {
                self.totalItems = count;
                console.dir(self.totalItems);
              }
              self.canClick = true;
              self.isLoading = false;
            })
            .fail(function(response) {
              self.isLoading = false;
              self.canClick = false;
              console.dir(response);
              self.PrepareModal(
                response.statusText,
                null,
                [{ text: "Well, OK then..." }],
                null,
                response.responseText
              );
              self.$refs.modal1.ShowModal();
            });
        })
        .fail(function(response) {
          if (response.status == 401) window.location.replace("Authenticate");
          else {
            self.isLoading = false;
            self.canClick = false;
            console.dir(response);
            self.PrepareModal(
              response.statusText,
              null,
              [{ text: "Well, OK then..." }],
              null,
              response.responseText
            );
            self.$refs.modal1.ShowModal();
          }
        });
    },
    PrepareModal: function(header, content, buttons, table, error) {
      this.modalHeader = header;
      this.modalText = content;
      this.modalButtons = buttons;
      this.modalTable = table;
      this.modalError = error;
    }
  },
  watch: {
    onlyUnresolved: function() {
      console.dir(this.onlyUnresolved);
      this.pageSelected = 1;
      this.paginationString = "";
      var self = this;
      var totalItems = provider.GetData(
        "RequestsOverview/GetItemCount",
        "onlyUnresolved=" + self.onlyUnresolved
      );
      totalItems.done(function(count) {
        self.totalItems = count;
        self.GetPageOnlyData();
      });
    }
  }
};
</script>

<style>
#app {
  margin-top: 50px;
}
</style>
