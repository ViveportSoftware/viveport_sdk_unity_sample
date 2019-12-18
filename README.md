# Viveport SDK Unity Sample

This is a demo of Viveport SDK. You can go through it to know how to use Viveport SDK.

## Getting Started

* Open an new Unity project after 2017.4.10f version
* Add unity Asset Store  SteamVR Plugin 2.5.0    https://assetstore.unity.com/packages/tools/integration/steamvr-plugin-32647
* Add Unity Asset Store  VIVE Input Utility 1.10.6  https://assetstore.unity.com/packages/tools/integration/vive-input-utility-64219
* Add Viveport SDK 1.7.16  https://developer.viveport.com/documents/sdk/en/download.html
* Add this Sample Unity Package 
* Open Scene in Viveport_SDK_Sample/Scene/Sample
* Click ViveportSDKManager and change ViveportSDK_Sample_TopApi Component Your VIVEPORT_ID and VIVEPORT_KEY. 
( How to get a VIVEPORT ID and VIVEPORT key  https://developer.viveport.com/documents/sdk/en/api_drm.html)

* If you want to test IAP API , you should change ViveportSDK_Sample_IAP Component Your VIVEPORT_API_KEY.
( How to get a VIVEPORT API key https://developer.viveport.com/documents/sdk/en/api_iap.html)

* If you want to test Stats & Achievements API , you should add Stats & Achievement in VIVEPORT Developer(https://developer.viveport.com/console/). In this Demo, Stat is "SampleStats"; and Achievements is "SampleAchievements". Please refer to this link's instruction (https://developer.viveport.com/documents/sdk/en/api_stats.html) to add Stat "SampleStats" and Achievements "SampleAchievements".

* If you want to test Leaderboards API , you should add Leaderboards in VIVEPORT Developer(https://developer.viveport.com/console/). In this Demo, Leaderboards is "SampleLeadervoard". Please refer to this link's instruction(https://developer.viveport.com/documents/sdk/en/api_leaderboard.html) to add Leaderboard "SampleLeadervoard".

## Sample Usage

* DRM API will be triggered automatically after you start to play sample scene.
* You can click the buttons with corresponding API names to test Viveport SDK API as the following screenshots.
![image](https://github.com/ViveportSoftware/viveport_sdk_unity_sample/blob/sdk_master/samplePic/all.jpg?raw=true)
