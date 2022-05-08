"use strict";

var connection = new signalR.HubConnectionBuilder()
    .withUrl("/notifications")
    .withAutomaticReconnect()
    .configureLogging(signalR.LogLevel.Debug)
    .build();

connection.onreconnecting(() => {
    console.log("reconnecting")
})

connection.on("SendAlert", function (message) {
    console.log(message)

    var li = document.createElement("li");
    li.classList.add("d-flex", "justify-content-between", "list-group-item",  "list-group-item-danger"); //Bootstrap
    document.getElementById("alertList").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    var p = document.createElement("p");
    p.textContent = `${message}`;

    var button = document.createElement("button");
    button.textContent = "Ok";
    button.classList.add("btn", "btn-light")
    button.addEventListener("click", () => {
        li.remove();
    })

    li.appendChild(p);
    li.appendChild(button);
});

connection.start().then(() => {
    console.log("Connection opened!")
})





