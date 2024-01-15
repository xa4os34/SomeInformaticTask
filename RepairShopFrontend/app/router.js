import EmberRouter from '@ember/routing/router';
import config from './config/environment';

const Router = EmberRouter.extend({
  location: config.locationType,
  rootURL: config.rootURL
});

Router.map(function() {
  this.route('employees');
  this.route('users');
  this.route('repairRecords');
  this.route('request', { path: 'request/:id'});
  this.route('requests');

  this.route('new/request');
});

export default Router;
