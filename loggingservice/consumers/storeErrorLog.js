const amqp = require('amqplib');
const uri = process.env.MESSAGEBUS
const EXCHANGE_NAME = "storeErrorLog"
const QUEUE_NAME = "storeErrorLogQueue"

var mongoose = require('mongoose');

var ErrorLog = mongoose.model('ErrorLog');

const consume = async () => {
    try {
        const connection = await amqp.connect(uri);
        let channel = await connection.createChannel();

        await channel.assertExchange(EXCHANGE_NAME, 'direct')
        let q = await channel.assertQueue(QUEUE_NAME, { exclusive: false, durable: true });
        await channel.bindQueue(q.queue, EXCHANGE_NAME, "");

        await channel.consume("", data => {

            let content = JSON.parse(data.content)

            console.log(content);

            // let id = content.Id ?? "";
            // let replyToId = content.ReplyToId ?? "";
            // let from = content.From ?? "";
            // let message = content.Message ?? "";

            // if (id) {
            //     if (replyToId) {
            //         var reply = { _id: id, message: message };

            //         Interaction.findOneAndUpdate(
            //             { _id: replyToId },
            //             { $push: { replies: reply } },
            //             function (error, result) {
            //                 if (error) {
            //                     console.log("ERROR: " + error);
            //                 } else {
            //                     if (result) {
            //                         channel.ack(data)
            //                     }
            //                 }
            //             });
            //     } else {
            //         let interaction = Interaction({
            //             _id: id,
            //             from: from,
            //             message: message,
            //         })

            //         interaction.save(function (err, result) {
            //             if (result) {
            //                 channel.ack(data)
            //             } else if (err) {
            //                 throw err;
            //             }
            //         })
            //     }
            // }
        },
            {
                noAck: false
            });

    } catch (error) {
        console.log(`error is: ${error}`);
    }
}

module.exports = consume;