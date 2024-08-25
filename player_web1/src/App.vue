<template>
  <div v-show="$store.state.isLoading">
    <div class="header">
      <nav-bar></nav-bar>
      <my-player></my-player>
    </div>
    <div class="body">
      <HomeView></HomeView>
    </div>
    <div class="footer">
    </div>
  </div>
  <my-loader></my-loader>
</template>

<script>
import NavBar from '@/components/NavBar';
import MyPlayer from '@/components/MyPlayer';
import HomeView from '@/views/HomeView';
export default {
  components: {
    NavBar, HomeView, MyPlayer
  }
}
</script>

<style>
* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
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
</style>
