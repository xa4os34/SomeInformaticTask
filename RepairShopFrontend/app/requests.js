export function sendPost(url, data) {
  return new Promise(function(resolve, reject) {
    let xhr = new XMLHttpRequest();
    //xhr.withCredentials = true;

    xhr.open('POST', url);
    xhr.onreadystatechange = handler;
    xhr.responseType = 'json';
    xhr.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
    xhr.setRequestHeader('Accept', 'application/json');
    xhr.setRequestHeader('ngrok-skip-browser-warning', "true");
    xhr.setRequestHeader('X-Requested-With', 'XMLHttpRequest');
    xhr.send(data);


    function handler() {
      if (this.readyState === this.DONE) {
        if (this.status === 200) {
          resolve(this.response);
        } else {
          reject(new Error('sendPost: `' + url + '` failed with status: [' + this.status + ']'));
        }
      }
    }
  });
}

export function sendGet(url) {
  return new Promise(function(resolve, reject) {
    let xhr = new XMLHttpRequest();

    xhr.open('GET', url);
    xhr.onreadystatechange = handler;
    xhr.responseType = 'json';
    xhr.setRequestHeader('Accept', 'application/json');
    xhr.setRequestHeader('ngrok-skip-browser-warning', "true");
    xhr.setRequestHeader('X-Requested-With', 'XMLHttpRequest');
    xhr.send();


    function handler() {
      if (this.readyState === this.DONE) {
        if (this.status === 200) {
          resolve(this.response);
        } else {
          reject(new Error('sendGet: `' + url + '` failed with status: [' + this.status + ']'));
        }
      }
    }
  });
}

export function sendPut(url, data) {
  return new Promise(function(resolve, reject) {
    let xhr = new XMLHttpRequest();
    //xhr.withCredentials = true;

    xhr.open('PUT', url);
    xhr.onreadystatechange = handler;
    xhr.responseType = 'json';
    xhr.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
    xhr.setRequestHeader('Accept', 'application/json');
    xhr.setRequestHeader('ngrok-skip-browser-warning', "true");
    xhr.setRequestHeader('X-Requested-With', 'XMLHttpRequest');
    xhr.send(data);


    function handler() {
      if (this.readyState === this.DONE) {
        if (this.status === 204) {
          resolve(this.response);
        } else {
          reject(new Error('sendPut: `' + url + '` failed with status: [' + this.status + ']'));
        }
      }
    }
  });
}

export function sendDelete(url) {
  return new Promise(function(resolve, reject) {
    let xhr = new XMLHttpRequest();
    //xhr.withCredentials = true;

    xhr.open('DELETE', url);
    xhr.onreadystatechange = handler;
    xhr.responseType = 'json';
    xhr.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
    xhr.setRequestHeader('Accept', 'application/json');
    xhr.setRequestHeader('ngrok-skip-browser-warning', "true");
    xhr.setRequestHeader('X-Requested-With', 'XMLHttpRequest');
    xhr.send();


    function handler() {
      if (this.readyState === this.DONE) {
        if (this.status === 200) {
          resolve(this.response);
        } else {
          reject(new Error('sendDelete: `' + url + '` failed with status: [' + this.status + ']'));
        }
      }
    }
  });
}
