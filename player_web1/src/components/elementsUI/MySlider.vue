<template>
  <div class="slider-container">
  <!-- <transition name="slide-fade">
      <my-text class="value-slider" v-if="isShow">{{ modelValue }}</my-text>
      <my-text v-else></my-text>
    </transition> -->
    <input :value="modelValue" @input="updateInput" type="range" min="0" max="100" class="slider">
  </div>
</template>

<script>
export default {
  name: 'my-slider',
  data() {
    return {
      isShow: false,
      timer: null
    }
  },
  props: {
    modelValue: {
            type: Number,
            required: false
        }
  },
  methods: {

    updateInput(event) {
      this.$emit('update:modelValue', event.target.value)
    }
  },
  watch: {
    modelValue() {
      clearTimeout(this.timer);
      this.isShow = true;
      this.timer = setTimeout(() => {
        this.isShow = false;
      }, 2500);
    }
  }
}
</script>

<style scoped>
.slider-container{
  justify-content: flex-end; 
  align-items: center; 
}
.slider {
  -webkit-appearance: none;
  appearance: none;
  width: 80px;
  height: 4px;
  border-radius: 2px;
  background: rgb(107, 105, 117);
  opacity: 0.7;
  -webkit-transition: .2s;
  transition: opacity .2s;
}

.slider:hover {
  opacity: 1;
}

.slider::-webkit-slider-thumb {
  -webkit-appearance: none;
  appearance: none;
  width: 12px;
  height: 12px;
  border-radius: 6px;
  background: rgb(235, 234, 240);
  cursor: pointer;
}
.slide-fade-enter-active {
  transition: opacity 0.2s cubic-bezier(1.0, 0.5, 0.8, 1.0);
}

.slide-fade-leave-active {
  transition: opacity 0.2s cubic-bezier(1.0, 0.5, 0.8, 1.0);
}

.slide-fade-leave-from,
.slide-fade-enter-to{
  opacity: 1;
}
.slide-fade-leave-to,
.slide-fade-enter-from {
  opacity: 0;
}

.value-slider {
  margin-right: 7px;
  color:rgb(148, 143, 165);
}
</style>