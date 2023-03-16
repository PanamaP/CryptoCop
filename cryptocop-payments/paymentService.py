import json
import pika
from time import sleep

exchange_name = "topic_create_order"
create_order_routing_key = "create-order"
payment_queue_name = "payment-queue"
host_connect = "rabbitmq" #rabbitmq

def connect_to_mb():
    error = False
    while not error:
        try:
            connection_parameters = pika.ConnectionParameters(host_connect)
            connection = pika.BlockingConnection(connection_parameters)
            channel = connection.channel()
            return channel
        except:
            sleep(5)
            continue


def luhn_checksum(card_number):
    def digits_of(n):
        return [int(d) for d in str(n)]
    digits = digits_of(card_number)
    odd_digits = digits[-1::-2]
    even_digits = digits[-2::-2]
    checksum = 0
    checksum += sum(odd_digits)
    for d in even_digits:
        checksum += sum(digits_of(d*2))
    return checksum % 10

def is_card_valid(card_number):
    return luhn_checksum(card_number) == 0

def on_message_recevied(ch, method, properties, data):
    parsed_msg = json.loads(data)
    card_number = parsed_msg['creditCard']
    if is_card_valid(card_number):
        print("Card Number Is Valid")
    else:
        print("Card Number IS NOT Valid!")
    #is_card_valid(body['creditCard'])




channel = connect_to_mb()


channel.exchange_declare(exchange=exchange_name, exchange_type='topic')
result = channel.queue_declare(queue=payment_queue_name)
channel.queue_bind(queue=payment_queue_name, exchange=exchange_name, routing_key=create_order_routing_key)

queue_name = result.method.queue
channel.basic_consume(queue=queue_name, 
                    auto_ack=True, 
                    on_message_callback=on_message_recevied)

print("Waiting on payment")
channel.start_consuming()
channel.close()

