<template>
  <div class="home_page">
    <play-list :audiosMetadata="audiosMetadata"></play-list>
    <div ref="observer" class="observer"></div>
  </div>
  
</template>

<script>
import PlayList from '@/components/PlayList';
import { mapState, mapActions, mapMutations } from 'vuex'

export default {
  components: {
    PlayList
  },
  mounted() {
    this.fetchAudios();
    const options = {
      rootMagrgin: '0px',
      threshold: 1.0
    }
    const callback = (entries) => {
      if(entries[0].isIntersecting){
        this.fetchAudios();
      }
    }
    const observer = new IntersectionObserver(callback, options);
    observer.observe(this.$refs.observer);
  },
  methods: {
    ...mapMutations({
      setAudiosMetadata: 'homeview/setAudiosMetadata'
    }),
    ...mapActions({
      fetchAudios: 'homeview/fetchAudios'
    })
  },
  computed: {
    ...mapState({
      audiosMetadata: state => state.homeview.audiosMetadata
    })
  },
}
</script>

<style>
.observer{
  height: 10px;
}
</style>
