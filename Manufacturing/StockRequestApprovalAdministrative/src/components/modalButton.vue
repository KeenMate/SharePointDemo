<template>
  <a @click.prevent="$emit('buttonclick', text)" :key="index" href="#!" class="modal-action modal-close waves-effect waves-green btn-flat">{{ text }}</a>
</template>

<script>
export default {
    name: "modalButton",
    props: ["text"]
}
</script>

<style>

</style>
