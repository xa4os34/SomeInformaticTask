import Route from '@ember/routing/route';
import config from '../config/environment';
import requests from "../requests"

export default Route.extend({
  employees: null,
  request: null,
  repairRecord: null,
  isHasRepairRecord: false,

  beforeModel(transition) {
    var params = transition.params.request;
    this.isHasRepairRecord = false;
    requests.sendGet(`${config.APP.backendUrl}/Employees`)
    .then(employees => this.employees = employees);
    requests.sendGet(`${config.APP.backendUrl}/Requests/${params.id}/RepairInfo`)
      .then(repairRecord => {
        this.repairRecord = repairRecord;
        this.isHasRepairRecord = true;
      });
    return requests.sendGet(`${config.APP.backendUrl}/Requests/${params.id}`)
      .then(request => this.request = request);
  },

  model() {
    return {
      employees: this.employees,
      request: this.request,
      repairRecord: this.repairRecord != null ? this.repairRecord :
      {
        compeletionDate: 0,
        employeeId: this.employees.length > 0 ? this.employees[0].id : 0,
      },
      isHasRepairRecord: this.isHasRepairRecord
    };
  },

  actions: {
    editRequest(employee) {
      let controller = this;
      requests
        .sendPut(
          `${config.APP.backendUrl}/Requests/${employee.id}`,
          JSON.stringify(employee))
        .finally(function() {
          controller.refresh();
        });
    },

    createRepairRecord(repairRecord) {
      let controller = this;
      var request = this.get("context.request");
      requests
        .sendPost(
          `${config.APP.backendUrl}/Requests/${request.id}/RepairInfo`,
          JSON.stringify(repairRecord))
        .finally(function() {
          controller.refresh();
        });
    },

    updateEmployeeId(repairRecord, id) {
      repairRecord.EmployeeId = id;
    }
  }
});
