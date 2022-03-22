<template>
  <div class="hello">
    <iframe src="https://docs.google.com/document/d/e/2PACX-1vTEp1e_xQFfV1SXnE2loEbD2wF40_XT1Mk9VRQRpURM7u5oNp5VYztIXaLzd4_wC_lmBoesP8Mz4tnV/pub?embedded=true"></iframe>
    <div>
      <b-button>Button</b-button>
      <b-button variant="danger">Button</b-button>
      <b-button variant="success">Button</b-button>
      <b-button variant="outline-primary">Button</b-button>
    </div>
  
  </div>
  
</template>

<script>
export default {
  name: 'HelloWorld',
  props: {
    msg: String
  }
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
iframe {
  width: 35%;
  height: 550px;
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
