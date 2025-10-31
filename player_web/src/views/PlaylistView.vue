<template>
    <div class="page">
        <div class="page-head">
            <img :src="getSrc" alt="" class="cover-playlist">
            <div class="playlist-info">
                <my-input class="playlist-title" v-model="playlistTitle" @blur="sendNewTitle"></my-input>
                <my-input-multiline class="playlist-description" :placeholder="'Добавить описание'"
                    v-model="playlistDescription" @blur="sendNewDescription"></my-input-multiline>
                <my-text>Всего аудио: {{ totalCount }}</my-text>
            </div>
        </div>
        <audio-list :audiosMetadata="audios" class="audio-list"></audio-list>
    </div>
    <div ref="observer" class="observer"></div>
</template>

<script>
import AudioList from '@/components/AudioList';
import { mapState, mapActions, mapGetters } from 'vuex';

export default {
    props: {
        id: {
            type: Number
        }
    },
    data() {
        return {
            isFirstLoad: true,
            playlistTitle: '',
            playlistDescription: ''
        }
    },
    components: {
        AudioList
    },
    computed: {
        ...mapState({
            playlist: state => state.playlist.currentPlaylist,
            audios: state => state.playlist.audiosMetadata,
            totalCount: state => state.playlist.totalCount
        }),
        ...mapGetters({
            getSrc: 'playlist/getSrc'
        })
    },
    methods: {
        ...mapActions({
            fetchAudiosByPlaylists: 'playlist/fetchAudiosByPlaylists',
            getPlaylist: 'playlist/getPlaylist',
            setTitle: 'playlist/setTitle',
            setDescription: 'playlist/setDescription'
        }),
        sendNewTitle() {
            this.setTitle(this.playlistTitle);
        },
        sendNewDescription() {
            this.setDescription(this.playlistDescription);
        }
    },
    mounted() {
        this.$store.commit('setIsLoading', true);

        this.getPlaylist(this.id).then(() => {
            this.playlistTitle = this.playlist.Title;
            this.playlistDescription = this.playlist.Description;
        });

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

<style scoped>
.cover-playlist {
    border-radius: 5px;
    width: 180px;
    min-width: 180px;
    height: 180px;
    background-size: cover;
}

.page-head {
    display: flex;
    width: 70%;
    margin: 30px auto 10px auto;
    box-sizing: border-box;
}

.playlist-title {
    font-size: 1.4em;
    margin-top: 10px;
}

.playlist-description {
    margin-top: 5px;
    resize: none;
}

.playlist-info {
    margin-left: 25px;
}
</style>