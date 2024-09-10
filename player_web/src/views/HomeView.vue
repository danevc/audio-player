<template>
  <div class="home_page">
    <play-list :audiosMetadata="audiosMetadata"></play-list>
  </div>
  <div ref="observer" class="observer"></div>
</template>

<script>
import PlayList from '@/components/PlayList';
import { mapState, mapActions } from 'vuex'

export default {
  components: {
    PlayList
  },
  computed: {
    ...mapState({
      audiosMetadata: state => state.playlist.audiosMetadata
    })
  },
  methods: {
    ...mapActions({
      fetchAudios: 'playlist/fetchAudios'
    })
  },
  mounted() {
    this.$store.commit('setIsLoading', true);
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
    setTimeout(() => this.$store.commit('setIsLoading', false), 1200);
  }
}
</script>

<style>
.observer {
  display: flex;
  justify-content: center;
}
</style>
