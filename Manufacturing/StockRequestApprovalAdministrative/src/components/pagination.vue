<template>  
  <p class="center-align">Page {{ selected }} of {{ GetNumOfPages() }}</p>            
</template>

<script>
export default {
  name: "pagination",
  props: ["count", "selected", "itemsPerPage"],
  methods: {
    GetNumOfPages: function() {
      var pages = this.count / this.itemsPerPage;
      if (Number.isInteger(pages)) return pages;
      else return Math.floor(pages) + 1;
    }
  }
};
</script>

<style>

</style>
