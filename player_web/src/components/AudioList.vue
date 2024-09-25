<template>
    <div class="play-list" @click="playlistClickHandler">
        <audio-elem v-for="a in audiosMetadata" :key=a.Id :audio="a"></audio-elem>
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
            
            console.log('setQueuePlayback');
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