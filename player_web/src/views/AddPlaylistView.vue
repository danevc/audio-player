<template>
    <div class="page">
        <div class="playlist-info-container">
            <img ref="previewcover" class="add-cover" src="#" @click="$refs.uploadcover.click()"/>
            <div class="playlist-info">
                <my-input :placeholder="'Название'" class="playlist-title" v-model="title"></my-input>
                <my-input-multiline class="playlist-description" :placeholder="'Описание'"
                    v-model="description"></my-input-multiline>
                <my-button @click="sendHandler" class="send-audio-btn">
                    <my-text>Сохранить</my-text>
                </my-button>
            </div>
            <input ref="uploadcover" type="file" id="upload-cover" v-show="false" @change="previewCover" />
        </div>
        <div class="add-tracks">
            <div class="search">
                <my-search @searchQuery="search"></my-search>
                <audio-list :audiosMetadata="audiosSearchResult" class="audio-list"></audio-list>
            </div>
        </div> 
    </div>
</template>

<script>
import { mapState, mapActions } from 'vuex';
import AudioList from '@/components/AudioList';

export default {
    components: {
        AudioList
    },
    data() {
        return {
            cover: null,
            title: '',
            description: '',
            addedTracks: []
        }
    },
    methods: {
        previewCover(cover) {
            this.$refs.previewcover.src = URL.createObjectURL(cover.target.files[0])
            this.cover = cover.target.files[0];
        },
        sendHandler() {
            
            
            var playlist = { title: this.title, description: this.description, cover: this.cover };
            console.log('send');
            console.log(playlist);
            //this.createPlaylist(playlist);
        },
        search(query){
            this.fetchAudiosSearchResult(query);
        },
        ...mapActions({
            createPlaylist: 'playlist/createPlaylist',
            fetchAudiosSearchResult: 'search/fetchAudiosSearchResult'
        })
    },
    computed: {
        ...mapState({
            audiosSearchResult: state => state.search.audiosSearchResult
        })
    },
    mounted() {
        this.$refs.previewcover.src = "";
    }
}
</script>

<style scoped>
.playlist-info-container {
    display: flex;
    justify-content: space-between;
}
.playlist-info{
    width: 100%;
}
.add-cover {
    width: 200px;
    height: 200px;
    background-color: rgb(107, 105, 114);
}
.send-audio-btn{
    width:200px;
    background-color: rgb(99, 92, 121);
    margin-right: 0;
    float: right;
}
.playlist-title:first-child {
    margin-left: 20px;
    height: 40px;
    background-color: rgb(60, 57, 71);
    color: white;
    border-color: rgb(99, 92, 121);
}

.playlist-description {
    width: 100%;
    margin: 20px;
    background-color: rgb(107, 105, 114);
    border-color: rgb(73, 70, 82);
    color: white;
    resize: none;
}

.search {
    text-align: center;
    width: 100%;
    min-width: 300px;
    margin-bottom: 10px;
}

.audio-list{
    width: 100%;
}

.page{
    width: 70%;
    margin: 30px auto 10px auto;
    box-sizing: border-box;
}
</style>