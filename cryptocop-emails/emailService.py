import json
import pika
import requests
from time import sleep

email_address = "EMAILSERVICEGOESHERE" # I used mailgun. Example: https://api.mailgun.net/v3/sandboxa*********.mailgun.org/messages
email_auth = "EMAILAUTHCODE"
email_from = "Who it is from"  # CryptoCop <mailgun@sandboxa*********.mailgun.org>

exchange_name = "topic_create_order"
create_order_routing_key = "create-order"
email_queue_name = "email-queue"
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

def create_template(user_info):
    email_template = f'<h2>Thank you for ordering!</h2><p>We hope you will enjoy our lovely product and don\'t hesitate to contact us if there are any questions.<p>{user_info["fullName"]}</p><p>{user_info["streetName"]} {user_info["houseNumber"]}</p><p>{user_info["city"]}, {user_info["zipCode"]}, {user_info["country"]}</p><p>{user_info["orderDate"]}</p>'
    email_template += '</p><table><thead><tr style="background-color: rgba(155, 155, 155, .2)"><th>Currency</th><th>Unit price</th><th>Quantity</th><th>price</th></tr></thead><tbody>%s</tbody></table>'
    email_template += f'<p><strong>Total Price: </strong>{user_info["totalPrice"]} USD</p>'
    return email_template

def send_simple_message(to, subject, body):
    r = requests.post(
        email_address,
        auth=("api", email_auth),
        data={"from": email_from,
              "to": to,
              "subject": subject,
              "html": body})
    if r.status_code == 200:
         print(f'Email sent')
    else:
        print(f'Email failed to send.')
    


def on_message_recevied(ch, method, properties, data):
    parsed_msg = json.loads(data)
    items_html = ''.join([ '<tr><td>%s</td> <td>%s</td> <td>%s</td> <td>%s</td> </tr>' % (item['productIdentifier'], item['unitPrice'], item['quantity'], item['totalPrice']) for item in parsed_msg['orderItems'] ])
    email_template = create_template(parsed_msg)
    representation = email_template % items_html
    send_simple_message(parsed_msg['email'], 'Successful order!', representation)


channel = connect_to_mb()

channel.exchange_declare(exchange=exchange_name, exchange_type='topic')
result = channel.queue_declare(queue=email_queue_name)
channel.queue_bind(queue=email_queue_name, exchange=exchange_name, routing_key=create_order_routing_key)

queue_name = result.method.queue
channel.basic_consume(queue=queue_name, 
                    auto_ack=True, 
                    on_message_callback=on_message_recevied)

print("Waiting on order")
channel.start_consuming()
channel.close()

