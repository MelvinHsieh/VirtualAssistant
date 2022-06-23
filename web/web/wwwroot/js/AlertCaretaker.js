"use strict";

var connection = new signalR.HubConnectionBuilder()
    .withUrl("/notifications")
    .withAutomaticReconnect()
    .configureLogging(signalR.LogLevel.Debug)
    .build();

connection.onreconnecting(() => {
    console.log("reconnecting")
})

connection.on("SendAlert", function (message, uri) {
    var li = document.createElement("li");
    li.classList.add("d-flex", "justify-content-between", "list-group-item", "list-group-item-danger"); //Bootstrap

    var link = document.createElement("a")
    link.setAttribute("href", `${uri}`)
    link.appendChild(li);

    document.getElementById("alertList").appendChild(link);
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





