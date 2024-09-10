import { createStore } from 'vuex'
import { playerModule } from './playerModule'
import { playlistModule } from './playlistModule'
import { searchModule } from './searchModule'
import axios from "axios";


export default createStore({
  state: () => ({
    baseURL: 'https://localhost:44325/',
    isLoading: true,
    searchHint: []
  }),
  mutations: {
    setIsLoading(state, bool) {
      state.isLoading = bool;
    },
    setSearchResult(state, res){
      state.searchHint = [];
      res.Audios.forEach(el => {
        state.searchHint.push({
          type: 'audiotype',
          value: el
        })
      });
      res.Performers.forEach(el => {
        state.searchHint.push({
          type: 'performertype',
          value: el
        })
      });
      state.searchHint.sort((a, b) => a.value > b.value ? 1 : -1);
    }
  },
  actions: {
    async SearchHint(context, query) {
      try {
        var url = context.state.baseURL + 'SearchHint';
        const response = await axios.get(url, {
          params: {
            query: query
          }
        });
        context.commit('setSearchResult', response.data);
      } catch (error) {
        console.log(error);
      }
    }
  },
  modules: {
    playlist: playlistModule,
    player: playerModule,
    search: searchModule
  }
})
