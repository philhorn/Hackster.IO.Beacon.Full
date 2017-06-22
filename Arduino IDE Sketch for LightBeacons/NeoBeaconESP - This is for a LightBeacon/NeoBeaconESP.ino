/**
 * NeoBeaconESP
 * LightBeacon Code
 * Code for ESP Based NeoBeacon
 * TODO: LICENSE STUFF - CODE CLEANUP - OLED - NFC - AUDIO
 * 
 * -- @SysOpsRalf A.K.A. Ninja
 *
 *
 * @license The Unlicense, http://unlicense.org/
 * @version 0.1
 * @author  @SysOpsRalf A.K.A Ninja 
 * @updated 2017-06-21
 * @link    www.dreamitsystems.com
 *
 *
 */
#include <ESP8266WiFi.h>
#include <WiFiClientSecure.h>
#include <WiFiUdp.h>
#include <WiFiManager.h>          //https://github.com/tzapu/WiFiManager    
#include <DNSServer.h>
#include <NTPClient.h>
#include <ESP8266WebServer.h>
#include <time.h>
#include <sys/time.h>   
#include <NeoPixelBus.h> //LED RING LIB
#include <NeoPixelAnimator.h> //LED RING LIB
#include <Wire.h>
#include "config.h"
#include <AzureIoTHub.h>
#include <AzureIoTUtility.h>
#include <AzureIoTProtocol_MQTT.h>
//#include <U8g2lib.h> - Future Use - OLED Library

// SETTINGS FOR AZURE ///////////////////////////////////////////////////////////////////////////////////
static bool messagePending = false;
static bool messageSending = false; // Setting to true will send data on start
static char *connectionString; // Stored in EEPROM
static char *ssid; // Legacy
static char *pass; // Legacy
static int interval = INTERVAL; // Rate at which to probe for sensor data
static IOTHUB_CLIENT_LL_HANDLE iotHubClientHandle;
// END AZURE STUFF //////////////////////////////////////////////////////////////////////////////////////

// GRAPHICS STUFF //////////////////////////////////////////////////////////////////////////////////////
const uint16_t PixelCount = 24; // Led Count on Ring
const uint16_t PixelPin = 2;  // make sure to set this to the correct pin, ignored for Esp8266
const uint16_t AnimCount = 2; 
const uint16_t TailLength = 6; // length of the tail, must be shorter than PixelCount
const float MaxLightness = 0.4f; // max lightness at the head of the tail (0.5f is full bright)
uint16_t frontPixel = 0;  // the front of the loop
RgbColor frontColor;  // the color at the front of the loop
NeoGamma<NeoGammaTableMethod> colorGamma; // for any fade animations, best to correct gamma
NeoPixelBus<NeoGrbFeature, Neo800KbpsMethod> strip(PixelCount);
NeoPixelAnimator animations(AnimCount); // NeoPixel animation management object
const uint16_t PixelFadeDuration = 300; // third of a second
// one second divide by the number of pixels = loop once a second
const uint16_t NextPixelMoveDuration = 1000 / PixelCount; // how fast we move through the pixels
struct MyAnimationState
{
    RgbColor StartingColor;
    RgbColor EndingColor;
    uint16_t IndexPixel; // which pixel this animation is effecting
};
MyAnimationState animationState[AnimCount];

