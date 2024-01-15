import Route from '@ember/routing/route';
import config from '../config/environment';
import requests from "../requests"

export default Route.extend({
  employees: [],
  repairRecords: [],

  beforeModel() {
    requests.sendGet(`${config.APP.backendUrl}/RepairInfos`)
      .then(repairRecords =>{ if (this.repairRecords.length === 0) {this.refresh();} this.repairRecords = repairRecords; });
    return requests.sendGet(`${config.APP.backendUrl}/Employees`)
      .then(employees => this.employees = employees);
  },

  model() {
    return {
      employees: this.get("employees"),
      repairRecords: this.get("repairRecords")
    };
  },

  actions: {
    updateEmployee(repairRecord, id) {
      Ember.set(repairRecord, 'employee.id', id);
    },

    editRepairRecord(repairRecord) {
      let controller = this;
      requests
        .sendPut(
          `${config.APP.backendUrl}/Requests/${repairRecord.request.id}/RepairInfo`,
          JSON.stringify({ completionDate: repairRecord.completionDate, employeeId: repairRecord.employee.id}))
        .finally(function() {
          controller.refresh();
        });
    },

    deleteRepairRecord(id) {
      let controller = this;
      requests.sendDelete(`${config.APP.backendUrl}/Requests/${id}/RepairInfo`)
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
