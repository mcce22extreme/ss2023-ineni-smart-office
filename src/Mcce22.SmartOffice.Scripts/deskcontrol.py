# Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
# SPDX-License-Identifier: Apache-2.0.

import AWSIoTPythonSDK.MQTTLib as AWSIoTPyMQTT
import sys
import threading
import time as t
import json
from gpiozero import CPUTemperature 
import subprocess

#Define ENDPOINT, CLIENT_ID, PATH_TO_CERTIFICATE, PATH_TO_PRIVATE_KEY, PATH_TO_AMAZON_ROOT_CA_1, MESSAGE, TOPIC, and RANGE
ENDPOINT = "a4e4i823i87hm-ats.iot.us-east-1.amazonaws.com"
CLIENT_ID = "iot_desk1"  
PATH_TO_AMAZON_ROOT_CA_1 = "/home/admin/Documents/desk_control/aws_resources/root-CA.crt" 
PATH_TO_CERTIFICATE = "/home/admin/Documents/desk_control/aws_resources/iot_deskControl.cert.pem" 
PATH_TO_PRIVATE_KEY = "/home/admin/Documents/desk_control/aws_resources/iot_deskControl.private.key" 
MESSAGE = "Hello World"
TOPIC_DESK = "sdk/deskheight/python"
TOPIC_PICTURE = "sdk/picture/python"
TOPIC_TEMP = "sdk/temperature/python"
RANGE = 20

# Bash script paths
set_desk_script = "/home/admin/Documents/desk_control/bash_scripts/move_desk.sh"
set_picture_script = "/home/admin/Documents/desk_control/bash_scripts/display_picture.sh"

# Callback when connection is accidentally lost.
def on_connection_interrupted(connection, error, **kwargs):
    print("Connection interrupted. error: {}".format(error))


# Callback when an interrupted connection is re-established.
def on_connection_resumed(connection, return_code, session_present, **kwargs):
    print("Connection resumed. return_code: {} session_present: {}".format(return_code, session_present))

    if return_code == mqtt.ConnectReturnCode.ACCEPTED and not session_present:
        print("Session did not persist. Resubscribing to existing topics...")
        resubscribe_future, _ = connection.resubscribe_existing_topics()

        # Cannot synchronously wait for resubscribe result because we're on the connection's event-loop thread,
        # evaluate result with a callback instead.
        resubscribe_future.add_done_callback(on_resubscribe_complete)


def on_resubscribe_complete(resubscribe_future):
    resubscribe_results = resubscribe_future.result()
    print("Resubscribe results: {}".format(resubscribe_results))

    for topic, qos in resubscribe_results['topics']:
        if qos is None:
            sys.exit("Server rejected resubscribe to topic: {}".format(topic))


# Callback function for handling incoming messages
def message_callback(client, userdata, message):
    topic = message.topic
    payload = message.payload.decode('utf-8')
    print("Message received: Topic {}".format(topic))
    print("Message received: PAyload {}".format(payload))
    
    if message.topic == TOPIC_DESK:
        print("Desk heigth set to {}".format(payload))
        # Call set_desk.sh script with desk height parameter
        subprocess.call([set_desk_script, payload])
    elif message.topic == TOPIC_PICTURE:
        # Call set_picture.sh script with picture URL parameter
        subprocess.call([set_picture_script, payload])

#########################################################################
############           Programm Start              ######################
#########################################################################


myAWSIoTMQTTClient = AWSIoTPyMQTT.AWSIoTMQTTClient(CLIENT_ID)
myAWSIoTMQTTClient.configureEndpoint(ENDPOINT, 8883)
myAWSIoTMQTTClient.configureCredentials(PATH_TO_AMAZON_ROOT_CA_1, PATH_TO_PRIVATE_KEY, PATH_TO_CERTIFICATE)

myAWSIoTMQTTClient.connect()

myAWSIoTMQTTClient.subscribe(TOPIC_DESK, 1, message_callback)
myAWSIoTMQTTClient.subscribe(TOPIC_PICTURE, 1, message_callback)

cpu = CPUTemperature()

print('Begin Publish')
for i in range (RANGE):
    MESSAGE = cpu.temperature
    data = "{} [{}]".format(MESSAGE, i+1)
    message = {"message" : data}
    myAWSIoTMQTTClient.publish(TOPIC_TEMP, json.dumps(message), 1) 
    print("Published: '" + json.dumps(message) + "' to the topic: " + "'{}".format(TOPIC_TEMP))
    t.sleep(5)
print('Publish End')
myAWSIoTMQTTClient.disconnect()
