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
            Description: ''
        },
    }),
    getters: {
        getSrc: (state, getters, rootState) => {
            return `${rootState.baseURL}Playlist/GetPlaylistCover?id=${state.currentPlaylist.Id}`
        }
    },
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
                    const response = await axios.get('Playlist/GetPlaylists');
                    commit('setPlaylistsMetadata', response.data.Playlists);
            } catch (error) {
                console.log(error);
            }
        },
        async fetchAudiosByPlaylists({ commit, state }, playlistId) {
            try {
                if(state.currentPlaylist.Id != playlistId){
                    state.audiosMetadata = [];
                    state.page = 0;
                    state.totalCount = 0;
                }
                if(state.limit * state.page <= state.totalCount){
                    const response = await axios.get('Playlist/GetAudiosByPlaylist', {
                        params: {
                            page: state.page,
                            limit: state.limit,
                            id: playlistId
                        }
                    });
                    commit('setTotalCount', response.data.QuantityAudios);
                    commit('setAudiosMetadata', [...state.audiosMetadata, ...response.data.Audios]);
                    commit('setPage', state.page + 1);
                }
            } catch (error) {
                console.log(error);
            }
        },
        async getPlaylist({ commit }, playlistId) {
            try {
                    const response = await axios.get('Playlist/GetPlaylist', {
                        params: {
                            id: playlistId
                        }
                    });
                    commit('setTotalCount', response.data.Count);
                    commit('setCurrentPlaylist', response.data);                      
                } catch (error) {
                console.log(error);
            }
        },
        async createPlaylist({ commit }, playlist) {
            try {
                console.log('tuta');
                console.log(`${playlist.title} ${playlist.description}`);
                    const response = await axios.post('Playlist/CreatePlaylist', {title: playlist.title, description: playlist.description, cover: playlist.cover}, {
                        headers: {
                            'Content-Type': 'application/json'
                        }
                    });
                    commit('setTotalCount', response.data.Count);
                    commit('setCurrentPlaylist', response.data);                      
                } catch (error) {
                console.log(error);
            }
        },
        async setTitle({ state }, title) {
            try {;
                    await axios.post('Playlist/SetTitle', {id: state.currentPlaylist.Id, value: title}, {
                        headers: {
                            'Content-Type': 'application/json'
                        }
                    }); 
                } catch (error) {
                console.log(error);
            }
        },
        async setDescription({ state }, description) {
            try {
                    await axios.post('Playlist/SetDescription', {id: state.currentPlaylist.Id, value: description}, {
                        headers: {
                            'Content-Type': 'application/json'
                        }
                    });  
                } catch (error) {
                console.log(error);
            }
        }
    },
    namespaced: true
}