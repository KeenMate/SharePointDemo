<template>
  <table class="bordered">
      <thead>
          <tr>
              <th>Name</th>
              <th>Amount</th>
              <th>Price</th>
          </tr>
      </thead>
      <tbody>
          <tr v-for="(row, index) in rows" :key="index">
              <td>{{ row.Title }}</td>
              <td>{{ row.Amount }}</td>
              <td>{{ row.TotalPrice }} €</td>
          </tr>
      </tbody>
  </table>
</template>

<script>
export default {
    name: "modalTable",
    props: ["rows"]
}
</script>

<style>

</style>
