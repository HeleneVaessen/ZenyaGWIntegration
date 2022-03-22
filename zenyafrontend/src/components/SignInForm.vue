<template>

<div class="d-flex justify-content-center align-items-center container">
  <div class="row">    
      <button v-on:click="ConnectAccounts()" type="submit" class="btn btn-primary">Log in</button>
      
  </div>
  {{this.msg}}     
</div>

  
</template>

<script>
import axios from 'axios'
export default {
  name: 'SignInForm',
  props: {

  },
  data() {
    return {
      msg: "",

    };
  },


  methods:{
    ConnectAccounts: function () {
      axios
      .post('https://localhost:7112/connectaccounts/connectaccounts')
      .then(response => {
        this.msg = "Response " + response.status
      }).catch(error => {
        this.msg =  error
      })
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

img {
  width: 20%;
  margin: 24px;
}

h3 {
  margin: 40px 0 0;
}
ul {
  list-style-type: none;
  padding: 0;
}
li {
  display: inline-block;
  margin: 0 10px;
}
a {
  color: #42b983;
}
</style>