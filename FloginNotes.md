# Notes for FLogin Tutorial 


## Facebook Login Tutorial Link 
	https://www.androidtutorialpoint.com/material-design/android-facebook-login-tutorial/

 * FacebookFragment.java has 
 	import com.facebook.FacebookSdk;
	import com.facebook.appevents.AppEventsLogger;
 * build.gradle (Module app ) has 
 	repositories {
   	 mavenCentral()
	}

## Package name is in AndroidManifest.xml
	<manifest xmlns:android="http://schemas.android.com/apk/res/android"
    	package="com.androidtutorialpoint.flogin" >
## Main Activity Class is also
 * The field should be your main activity (associated with MAIN and LAUNCHER)
 
 ## Generate an Android Key Hash
 	Navigate in the terminal to the directory where your Android debug.keystore is stored. (library/android/avd) 
		cd ~/.android/
		Note: cmd shift G to open finder
	Mostly it will located under ?/Users/user_name/.android/? (In Windows will be C:\Documents and Settings\.android).
	Once you are in the ?.android? directory, run the following command.

	keytool -exportcert -alias androiddebugkey -keystore debug.keystore | openssl sha1 -binary | openssl base64
	When it prompts you for a password, type android and hit Enter