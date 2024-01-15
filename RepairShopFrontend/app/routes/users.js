import Route from '@ember/routing/route';
import config from '../config/environment';
import requests from "../requests"

export default Route.extend({
  model() {
    return requests.sendGet(`${config.APP.backendUrl}/Users`);
  },

  actions: {
    createUser() {
      let controller = this;
      requests
        .sendPost(
          `${config.APP.backendUrl}/Users`,
          JSON.stringify({ name: "none", email: "none@none", phone: "1234", location: "some"}))
        .finally(function() {
          controller.refresh();
        });
    },

    editUser(user){
      let controller = this;
      requests
        .sendPut(
          `${config.APP.backendUrl}/Users/${user.id}`,
          JSON.stringify(user))
        .finally(function() {
          controller.refresh();
        });
    },

    deleteUser(id) {
      let controller = this;
      requests.sendDelete(`${config.APP.backendUrl}/Users/${id}`)
        .finally(function() {
          controller.refresh();
        });
    }
  }
});
