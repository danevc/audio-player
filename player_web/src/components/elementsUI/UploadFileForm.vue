<template>
    <div class="upload-audio-container">
        <my-text>Выберите файлы формата mp3</my-text>
        <div v-if="files.length == 0">
            <my-button @click="$refs.uploadaudio.click()" class="add-audio-btn">
                <my-text>Добавить аудио</my-text>
            </my-button>
        </div>
        <div v-else>
            <my-button @click="sendFiles" class="send-audio-btn">
                <my-text>Отправить</my-text>
            </my-button>
        </div>
        <input ref="uploadaudio" type="file" id="upload-audio" v-show="false" @change="previewFiles" multiple />
    </div>
    <div div="list-audios" v-if="files.length > 0">
        <div v-for="f in files" :key=f.id class="file">
            <my-text>{{ f.name.slice(0, -4) }}</my-text>
            <div @click="deleteAudio(f)" class="delete-audio-btn"></div>
        </div>
    </div>
</template>

<script>
import { mapActions } from 'vuex';
export default {
    name: 'upload-file-form',
    data() {
        return {
            files: []
        }
    },
    methods: {
        ...mapActions([
            'uploadFiles',
            'uploadFile',
        ]),
        previewFiles(event) {
            this.files = [...event.target.files];
            for (let i = 0; i < this.files.length; i++) {
                this.files[i].id = i;
                var isValid = false;
                var parts = this.files[i].name.split('.');
                if (i < 10 && parts.length > 1) {
                    if (parts.pop() == 'mp3' && this.files[i].size < 30000000) {
                        isValid = true;
                    }
                }
                if (!isValid) {
                    this.files.splice(i, 1);
                    i--;
                }
            }
        },
        async sendFiles() {
            for (var file of this.files) {
                var res = await this.uploadFile(file);
                if (res = 200) {
                    this.files = this.files.filter(a => a.id !== file.id);
                }
            };
        },
        deleteAudio(f) {
            this.files = this.files.filter(a => a.id !== f.id);
        }
    }
}
</script>

<style>
.upload-audio-container {
    text-align: center;
}

.delete-audio-btn {
    width: 20px;
    height: 20px;
    background-size: cover;
    background-image: url('@/icones/delete_btn.png');
    cursor: pointer;
}

.list-audios {
    text-align: center;
}

.send-audio-btn {
    margin: 12px;
    font-size: 0.98em;
    width: 130px;
    height: 30px;
    background-color: rgb(142, 128, 196);
    border-radius: 5px;
}

.add-audio-btn {
    margin: 12px;
    font-size: 0.98em;
    width: 130px;
    height: 30px;
    background-color: rgb(142, 128, 196);
    border-radius: 5px;
}

.file {
    width: 100%;
    margin-top: 10px;
    padding: 8px;
    border-radius: 8px;
    background-color: rgb(130, 184, 135);
    display: flex;
    justify-content: space-between;
}
</style>