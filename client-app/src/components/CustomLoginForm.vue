<template>
  <div class="login-page">
    <main class="gray-border">
      <div class="login-group">
        <div class="white-reg"></div>
        <div class="login-form">
          <div class="login">
            <div class="username">
              <div class="txt-username">Tên đăng nhập</div>
              <div class="username-holder">
                <img class="icon-group" alt="" src="../assets/icongroup.svg" />
                <input
                  v-model="Username"
                  class="insert-username"
                  placeholder="Nhập tên tài khoản"
                  type="text"
                  required
                />
              </div>
            </div>
            <div class="password">
              <div class="txt-password">Mật khẩu</div>
              <div class="password-holder">
                <img class="secure-icon" alt="" src="../assets/secureicon.svg" />
                <input
                  v-model="Password"
                  class="insert-password"
                  placeholder="Nhập mật khẩu"
                  type="password"
                  required
                />
              </div>
            </div>
          </div>
          <button class="submit" @click="login">
            Đăng nhập
          </button>
          <div v-if="showError" class="error-message">
            Tài khoản hoặc mật khẩu không đúng
          </div>
          <div class="forget">
            <a class="forget1" href="#" @click.prevent="navigateToForgotPassword">Quên mật khẩu ?</a>
          </div>
        </div>
      </div>
      <div class="login-dark">
        <div class="dark-reg"></div>
        <h2 class="txt-intro">Hệ thống điểm danh sinh viên tự động</h2>
      </div>
    </main>
  </div>
</template>

<script>
import { ref } from 'vue';
import axios from 'axios';
import { useStore } from 'vuex';
import {useRouter} from 'vue-router';

export default {
  name: 'CustomLoginForm',
  setup() {
    const store = useStore();
    const router = useRouter();

    const Username = ref('');
    const Password = ref('');
    const showError = ref(false);

    const navigateToForgotPassword = () => {
      router.push('/forgot-password')
    }

    const login = async () => {
      try {
        const response = await axios.post('http://localhost:5016/api/auth/login', {
          Username: Username.value,
          Password: Password.value
        });

        console.log(response.data);

        store.commit('setUser', response.data);

        // Chuyển hướng đến trang chính sau khi đăng nhập thành công
        if (response.data.roles === 'giangvien') {
          router.push('/');
        } else if (response.data.roles === 'nhanvien') {
          router.push('/manage');
        }

      } catch (error) {
        console.error(error);
        showError.value = true;
      }
    };

    return {
      Username,
      Password,
      showError,
      navigateToForgotPassword,
      login
    };
  }
};
</script>
  
