<template>
    <div class="dialog" v-if="show" @click.stop="hideDialog" >
      <div @click.stop class="dialog___content">
        <slot></slot>
      </div>
    </div>
  </template>

<script>
export default{
    name: 'dialog-window',
    props:{
        show:{
            type: Boolean,
            default: false
        }
    },
    methods: {
        hideDialog(){
            this.$emit('update:show', false);
        }
    }
}

</script>

<style scoped>
.dialog {
  top: 0;
  bottom: 0;
  right: 0;
  left: 0;
  background: rgba(0, 0, 0, 0.5);
  position: fixed;
  display: flex;
}

.dialog___content {
  margin: 100px auto auto auto;
  background: rgb(81, 78, 92);
  border-radius: 12px;
  min-height: 200px;
  min-width: 450px;
  padding: 20px;
}
</style>
