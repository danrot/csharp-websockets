const $randomNumber = document.getElementById('random-number');
const $creationDate = document.getElementById('creation-date');

const webSocket = new WebSocket(`wss://${location.host}/WebSocket`);

webSocket.onmessage = function (event) {
    const data = JSON.parse(event.data);
    $randomNumber.innerHTML = data.random;
    $creationDate.innerHTML = data.created;
}
