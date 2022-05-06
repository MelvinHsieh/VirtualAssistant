"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/notificationHub").build();

connection.onreconnecting(() => {
    console.log("reconnecting")
})

connection.onclose(() => {
    console.log("Closed")
    connection.start().then(() => {
        console.log(connection)
        connection.on("ReceiveCaretakerAlert", function (user, message) {
            console.log("alert")
            var li = document.createElement("li");
            document.getElementById("notificationList").appendChild(li);
            // We can assign user-supplied strings to an element's textContent because it
            // is not interpreted as markup. If you're assigning in any other way, you 
            // should be aware of possible script injection concerns.
            li.textContent = `${user} says ${message}`;
        });

        connection.send("AlertCaretaker")
    })
})

connection.start().then(() => {
    console.log(connection)
    connection.on("ReceiveCaretakerAlert", function (user, message) {
        console.log("alert")
        var li = document.createElement("li");
        document.getElementById("notificationList").appendChild(li);
        // We can assign user-supplied strings to an element's textContent because it
        // is not interpreted as markup. If you're assigning in any other way, you 
        // should be aware of possible script injection concerns.
        li.textContent = `${user} says ${message}`;
    });

    connection.send("AlertCaretaker")
})



