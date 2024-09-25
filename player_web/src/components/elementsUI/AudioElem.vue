<template>
    <transition name="slide">
        <div class="audio-element" @click="audioClickHandler">
            <div style="display: flex;">
                <div :class="this.isPlaying && this.audioMetadata.Id == audio.Id ? 'pause_btn' : 'play_btn'"></div>
                <div>
                    <my-text class="title">{{ audio.Title }}</my-text>
                    <div class="performers">
                        <my-text class="performer" v-for="artist in audio.Performer" :key="artist.Id"
                            :performer-id="`${artist.Id}`">{{ artist.Name }}
                        </my-text>
                    </div>
                </div>
            </div>
            <my-text class="duration">
                {{ this.secToMin(audio.Duration) }}
            </my-text>
        </div>
    </transition>
</template>

<script>
import utils from '@/mixins/utils.js';
import { mapState } from 'vuex';

export default {
    name: 'audio-elem',
    mixins: [utils],
    props: {
        audio: {
            type: Object,
            required: true
        }
    },
    methods: {
        audioClickHandler(event) {
            if (event.target.getAttribute("performer-id")) {
                console.log("perff");
            } else {
                console.log(this.audio.Id);
                if (this.isPlaying && this.audioMetadata.Id == this.audio.Id) {
                    this.$store.dispatch('player/pauseAudio', this.audio.Id);
                }
                else {
                    this.$store.commit('player/setCurrentTime', 0);
                    this.$store.dispatch('player/playAudio', this.audio.Id);
                }
            }
        }
    },
    computed: {
        ...mapState({
            isPlaying: state => state.player.isPlaying,
            audioMetadata: state => state.player.audioMetadata
        })
    },
}
</script>

<style scoped>
.slide-fade-enter-active {
  transition: all 0.3s ease-out;
}

.slide-fade-leave-active {
  transition: all 0.8s cubic-bezier(1, 0.5, 0.8, 1);
}

.slide-fade-enter-from,
.slide-fade-leave-to {
  transform: translateX(20px);
  opacity: 0;
}

.title {
    font-family: "Trebuchet MS";
    font-weight: bold;
    margin-bottom: 5px;
}

.performers {
    display: flex;
    overflow: hidden;
    white-space: nowrap;
}

.performer {
    font-size: 0.9em;
    font-family: "Trebuchet MS";
    font-weight: normal;
}

.performer:hover {
    cursor: pointer;
    color: rgb(226, 216, 216);
}

.performer:not(:last-child):after {
  content: ', ';
  margin-right: 6px;
}

.play_btn {
    margin-right: 20px;
    width: 30px;
    height: 30px;
    background-image: url('@/icones/play_btn__white.png');
    background-size: cover;
}

.pause_btn {
    margin-right: 20px;
    width: 30px;
    height: 30px;
    background-image: url('@/icones/pause_btn__white.png');
    background-size: cover;
}

.audio-element {
    padding: 15px;
    margin-top: 15px;
    width: 100%;
    min-width: 300px;
    height: 55px;
    background-color: rgb(39, 37, 45);
    display: flex;
    justify-content: space-between;
    align-items: center;
    border-radius: 5px;
}

.audio-element:hover {
    background-color: rgb(49, 46, 56);
    cursor: pointer;
}
</style>