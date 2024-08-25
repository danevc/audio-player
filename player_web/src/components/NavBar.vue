<template>
  <div class="navbar">
    <div @click="$router.push('/')">
      <my-text class="title-header">musica</my-text>
    </div>
    <div @click="$router.push('/')">
      <my-text class="performers-header">performers</my-text>
    </div>
    <div class="search">
      <my-input :placeholder="'Поиск'" v-model="searchValue"></my-input>
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      searchValue: ''
    }
  },
  methods: {

  },
  watch: {
    searchValue(value) {
      console.log(value);
    }
  }
}
</script>

<style>
.title-header {
  font-size: 1.3em;
  margin:20px;
}

.performers-header{
  font-size: 1.2em;
}

.navbar {
  height: 60px;
  background-color: rgb(39, 37, 45);
  color: white;
  font-family: Arial;
  letter-spacing: 0.05cap;
  display: flex;
  align-items: center;
  padding: 0 15px;
  margin-bottom: 30px;
  box-shadow: 0px 3px 5px 0px rgba(39, 37, 45, 0.3);
}

.search {
  width: 30%;
  position: relative;
  margin-left: auto;
  margin-right: auto;
  margin-bottom: 10px;
}
</style>
