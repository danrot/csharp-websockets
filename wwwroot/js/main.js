const $randomNumber = document.getElementById('random-number');
const $creationDate = document.getElementById('creation-date');
const $updateButton = document.getElementById('update-button');

const webSocket = new WebSocket(`wss://${location.host}/WebSocket`);

webSocket.onopen = function (event) {
    $updateButton.addEventListener('click', function() {
        webSocket.send(JSON.stringify({type: 'update'}));
    });
}

webSocket.onmessage = function (event) {
    const data = JSON.parse(event.data);
    $randomNumber.innerHTML = data.random;
    $creationDate.innerHTML = data.created;
}
