<template>
  <div class="App">
    <div class="header">
      <nav-bar></nav-bar>
      <my-player></my-player>
    </div>
    <div class="body">
      <router-view></router-view>
    </div>
    <div class="footer">
    </div>
  </div>
  <div v-show="$store.state.isLoading">
    <screen-loader></screen-loader>
  </div>
</template>

<script>
import NavBar from '@/components/NavBar';
import MyPlayer from '@/components/MyPlayer';
export default {
  components: {
    NavBar, MyPlayer
  }
}
</script>

<style>
* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}
.App{
  min-width: 1000px;
  min-height: 100vh;
  background-color: rgb(39, 37, 45);
}
.header {
  position: sticky;
  top: 0;
  background-color: rgb(50, 48, 57);
  box-shadow: 0px 3px 13px 0px rgba(15, 13, 24, 0.3);
}
.body {
  background-color: rgb(39, 37, 45);
}
::-webkit-scrollbar {
  width:11px;
  background-color: rgb(64, 60, 75);
}

::-webkit-scrollbar-thumb {
  border-radius: 10px;
  background-color: rgb(126, 123, 136);
}

::-webkit-scrollbar-track {
  -webkit-box-shadow: inset 0 0 6px rgba(0,0,0,0.2);
  border-radius: 10px;
  background-color: rgb(64, 60, 75);
}
</style>
