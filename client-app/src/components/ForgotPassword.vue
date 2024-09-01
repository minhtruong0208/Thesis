<template>
  <div class="forget">
      <main class="grey-border">
          <div class="forget-form-all">
              <div class="forget-form">
                  <h2 class="txt-forget-intro">
                      <p class="dng-tn-ng">Dùng tên đăng nhập</p>
                      <p class="ly-li-mt">để lấy lại mật khẩu</p>
                  </h2>
                  <div class="forget-submit-form">
                      <div class="username">
                          <div class="txt-username">Tên đăng nhập</div>
                          <div class="username-holder">
                              <img class="icon-group" alt="" src="../assets/icongroup.svg" />
                              <input
                                  v-model="Username"
                                  class="insert-username"
                                  placeholder="Nhập tên tài khoản"
                                  type="text"
                              />
                          </div>
                      </div>
                      <div class="forget-submit-href">
                          <button class="submit-btn" @click="resetPassword">Đặt lại mật khẩu</button>
                          <div class="forget-href">
                              <a class="txt-return-login-page" href="#" @click.prevent="navigateToLogin">Trở lại trang đăng nhập</a>
                          </div>
                      </div>
                  </div>
              </div>
          </div>
          <div class="theme-group">
              <div class="dark-reg"></div>
              <h2 class="txt-intro">Hệ thống điểm danh sinh viên tự động</h2>
          </div>
      </main>
  </div>
</template>

<script>
import axios from 'axios'
import { useRouter } from 'vue-router'
import { ref } from 'vue'

export default {
name: 'ForgotPassword',
setup() {
  const router = useRouter();
  const Username = ref('');

  const navigateToLogin = () => {
    router.push('/login')
  }

  const resetPassword = async () => {
    try {
      const response = await axios.post('http://localhost:5016/api/auth/reset-password', {
        Username: Username.value
      })
      console.log(response.data)
      alert('Mật khẩu mới của bạn là "abcd1234"')
    } catch (error) {
      console.error(error)
      alert('Đã xảy ra lỗi khi đổi mật khẩu')
    }
  }

  return {
    Username,
    navigateToLogin,
    resetPassword
  }
}
}
</script>

