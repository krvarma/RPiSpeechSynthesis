using Newtonsoft.Json.Linq;
using PubNubMessaging.Core;
using System;
using System.Diagnostics;
using Windows.Foundation;
using Windows.Media.SpeechRecognition;
using Windows.Media.SpeechSynthesis;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace w10tts
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        // PubNub publish and subscribe keys
        private Pubnub pubnub = new Pubnub(
            "<<publish-key>>", 
            "<<subscribe-key>>");

        private IAsyncOperation<SpeechRecognitionResult> recognitionOperation;

        public MainPage()
        {
            this.InitializeComponent();
            
            SubscribePubNubMessages();

            Speak(
                "Hello there, welcome to Raspberry Pi and Windows 10 IoT Core Speech Synthesise demo. " + 
                "I am ready to accept voice messages. Enjoy!.");
        }

        // Subscribe PubNub messages
        private void SubscribePubNubMessages()
        {
            pubnub.Subscribe<string>(
                "rpipb-vmsg", 
                PubNubSubscribeSuccess, 
                DisplaySubscribeConnectStatusMessage, 
                PubNubError);
        }

        // Receive PubNub messages
        private void PubNubSubscribeSuccess(string publishResult)
        {
            Debug.WriteLine("Message: " + publishResult);

            JArray message = JArray.Parse(publishResult);

            string text = message[0]["speak"].ToString();

            Debug.WriteLine("Text Length: " + text.Length);

            Speak(text);
        }
        
        private void DisplaySubscribeConnectStatusMessage(string publishResult)
        {
            Debug.WriteLine("Connection: " + publishResult);
        }

        // We have some issue sending the message, simply display it on the console
        private void PubNubError(PubnubClientError error)
        {
            Debug.WriteLine("Error: " + error.ToString());
        }

        // Speak the text
        private async void Speak(string text)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
                {
                    _Speak(text);
                }
            );
        }

        // Internal speak method
        private async void _Speak(string text)
        {
            MediaElement mediaElement = new MediaElement();
            SpeechSynthesizer synth = new SpeechSynthesizer();

            foreach (VoiceInformation voice in SpeechSynthesizer.AllVoices)
            {
                Debug.WriteLine(voice.DisplayName + ", " + voice.Description);
            }

            // Initialize a new instance of the SpeechSynthesizer.
            SpeechSynthesisStream stream = await synth.SynthesizeTextToStreamAsync(text);

            // Send the stream to the media object.
            mediaElement.SetSource(stream, stream.ContentType);
            mediaElement.Play();

            mediaElement.Stop();
            synth.Dispose();
        }
    }
}
