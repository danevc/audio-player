<template>
    <div class="page">
        <my-text>{{ playlist.Title }}</my-text>
        <audio-list v-if="this.currentCase == 'audio'"
            :audiosMetadata="audios" class="audio-list"></audio-list>
    </div>
    <div ref="observer" class="observer"></div>
</template>

<script>
import AudioList from '@/components/AudioList';
import { mapState, mapActions } from 'vuex';

export default {
    props: {
        id: {
            type: Number
        }
    },
    data() {
        return {
            currentCase: 'audio',
            isFirstLoad: true
        }
    },
    components: {
        AudioList
    },
    computed: {
        ...mapState({
            playlist: state => state.playlist.currentPlaylist,
            audios: state => state.playlist.audiosMetadata
        })
    },
    methods: {
        ...mapActions({
            fetchAudiosByPlaylists: 'playlist/fetchAudiosByPlaylists',
            getPlaylist: 'playlist/getPlaylist'
        })
    },
    mounted() {
        this.$store.commit('setIsLoading', true);
        this.getPlaylist(this.id);
        this.fetchAudiosByPlaylists(this.id).then(() => {
            const options = {
                rootMagrgin: '0px',
                threshold: 1.0
            }
            const callback = (entries) => {
                if (entries[0].isIntersecting && !this.isFirstLoad) {
                    this.fetchAudiosByPlaylists(this.id);
                }
                this.isFirstLoad = false;
            }
            const observer = new IntersectionObserver(callback, options);
            observer.observe(this.$refs.observer);
            this.$store.commit('setIsLoading', false);
        });
    }
}
</script>

<style>

</style>