void SetRandomSeed()
{
    uint32_t seed;

    // random works best with a seed that can use 31 bits
    // analogRead on a unconnected pin tends toward less than four bits
    seed = analogRead(0);
    delay(1);

    for (int shifts = 3; shifts < 31; shifts += 3)
    {
        seed ^= analogRead(0) << shifts;
        delay(1);
    }

    // Serial.println(seed);
    randomSeed(seed);
}
void FadeOutAnimUpdate(const AnimationParam& param)
{
    // this gets called for each animation on every time step
    // progress will start at 0.0 and end at 1.0
    // we use the blend function on the RgbColor to mix
    // color based on the progress given to us in the animation
    RgbColor updatedColor = RgbColor::LinearBlend(
        animationState[param.index].StartingColor,
        animationState[param.index].EndingColor,
        param.progress);
    // apply the color to the strip
    strip.SetPixelColor(animationState[param.index].IndexPixel, 
        colorGamma.Correct(updatedColor));
}
void LoopAnimUpdate2(const AnimationParam& param)
{
    // wait for this animation to complete,
    // we are using it as a timer of sorts
    if (param.state == AnimationState_Completed)
    {
        // done, time to restart this position tracking animation/timer
        animations.RestartAnimation(param.index);

        // pick the next pixel inline to start animating
        // 
        frontPixel = (frontPixel + 1) % PixelCount; // increment and wrap
        if (frontPixel == 0)
        {
            // we looped, lets pick a new front color
            frontColor = HslColor(random(360) / 360.0f, 1.0f, 0.25f);
        }

        uint16_t indexAnim;
        // do we have an animation available to use to animate the next front pixel?
        // if you see skipping, then either you are going to fast or need to increase
        // the number of animation channels
        if (animations.NextAvailableAnimation(&indexAnim, 1))
        {
            animationState[indexAnim].StartingColor = frontColor;
            animationState[indexAnim].EndingColor = RgbColor(0, 0, 0);
            animationState[indexAnim].IndexPixel = frontPixel;

            animations.StartAnimation(indexAnim, PixelFadeDuration, FadeOutAnimUpdate);
        }
    }
}

void LoopAnimUpdate(const AnimationParam& param)
{
    // wait for this animation to complete,
    // we are using it as a timer of sorts
    if (param.state == AnimationState_Completed)
    {
        // done, time to restart this position tracking animation/timer
        animations.RestartAnimation(param.index);

        // rotate the complete strip one pixel to the right on every update
        strip.RotateRight(1);
    }
}

void DrawRandomTailPixels()
{
    float hue = random(360) / 360.0f;
    for (uint16_t index = 0; index < strip.PixelCount() && index <= TailLength; index++)
    {
        float lightness = index * MaxLightness / TailLength;
        RgbColor color = HslColor(hue, 1.0f, lightness);

        strip.SetPixelColor(index, colorGamma.Correct(color));
    }
    strip.Show();
}
void DrawTailPixels()
{
    float hue = 137 / 360.0f;
    for (uint16_t index = 0; index < strip.PixelCount() && index <= TailLength; index++)
    {
        float lightness = index * MaxLightness / TailLength;
        RgbColor color = HslColor(hue, 1.0f, lightness);

        strip.SetPixelColor(index, colorGamma.Correct(color));
    }
}
void DrawWeatherAlertPixels()
{
    RgbColor color_lightning = RgbColor(0,255,230);
    RgbColor color_red = RgbColor(255,10,11);
    RgbColor color_white = RgbColor(255,255,255);
    
    for (uint16_t index = 0; index < strip.PixelCount(); index++)
    {
       
        if(index % 2== 0){
          //set even leds on
        strip.SetPixelColor(index, colorGamma.Correct(color_lightning));
        }
    }
}

void DrawAmberTailPixels()
{
    
    float hue = 53 / 360.0f;
    for (uint16_t index = 0; index < strip.PixelCount() && index <= TailLength; index++)
    {
        float lightness = index * MaxLightness / TailLength;
        RgbColor color = HslColor(hue, 1.0f, lightness);

        strip.SetPixelColor(index, colorGamma.Correct(color));
    }
}
// END GRAPHICS STUFF /////////////////////////////////////////////////////////////////
//for LED status on WifiManager Boot
#include <Ticker.h>
Ticker ticker;
int prev_state;
void tick()
{
  //toggle state
  int state = digitalRead(BUILTIN_LED);  // get the current state of GPIO1 pin
  digitalWrite(BUILTIN_LED, !state);     // set pin to the opposite state
  if(state == 1){
    RgbColor color_red = RgbColor(255,0,0);
    strip.ClearTo(colorGamma.Correct(color_red));
    strip.Show();
  }else{
    clearScreen();
  }
  prev_state = state;
}

//gets called when WiFiManager enters configuration mode
void configModeCallback (WiFiManager *myWiFiManager) {
  Serial.println("Entered config mode");
  Serial.println(WiFi.softAPIP());
  //if you used auto generated SSID, print it
  Serial.println(myWiFiManager->getConfigPortalSSID());
 
  //entered config mode, make led toggle faster
  ticker.attach(0.2, tick);
}

