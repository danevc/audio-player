<template>
  <div class="navbar">
    <div @click="$router.push('/')">
      <div class="title-header"></div>
    </div>
    <div class="search">
      <my-search @searchQuery="search"></my-search>
    </div>
    <div class="upload-audio-container">
      <my-button @click="uploadFileHandler" style="padding: 5px;">
        <my-text>
          Загрузить аудио
        </my-text>
      </my-button>
    </div>
    <dialog-window v-model:show="isShowUploadFile">
      <upload-file-form></upload-file-form>
    </dialog-window>
  </div>
</template>

<script scoped>

export default {
  data() {
    return {
      isShowUploadFile: false
    }
  },
  methods: {
    uploadFileHandler() {
      this.isShowUploadFile = true;
    },
    search(query){
      this.$router.push({ name: 'search', params: { query: query } });
    }
  }
  
}
</script>

<style scoped>
.upload-audio-container {
  margin-left: auto;
}

.title-header {
  width: 70px;
  height: 35px;
  background-image: url('@/icones/logo1.png');
  background-size:cover;
  margin: 10px;
  cursor: pointer;
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
  position: absolute;
  top: 0;
  margin-left: auto;
  margin-right: auto;
  left: 0;
  right: 0;
  text-align: center;
  width: 30%;
  min-width: 300px;
  margin-bottom: 10px;
}
</style>
