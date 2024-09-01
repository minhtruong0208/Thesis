import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import store from './store'

const app = createApp(App)
// eslint-disable-next-line no-undef
app.use(router)
app.use(store)
app.mount('#app')