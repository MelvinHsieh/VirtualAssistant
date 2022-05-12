const amqp = require('amqplib');
const uri = process.env.MESSAGEBUS
const QUEUE_NAME = "storeInteractionQueue"

var mongoose = require('mongoose');

var Scoring = mongoose.model('Interaction');

const consume = async () => {
    try {
        const connection = await amqp.connect(uri);
        let channel = await connection.createChannel();

        let q = await channel.assertQueue(QUEUE_NAME, { exclusive: false, durable: false });
        await channel.bindQueue(q.queue, "amq.direct");

        await channel.consume("", data => {
            let content = JSON.parse(data.content)

            console.log(content.data);

            // let _id = content.data._id ?? ""
            // let image = content.data.image

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