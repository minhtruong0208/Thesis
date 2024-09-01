<template>
  <IndexLayout>
    <div class="final-result">
      <div class="content-container">
        <div class="video-container">
          <div class="video-frame">
            <video v-if="videoUrl" :src="videoUrl" width="640" height="480" controls></video>
            <p v-else>Loading video...</p>
          </div>
          <div v-if="emailsSent" class="email-notification">
            Email thông báo đã được gửi cho các sinh viên vắng mặt có địa chỉ email.
          </div>
          <div v-if="unknownCount > 0" class="unknown-notification">
            Phát hiện {{ unknownCount }} người lạ trong lớp!
          </div>
          <div v-if="unknownPersons.length > 0" class="unknown-persons-container">
            <h3>Người lạ:</h3>
            <div class="unknown-persons-grid">
              <div v-for="person in unknownPersons" :key="person.name" class="unknown-person-item">
                <div class="name-badge name-unknown">
                  {{ person.name }}
                </div>
                <img v-if="person.imagePath" 
                    :src="getImageUrl(person.imagePath)" 
                    :alt="person.name" 
                    class="unknown-face-image" />
              </div>
            </div>
          </div>
        </div>
        <div class="attendance-container">
          <div v-if="knownPersons.length > 0" class="persons-grid">
            <div v-for="person in knownPersons" :key="person.name" class="person-item">
              <div :class="['name-badge', getAttendanceClass(person)]">
                {{ person.name }}
              </div>
              <img v-if="person.imageUrl" 
                   :src="person.imageUrl" 
                   :alt="person.name" 
                   class="person-image" 
                   @error="handleImageError"/>
            </div>
          </div>
          <p v-else>Loading attendance data...</p>
        </div>
      </div>
    </div>
  </IndexLayout>
</template>

<script>
import axios from 'axios'
import { watch } from 'vue'
import { useStore } from 'vuex'
import { ref, computed, onMounted } from 'vue'
import { useRouter } from 'vue-router' 
import IndexLayout from './IndexLayout.vue'

export default {
  name: 'FinalResult',
  components: {
    IndexLayout
  },
  setup() {
    const store = useStore()
    const router = useRouter()
    const emailsSent = ref(false)
    const persons = ref([])
    const unknownCount = ref(0)

    const videoUrl = computed(() => store.state.videoUrl)
    const videoFileName = computed(() => store.state.videoFileName)

    const knownPersons = computed(() => persons.value.filter(p => p.attendanceStatus !== 'unknown'))
    const unknownPersons = computed(() => persons.value.filter(p => p.attendanceStatus === 'unknown'))

    const navigateToDetail = () => {
      router.push('/detail')
    }

    const handleImageError = (event) => {
      event.target.src = 'path/to/default/image.jpg'
    }

    watch(persons, (newPersons) => {
      console.log('Persons changed:', newPersons)
    }, { deep: true })

    const getVideoResult = async () => {
      try {
        const response = await axios.get(`http://localhost:5016/api/home/output/processed_${videoFileName.value}`, { responseType: 'blob' })
        store.commit('setVideoUrl', URL.createObjectURL(response.data))
      } catch (error) {
        console.error('Error getting video result:', error)
      }
    }

    const getResult = async () => {
      try {
        const response = await axios.get(`http://localhost:5016/api/home/result/processed_${store.state.videoFileName}`)
        console.log('API response data:', response.data);
        
        if (Array.isArray(response.data.persons)) {
          persons.value = response.data.persons.map(person => ({
            ...person,
            imageUrl: person.attendanceStatus === 'unknown'
              ? `http://localhost:5016${person.imagePath}`
              : `http://localhost:5016/api/home/student-image/${person.name}`
          }))
        } else {
          console.error('Received invalid data for persons:', response.data)
          persons.value = []
        }
        
        emailsSent.value = response.data.emailsSent;
        unknownCount.value = response.data.unknownCount;
      } catch (error) {
        console.error('Error getting result:', error)
        persons.value = []
      }
    }

    const getAttendanceClass = (person) => {
      if (person.attendanceStatus === 'unknown') return 'name-unknown'
      if (person.appearancePercentage >= 80) return 'name-high'
      if (person.appearancePercentage >= 50) return 'name-medium'
      return 'name-low'
    }

    const getImageUrl = (imagePath) => {
      return `http://localhost:5016${imagePath}`
    }

    onMounted(async () => {
      await getVideoResult()
      await getResult()
    })

    return {
      emailsSent,
      navigateToDetail,
      videoUrl,
      persons,
      knownPersons,
      unknownPersons,
      unknownCount,
      getAttendanceClass,
      getImageUrl,
      handleImageError
    }
  }
}
</script>

<style scoped>
.content-container {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
}

.video-container {
  flex: 0 0 640px;
  margin-right: 20px;
}

.video-frame {
  width: 100%;
  height: 480px;
  background-color: #999;
}

.attendance-container {
  flex: 1;
}

.persons-grid {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 70px; /* Adjust the gap between columns */
}

.person-item, .unknown-person-item {
  display: flex;
  flex-direction: column;
  align-items: center;
}

.name-badge {
  width: 100%;
  padding: 10px;
  border-radius: 5px;
  text-align: center;
  color: white;
  margin-bottom: 10px;
}

.name-high { background-color: #1fd61b; }
.name-medium { background-color: #d1b317; }
.name-low { background-color: #342e37; }
.name-unknown { background-color: #ff0000; }

.unknown-face-image {
  width: 100%;
  max-width: 100px;
  height: auto;
  object-fit: cover;
  margin-top: 10px;
  border-radius: 5px;
}

.unknown-notification {
  background-color: #ff0000;
  color: white;
  padding: 10px;
  margin-top: 20px;
  font-weight: bold;
  width: 640px; /* Same width as the video */
  text-align: center;
}

.email-notification {
  background-color: #4CAF50;
  color: white;
  padding: 10px;
  margin-top: 20px;
  width: 640px; /* Same width as the video */
  text-align: center;
}

.unknown-persons-container {
  margin-top: 20px;
}

.unknown-persons-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(150px, 1fr));
  gap: 15px;
}

.person-image {
  width: 100px;
  height: 100px;
  object-fit: cover;
  border-radius: 50%;
} 
</style>
