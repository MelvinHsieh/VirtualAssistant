"use strict";

var connection = new signalR.HubConnectionBuilder()
    .withUrl("/notifications")
    .withAutomaticReconnect()
    .configureLogging(signalR.LogLevel.Debug)
    .build();

function drawAlerts() {
    var alerts = JSON.parse(window.localStorage.getItem("alerts"))
    console.log(alerts)
    alerts.forEach((a) => {
        console.log(a)
        document.getElementById("alertList").appendChild(drawAlertItem(a))
    })
}

function removeAlert(id) {
    var alerts = JSON.parse(window.localStorage.getItem("alerts"));
    alerts = alerts.filter(a => a.storageId != id);
    window.localStorage.setItem("alerts", JSON.stringify(alerts))
}

function drawAlertItem(alertObj) {
    var li = document.createElement("li");
    li.classList.add("d-flex", "justify-content-between", "list-group-item", "list-group-item-danger"); //Bootstrap
    var p = document.createElement("p");
    p.textContent = `${alertObj.message} `;

    var buttons = document.createElement("div")
    var confirmButton = document.createElement("button");
    confirmButton.textContent = "Hulp is onderweg!";
    confirmButton.classList.add("btn", "btn-light", "mx-2")

    var cancelButton = document.createElement("button");
    cancelButton.textContent = "X";
    cancelButton.classList.add("btn", "btn-light", "mx-2")
    cancelButton.addEventListener("click", () => {
        li.remove();
        removeAlert(alertObj.storageId)
    })

    if (alertObj.patientId) {
        var dossierLink = document.createElement("a")
        dossierLink.textContent = "Patientoverzicht"
        dossierLink.href = `/Patient/Details/${alertObj.patientId}` //hardcoded
        p.appendChild(dossierLink)
    }

    buttons.appendChild(confirmButton);
    buttons.append(cancelButton);
    li.appendChild(p);
    li.appendChild(buttons);

    return li
}

connection.onreconnecting(() => {
    console.log("reconnecting")
})

connection.on("SendAlert", function (alert) {
    // Gets the alerts from localstorage and updates it with the new localstorage
    var id = "id" + Math.random().toString(16).slice(2)
    var storageObj = window.localStorage.getItem("alerts")
    var alertObj = JSON.parse(alert);
    alertObj.storageId = id
    var alerts = [];
    if (storageObj) {
        alerts = JSON.parse(storageObj)   
    }
    console.log(alerts);
    alerts.push(alertObj)
    window.localStorage.setItem("alerts", JSON.stringify(alerts))

    //Draw the new element, assuming the old ones are already visible
    document.getElementById("alertList").appendChild(drawAlertItem(alertObj))
});

connection.start().then(() => {
    drawAlerts()
})





