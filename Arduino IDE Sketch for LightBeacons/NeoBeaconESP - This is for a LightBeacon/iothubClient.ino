static WiFiClientSecure sslClient; // for ESP8266

const char *onSuccess = "\"Successfully invoke device method\"";
const char *notFound = "\"No method found\"";

#ifdef AzureIoTHubVersion
static AzureIoTHubClient iotHubClient;
void initIoThubClient()
{
    iotHubClient.begin(sslClient);
}
#else
static AzureIoTHubClient iotHubClient(sslClient);
void initIoThubClient()
{
    iotHubClient.begin();
}
#endif

static void sendCallback(IOTHUB_CLIENT_CONFIRMATION_RESULT result, void *userContextCallback)
{
    if (IOTHUB_CLIENT_CONFIRMATION_OK == result)
    {
        Serial.println("Message sent to Azure IoT Hub");
        blinkLED();
    }
    else
    {
        Serial.println("Failed to send message to Azure IoT Hub");
    }
    messagePending = false;
}

static void sendMessage(IOTHUB_CLIENT_LL_HANDLE iotHubClientHandle, char *buffer, bool temperatureAlert)
{
    IOTHUB_MESSAGE_HANDLE messageHandle = IoTHubMessage_CreateFromByteArray((const unsigned char *)buffer, strlen(buffer));
    if (messageHandle == NULL)
    {
        Serial.println("unable to create a new IoTHubMessage");
    }
    else
    {
        MAP_HANDLE properties = IoTHubMessage_Properties(messageHandle);
        Map_Add(properties, "temperatureAlert", temperatureAlert ? "true" : "false");
        Serial.print("Sending message: ");
        Serial.println(buffer);
        if (IoTHubClient_LL_SendEventAsync(iotHubClientHandle, messageHandle, sendCallback, NULL) != IOTHUB_CLIENT_OK)
        {
            Serial.println("Failed to hand over the message to IoTHubClient");
        }
        else
        {
            messagePending = true;
            Serial.println("IoTHubClient accepted the message for delivery");
        }

        IoTHubMessage_Destroy(messageHandle);
    }
}

void start()
{
    Serial.println("Start sending temperature and humidity data");
    messageSending = true;
}

void clear()
{
  clearScreen();
  initLoadAnim();
}

void stop()
{
    Serial.println("Stop sending temperature and humidity data");
    messageSending = false;
}

IOTHUBMESSAGE_DISPOSITION_RESULT receiveMessageCallback(IOTHUB_MESSAGE_HANDLE message, void *userContextCallback)
{
    IOTHUBMESSAGE_DISPOSITION_RESULT result;
    const unsigned char *buffer;
    size_t size;
    if (IoTHubMessage_GetByteArray(message, &buffer, &size) != IOTHUB_MESSAGE_OK)
    {
       Serial.println("unable to IoTHubMessage_GetByteArray");
        result = IOTHUBMESSAGE_REJECTED;
    }
    else
    {
        /*buffer is not zero terminated*/
        char *temp = (char *)malloc(size + 1);

        if (temp == NULL)
        {
            return IOTHUBMESSAGE_ABANDONED;
        }

        strncpy(temp, (const char *)buffer, size);
        temp[size] = '\0';
        Serial.print("Receive C2D message: ");
        Serial.println(temp);
        free(temp);
        blinkLED();
    }
    return IOTHUBMESSAGE_ACCEPTED;
}

int deviceMethodCallback(const char *methodName, const unsigned char *payload, size_t size, unsigned char **response, size_t *response_size, void *userContextCallback)
{
    Serial.print("Trying to invoke method ");
    Serial.println(methodName);
    const char *responseMessage = onSuccess;
    int result = 200;

    if (strcmp(methodName, "start_sensors") == 0)
    {
        start();
    }
    else if (strcmp(methodName, "stop_sensors") == 0)
    {
        stop();
    }
    else if (strcmp(methodName, "all_clear") == 0)
    {
        clear();
    }
    else if (strcmp(methodName, "new_tail_color") ==0)
    {
      clear();
      DrawRandomTail();
    }
    else if (strcmp(methodName, "amber_alert") == 0)
    {
        String t = "Future Use";
        AmberAlert(t,3000);
    }
    else if (strcmp(methodName, "weather_alert") == 0)
    {
        String t = "Future Use";
        WeatherAlert(t,3000);
    }
    else
    {
        Serial.print("Not Found: ");
        Serial.println(methodName);
        responseMessage = notFound;
        result = 404;
    }

    *response_size = strlen(responseMessage);
    *response = (unsigned char *)malloc(*response_size);
    strncpy((char *)(*response), responseMessage, *response_size);

    return result;
}

void twinCallback(
    DEVICE_TWIN_UPDATE_STATE updateState,
    const unsigned char *payLoad,
    size_t size,
    void *userContextCallback)
{
    char *temp = (char *)malloc(size + 1);
    for (int i = 0; i < size; i++)
    {
        temp[i] = (char)(payLoad[i]);
    }
    temp[size] = '\0';
    parseTwinMessage(temp);
    free(temp);
}

