<template>
    <div class="playlists-section">
        <div class="head-playlists-section">
            <my-text>{{ title }}</my-text>
            <my-button class="add-playlist-btn" @click="addPlaylistBtnHandler">Добавить</my-button>
        </div>
        <div class="body-playlists-section">
            <playlist-tile v-for="p in playlists" :key=p.Id :playlist="p"></playlist-tile>
        </div>
    </div>
</template>

<script>
import { mapMutations } from 'vuex';

export default {
    props: {
        playlists: {
            type: Array,
            required: true
        },
        title: {
            type: String,
            required: true
        }
    },
    methods: {
        ...mapMutations({
            setQueuePlayback: 'player/setQueuePlayback',
        }),
        playlistClickHandler() {
            this.setQueuePlayback(this.audiosMetadata);
        },
        addPlaylistBtnHandler(){
            this.$router.push({ name: 'add-playlist' });
        }
    }
}
</script>

<style scoped>
.playlist-title {
    font-size: 1.4em;
}
.playlists-section{
    margin-top:20px;
}
.body-playlists-section {
    display: flex;
    flex-wrap: wrap;
}
.head-playlists-section {
    padding:10px;
    display: flex;
    justify-content: space-between;
}
.add-playlist-btn {
    color: white;
    padding: 5px;
    border: none;
    cursor: pointer;
}
</style>