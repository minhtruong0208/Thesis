import { createRouter, createWebHistory } from 'vue-router'
import UploadVideo from './components/UploadVideo.vue'
import VideoResult from './components/VideoResult.vue'
import FinalResult from './components/FinalResult.vue'
import CustomLoginForm from './components/CustomLoginForm.vue'
import ForgotPassword from './components/ForgotPassword.vue'
import Manage from './components/Manage.vue'
import Detail from './components/Detail.vue'
import store from './store'

const routes = [
  { path: '/', component: UploadVideo, meta: { requiresAuth: true } },
  { path: '/video-result', component: VideoResult, meta: { requiresAuth: true } },
  { path: '/final-result', component: FinalResult, meta: { requiresAuth: true } },
  { path: '/login', component: CustomLoginForm },
  { path: '/forgot-password', component: ForgotPassword},
  { path: '/detail', component: Detail, meta: { requiresAuth: true } },
  { path: '/manage', component: Manage, meta: {requiresAuth: true}}
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

router.beforeEach((to, from, next) => {
  if (to.matched.some(record => record.meta.requiresAuth)) {
    if (store.state.user) {
      next()
    } else {
      next('/login')
    }
  } else {
    next()
  }
})

export default router