import { createStore } from 'vuex'

const store = createStore({
  state() {
    return {
      user: null,
      videoUrl: '',
      videoFileName: '',
      currentRoute: null,
      isLoading: false,
      persons: []
    }
  },
  mutations: {
    setUser(state, user) {
      state.user = user
    },
    setVideoUrl(state, url) {
      state.videoUrl = url;
    },
    setVideoFileName(state, fileName) {
      state.videoFileName = fileName
    },
    setCurrentRoute(state, route) {
      state.currentRoute = route
    },
    setPersons(state, persons){
      state.persons = persons
    },
    setIsLoading(state, loading) {
      state.isLoading = loading
    },
  }
})

export default store