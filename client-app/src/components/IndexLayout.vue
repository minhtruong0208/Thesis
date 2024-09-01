/* eslint-disable */
<template>
  <div class="index">
    <div class="index-all">
      <div class="index-nav">
        <div class="navigation">
          <div class="avatar"></div>
          <a href="#" class="btn-home" id="btnHomeText" @click.prevent="goHome">
          <span class="btn-home-txt">
            <p class="home">Home</p>
          </span>
        </a>
        <router-link to="/detail" class="btn-detail">Detail</router-link>
          <a class="btn-logout" href="#" @click.prevent="logout">ĐĂNG XUẤT</a>
        </div>
      </div>
      <div class="index-body">
        <div class="index-body-border">
          <div class="index-content">
            <slot></slot>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { useRouter } from 'vue-router'
import { useStore } from 'vuex'

export default {
  name: 'IndexLayout',
  setup() {
    const router = useRouter()
    const store = useStore()

    const logout = () => {
      // Xóa thông tin người dùng khỏi Vuex store
      store.commit('setUser', null)

      // Xóa token (nếu có) khỏi localStorage
      localStorage.removeItem('token')

      // Chuyển hướng về trang đăng nhập
      router.push('/login')
    }

    const goHome = () => {
      router.go(-1)
    }
    
    return {
      logout,
      goHome
    }
  }
}
</script>

<style>
/* Liên kết Google Fonts */
@import url('https://fonts.googleapis.com/css2?family=Roboto:wght@400;500;700&display=swap');

body {
  margin: 0;
  line-height: normal;
  font-family: 'Roboto', sans-serif;
}
.index {
  width: 100%;
  position: relative;
  background-color: rgba(255, 255, 255, 0.2);
  min-height: 1036px;
  overflow: hidden;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: flex-start;
  padding: 40px 100px;
  box-sizing: border-box;
  text-align: center;
  font-size: 24px;
  color: #000;
  font-family: 'Roboto', sans-serif;
}
.avatar {
  width: 83px;
  position: relative;
  border-radius: 50%;
  background-color: #d9d9d9;
  height: 83px;
}
.home {
  margin: 0;
}
.btn-home-txt {
  line-break: anywhere;
  width: 100%;
}
.btn-home {
  width: 278px;
  position: relative;
  font-weight: 500;
  display: flex;
  align-items: center;
  height: 79px;
  flex-shrink: 0;
  cursor: pointer;
  font-family: 'Roboto', sans-serif;
}
.btn-detail {
  width: 278px;
  position: relative;
  font-weight: 500;
  display: flex;
  align-items: center;
  justify-content: center;
  height: 79px;
  flex-shrink: 0;
  font-family: 'Roboto', sans-serif;
}
.btn-logout {
  width: 278px;
  position: relative;
  font-weight: 500;
  color: #ff0000;
  display: flex;
  align-items: center;
  justify-content: center;
  height: 79px;
  flex-shrink: 0;
  font-family: 'Roboto', sans-serif;
}
.navigation {
  background-color: #fff;
  display: flex;
  flex-direction: row;
  align-items: center;
  justify-content: flex-start;
  gap: 108px;
  font-family: 'Roboto', sans-serif;
}
.index-nav {
  background-color: rgba(255, 255, 255, 0.2);
  display: flex;
  padding: 10px;
  flex-direction: column;
  align-items: center;
  justify-content: flex-start;
  font-family: 'Roboto', sans-serif;
}
.video-frame {
  width: 640px;
  position: relative;
  background-color: #999;
  height: 480px;
}
.attendance-execute {
  position: relative;
  font-weight: 500;
  font-family: 'Roboto', sans-serif;
}
.attendance-button {
  background-color: #1fd61b;
  display: flex;
  flex-direction: row;
  align-items: center;
  justify-content: center;
  padding: 10px;
}
.index-video {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: flex-start;
  gap: 34px;
}
.name {
  border-radius: 10px;
  background-color: #342e37;
  height: 52px;
  display: flex;
  flex-direction: row;
  align-items: center;
  justify-content: center;
  padding: 4px 49px;
  box-sizing: border-box;
  color: #fffbfa;
  font-family: 'Roboto', sans-serif;
}
.name1 {
  border-radius: 10px;
  background-color: #b3bfb8;
  height: 52px;
  display: flex;
  flex-direction: row;
  align-items: center;
  justify-content: center;
  padding: 4px 49px;
  box-sizing: border-box;
  font-family: 'Roboto', sans-serif;
}
.person-3 {
  width: 84px;
  position: relative;
  font-weight: 500;
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
  font-family: 'Roboto', sans-serif;
}
.name2 {
  border-radius: 10px;
  background-color: #b3bfb8;
  height: 52px;
  display: flex;
  flex-direction: row;
  align-items: center;
  justify-content: center;
  padding: 4px 49px;
  box-sizing: border-box;
  color: rgba(0, 0, 0, 0.73);
  font-family: 'Roboto', sans-serif;
}
.name8 {
  border-radius: 10px;
  background-color: #d1b317;
  height: 52px;
  display: flex;
  flex-direction: row;
  align-items: center;
  justify-content: center;
  padding: 4px 49px;
  box-sizing: border-box;
  color: #d9d9d9;
  font-family: 'Roboto', sans-serif;
}
.index-people-holder {
  width: 404px;
  min-height: 604.95px;
  display: flex;
  flex-direction: row;
  flex-wrap: wrap;
  align-items: flex-start;
  justify-content: flex-start;
  gap: 27px 40px;
  font-size: 18px;
  font-family: 'Roboto', sans-serif;
}
.index-content {
  margin: 0 !important;
  position: absolute;
  top: 39px;
  left: 67px;
  display: flex;
  flex-direction: row;
  align-items: center;
  justify-content: flex-start;
  gap: 70px;
  z-index: 0;
  font-family: 'Roboto', sans-serif;
}
.index-body-border {
  width: 1500px;
  top: -80px;
  background-color: rgba(255, 255, 255, 0.9);
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
  min-height: 1200px;
  overflow: auto;
  flex-shrink: 0;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: flex-start;
  padding: 40px 100px;
  box-sizing: border-box;
  position: relative;
  font-family: 'Roboto', sans-serif;
}
.index-body {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: flex-start;
  font-size: 20px;
  font-family: 'Roboto', sans-serif;
}
.index-all {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: flex-start;
  gap: 116px;
  font-family: 'Roboto', sans-serif;
}
</style>
