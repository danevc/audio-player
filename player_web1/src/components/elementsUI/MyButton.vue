<template>
    <button class="btn">
        <slot></slot>
    </button>
</template>

<script>
export default {
    name: 'my-button'
}
</script>

<style>
.btn {
    border: 0cap;
    border-radius: 5px;
    transition: all 100ms ease;
    cursor: pointer;
    background-color: rgb(50, 48, 57);
}

.btn:hover {
    transform: scale(1.05);
    transition-duration: 0.2s;
}
</style>