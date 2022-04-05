<template>
  <div class="hello">
    <sign-in-form />
  </div>
  
</template>

<script>
import SignInForm from "@/components/SignInForm.vue";
import ajax from "vuejs-ajax"



export default {
  
  name: "SignIn",
  components: {
    SignInForm
  },
  methods: {
    ZenyaAPI: function(config){
      /// <summary>Helper class for call iProva API's</summary>
      /// <param name="iProvaUrl" type="string">URL of iProva</param>
      /// <param name="config" type="object">Client configuration object</param>
      /// <returns></returns>

      this._iProvaURL = config.iProvaUrl;
      this._logonMethod = config.logonMethod;
      this._alwaysGetNewUserToken = config.alwaysGetNewUserToken;
      this._version = config.version;
      this._apiKey = config.apiKey;

      if (config.logonMethod === this.ZenyaAPI.Enumerations.LogonMethod.WindowsAuthentication && !config.apiKey)
        throw new Error('When using WindowsAuthentication an api key must be used.')

      //append trailing / if needed
      if (this._iProvaURL[this._iProvaURL.length - 1] != "/")
        this._iProvaURL += "/";

      //initiate token helper
      if (this._logonMethod !== this.ZenyaAPI.Enumerations.LogonMethod.None)
        this._tokenHelper = new this.ZenyaAPI.TokenHelper(this._iProvaURL, this._logonMethod);

      return this;
    },

    _ajaxError: function (jqxhr, textStatus, errorThrown) {
      /// <summary>Handles jQuery ajax request error</summary>
      /// <param name="jqxhr" type="jqXHR">jQuery jqXHR object, which is a superset of the XMLHTTPRequest object. (http://api.jquery.com/jQuery.ajax/#jqXHR)</param>
      /// <param name="textStatus" type="string">Possible values are "timeout", "error", "abort", and "parsererror"</param>
      /// <param name="errorThrown" type="string">textual portion of the HTTP status</param>
      /// <returns></returns>

      this.ZenyaAPI.showAjaxError(jqxhr, textStatus, errorThrown, '@@textStatus@@ during API Call.')
    },

    callREST: function (callParameters, forceNewToken) {
      var apiCallFunction = function (callParameters, token)
      {
        ajax({
          method: callParameters.method,
          url: this._iProvaURL + callParameters.path,
          crossDomain: true,
          xhrFields: {
            withCredentials: (this._logonMethod === this.ZenyaAPI.Enumerations.LogonMethod.Cookie)
          },
          beforeSend: function (request)
          {
            //if a token is set, set the token in authorization header
            if (token)
              request.setRequestHeader('Authorization', 'token ' + token);

            if (this._version)
              request.setRequestHeader("x-api-version", this._version);

            if (this._apiKey)
              request.setRequestHeader("x-api_key", this._apiKey);
          }.bind(this),
          contentType: "application/json",
          data: (callParameters.parameters ? JSON.stringify(callParameters.parameters) : null),
          success: function (result)
          {
            callParameters.callback(result);
          },
          error: function (jqxhr, textStatus, errorThrown)
          {
            //token expired?
            if (jqxhr.responseJSON && jqxhr.responseJSON.ErrorCode == 1014)
              this.callREST(callParameters, true);
            else
              this._ajaxError(jqxhr, textStatus, errorThrown);

          }.bind(this)
        });
      }.bind(this, callParameters);

        //get token first? (winauth / saml variants)
        if (this._logonMethod >= this.ZenyaAPI.Enumerations.LogonMethod.WindowsAuthentication)
          {
          if (arguments.length == 1)
            forceNewToken = false;
            forceNewToken = (forceNewToken || this.alwaysGetNewUserToken);
            this._tokenHelper.getToken(apiCallFunction, forceNewToken);
        }
        else //direct API call
        {
          apiCallFunction();
        }
    },

    tokenHelper: function (iProvaUrl, logonMethod) {
      /// <summary>TokenHelper constructor</summary>
      /// <param name="iProvaUrl" type="string">URL of iProva (with trailing /)</param>
      /// <param name="logonMethod" type="iProvaAPI.Enumerations.LogonMethod">Login method to use to identify user.</param>
      /// <returns></returns>

      this._iProvaURL = iProvaUrl;
      this._logonMethod = logonMethod;
      this._token = null;

      return this;
    },

    getToken(){
      
    }
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
