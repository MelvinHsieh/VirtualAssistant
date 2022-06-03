const AMQP = require('amqplib');
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
        const connection = await AMQP.connect(URI, opt);
        let channel = await connection.createChannel();

        await channel.assertExchange(EXCHANGE_NAME, 'direct')

        let q = await channel.assertQueue(QUEUE_NAME, { exclusive: false, durable: true });
        await channel.bindQueue(q.queue, EXCHANGE_NAME, "");

        await channel.consume("", data => {

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