<template>
    <my-input :placeholder="'Поиск'" v-model="searchValue" @keyup.enter="enterHandler"></my-input>
    <search-bar ref="searchBar" v-show="isVisibleSearchBar" @setQuery="search"></search-bar>
</template>

<script>
export default {
    name: 'my-search',
    emits: ["searchQuery"],
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
            this.$emit('searchQuery', query)
        },
        enterHandler() {
            this.search(this.searchValue);
        }
    },
    watch: {
        searchValue(value) {
            if (value) {
                this.isVisibleSearchBar = true;
                this.$store.dispatch('search/SearchHint', value);
            }
        }
    },
    mounted() {
        document.addEventListener('click', (e) => {
            if (this.isVisibleSearchBar) {
                const withinBoundaries = e.composedPath().includes(this.$refs.searchBar);
                if (!withinBoundaries) {
                    this.isVisibleSearchBar = false;
                }
            }
        })
    }
}
</script>

<style scoped>
.input {
    width: 100%;
    height: 33px;
    border: 2px solid rgb(30, 28, 39);
    background-color: rgb(51, 48, 59);
    padding-left: 10px;
    margin-top: 15px;
    border-radius: 10px;
    color: white;
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