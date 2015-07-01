Windows 10 IoT Core Speech Synthesis
------------------------------------

As you already know, Microsoft has release a new Windows 10 IoT Core build last week. One of the major addition in case Raspberry Pi was Audio Output (I was expecting Audio Input to try Speech Recognition, with still Audio Input is not supported in Raspberry Pi, but it is coming). Last week I tried, Text to Speech using SpeechSynthesizer, it was really simple and easy, of course and it was fun too.

Here is a project I created last week, it uses SpeechSynthesizer to speak out the text. I an also using PubNub in this project because you can send the command to speak the text from outside world. The application listens for incoming PubNub messages on the channel *`rpipb-vmsg`*. When ever a message us received, the app speak out the text. The message should be in the form of *`{ "speak": <<text to speak>> }`*. 

I have created a simple web application to send command to Raspberry Pi. The web app uses PubNub JavaScript library to send message.

**Screenshots**



**Demo Video**