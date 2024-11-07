import { createStore } from 'vuex'
import { playerModule } from './playerModule'
import { audiosModule } from './audiosModule'
import { playlistModule } from './playlistModule'
import { searchModule } from './searchModule'
import axios from "axios";


export default createStore({
  state: () => ({
    baseURL: 'https://localhost:44325/',
    isLoading: false
  }),
  getters: {
    distinct: () => (a) => {
      var prims = { "boolean": {}, "number": {}, "string": {} }, objs = [];

      return a.filter(function (item) {
        var type = typeof item;
        if (type in prims)
          return prims[type].hasOwnProperty(item) ? false : (prims[type][item] = true);
        else
          return objs.indexOf(item) >= 0 ? false : objs.push(item);
      });
    }
  },
  mutations: {
    setIsLoading(state, bool) {
      state.isLoading = bool;
    }
  },
  actions: {
    async uploadFiles({  }, files) {
      try {
        var formData = new FormData();
        for (let i = 0; i < files.length; i++) {
          formData.append('files', files[i]);
        }
        const response = await axios.post('Audio/AddAudios', formData,
          {
            headers: {
              'Content-Type': 'multipart/form-data'
            }
          }
        );
        console.log(response);
      }
      catch (error) {
        console.log(error);
      }
    },
    async uploadFile({  }, file) {
      try {
        var formData = new FormData();
        formData.append('files', file);
        const response = await axios.post('Audio/AddAudios', formData,
          {
            headers: {
              'Content-Type': 'multipart/form-data'
            }
          }
        )
        console.log('in action: ' + response.status);
        return response.status;
      }
      catch (error) {
        console.log('err');
        return error;
      }
    }
  },
  modules: {
    audios: audiosModule,
    player: playerModule,
    search: searchModule,
    playlist: playlistModule
  }
})