//clears the oled screen
void clearScreen(){
  RgbColor color_off = RgbColor(0,0,0);
  strip.ClearTo(colorGamma.Correct(color_off));
  strip.Show();
}

void initRing(){
  strip.Begin();
  strip.Show();
}

void initLED(){
  pinMode(BUILTIN_LED, OUTPUT);
}

void blinkLED()
{
    digitalWrite(LED_PIN, HIGH);
    delay(500);
    digitalWrite(LED_PIN, LOW);
}

void initWIFI(){
  WiFiManager wifiManager;
  //wifiManager.resetSettings();
  ticker.attach(0.6, tick);

  wifiManager.setAPCallback(configModeCallback);
  if (!wifiManager.autoConnect("BeaconSetup")) {
    Serial.println("failed to connect and hit timeout");
    //reset and try again, or maybe put it to deep sleep
    ESP.reset();
    delay(1000);
  }
  Serial.println("Connected!");
  ticker.detach(); 
  digitalWrite(BUILTIN_LED, LOW);
  clearScreen();
   
}
void DrawRandomTail(){
  clearScreen();
  DrawRandomTailPixels();
}
void AmberAlert(String details, int duration){
    clearScreen();
    //get the users attention!
    RgbColor color_amber = RgbColor(249,193,11);
    RgbColor color_red = RgbColor(255,10,11);
    RgbColor color_white = RgbColor(255,255,255);
    RgbColor color_blue = RgbColor(0,0,255);
    strip.ClearTo(colorGamma.Correct(color_white));
    strip.Show();
    delay(100);
    strip.ClearTo(colorGamma.Correct(color_red));
    strip.Show();
    delay(100);
    strip.ClearTo(colorGamma.Correct(color_white));
    strip.Show();
    delay(100);
    strip.ClearTo(colorGamma.Correct(color_blue));
    strip.Show();
    delay(100);
    strip.ClearTo(colorGamma.Correct(color_white));
    strip.Show();
    delay(100);
    strip.ClearTo(colorGamma.Correct(color_red));
    strip.Show();
    strip.ClearTo(colorGamma.Correct(color_white));
    strip.Show();
    delay(100);
    strip.ClearTo(colorGamma.Correct(color_red));
    strip.Show();
    delay(100);
    strip.ClearTo(colorGamma.Correct(color_white));
    strip.Show();
    delay(100);
    strip.ClearTo(colorGamma.Correct(color_blue));
    strip.Show();
    delay(100);
    strip.ClearTo(colorGamma.Correct(color_white));
    strip.Show();
    delay(100);
    strip.ClearTo(colorGamma.Correct(color_red));
    strip.Show();
    delay(100);
    strip.ClearTo(colorGamma.Correct(color_white));
    strip.Show();
    delay(100);
    strip.ClearTo(colorGamma.Correct(color_red));
    strip.Show();
    delay(100);
    strip.ClearTo(colorGamma.Correct(color_white));
    strip.Show();
    delay(100);
    strip.ClearTo(colorGamma.Correct(color_blue));
    strip.Show();
    delay(100);
    strip.ClearTo(colorGamma.Correct(color_white));
    strip.Show();
    delay(100);
    strip.ClearTo(colorGamma.Correct(color_red));
    strip.Show();
    delay(100);
    strip.ClearTo(colorGamma.Correct(color_amber));
    strip.Show();
    DrawAmberTailPixels();
    animations.StartAnimation(1, NextPixelMoveDuration, LoopAnimUpdate2); 
    
}
void WeatherAlert(String details, int duration){
    clearScreen();
    //get the users attention!
    RgbColor color_lightning = RgbColor(0,255,230);
    RgbColor color_red = RgbColor(255,10,11);
    RgbColor color_white = RgbColor(255,255,255);
    RgbColor color_blue = RgbColor(0,0,255);
    strip.ClearTo(colorGamma.Correct(color_lightning));
    strip.Show();
    delay(300);
    clearScreen();
    strip.ClearTo(colorGamma.Correct(color_white));
    strip.Show();
    delay(230);
    strip.ClearTo(colorGamma.Correct(color_white));
    strip.Show();
    delay(100);
    strip.ClearTo(colorGamma.Correct(color_lightning));
    strip.Show();
    delay(100);
    strip.ClearTo(colorGamma.Correct(color_white));
    strip.Show();
    delay(100);
    strip.ClearTo(colorGamma.Correct(color_blue));
    strip.Show();
    delay(23);
    strip.ClearTo(colorGamma.Correct(color_white));
    strip.Show();
    delay(100);
    strip.ClearTo(colorGamma.Correct(color_lightning));
    strip.Show();
    delay(400);
    strip.ClearTo(colorGamma.Correct(color_white));
    strip.Show();
    delay(100);
    strip.ClearTo(colorGamma.Correct(color_blue));
    strip.Show();
    delay(240);
    strip.ClearTo(colorGamma.Correct(color_white));
    strip.Show();
    delay(300);
    strip.ClearTo(colorGamma.Correct(color_lightning));
    strip.Show();
    delay(100);
    strip.ClearTo(colorGamma.Correct(color_white));
    strip.Show();
    delay(100);
    strip.ClearTo(colorGamma.Correct(color_blue));
    strip.Show();
    delay(120);
    strip.ClearTo(colorGamma.Correct(color_white));
    strip.Show();
    delay(100);
    strip.ClearTo(colorGamma.Correct(color_blue));
    strip.Show();
    delay(130);
    strip.ClearTo(colorGamma.Correct(color_white));
    strip.Show();
    delay(190);
    strip.ClearTo(colorGamma.Correct(color_lightning));
    strip.Show();
    delay(100);
    strip.ClearTo(colorGamma.Correct(color_blue));
    strip.Show();
    DrawWeatherAlertPixels();
    animations.StartAnimation(1, NextPixelMoveDuration, LoopAnimUpdate2); 
    
}
void initLoadAnim(){
  DrawTailPixels();
  animations.StartAnimation(0, 66, LoopAnimUpdate); 
}
void setup() {
  Serial.begin(115200);
  readCredentials();
  SetRandomSeed();
  initRing();
  initLED();
  initWIFI();
  initLoadAnim();
  Serial.println("Anime Done. Load Time");
  initTime();
  Serial.println("Time Done. Load Sensor");
  initSensor();
  yield();
  Serial.println("Sensor Done. Load IOT");
  initIoThubClient();
  Serial.println("IOT Done. Have Fun!");
  yield();
    iotHubClientHandle = IoTHubClient_LL_CreateFromConnectionString(connectionString, MQTT_Protocol);
    if (iotHubClientHandle == NULL)
    {
        Serial.println("Failed on IoTHubClient_CreateFromConnectionString");
        while (1);
    }

    IoTHubClient_LL_SetMessageCallback(iotHubClientHandle, receiveMessageCallback, NULL);
    IoTHubClient_LL_SetDeviceMethodCallback(iotHubClientHandle, deviceMethodCallback, NULL);
    IoTHubClient_LL_SetDeviceTwinCallback(iotHubClientHandle, twinCallback, NULL);
}

static int messageCount = 1;
void DoAzure() {
   if (!messagePending && messageSending)
    {
        char messagePayload[MESSAGE_MAX_LEN];
        bool temperatureAlert = readMessage(messageCount, messagePayload);
        sendMessage(iotHubClientHandle, messagePayload, temperatureAlert);
        messageCount++;
        //delay(interval);
    }
    IoTHubClient_LL_DoWork(iotHubClientHandle);
    delay(10);
}
void loop() {
  animations.UpdateAnimations();
  strip.Show();
  DoAzure();
}

// NET STUFFZ ////

// Pause for a 1 minute
void wait() {
  Serial.println("Wait 60 seconds");
  delay(60000);
}

void initTime()
{
    time_t epochTime;
    configTime(0, 0, "pool.ntp.org", "time.nist.gov");

    while (true)
    {
        epochTime = time(NULL);

        if (epochTime == 0)
        {
            LogInfo("Fetching NTP epoch time failed! Waiting 2 seconds to retry.");
            delay(2000);
        }
        else
        {
            LogInfo("Fetched NTP epoch time is: %lu", epochTime);
            break;
        }
    }
}



