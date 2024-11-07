<template>
    <div class="playlist-tile-container" @click="clickPlaylistTileHandler">
        <img :src="getSrc" alt="" class="cover-playlist">
        <my-text class="title-playlist">{{ playlist.Title }}</my-text>
    </div>
</template>

<script>
export default {
    name: 'playlist-tile',
    props: {
        playlist: {
            type: Object,
            required: true
        }
    },
    methods: {
        clickPlaylistTileHandler() {
            this.$router.push({ name: 'playlist', params: { id: this.playlist.Id } });
        }
    },
    computed: {
        getSrc() {
            var baseUrl = this.$store.state.baseURL;
            return `${baseUrl}Playlist/GetPlaylistCover?id=${this.playlist.Id}`
        }
    },
}
</script>

<style scoped>
.cover-playlist {
    border-radius: 5px;
    width: 140px;
    height: 140px;
    background-size: cover;
}

.playlist-tile-container {
    padding: 5px;
    width: 150px;
    height: 180px;
    margin: 5px;
    cursor: pointer;
    border-radius: 5px;
}

.playlist-tile-container:hover {
    outline: 2px solid #afadad;
}

.title-playlist {
    padding: 5px;
    font-size: 0.95em;
    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
}
</style>