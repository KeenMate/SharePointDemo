<template>
    <tr v-if="!expanded">
        <td style="width: 30px"><a href="#" @click.prevent="Expand()"><i ref="i" class="material-icons">expand_more</i></a></td>
        <td>{{ item.Created }}</td>
        <td>{{ item.CreatedBy }}</td>
        <td>{{ item.Status }}</td>
    </tr>	
    <tr v-else>
        <td style="width: 30px"><a href="#" @click.prevent="Expand()"><i ref="i" class="material-icons">expand_less</i></a></td>
        <td>
            <table class="bordered">
                <tbody>
                    <tr>
                        <th>Created</th>
                        <td>{{ item.Created }}</td>
                    </tr>
                    <tr>
                        <th>Created By</th>
                        <td>{{ item.CreatedBy }}</td>
                    </tr>
                    <tr>
                        <th>Status</th>
                        <td>{{ item.Status }}</td>
                    </tr>
                    <tr>
                        <th>Delivered on</th>
                        <td>{{ item.DeliveredOn }}</td>
                    </tr>
                    <tr>
                        <th>Approved By</th>
                        <td v-html="DisplayArrayOfNames(item.ApprovedBy)"></td>
                    </tr>
                    <tr v-if="item.Status === 'Waiting for approval'">
                        <th>Remaining approvers</th>
                        <td v-html="GetRemainingApprovers()"></td>
                    </tr>
                    <tr v-if="item.Status === 'Rejected'">
                        <th>Rejected by</th>
                        <td>{{ item.ModifiedBy }}</td>
                    </tr>
                    <tr>
                        <th>Allowed approvers</th>
                        <td v-html="DisplayArrayOfNames(item.AllowedApprovers)"></td>
                    </tr>
                    <tr v-if="item.Status === 'Waiting for approval' && item.AllowedApprovers.contains(user) && !item.ApprovedBy.contains(user)">
                        <th>Action</th>
                        <td>
                            <a @click.prevent="$emit('modalapprove', item)" class="waves-effect waves-light btn modal-trigger" href="#modal1"><i class="material-icons">done</i></a>
                            <a @click.prevent="$emit('modalreject', item)" class="waves-effect waves-light btn modal-trigger" href="#modal1"><i class="material-icons">remove</i></a>
                        </td>
                    </tr>
                    <tr v-if="item.Status === 'Processing...'">
                        <th>Action</th>
                        <td>
                            <loader size="small"></loader>
                        </td>
                    </tr>
                </tbody>
            </table>
        </td>
        <td colspan="2">
            <h5>Items</h5>
            <table class="bordered">
                <thead>
                    <th>Name</th>
                    <th>Amount</th>
                    <th>Price</th>
                    <th>Material Type</th>
                </thead>
                <tbody>
                    <tr v-for="(i, index) in item.Items" :key="index">
                        <td>{{ i.Title }}</td>
                        <td>{{ i.Amount }}</td>
                        <td>{{ num(i.TotalPrice).format('0,0[.]00') }} €</td>
                        <td>{{ i.MaterialType }}</td>
                    </tr>
                </tbody>
            </table>
        </td>
    </tr>
</template>

<script>
import loader from "./loader.vue";

export default {
  name: "row",
  components: {
    loader
  },
  props: ["item", "user"],
  data: function() {
    return {
      expanded: false,
      num: require("numeral")
    };
  },
  methods: {
    Expand: function() {
      if (this.expanded) {
        this.$refs.i.innerHTML = "expand_more";
        this.expanded = false;
      } else {
        this.$refs.i.innerHTML = "expand_less";
        this.expanded = true;
      }
    },
    DisplayArrayOfNames: function(array) {
      var str = "";
      array.forEach(function(el) {
        str += el + "<br>";
      });
      str = str.trimRight("<br>");
      return str;
    },
    GetRemainingApprovers: function() {
      var self = this;
      var arr = [];
      this.item.AllowedApprovers.forEach(function(el) {
        if (!self.item.ApprovedBy.contains(el)) arr.push(el);
      });
      return self.DisplayArrayOfNames(arr);
    }
  }
};
</script>

<style>

</style>
