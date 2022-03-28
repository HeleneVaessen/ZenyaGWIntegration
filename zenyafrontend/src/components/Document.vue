<template>
  <div class="hello">
    <iframe src="https://docs.google.com/document/d/e/2PACX-1vTEp1e_xQFfV1SXnE2loEbD2wF40_XT1Mk9VRQRpURM7u5oNp5VYztIXaLzd4_wC_lmBoesP8Mz4tnV/pub?embedded=true"></iframe>  
  </div>
  
</template>

<script>
export default {
  name: 'GoogleDocument',
  props: {
    msg: String
  }
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>

iframe {
  width: 65%;
  height: 600px;
  box-shadow: rgba(99, 99, 99, 0.2) 0px 2px 8px 0px;
  position: absolute;
  top: 50%;
  left: 50%;
  margin-right: -50%;
  transform: translate(-50%, -50%);
  border-radius: 5px;
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
