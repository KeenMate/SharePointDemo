<template>
    <div class="row" :id="'order' + item.OrderID">
        <div class="input-field inline col s5">
            <input disabled id="title" type="text" :value="item.Title" class="validate">
            <label class="active" for="title">Title</label>
        </div>
        <div class="input-field inline col s3">
            <input id="amount" type="number" min="0" :max="item.MaxAmount" @change="$emit('amountChanged')" v-model="item.Amount" class="validate">
            <label class="active" for="amount">Amount ({{item.MaxAmount}})</label>
        </div>
        <div class="input-field inline col s3">
            <input disabled id="totalPrice" type="number" :value="item.Amount * item.UnitPrice" class="validate">
            <label class="active" for="totalPrice">Total Price</label>
        </div>
        <a class="btn-floating btn-medium waves-effect waves-light red" @click="doFancyAnimation($event, item.Title)">
            <i class="material-icons">close</i>
        </a>
    </div>
</template>

<script>
export default {
    name: "item",
    props: [
        "item"
    ],
    methods: {
        doFancyAnimation: function (el, itemTitle) {
            this.$emit("lockButton", true);
            var self = this;
            $(el.target).parent().attr("disabled", true);
            $(el.target).closest("div").animate({ opacity: "0" }, function () {
                self.$emit("itemRemove", itemTitle);
            });
        }
    }
}
</script>
