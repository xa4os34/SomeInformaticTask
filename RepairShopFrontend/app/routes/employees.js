import Route from '@ember/routing/route';
import config from '../config/environment';
import requests from "../requests"

export default Route.extend({
  model() {
    return requests.sendGet(`${config.APP.backendUrl}/Employees`);
  },

  actions: {
    createEmployee() {
      let controller = this;
      requests
        .sendPost(
          `${config.APP.backendUrl}/Employees`,
          JSON.stringify({ name: "none", post: "none" }))
        .finally(function() {
          controller.refresh();
        });
    },

    editEmployee(employee){
      let controller = this;
      requests
        .sendPut(
          `${config.APP.backendUrl}/Employees/${employee.id}`,
          JSON.stringify(employee))
        .finally(function() {
          controller.refresh();
        });
    },

    deleteEmployee(id) {
      let controller = this;
      requests.sendDelete(`${config.APP.backendUrl}/Employees/${id}`)
        .finally(function() {
          controller.refresh();
        });
    }
  }
});
