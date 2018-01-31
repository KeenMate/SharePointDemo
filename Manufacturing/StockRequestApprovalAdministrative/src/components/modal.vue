<template>
  <div ref="modal1" id="modal1" class="modal">
    <div class="modal-content">
      <h4><slot name="header"></slot></h4>
      <p>
        <div v-if="errorMessage !== undefined && errorMessage !== '' && errorMessage !== null" v-html="errorMessage"></div>
        <slot name="body" v-else></slot>
      </p>
    </div>
    <div class="modal-footer">
      <m-button v-for="(button, index) in buttons" :key="index" @buttonclick="Post($event)" :text="button.text"></m-button>
      <!-- <a v-for="(button, index) in buttons" @click.prevent="$emit('buttonclick', button)" :key="index" href="#!" class="modal-action modal-close waves-effect waves-green btn-flat">{{ button.text }}</a> -->
    </div>
  </div>
</template>

<script>
import mButton from "./modalButton.vue";

export default {
  name: "modal",
  components: {
    mButton
  },
  props: ["buttons", "errorMessage"],
  data: function() {
    return {
      promise: null,
      returnResolve: null,
      buttonClicked: false
    };
  },
  mounted: function() {
    var self = this;
    $(this.$refs.modal1).modal({
      complete: function() {
        if (!self.buttonClicked && self.returnResolve !== undefined && self.returnResolve !== null) self.returnResolve("Cancel");
      }
    });
  },
  methods: {
    ShowModal: function() {
      debugger
      console.dir("Rajče");
      console.dir("Error message:" + this.errorMessage);
      var self = this;
      this.buttonClicked = false;
      $(this.$refs.modal1).modal("open");
      this.promise = new Promise(function(resolve, reject) {
        self.returnResolve = resolve;
      });
      return this.promise;
    },
    Post: function(e) {
      this.buttonClicked = true;
      if (this.returnResolve !== undefined && this.returnResolve !== null) this.returnResolve(e);
      this.$emit("buttonclick", e);
    }
  }
};
</script>

<style>

</style>
