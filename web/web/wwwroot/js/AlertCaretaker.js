"use strict";

var connection = new signalR.HubConnectionBuilder()
    .withUrl("/notifications")
    .withAutomaticReconnect()
    .configureLogging(signalR.LogLevel.Debug)
    .build();

connection.onreconnecting(() => {
    console.log("reconnecting")
})

connection.on("SendAlert", function (message, detailuri, confirmuri) {
    var li = document.createElement("li");
    li.classList.add("d-flex", "justify-content-between", "list-group-item", "list-group-item-danger"); //Bootstrap

    var link = document.createElement("a")
    link.appendChild(li);

    document.getElementById("alertList").appendChild(link);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    var p = document.createElement("p");
    p.textContent = `${message}`;

    var confirm = document.createElement("button");
    confirm.textContent = "Noodoproep bevestigen";
    confirm.classList.add("btn", "btn-light")
    confirm.addEventListener("click", () => { 
        fetch(confirmuri, {method: 'PUT'})
        .then(data => {
            return data
        })
        .then(res => {console.log(res)})
    })

    var button = document.createElement("button");
    button.textContent = "Naar detail pagina gaan";
    button.classList.add("btn", "btn-light")
    button.addEventListener("click", () => {
        location.href = `${detailuri}`;
        li.remove();
    })

    li.appendChild(p);
    li.appendChild(button);
    li.appendChild(confirm);
});

connection.start().then(() => {
    console.log("Connection opened!")
})





