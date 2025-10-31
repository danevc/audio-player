<template>
    <div class="play-list">
        <div v-if="audiosMetadata.length > 0">
            <audio-elem @click="playlistClickHandler" v-for="a in audiosMetadata" :key=a.Id :audio="a"></audio-elem>
        </div>
        <div v-else>
            <my-text>Пустой список</my-text>
        </div>
    </div>
</template>

<script>
import { mapMutations } from 'vuex';

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
    },
    methods:{
        ...mapMutations({
                setQueuePlayback: 'player/setQueuePlayback',
            }),
        playlistClickHandler(){
            this.setQueuePlayback(this.audiosMetadata);
        }
    }
}
</script>

<style>
.playlist-title {
    font-size: 1.4em;
}

.play-list {
    overflow-y: hidden;
    border-radius: 2px;
    background-color: rgb(39, 37, 45);
}
</style>