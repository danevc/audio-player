<template>
    <div class="play-list">
        <audio-elem v-for="a in audiosMetadata" :key=a.Id :audio="a"></audio-elem>
    </div>
</template>

<script>
export default {
    props: {
        audiosMetadata: {
            type: Array,
            required: true
        },
        playlistTitle: {
            type: String,
            required: false
        }
    }
}
</script>

<style>
.playlist-title {
    font-size: 1.4em;
}

.play-list {
    overflow-y: auto;
    width: 60%;
    height: 100%;
    margin-left: auto;
    margin-right: auto;
    border-radius: 2px;
    background-color: rgb(39, 37, 45);
    min-width: 600px;
}
</style>