<style>
.white-reg {
  height: 561px;
  width: 472px;
  background-color: var(--color-snow);
  display: none;
  max-width: 100%;
}
.icon-group,
.txt-username,
.white-reg {
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
  white-space: pre-wrap;
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
  padding: var(--padding-base) var(--padding-3xs);
  gap: var(--gap-8xs);
  opacity: 0.7;
}
.username {
  flex-direction: column;
  align-items: flex-start;
  gap: var(--gap-5xs);
}
.secure-icon,
.txt-password {
  position: relative;
}
.secure-icon {
  height: 24px;
  width: 24px;
}
.insert-password {
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
.login,
.password,
.password-holder {
  align-self: stretch;
  display: flex;
  justify-content: flex-start;
}
.password-holder {
  background-color: var(--gray-scale);
  flex-direction: row;
  align-items: center;
  padding: var(--padding-base) var(--padding-3xs);
  gap: var(--gap-8xs);
  opacity: 0.7;
}
.login,
.password {
  flex-direction: column;
}
.password {
  align-items: flex-start;
  gap: var(--gap-5xs);
}
.login {
  align-items: center;
  gap: 17px;
  text-align: left;
}

.login1 {
  position: relative;
  line-height: 20px;
}
  .submit {
  align-self: stretch;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  position: relative;
  gap: var(--gap-3xs);
  font-size: 16px;
  color: white; 
  background-color: #4CAF50; 
  border: none; 
  padding: 15px 32px; 
  text-align: center; 
  text-decoration: none; 
  display: inline-block; 
  font-family: Arial, sans-serif; 
  font-weight: bold; 
  border-radius: 12px; 
  box-shadow: 0 4px #999; 
  cursor: pointer; 
}

.submit:hover {
  background-color: #45a049; 
}

.submit:active {
  background-color: #3e8e41; 
  box-shadow: 0 5px #666; 
  transform: translateY(4px); 
}
.forget1 {
  position: relative;
  line-height: 16px;
}
.forget {
  display: flex;
  flex-direction: row;
  align-items: center;
  justify-content: center;
  padding: var(--padding-3xs);
  font-size: var(--t12-t12r-12regular-16-size);
  color: #257de4;
}
.login-form {
  flex: 1;
  display: flex;
  flex-direction: column;
  align-items: flex-start;
  justify-content: flex-start;
  gap: 24px;
  max-width: 100%;
  z-index: 1;
}
.dark-reg,
.login-group {
  box-sizing: border-box;
  max-width: 100%;
}
.login-group {
  height: 561px !important;
  width: 472px;
  background-color: var(--color-snow);
  display: flex;
  flex-direction: row;
  align-items: flex-start;
  justify-content: flex-start;
  padding: var(--padding-107xl) 74px var(--padding-107xl) 73px;
  gap: var(--gap-3xs);
  min-width: 472px;
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
.login-dark,
.txt-intro {
  justify-content: center;
  max-width: 100%;
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
  z-index: 2;
}
.login-dark {
  width: 750px;
  background-color: var(--color-darkslategray);
  border: 1px solid var(--color-gray-100);
  box-sizing: border-box;
  padding: var(--padding-205xl) var(--padding-xl) var(--padding-205xl) 77px;
  gap: var(--gap-3xs);
  min-width: 750px;
  min-height: 561px;
  z-index: 1;
  font-size: var(--t24-t24s-24light-29-size);
  color: var(--color-white);
}
.gray-border,
.login-dark,
.login-page {
  display: flex;
  flex-direction: row;
  align-items: flex-start;
}
.gray-border {
  flex: 1;
  overflow: hidden;
  justify-content: center;
  padding: 231px var(--padding-xl) 232px;
  box-sizing: border-box;
  row-gap: 20px;
  max-width: 100%;
  text-align: center;
  font-size: var(--font-size-base);
  color: var(--color-black);
  font-family: var(--t24-t24s-24light-29);
}
.login-page {
  width: 100%;
  position: relative;
  justify-content: flex-start;
  line-height: normal;
  letter-spacing: normal;
}
@media screen and (max-width: 1200px) {
  .login-group {
    flex: 1;
  }
  .login-dark {
    flex: 1;
    min-height: auto;
  }
  .gray-border {
    flex-wrap: wrap;
  }
}
@media screen and (max-width: 1050px) {
  .login-dark {
    min-width: 100%;
  }
  .gray-border {
    padding-top: 150px;
    padding-bottom: 151px;
    box-sizing: border-box;
  }
}
@media screen and (max-width: 750px) {
  .login-group {
    padding: var(--padding-63xl) 37px var(--padding-63xl) 36px;
    box-sizing: border-box;
    min-width: 100%;
  }
  .login-dark {
    padding-top: var(--padding-127xl);
    padding-bottom: var(--padding-127xl);
    box-sizing: border-box;
  }
}
@media screen and (max-width: 450px) {
  .txt-intro {
    font-size: 19px;
    line-height: 23px;
  }
  .login-dark {
    padding-left: var(--padding-xl);
    box-sizing: border-box;
  }
  .gray-border {
    padding-top: 97px;
    padding-bottom: 98px;
    box-sizing: border-box;
  }
}
.error-message {
    color: red;
    font-weight: bold;
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
  --t12-t12r-12regular-16-size: 12px;
  --t14-t14r-14regular-20-size: 14px;
  --font-size-base: 16px;

  /* Colors */
  --color-darkslategray: #342e37;
  --main-text-color: #252525;
  --color-gray-100: rgba(0, 0, 0, 0.7);
  --color-white: #fff;
  --color-snow: #fffbfa;
  --gray-scale: #c4c4c4;
  --color-black: #000;

  /* Gaps */
  --gap-3xs: 10px;
  --gap-5xs: 8px;
  --gap-8xs: 5px;

  /* Paddings */
  --padding-xl: 20px;
  --padding-205xl: 224px;
  --padding-127xl: 146px;
  --padding-107xl: 126px;
  --padding-63xl: 82px;
  --padding-3xs: 10px;
  --padding-base: 16px;
}
</style>