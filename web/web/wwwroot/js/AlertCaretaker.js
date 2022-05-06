"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/notificationHub").withAutomaticReconnect().build();

connection.onreconnecting(() => {
    console.log("reconnecting")
})

connection.on("receiveCaretakerAlert", function (user, message) {
    console.log("alert")
    var li = document.createElement("li");
    document.getElementById("notificationList").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    li.textContent = `${user} says ${message}`;
});

connection.start().then(() => {
    console.log("Connection opened!")
})





