<template>
  <div class="player-container">
    <div class="cover-container">
      <img class="cover" :src="coverSrc">
    </div>
    <div class="player-controls-container">
      <div class="timeline" @click="changeTimeLine" @mousedown="mousedownHandler">
        <div class="timelineNow" :style="{ width: lostPartTrack + '%' }"></div>
      </div>
      <div class="duration_info">
        <my-text class="cur_duration_info">{{ secToMin(Math.floor(this.currentTime)) }}</my-text>
        <my-text class="full_duration_info">{{ secToMin(audioMetadata.Duration) }}</my-text>
      </div>
      <div style="display: flex; justify-content: space-between; height: 70px">
        <div class="track_info" v-if="audioMetadata.Id != -1">
          <my-text class="track_title_info">{{ audioMetadata.Title }}</my-text>
          <div class="performers">
            <my-text class="performer" v-for="artist in audioMetadata.Performer" :key="artist.Id"
              :performer-id="`${artist.Id}`" @click="performerClickHandler">{{ artist.Name }}</my-text>
          </div>
        </div>
        <div class="track_info" v-else></div>
        <div class="buttons-audio-control-container">
          <my-button class="previous_track_button" @click="previousHandler"></my-button>
          <my-button :class="isPlaying ? 'pause_btn' : 'play_btn icon'" @click="playOrPauseHandler"></my-button>
          <my-button class="next_track_button" @click="nextHandler"></my-button>
        </div>
        <div class="control-buttons-container">
          <my-slider v-model="this.volumeChange">
          </my-slider>
          <my-button :class="isShuffle ? 'shuffle-button-enable icon' : 'shuffle-button-disable icon'"
            @click="shuffleHandler"></my-button>
          <my-button :class="isRepeat ? 'repeat-button-enable icon' : 'repeat-button-disable icon'"
            @click="repeatHandler"></my-button>
          <my-button class="options-player-button icon" @click="optionsHandler"></my-button>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import utils from '@/mixins/utils.js';
import { mapActions, mapMutations, mapState } from 'vuex';

export default {
  mixins: [utils],
  methods: {
    ...mapMutations({
      setIsPlaying: 'player/setIsPlaying',
      setLostPartTrack: 'player/setLostPartTrack',
      setAudioSrc: 'player/setAudioSrc',
      setAudioVolume: 'player/setAudioVolume',
      setAudioMetadata: 'player/setAudioMetadata',
      setCurrentTime: 'player/setCurrentTime',
      setVolume: 'player/setVolume',
      setIsShuffle: 'player/setIsShuffle',
      setIsRepeat: 'player/setIsRepeat',
      setCoverSrc: 'player/setCoverSrc'
    }),
    ...mapActions({
      initPLayer: 'player/initPlayer',
      playAudio: 'player/playAudio',
      pauseAudio: 'player/pauseAudio',
      playNext: 'player/playNext',
      playPrevious: 'player/playPrevious',
    }),
    mouseupHandler() {
      document.removeEventListener('mousemove', this.changeTimeLine);
      document.removeEventListener('mouseup', this.mouseupHandler);
    },
    optionsHandler() {
      console.log('options');
    },
    mousedownHandler() {
      document.addEventListener('mousemove', this.changeTimeLine);
      document.addEventListener('mouseup', this.mouseupHandler);
    },
    previousHandler() {
      this.playPrevious();
    },
    nextHandler() {
      this.playNext();
    },
    shuffleHandler() {
      this.setIsShuffle(!this.isShuffle);
    },
    repeatHandler() {
      this.setIsRepeat(!this.isRepeat);
    },
    playOrPauseHandler() {
      if (!this.isPlaying) {
        this.playAudio(this.audioMetadata.Id);
      }
      else if (this.audioHTML.played) {
        this.pauseAudio();
      }
    },
    changeTimeLine(event) {
      var precent = ((event.pageX - 220) / document.querySelector('.timeline').offsetWidth * 100).toFixed(1);

      if (precent > 100)
        precent = 100
      else if (precent < 0)
        precent = 0

      this.setLostPartTrack(precent);
      this.audioHTML.currentTime = this.audioMetadata.Duration * precent / 100;
    },
    performerClickHandler(event) {
      if (event.target.getAttribute("performer-id")) {
        console.log("perff");
      }
    }
  },
  computed: {
    ...mapState({
      isPlaying: state => state.player.isPlaying,
      lostPartTrack: state => state.player.lostPartTrack,
      audioMetadata: state => state.player.audioMetadata,
      audioHTML: state => state.player.audioHTML,
      url: state => state.player.url,
      currentTime: state => state.player.currentTime,
      volume: state => state.player.volume,
      isShuffle: state => state.player.isShuffle,
      isRepeat: state => state.player.isRepeat,
      coverSrc: state => state.player.coverSrc,
    }),
    volumeChange: {
      get() {
        return this.volume;
      },
      set(vol) {
        this.setVolume(parseInt(vol));
      }
    }
  },
  watch: {
    volume(value) {
      this.audioHTML.volume = value / 100;
    },
    isRepeat(value) {
      this.audioHTML.loop = value;
    }
  },
  mounted() {
    this.initPLayer();
  },
}
</script>

