const AMQP = require('amqp-connection-manager');
const URI = process.env.MESSAGE_BUS_URI
const USERNAME = process.env.MESSAGE_BUS_USERNAME
const PASSWORD = process.env.MESSAGE_BUS_PASSWORD
const EXCHANGE_NAME = process.env.MESSAGE_BUS_EXCHANGE_NAME2
const QUEUE_NAME = process.env.MESSAGE_BUS_QUEUE_NAME2

var mongoose = require('mongoose');

var ErrorLog = mongoose.model('ErrorLog');

const consume = async () => {
    try {
        const opt = { credentials: require('amqplib').credentials.plain(USERNAME, PASSWORD) };
        const connection = await AMQP.connect([URI], { connectionOptions: opt });

        var channelWrapper = connection.createChannel({
            json: true,
            setup: function (channel) {
                channel.assertExchange(EXCHANGE_NAME, 'direct')
                return channel.assertQueue(QUEUE_NAME, { exclusive: false, durable: true });
            },
        });

        channelWrapper.bindQueue(QUEUE_NAME, EXCHANGE_NAME, "")

        await channelWrapper.consume("", data => {

            let content = JSON.parse(data.content)

            let service = content.ServiceName ?? "";
            let message = content.ErrorMessage ?? "";

            let errorLog = ErrorLog({
                _id: new mongoose.Types.ObjectId(),
                service: service,
                error: message,
            })

            errorLog.save(function (err, result) {
                if (result) {
                    channel.ack(data)
                } else if (err) {
                    throw err;
                }
            })

        }, {
            noAck: false
        });

    } catch (error) {
        console.log(`error is: ${error}`);
    }
}


module.exports = consume;