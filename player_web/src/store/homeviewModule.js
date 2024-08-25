import axios from "axios";

export const homeviewModule = {
    state: () => ({
        audiosMetadata: [],
        audiosIds: [],
        page: 0,
        limit: 12
    }),
    getter: {
        getAudiosMetadata(state) {
            return state.audiosMetadata;
        }
    },
    mutations: {
        setAudiosMetadata(state, audiosMetadata) {
            state.audiosMetadata = audiosMetadata;
        },
        setAudiosIds(state, id) {
            state.audiosIds = id;
        },
        setPage(state, page) {
            state.page = page;
        }
    },
    actions: {
        async fetchAudios({ state, commit, rootState }) {
            try {
                commit('setPage', state.page + 1);
                var url = rootState.baseURL + 'GetAudiosPart';
                const response = await axios.get(url, {
                    params: {
                        page: state.page,
                        limit: state.limit
                    }
                });
                commit('setAudiosMetadata', [...state.audiosMetadata, ...response.data.Audios]);
                commit('player/setAudioMetadata', state.audiosMetadata[0], { root: true })
                setTimeout(() => commit('setIsLoading', true, { root: true }), 1200);
            } catch (error) {
                console.log(error);
            }
        }
    },
    namespaced: true
}