<style scoped>
.dng-tn-ng,
.ly-li-mt {
margin: 0;
}
.txt-forget-intro {
margin: 0;
width: 315px;
height: 60px;
position: relative;
font-size: inherit;
font-weight: 500;
font-family: inherit;
display: inline-block;
}
.icon-group,
.txt-username {
position: relative;
}
.icon-group {
height: 24px;
width: 24px;
}
.insert-username {
width: calc(100% - 44px);
border: 0;
outline: 0;
font-family: var(--t24-t24s-24light-29);
font-size: var(--t14-t14r-14regular-20-size);
background-color: transparent;
height: 16px;
flex: 1;
position: relative;
color: var(--color-black);
text-align: left;
display: flex;
align-items: center;
min-width: 166px;
padding: 0;
}
.username,
.username-holder {
align-self: stretch;
display: flex;
justify-content: flex-start;
}
.username-holder {
background-color: var(--gray-scale);
flex-direction: row;
align-items: center;
padding: 16px 10px;
gap: 5px;
opacity: 0.7;
}
.username {
flex-direction: column;
align-items: flex-start;
gap: 8px;
}
.submit-btn {
background-color: #257de4;
color: white;
border: none;
padding: 10px 20px;
font-size: 16px;
font-family: var(--t24-t24s-24light-29);
cursor: pointer;
border-radius: 4px;
transition: background-color 0.3s ease;
}
.submit-btn:hover {
background-color: #1e63b8;
}
.submit-btn:active {
background-color: #164a8a;
}
.txt-return-login-page {
position: relative;
line-height: 16px;
}
.forget-href {
display: flex;
flex-direction: column;
align-items: flex-start;
justify-content: center;
text-align: center;
font-size: var(--t12-t12r-12regular-16-size);
color: #257de4;
}
.forget-submit-href {
gap: 18px;
font-size: var(--t14-t14r-14regular-20-size);
color: var(--color-white);
}
.forget-form,
.forget-submit-form,
.forget-submit-href {
align-self: stretch;
display: flex;
flex-direction: column;
align-items: flex-start;
justify-content: flex-start;
}
.forget-submit-form {
gap: 19px;
font-size: 16px;
}
.forget-form {
gap: 29px;
}
.dark-reg,
.forget-form-all {
box-sizing: border-box;
max-width: 100%;
}
.forget-form-all {
width: 471px;
background-color: #fffbfa;
display: flex;
flex-direction: column;
align-items: flex-start;
justify-content: flex-start;
padding: 147px 73px;
min-width: 471px;
z-index: 1;
}
.dark-reg {
height: 561px;
width: 750px;
position: relative;
background-color: var(--color-darkslategray);
border: 1px solid var(--color-gray-100);
display: none;
}
.theme-group,
.txt-intro {
justify-content: center;
max-width: 100%;
z-index: 1;
}
.txt-intro {
margin: 0;
width: 343px;
position: relative;
font-size: inherit;
line-height: 29px;
font-weight: 300;
font-family: inherit;
display: flex;
align-items: center;
flex-shrink: 0;
}
.theme-group {
width: 750px;
background-color: var(--color-darkslategray);
border: 1px solid var(--color-gray-100);
box-sizing: border-box;
padding: var(--padding-205xl) var(--padding-xl) var(--padding-205xl) 77px;
min-width: 750px;
min-height: 561px;
text-align: center;
color: var(--color-white);
}
.forget,
.grey-border,
.theme-group {
display: flex;
flex-direction: row;
align-items: flex-start;
}
.grey-border {
flex: 1;
overflow: hidden;
justify-content: center;
padding: 231px var(--padding-xl);
box-sizing: border-box;
row-gap: 20px;
max-width: 100%;
text-align: left;
font-size: var(--t24-t24s-24light-29-size);
color: var(--color-black);
font-family: var(--t24-t24s-24light-29);
opacity: 1;
}
.forget {
width: 100%;
position: relative;
justify-content: flex-start;
line-height: normal;
letter-spacing: normal;
}
@media screen and (max-width: 1200px) {
.forget-form-all {
  flex: 1;
}
.theme-group {
  flex: 1;
  min-height: auto;
}
.grey-border {
  flex-wrap: wrap;
}
}
@media screen and (max-width: 1050px) {
.theme-group {
  min-width: 100%;
}
.grey-border {
  padding-top: var(--padding-131xl);
  padding-bottom: var(--padding-131xl);
  box-sizing: border-box;
}
}
@media screen and (max-width: 750px) {
.forget-form-all {
  padding: 96px 36px;
  box-sizing: border-box;
  min-width: 100%;
}
.theme-group {
  padding-top: var(--padding-127xl);
  padding-bottom: var(--padding-127xl);
  box-sizing: border-box;
}
}
@media screen and (max-width: 450px) {
.txt-forget-intro {
  font-size: var(--font-size-lgi);
}
.forget-form {
  gap: 16px;
}
.txt-intro {
  font-size: var(--font-size-lgi);
  line-height: 23px;
}
.theme-group {
  padding-left: var(--padding-xl);
  box-sizing: border-box;
}
.grey-border {
  padding-top: var(--padding-78xl);
  padding-bottom: var(--padding-78xl);
  box-sizing: border-box;
}
}
body {
margin: 0;
line-height: normal;
}

:root {
/* fonts */
--t24-t24s-24light-29: Roboto;

/* font sizes */
--t24-t24s-24light-29-size: 24px;
--font-size-lgi: 19px;
--t12-t12r-12regular-16-size: 12px;
--t14-t14r-14regular-20-size: 14px;

/* Colors */
--color-darkslategray: #342e37;
--main-text-color: #252525;
--color-gray-100: rgba(0, 0, 0, 0.7);
--color-white: #fff;
--gray-scale: #c4c4c4;
--color-black: #000;

/* Paddings */
--padding-xl: 20px;
--padding-131xl: 150px;
--padding-78xl: 97px;
--padding-205xl: 224px;
--padding-127xl: 146px;
}
</style>
