<template>  
  <ul class="pagination right-align">
    <li :class="(selected == 1 || allDisable) ? 'disabled' : 'waves-effect'"><a @click="PrevClick()" href="#!"><i class="material-icons">chevron_left</i></a></li>
  </ul>
            
</template>

<script>
export default {
  name: "pagePrev",
  props: ["selected", "allDisable"],
  methods: {
    PrevClick: function() {
      if (this.selected == 1 || this.allDisable) return;
      this.$emit("prevclick");
    },
  }
};
</script>