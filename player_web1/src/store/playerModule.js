// var Discogs = require('disconnect').Client;

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
        playAudio({ state, commit, dispatch, rootState }, id) {
            try {
                // var db = new Discogs().database();
                // db.getRelease(176126, function (err, data) {
                //     console.log(data);
                // });
                dispatch('setMetadataAudio', id, { root: true });
                state.audioHTML.src = rootState.baseURL + 'GetAudioFile?id=' + id;
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
        playNext({ commit, state, rootState, dispatch }) {
            var queue = rootState.homeview.audiosMetadata;
            var indexNext = 0;
            if (state.isShuffle) {
                indexNext = Math.floor(Math.random() * queue.length);
            }
            else {
                indexNext = queue.findIndex(a => a.Id == state.audioMetadata.Id) + 1;
            }
            commit('setCurrentTime', 0);
            commit('setLostPartTrack', 0);
            var idNext = queue[indexNext].Id;
            dispatch('playAudio', idNext);
        },
        playPrevious({ commit, state, rootState, dispatch }) {
            commit('setCurrentTime', 0);
            commit('setLostPartTrack', 0);
            var queue = rootState.homeview.audiosMetadata;
            var indexPrevious = queue.findIndex(a => a.Id == state.audioMetadata.Id) - 1;
            var idPrevious = queue[indexPrevious].Id;
            dispatch('playAudio', idPrevious);
        }
    },
    namespaced: true
}