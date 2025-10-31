<template>
  <div class="home_page">
    <playlists-section :playlists="playlistsMetadata" :title="'Мои плейлисты'" class="playlists-section"></playlists-section>
    <audio-list :audiosMetadata="audiosMetadata" class="audio-list"></audio-list>
  </div>
  <div ref="observer" class="observer"></div>
</template>

<script>
import AudioList from '@/components/AudioList';
import PlaylistsSection from '@/components/PlaylistsSection';
import { mapState, mapActions } from 'vuex'

export default {
  components: {
    AudioList,
    PlaylistsSection
  },
  data() {
    return {
    }
  },
  computed: {
    ...mapState({
      audiosMetadata: state => state.audios.audiosMetadata,
      playlistsMetadata: state => state.playlist.playlistsMetadata
    })
  },
  methods: {
    ...mapActions({
      fetchAudios: 'audios/fetchAudios',
      fetchPlaylists: 'playlist/fetchPlaylists'
    })
  },
  mounted() {
    this.fetchPlaylists();
    const options = {
      rootMagrgin: '0px',
      threshold: 1.0
    }
    const callback = (entries) => {
      if (entries[0].isIntersecting) {
        this.fetchAudios();
      }
    }
    const observer = new IntersectionObserver(callback, options);
    observer.observe(this.$refs.observer);
  }
}
</script>

<style>
.audio-list{
    width: 60%;
    height: 100%;
    margin-left: auto;
    margin-right: auto;
}
.playlists-section{
    width: 60%;
    margin-left: auto;
    margin-right: auto;
}
</style>
