<template>
    <table class="bordered">
        <tbody>
            <tr>
                <td>Price per Unit:</td>
                <td>{{data.pricePerUnit}}</td>
                <td>Items in stock:</td>
                <td>{{data.itemsInStock}}</td>
            </tr>
            <tr>
                <td>Material type:</td>
                <td>{{data.materialType}}</td>
                <td>Measuring unit:</td>
                <td>{{data.measuringUnit}}</td>
            </tr>
        </tbody>
    </table>
</template>

<script>
export default {
    name: "item-info",
    props: [
        "data"
    ]
}
</script>
