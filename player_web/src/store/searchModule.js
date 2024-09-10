import axios from "axios";

export const searchModule = {
    state: () => ({
        audiosSearchResult: [],
        query: '',
        page: 0,
        limit: 12,
        totalCount: 0
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
    },
    actions: {
        async loadAudiosSearchResult({ state, commit, rootState }, query) {
            try {
                if (state.query != query) {
                    commit('setSearchResult', []);
                    commit('setPage', 0);
                }
                if (state.limit * state.page <= state.totalCount) {
                    commit('setQuery', query);
                    var url = rootState.baseURL + 'GetAudiosPart';
                    const response = await axios.get(url, {
                        params: {
                            page: state.page,
                            limit: state.limit,
                            query: query
                        }
                    });
                    console.log(response.data);
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