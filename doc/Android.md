# How to Wirelessly Transfer Unity Apps to Android Devices via Wi-Fi from Mac (Build & Run)

## Prerequisites
* Your Mac and Android device must be connected to the **same Wi-Fi network**.
* This process uses **Wi-Fi (Wireless ADB connection)** instead of Bluetooth.

---

## 🛠️ Step 1: Prepare Your Android Device

1. **Enable Developer Options**
   * Open **Settings** > **About phone** (or **About device**) on your phone.
   * Scroll to the bottom and tap **Build number** 7 times consecutively (enter your lock screen PIN/pattern if prompted).
   * *Note: On some brands like Xiaomi/POCO, tap "OS Version" or "MIUI Version"; on Samsung, go to "Software information" first.*

2. **Enable Debugging Features**
   * Go back to **Settings** > **System** > **Developer options** (or **Additional settings** > **Developer options**).
   * Turn ON **USB debugging**.
   * Turn ON **Wireless debugging**.

3. **Check Connection Information**
   * Tap the **text label** of "Wireless debugging" to open its detailed settings screen.
   * Note down the **IP address and port** displayed on the screen (e.g., `192.168.1.5:43215`).
   * ⚠️ **Note**: Do **not** close this screen. The 5-digit port number changes dynamically every time you exit this menu or cycle Wi-Fi. Keep this screen open and proceed to Step 2.

---

## 💻 Step 2: Connect via Mac Terminal

1. **Open Terminal**
   * Open the **Terminal** app on your Mac (Finder > Applications > Utilities > Terminal).

2. **Navigate to the ADB Directory**
   * Copy and paste the following command, then press `Enter`:
     ```bash
     cd ~/Library/Android/sdk/platform-tools/
     ```

3. **Connect to Android Wirelessly**
   * Run the following command using the IP address and port you noted in Step 1:
     ```bash
     ./adb connect 192.168.1.5:43215
     ```
     * *Replace `192.168.1.5:43215` with the exact numbers shown on your Android screen.*

4. **Allow Debugging on the Phone**
   * A pop-up asking "Allow wireless debugging on this network?" will appear on your device. Tap **Always allow** or **Allow**.
   * Connection is successful when the Terminal displays `connected to ~`.

---

## 🎮 Step 3: Build & Run from Unity

1. **Open Build Settings**
   * In the Unity Editor, go to **File** > **Build Settings**.
   * Ensure that the **Platform** is set to **Android**.

2. **Select Your Device**
   * Click the drop-down menu next to **Run Device**.
   * Select your connected **Android Device** (or the corresponding IP address) from the list.
   * 💡 **Hint**: If your device is not listed, click the **Refresh** button on the right.

3. **Execute Build and Run**
   * Click **Build And Run**.
   * Choose a save destination for your APK file. Unity will compile the project, then automatically transfer and launch the app on your phone over Wi-Fi.
