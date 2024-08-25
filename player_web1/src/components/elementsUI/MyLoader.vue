<template>
    <div v-if="!$store.state.isLoading" class="loading-screen">
    <my-text class="loader-another"></my-text>
  </div>
</template>

<script>
export default {
    name: 'my-loader'
}
</script>

<style scoped>
.loading-screen {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  z-index: 999;
  display: flex;
  align-items: center;
  justify-content: center;
  background-color: rgb(39, 37, 45);
}

.loader {
  width: 85px;
  height: 50px;
  --g1:conic-gradient(from  90deg at left   3px top   3px,#0000 90deg,#fff 0);
  --g2:conic-gradient(from -90deg at bottom 3px right 3px,#0000 90deg,#fff 0);
  background: var(--g1),var(--g1),var(--g1), var(--g2),var(--g2),var(--g2);
  background-position: left,center,right;
  background-repeat: no-repeat;
  animation: l10 1.2s infinite alternate;
}
@keyframes l10 {
  0%,
  2%   {background-size:25px 50% ,25px 50% ,25px 50%}
  20%  {background-size:25px 25% ,25px 50% ,25px 50%}
  40%  {background-size:25px 100%,25px 25% ,25px 50%}
  60%  {background-size:25px 50% ,25px 100%,25px 25%}
  80%  {background-size:25px 50% ,25px 50% ,25px 100%}
  98%,
  100% {background-size:25px 50% ,25px 50% ,25px 50%}
}

/* HTML: <div class="loader"></div> */
.loader-another {
  font-weight: bold;
  font-family: sans-serif;
  font-size: 30px;
  animation: l1 0.6s linear infinite alternate;
}
.loader-another:before {
  content:"Loading..."
}
@keyframes l1 {to{opacity: 0}}
</style>