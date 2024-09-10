import axios from "axios";

export const playlistModule = {
    state: () => ({
        audiosMetadata: [],
        audiosIds: [],
        page: 0,
        limit: 15,
        totalCount: 0
    }),
    getters: {
        getAudiosMetadata(state) {
            return state.audiosMetadata;
        }
    },
    mutations: {
        setAudiosMetadata(state, audiosMetadata) {
            state.audiosMetadata = audiosMetadata;
        },
        setTotalCount(state, num) {
            state.totalCount = num;
        },
        setPage(state, page) {
            state.page = page;
        }
    },
    actions: {
        async fetchAudios({ state, commit, rootState }) {
            try {
                if(state.limit * state.page <= state.totalCount){
                    var url = rootState.baseURL + 'GetAudiosPart';
                    const response = await axios.get(url, {
                        params: {
                            page: state.page,
                            limit: state.limit
                        }
                    });
                    commit('setTotalCount', response.data.Count);
                    commit('setAudiosMetadata', [...state.audiosMetadata, ...response.data.Audios]);
                    if(state.page == 0){
                        commit('player/setAudioMetadata', response.data.Audios[0], {root:true});
                    }
                    commit('setPage', state.page + 1);
                }
            } catch (error) {
                console.log(error);
            }
        }
    },
    namespaced: true
}