import Route from '@ember/routing/route';
import config from '../../config/environment';
import requests from "../../requests"

export default Route.extend({
  users: [],

  beforeModel() {
    return requests.sendGet(`${config.APP.backendUrl}/Users`)
      .then(users => this.users = users);
  },

  model() {
    return {
      users: this.users,
      request: {
        priority: "",
        problemDescirption: "",
        status: "",
        userId: this.users.length > 0 ? this.users[0] : 0,
        device: {
          type: "",
          model: "",
          serialNumber: ""
        },
      }
    };
  },

  actions: {
    updateUserId(request, id) {
      console.log(id);
      Ember.set(request, "userId", id);
    },

    createRequest() {
      let request =this.get("context.request");
      console.log(request);
      requests
        .sendPost(
          `${config.APP.backendUrl}/Requests`,
          JSON.stringify(request))
        .then(function() {
          this.transitionTo('requests');
        });
    },

    cancleCreation(){
      this.transitionTo('requests');
    },
  }
});
