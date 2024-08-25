import { createStore } from 'vuex'
import { playerModule } from './playerModule'
import { homeviewModule } from './homeviewModule'
import axios from "axios";


export default createStore({
  state: () => ({
    baseURL: 'https://localhost:44325/',
    isLoading: false
  }),
  mutations: {
    setIsLoading(state, isLoading) {
      state.isLoading = isLoading;
    }
  },
  actions: {
    async setMetadataAudio(context, id) {
      try {
        var url = context.state.baseURL + 'GetAudio';
        const response = await axios.get(url, {
          params: {
            id: id
          }
        });
        context.commit('player/setAudioMetadata', response.data.Audio, { root: true });
      } catch (error) {
        console.log(error);
      }
    }
  },
  modules: {
    homeview: homeviewModule,
    player: playerModule
  }
})
