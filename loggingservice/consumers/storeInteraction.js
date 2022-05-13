const amqp = require('amqplib');
const uri = process.env.MESSAGEBUS
const EXCHANGE_NAME = "storeInteraction"
const QUEUE_NAME = "storeInteractionQueue"

var mongoose = require('mongoose');

var Interaction = mongoose.model('Interaction');

const consume = async () => {
    try {
        const connection = await amqp.connect(uri);
        let channel = await connection.createChannel();

        await channel.assertExchange(EXCHANGE_NAME, 'direct')
        let q = await channel.assertQueue(QUEUE_NAME, { exclusive: false, durable: true });
        await channel.bindQueue(q.queue, EXCHANGE_NAME, "");

        await channel.consume("", data => {

            let content = JSON.parse(data.content)

            let id = content.Id ?? "";
            let replyToId = content.ReplyToId ?? "";
            let from = content.From ?? "";
            let message = content.Message ?? "";

            if (id) {
                if (replyToId) {
                    var reply = { _id: id, message: message };

                    Interaction.findOneAndUpdate(
                        { _id: replyToId },
                        { $push: { replies: reply } },
                        function (error, result) {
                            if (error) {
                                console.log("ERROR: " + error);
                            } else {
                                if (result) {
                                    channel.ack(data)
                                }
                            }
                        });
                } else {
                    let interaction = Interaction({
                        _id: id,
                        from: from,
                        message: message,
                    })

                    interaction.save(function (err, result) {
                        if (result) {
                            channel.ack(data)
                        } else if (err) {
                            throw err;
                        }
                    })
                }
            }

            // if (image) {
            //     if (content.isTarget) {
            //         let result = new Photo(
            //             {
            //                 _id: _id,
            //                 image: image,
            //             }).save()

            //         result.then(data => {
            //             console.log(_id + " SAVED NEW TARGET IMAGE")

            //             submitScoring({ _id: _id, username: data.username, image_id: data._id, image: image }, true);
            //         })
            //     } else {
            //         let result = new Photo(
            //             {
            //                 _id: new mongoose.Types.ObjectId().toHexString(),
            //                 image: image,
            //             }).save()

            //         result.then(data => {
            //             console.log(data._id + " SAVED NEW ATTEMPT IMAGE")

            //             submitScoring({ _id: data._id, target_id: _id, image_id: data._id, image: image }, false);
            //         })
            //     }
            // }

            // channel.ack(data)
        },
            {
                noAck: false
            });

    } catch (error) {
        console.log(`error is: ${error}`);
    }
}

module.exports = consume;