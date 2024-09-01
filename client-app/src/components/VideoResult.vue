<template>
  <IndexLayout>
    <div class="video-result">
      <div class="video-frame">
        <video v-if="videoUrl" :src="videoUrl" width="640" height="480" controls></video>
        <p v-else>Loading video...</p>
      </div>
      <div class="button-container">
        <div class="attendance-button">
          <button @click="processVideo" :disabled="isLoading">
            <span v-if="isLoading" class="loading-icon">&#9696;</span>
            <span v-else>Process Cropped Faces</span>
          </button>
        </div>
      </div>
    </div>
  </IndexLayout>
</template>

<script>
import axios from 'axios'
import { useStore } from 'vuex'
import { useRouter } from 'vue-router'
import { computed } from 'vue'
import IndexLayout from './IndexLayout.vue'

export default {
  name: 'VideoResult',
  components: {
    IndexLayout
  },
  setup() {
    const store = useStore()
    const router = useRouter()

    const videoUrl = computed(() => store.state.videoUrl)
    const isLoading = computed(() => store.state.isLoading)
    const videoFileName = computed(() => store.state.videoFileName)
    
    const navigateToDetail = () => {
      router.push('/detail')
    }

    const getProcessedVideo = async () => {
      try {
        const response = await axios.get(`http://localhost:5016/api/home/output/processed_${videoFileName.value}`, { responseType: 'blob' })
        store.commit('setVideoUrl', URL.createObjectURL(response.data))
      } catch (error) {
        console.error('Error getting processed video:', error)
      }
    }

    const processVideo = async () => {
      store.commit('setIsLoading', true)
      try {
        await axios.post(`http://localhost:5016/api/home/process-cropped-faces/processed_${videoFileName.value}`)
        console.log('Cropped faces processed successfully')
        router.push('/final-result')
      } catch (error) {
        console.error('Error processing cropped faces:', error)
      } finally {
        store.commit('setIsLoading', false)
      }
    }

    getProcessedVideo()

    return {
      navigateToDetail,
      videoUrl,
      isLoading,
      processVideo
    }
  }
}
</script>

<style>
.video-frame {
  width: 640px;
  height: 480px;
  background-color: #999;
}

.button-container {
  display: flex;
  justify-content: center;
  margin-top: 20px;
}

.attendance-button {
  background-color: #1fd61b;
  display: inline-flex;
  align-items: center;
  justify-content: center;
  padding: 10px;
}

.attendance-button button {
  background-color: transparent;
  border: none;
  color: inherit;
  padding: 0;
}
.loading-icon {
  animation: spin 1s infinite linear;
}

@keyframes spin {
  from {
    transform: rotate(0deg);
  }
  to {
    transform: rotate(360deg);
  }
}
</style>