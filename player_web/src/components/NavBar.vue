<template>
  <div class="navbar">
    <div @click="$router.push('/')">
      <my-text class="title-header">daire</my-text>
    </div>
    <div class="search">
      <my-input :placeholder="'Поиск'" v-model="searchValue" @keyup.enter="enterHandler"></my-input>
      <search-bar ref="searchBar" v-show="isVisibleSearchBar" @setQuery="search"></search-bar>
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      searchValue: '',
      isSearchFocus: false,
      isVisibleSearchBar: false
    }
  },
  methods: {
    search(query) {
      this.isVisibleSearchBar = false;
      this.searchValue = query;
      this.$router.push({ name: 'search', params: { query: query } });
    },
    enterHandler() {
      this.search(this.searchValue);
    }
  },
  watch: {
    searchValue(value) {
      if (value) {
        this.isVisibleSearchBar = true;
        this.$store.dispatch('SearchHint', value);
      }
    }
  },
  mounted() {
    document.addEventListener('click', (e) => {
      if(this.isVisibleSearchBar){
        const withinBoundaries = e.composedPath().includes(this.$refs.searchBar);
        if (!withinBoundaries) {
          this.isVisibleSearchBar = false; 
        }
      }
    })
  }
}
</script>

<style>
.title-header {
  font-size: 1.3em;
  margin: 20px;
}

.performers-header {
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
