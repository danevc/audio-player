<template>
    <input :value="modelValue" @input="updateInput" class="input" type="text" :placeholder="placeholder">
</template>

<script>
export default {
    name: 'my-input',
    props: {
        modelValue: [String, Number],
        placeholder: {
            type: String,
            required: false
        }
    },
    methods: {
        updateInput(event) {
            this.$emit('update:modelValue', event.target.value)
        }
    }
}
</script>

<style scoped>
.input {
    width: 100%;
    height: 33px;
    border: 2px solid rgb(30, 28, 39);
    background-color: rgb(51, 48, 59);
    padding-left:10px;
    margin-top: 15px;
    border-radius: 10px;
    color:white;
    font-family: 'Trebuchet MS';
    font-size: 1.0em;
}
.input:focus {
    border: 2px solid rgb(30, 28, 39);
    background-color: rgb(51, 48, 59);
    padding: 5px 5px;
    margin-top: 15px;
    border-radius: 12px;
    border: 2px solid rgb(30, 28, 39);
}
</style>