<style scoped>
.player-controls-container {
  width: 100%;
  min-width: 700px;
}

.cover {
  object-fit: contain;
  border-radius: 10px;
  width: 100%;
  height: 100%;
}

.cover-container {
  height: 100px;
  width: 100px;
  min-width: 100px;
  border-radius: 15px;
  margin-right: 20px;
}

.icon {
  width: 20px;
  height: 20px;
  background-size: cover;
  margin-left: 15px;
  margin-right: 15px;
}

.options-player-button {
  background-image: url('@/icones/options_player_btn__white.png');
  margin-right: 0px;
}

.repeat-button-enable {
  background-image: url('@/icones/repeat_btn_enable__white.png');
}

.repeat-button-disable {
  background-image: url('@/icones/repeat_btn_disable__white.png');
}

.shuffle-button-enable {
  background-image: url('@/icones/shuffle_btn_enable__white.png');
  margin-left: 25px;
}

.shuffle-button-disable {
  background-image: url('@/icones/shuffle_btn_disable__white.png');
  margin-left: 25px;
}

.track_title_info {
  font-size: 1.3em;
  font-weight: bold;
}

.previous_track_button {
  width: 25px;
  height: 25px;
  background-image: url('@/icones/left_arrow_btn__white.png');
  background-size: cover;
}

.next_track_button {
  width: 25px;
  height: 25px;
  background-image: url('@/icones/right_arrow_btn__white.png');
  background-size: cover;
}

.play_btn {
  width: 40px;
  height: 40px;
  background-image: url('@/icones/play_btn__white.png');
}

.pause_btn {
  width: 40px;
  height: 40px;
  background-image: url('@/icones/pause_btn__white.png');
  background-size: cover;
  margin-left: 15px;
  margin-right: 15px;
}

.buttons-audio-control-container {
  width: 150px;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.duration_info {
  margin-top: 5px;
  display: flex;
  justify-content: space-between;
}

.track_info {
  width: 250px;
  height: 80%;
  text-align: left;
  justify-self: left;
  display: inline-block;
}

.track_title_info {
  margin: 5px;
  width: 100%;
  height: 40%;
  overflow: hidden;
}

.performer:hover {
  cursor: pointer;
  color: rgb(226, 216, 216);
}

.performer {
  display: inline-block;
}

.performer:not(:last-child):after {
  content: ', ';
  margin-right: 6px;
}

.performers {
  margin: 5px;
  margin-top:7px;
  display: flex;
  overflow:hidden;
  white-space: nowrap;
}

.player-container {
  display: flex;
  width: 100%;
  height: 120px;
  border-radius: 7px;
  background-color: rgb(50, 48, 57);
  margin-left: auto;
  margin-right: auto;
  padding-left: 100px;
  padding-right: 100px;
}

.timelineNow {
  height: 10px;
  background-color: rgb(245, 241, 255);
  border-radius: 7px;
}

.timeline {
  width: 100%;
  height: 10px;
  border-radius: 7px;
  background-color: rgb(103, 99, 112);
}

.timeline:hover {
  cursor: pointer;
}

.control-buttons-container {
  display: flex;
  justify-content: right;
  align-items: center;
  width: 250px;
  margin-top: 10px;
}
</style>
