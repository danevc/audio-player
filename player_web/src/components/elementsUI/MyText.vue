<template>
    <div class="text">
        <slot></slot>
    </div>
</template>

<script>
export default{
   name: 'my-text'
}
</script>

<style>
.text{
    color: rgb(255, 255, 255);
    font-family: 'Trebuchet MS';
    text-overflow: ellipsis;
}
</style>