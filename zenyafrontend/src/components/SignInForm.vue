<template>

<div class="d-flex justify-content-center align-items-center container">
  <h3 v-if="this.user">User: {{this.user}}</h3><br/>

  <GoogleLogin :params="params" :renderParams="renderParams" :onSuccess="onSuccess"></GoogleLogin>
  <GoogleLogin class="googleButton" :params="params" :logoutButton="true">Logout</GoogleLogin>

  
</div>

  
</template>

<script>
import GoogleLogin from 'vue-google-login';
  export default {
    name: 'App',
    data() {
      return {
        // client_id is the only required property but you can add several more params, full list down bellow on the Auth api section
        params: {
            client_id: "408797503550-r3ag70nqn6uhrbcadfs1a59pf29jrmfs.apps.googleusercontent.com"
        },
        user: null,
        // only needed if you want to render the button with the google ui
        renderParams: {
            width: 250,
            height: 50,
            longtitle: true
        }
      }
    },
    components: {
        GoogleLogin
    },
    methods: {
        onSuccess(googleUser) {
            console.log(googleUser);
 
            // This only gets the user information: id, name, imageUrl and email
            console.log(googleUser.getBasicProfile());
            this.user = googleUser.getBasicProfile().yv;
        }
    }
  }
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
.container {
  display: flex;
  justify-content: center;
  align-items: center;
  text-align: center;
  min-height: 100vh;
  flex-direction: column;
}

.googleButton {
  padding: 8px;
  margin: 12px;
  border: none;
  border-radius: 1px;
  box-shadow: 0 2px 4px 0 rgb(0 0 0 / 25%);
  -moz-box-sizing: border-box;
  box-sizing: border-box;
  -webkit-transition: background-color .218s,border-color .218s,box-shadow .218s;
  transition: background-color .218s,border-color .218s,box-shadow .218s;
  -webkit-user-select: none;
  -webkit-appearance: none;
  background-color: #fff;
  background-image: none;
  color: #262626;
  cursor: pointer;
  outline: none;
  overflow: hidden;
  position: relative;
  text-align: center;
  vertical-align: middle;
  white-space: nowrap;
  width: auto;
  color: #757575;
  background-color: #fff;
}

</style>