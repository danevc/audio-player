<template>
    <div class="search-bar">
        <my-text @click="hintClickHandler" :hint="`${a.value}`" class="elem-search" :class="a.type"
            v-for="a in searchHint">{{ a.value
            }}
            <div v-if="a.type === 'audiotype'">
                <img class="icon" src="@/icones/audiotype__white.png" alt="">
            </div>
            <div v-if="a.type === 'performertype'">
                <img class="icon" src="@/icones/perftype__white.png" alt="">
            </div>
        </my-text>
    </div>
</template>

<script>
import { mapState } from 'vuex';
export default {
    name: 'search-bar',
    methods: {
        hintClickHandler(event) {
            var query = event.target.getAttribute("hint");
            this.$emit('setQuery', query);
        }

    },
    computed: {
        ...mapState({
            searchHint: state => state.search.searchHint,
        }),
    }
}
</script>

<style scoped>
.icon {
    width: 15px;
    height: 15px;
    background-size: cover;
}

.elem-search {
    width: 100%;
    padding: 2px;
    height: 25px;
    margin-top: 3px;
    display: flex;
    justify-content: space-between;
}

.elem-search:hover {
    cursor: pointer;
    background-color: rgb(54, 51, 65);
}

.search-bar {
    padding: 7px;
    text-align: left;
    width: 100%;
    max-height: 450px;
    border-radius: 10px;
    background-color: rgb(37, 35, 43);
    display: flex;
    flex-direction: column;
}
</style>