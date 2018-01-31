<template>  
  <ul class="pagination right-align">
    <li :class="(selected == GetNumOfPages() || allDisable) ? 'disabled' : 'waves-effect'"><a @click="NextClick()" href="#!"><i class="material-icons">chevron_right</i></a></li>
  </ul>
            
</template>

<script>
export default {
  name: "pageNext",
  props: ["count", "selected", "itemsPerPage", "allDisable"],
  methods: {
    GetNumOfPages: function() {
      var pages = this.count / this.itemsPerPage;
      return Math.floor(pages) + 1;
    },
    NextClick: function() {
      if (this.selected == this.GetNumOfPages() || this.allDisable) return;
      this.$emit("nextclick");
    },
  }
};
</script>