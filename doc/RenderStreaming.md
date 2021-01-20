# Render Streaming

<img src="/doc/render_streaming.jpg" width=500>

This project tests [Unity Render Streaming](https://docs.unity3d.com/Packages/com.unity.renderstreaming@2.0/manual/index.html).

## Code

Requirements: "Standard Assets" from Unity

=> [code](../RenderStreaming)

## HTML5 dump

Two video elements are in the HTML5 code:

<img src="/doc/render_streaming_html5.jpg" width=800>

## Settings on RenderStreaming

Since my PC is not equipped with NVIDIA GPU, I have uncheked "Hardware Encode Support" in the cyan frame.

Button events from the browser can be distributed to my custom functions by modifying the settings in the yellow frame.

<img src="/doc/render_streaming_settings.jpg" width=800>

## Code reading

### JavaScript part of <div id="player"></div>

https://github.com/Unity-Technologies/UnityRenderStreaming/blob/release/2.2.2/WebApp/public/scripts/app.js

### Video player

https://github.com/Unity-Technologies/UnityRenderStreaming/blob/release/2.2.2/WebApp/public/scripts/video-player.js

### Handling input events

https://github.com/Unity-Technologies/UnityRenderStreaming/blob/release/2.2.2/WebApp/public/scripts/register-events.js

### Handling gamepad events

I have confirmed this code works with my Logicool F310

https://github.com/Unity-Technologies/UnityRenderStreaming/blob/release/2.2.2/WebApp/public/scripts/gamepadEvents.js
