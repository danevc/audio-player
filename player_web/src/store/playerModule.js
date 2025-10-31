import axios from "axios";

export const playerModule = {
    state: () => ({
        isPlaying: false,
        isShuffle: false,
        isRepeat: false,
        lostPartTrack: 0,
        audioMetadata: {
            Id: -1,
            Title: '',
            Duration: 0,
            Path: ''
        },
        queuePlayback: {},
        audioHTML: new Audio(),
        currentTime: 0,
        volume: 15,
        coverSrc: 'https://i1.sndcdn.com/avatars-000209783484-m3o97r-t240x240.jpg'
    }),
    getters: {
        getIsShuffle(state) {
            return state.isShuffle;
        },
        getCurAudioId(state) {
            return state.audioMetadata.Id;
        }
    },
    mutations: {
        setIsPlaying(state, bool) {
            state.isPlaying = bool;
        },
        setQueuePlayback(state, audios) {
            state.queuePlayback.audios = audios;
        },
        setIsShuffle(state, bool) {
            state.isShuffle = bool;
        },
        setIsRepeat(state, bool) {
            state.isRepeat = bool;
        },
        setLostPartTrack(state, time) {
            state.lostPartTrack = time;
        },
        setAudioSrc(state, src) {
            state.audio.src = src;
        },
        setVolume(state, volume) {
            state.volume = volume;
        },
        setAudioMetadata(state, audio) {
            state.audioMetadata = audio;
        },
        setUrl(state, url) {
            state.url = url;
        },
        setCurrentTime(state, time) {
            state.currentTime = time;
        },
        setCoverSrc(state, src) {
            state.coverSrc = src;
        }
    },
    actions: {
        initPlayer({ commit, state, dispatch }) {
            state.audioHTML.volume = state.volume / 100;
            state.audioHTML.addEventListener("timeupdate", ({ target }) => {
                commit('setCurrentTime', target.currentTime);
                commit('setLostPartTrack', target.currentTime / state.audioHTML.duration * 100);
                if (state.audioHTML.ended) {
                    dispatch('playNext');
                }
            });
            state.audioHTML.preload = true;
        },
        async getAudio({ commit }, id) {
            const response = await axios.get('Audio/GetAudioById', {
                params: {
                    id: id
                }
            });
            commit('setAudioMetadata', response.data);
        },
        playAudio({ state, commit, rootState, dispatch }, id) {
            try {
                dispatch('getAudio', id);
                state.audioHTML.src = rootState.baseURL + 'Audio/GetAudioFile?id=' + id;
                state.audioHTML.currentTime = state.currentTime;
                var playPromise = state.audioHTML.play();
                if (playPromise !== undefined) {
                    playPromise.then(() => {
                        commit('setIsPlaying', true);
                    }).catch(error => {
                        console.log(error);
                    });
                }
            } catch (error) {
                console.log(error);
            }
        },
        pauseAudio({ state, commit }) {
            try {
                state.audioHTML.pause();
                commit('setIsPlaying', false);
            }
            catch (error) {
                console.log(error);
            }
        },
        playNext({ commit, state, dispatch }) {
            var queue = state.queuePlayback.audios;
            var indexNext = 0;
            if (state.isShuffle) {
                var rnd = Math.random();
                indexNext = Math.floor(rnd * queue.length);
            }
            else {
                indexNext = queue.findIndex(a => a.Id == state.audioMetadata.Id) + 1;
                if (indexNext >= queue.length)
                    indexNext = 0
            }
            commit('setCurrentTime', 0);
            commit('setLostPartTrack', 0);
            var idNext = queue[indexNext].Id;
            dispatch('playAudio', idNext);
        },
        playPrevious({ commit, state, dispatch }) {
            commit('setCurrentTime', 0);
            commit('setLostPartTrack', 0);
            var queue = state.queuePlayback.audios;
            var indexPrevious = queue.findIndex(a => a.Id == state.audioMetadata.Id) - 1;
            if (indexPrevious < 0) {
                indexPrevious = 0;
            }

            var idPrevious = queue[indexPrevious].Id;
            dispatch('playAudio', idPrevious);
        }
    },
    namespaced: true
}