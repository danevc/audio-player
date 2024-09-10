<template>
    <div class="page">
        <div class="switch-search" v-if="true">
            <my-text :class="this.currentCase == 'audio' ? 'activecase case' : 'case'"
                @click="this.currentCase = 'audio'">Аудио</my-text>
            <my-text :class="this.currentCase == 'performer' ? 'activecase case' : 'case'"
                @click="this.currentCase = 'performer'">Исполнители</my-text>
            <my-text :class="this.currentCase == 'album' ? 'activecase case' : 'case'"
                @click="this.currentCase = 'album'">Альбомы</my-text>
        </div>
        <play-list v-if="this.currentCase == 'audio'" :audiosMetadata="audiosSearchResult"></play-list>
    </div>
    <div ref="observer" class="observer"></div>
</template>

<script>
import PlayList from '@/components/PlayList';
import { mapState, mapActions } from 'vuex';

export default {
    props: {
        query: {
            type: String
        }
    },
    data() {
        return {
            currentCase: 'audio'
        }
    },
    components: {
        PlayList
    },
    computed: {
        ...mapState({
            audiosSearchResult: state => state.search.audiosSearchResult
        })
    },
    methods: {
        ...mapActions({
            loadAudiosSearchResult: 'search/loadAudiosSearchResult'
        })
    },
    watch: {
        query(val) {
            this.loadAudiosSearchResult(val);
        }
    },
    mounted() {
        this.$store.commit('setIsLoading', true);
        const options = {
            rootMagrgin: '0px',
            threshold: 1.0
        }
        const callback = (entries) => {
            if (entries[0].isIntersecting) {
                this.loadAudiosSearchResult(this.query);
            }
        }
        const observer = new IntersectionObserver(callback, options);
        observer.observe(this.$refs.observer);
        this.$store.commit('setIsLoading', false);
    }
}
</script>

<style>
.case {
    margin-left: 7px;
    margin-right: 7px;
    cursor: pointer;
}

.case:hover {
    color: rgb(213, 218, 223);
}

.activecase {
    text-decoration-line: underline;
    text-underline-offset: 5px;
    text-decoration-thickness: 2px;
}

.switch-search {
    margin-top: 10px;
    display: flex;
    justify-content: center;
}
</style>