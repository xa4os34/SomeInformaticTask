import Route from '@ember/routing/route';
import config from '../config/environment';
import requests from "../requests"

export default Route.extend({
  model() {
    return requests.sendGet(`${config.APP.backendUrl}/Requests`);
  },

  actions: {
    createRequest() {
      this.transitionTo("new/request")
      this.refresh();
    },

    deleteRequest(id) {
      let controller = this;
      requests.sendDelete(`${config.APP.backendUrl}/Requests/${id}`)
        .finally(function() {
          controller.refresh();
        });
    },

    toRequestDetails(id){
      this.transitionTo('request', id);
      this.refresh();
    }
  }
});
