<template>
    <textarea :value="modelValue" @input="updateInput" rows="3" cols="60" class="textarea" :placeholder="placeholder"></textarea>
</template>

<script>
export default {
    name: 'my-input-multiline',
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
.textarea {
    border: 2px solid rgb(39, 37, 45);
    background-color: rgb(39, 37, 45);
    padding:10px;
    border-radius: 10px;
    font-family: 'Trebuchet MS';
    font-size: 0.9em;
    color:rgb(219, 221, 224);
}
.textarea:focus {
    background-color: rgb(51, 48, 59);
    color:white;
}
.textarea:active, :hover, :focus {
    outline: 0;
    outline-offset: 0;
}
</style>