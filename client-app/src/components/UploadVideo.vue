<template>
  <IndexLayout>
    <div v-if="isAuthenticated" class="index-video">
      <div class="video-frame">
        <video v-if="videoUrl" :src="videoUrl" controls></video>
        <p v-else>Chưa có video nào được tải lên</p>
      </div>
      <div class="upload-container">
        <input type="file" accept="video/*" @change="handleFileUpload" />
        <div class="attendance-button">
          <button @click="submitVideo" :disabled="isLoading || !videoFile">
            <span v-if="isLoading" class="loading-icon">&#9696;</span>
            <span v-else>Upload Video</span>
          </button>
        </div>
      </div>
    </div>
    <div v-else>
      Vui lòng đăng nhập để sử dụng tính năng này.
      <button @click="redirectToLogin">Đăng nhập</button>
    </div>
  </IndexLayout>
</template>

<script>
import { ref, computed } from 'vue'
import { useStore } from 'vuex'
import { useRouter } from 'vue-router'
import axios from 'axios'
import IndexLayout from './IndexLayout.vue'

export default {
  name: 'UploadVideo',
  components: {
    IndexLayout
  },
  setup() {
    const store = useStore()
    const router = useRouter()

    const isAuthenticated = computed(() => !!store.state.user)
    const videoUrl = computed(() => store.state.videoUrl || '')
    const videoFile = ref(null) 
    const isLoading = ref(false)

    const navigateToDetail = () => {
      router.push('/detail')
    }

    const handleFileUpload = (event) => {
      videoFile.value = event.target.files[0]
    }

    const submitVideo = async () => {
      if (!videoFile.value) return
      isLoading.value = true
      const formData = new FormData()
      formData.append('videoFile', videoFile.value)

      try {
        await axios.post('http://localhost:5016/api/home/upload', formData)
        console.log('Video uploaded successfully')
        store.commit('setVideoFileName', videoFile.value.name)
        store.commit('setVideoUrl', 'http://example.com/video.mp4')
        router.push('/video-result')
      } catch (error) {
        console.error('Error uploading video:', error)
      } finally {
        isLoading.value = false
      }
    }

    const redirectToLogin = () => {
      router.push('/login')
    }

    return {
      navigateToDetail,
      isAuthenticated,
      videoUrl,
      videoFile,
      isLoading,
      handleFileUpload,
      submitVideo,
      redirectToLogin
    }
  }
}
</script>

<style scoped>
.video-frame {
  width: 640px;
  height: 480px;
  background-color: #999;
}

.upload-container {
  display: flex;
  align-items: center;
  margin-top: 20px;
}

.attendance-button {
  background-color: #1fd61b;
  display: inline-flex;
  align-items: center;
  justify-content: center;
  padding: 10px;
  margin-left: 10px;
  transition: background-color 0.3s ease;
}

.attendance-button.button-clicked {
  background-color: #15a641;
}

.attendance-button button {
  background-color: transparent;
  border: none;
  color: inherit;
  padding: 0;
}
.loading-icon {
  animation: spin 1s infinite linear;
  display: inline-block;
}

@keyframes spin {
  0% {
    transform: rotate(0deg);
  }
  100% {
    transform: rotate(360deg);
  }
}
</style>
