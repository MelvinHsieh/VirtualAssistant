"use strict";

var connection = new signalR.HubConnectionBuilder()
    .withUrl("/notifications")
    .withAutomaticReconnect()
    .configureLogging(signalR.LogLevel.Debug)
    .build();

connection.onreconnecting(() => {
    console.log("reconnecting")
})

connection.on("SendAlert", function (alert) {
    var alertObj = JSON.parse(alert);
    console.log(alertObj);

    var li = document.createElement("li");
    li.classList.add("d-flex", "justify-content-between", "list-group-item",  "list-group-item-danger"); //Bootstrap
    document.getElementById("alertList").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
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
    })

    if (alertObj.patientId) {
        var dossierLink = document.createElement("a")
        dossierLink.textContent = "Patientoverzicht"
        dossierLink.href = `/Patient/Details/${alertObj.patientId}`
        p.appendChild(dossierLink)
    }

    buttons.appendChild(confirmButton);
    buttons.append(cancelButton);
    li.appendChild(p);
    li.appendChild(buttons);
});

connection.start().then(() => {
    console.log("Connection opened!")
})





