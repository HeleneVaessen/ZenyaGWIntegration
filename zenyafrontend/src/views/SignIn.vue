<template>
  <div class="hello">
    <sign-in-form />
  </div>
  
</template>

<script>
import SignInForm from "@/components/SignInForm.vue";

export default {
  name: "SignIn",
  components: {
    SignInForm
  }
};

</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>

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
