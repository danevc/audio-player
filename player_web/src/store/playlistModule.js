import axios from "axios";

export const playlistModule = {
    state: () => ({
        playlistsMetadata: [],
        totalCount: 0,
        audiosMetadata: [],
        limit: 10,
        page: 0,
        currentPlaylist: {
            Id: -1,
            Title: '',
        },
    }),
    mutations: {
        setPlaylistsMetadata(state, playlistsMetadata) {
            state.playlistsMetadata = playlistsMetadata;
        },
        setTotalCount(state, totalCount) {
            state.totalCount = totalCount;
        },
        setCurrentPlaylist(state, playlist) {
            state.currentPlaylist = playlist;
        },
        setPage(state, page) {
            state.page = page;
        },
        setAudiosMetadata(state, audiosMetadata) {
            state.audiosMetadata = audiosMetadata;
        }

    },
    actions: {
        async fetchPlaylists({ commit }) {
            try {
                    const response = await axios.get('GetPlaylistsPart');
                    commit('setPlaylistsMetadata', response.data.Playlists);
            } catch (error) {
                console.log(error);
            }
        },
        async fetchAudiosByPlaylists({ commit, state }, playlistId) {
            try {
                console.log()
                if(state.currentPlaylist.Id != playlistId){
                    state.audiosMetadata = [];
                    state.page = 0;
                }
                if(state.limit * state.page <= state.totalCount){
                    const response = await axios.get('GetAudiosPart', {
                        params: {
                            page: state.page,
                            limit: state.limit,
                            playlistId: playlistId
                        }
                    });
                    commit('setTotalCount', response.data.Count);
                    commit('setAudiosMetadata', [...state.audiosMetadata, ...response.data.Audios]);
                    commit('setPage', state.page + 1);
                }
            } catch (error) {
                console.log(error);
            }
        },
        async getPlaylist({ commit }, playlistId) {
            try {
                    const response = await axios.get('GetPlaylist', {
                        params: {
                            id: playlistId
                        }
                    });
                    commit('setTotalCount', response.data.Count);
                    commit('setCurrentPlaylist', response.data);                      
                    console.log(response.data);
                } catch (error) {
                console.log(error);
            }
        }
    },
    namespaced: true
}