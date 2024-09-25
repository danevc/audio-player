import axios from "axios";

export const searchModule = {
  state: () => ({
    audiosSearchResult: [],
    query: '',
    page: 0,
    limit: 12,
    totalCount: 0,
    searchHint: []
  }),
  getters: {
    getAudiosMetadata(state) {
      return state.audiosSearchResult;
    }
  },
  mutations: {
    setSearchResult(state, res) {
      state.audiosSearchResult = res;
    },
    setPage(state, page) {
      state.page = page;
    },
    setQuery(state, query) {
      state.query = query;
    },
    setTotalCount(state, num) {
      state.totalCount = num;
    },
    setSearchHint(state, res) {
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
        const response = await axios.get('SearchHint', {
          params: {
            query: query
          }
        });
        context.commit('setSearchHint', response.data);
      } catch (error) {
        console.log(error);
      }
    },
    async fetchAudiosSearchResult({ state, commit }, query) {
      try {
        if (state.query != query) {
          commit('setSearchResult', []);
          commit('setPage', 0);
        }

        if (state.limit * state.page <= state.totalCount) {
          commit('setQuery', query);
          const response = await axios.get('GetAudiosPart', {
            params: {
              page: state.page,
              limit: state.limit,
              query: query
            }
          });
          commit('setPage', state.page + 1);
          commit('setTotalCount', response.data.Count);
          commit('setSearchResult', [...state.audiosSearchResult, ...response.data.Audios]);
        }

      } catch (error) {
        console.log(error);
      }
    }
  },
  namespaced: true
}