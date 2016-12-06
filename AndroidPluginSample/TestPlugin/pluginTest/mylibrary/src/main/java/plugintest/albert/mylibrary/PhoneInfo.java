package plugintest.albert.mylibrary;

import android.content.Context;
import android.net.wifi.WifiInfo;
import android.net.wifi.WifiManager;
import android.os.Build;
import android.telephony.TelephonyManager;
import android.text.format.Formatter;
import android.util.Log;
import android.provider.Settings;

import java.io.BufferedReader;
import java.io.FileReader;
import java.io.IOException;

import com.unity3d.player.UnityPlayer;

public class PhoneInfo {


    private static String TAG = "PhoneInfo";
    private static Context mContext;
    private static TelephonyManager mPhoneManager;

    public PhoneInfo() {
        mContext = UnityPlayer.currentActivity.getApplicationContext();
        mPhoneManager = (TelephonyManager) mContext.getSystemService(Context.TELEPHONY_SERVICE);
    }


    public static String getDeviceId() {
        String id;
        //android.telephony.TelephonyManager
        id = mPhoneManager.getDeviceId();
        if (id == null) {
            //android.provider.Settings;
            id = Settings.Secure.getString(mContext.getContentResolver(), Settings.Secure.ANDROID_ID);
        }
        return id;
    }

    public static String getPhoneModule() {
        return Build.MODEL;
    }

    public static String getSerialNumber() {
        return Build.SERIAL;
    }

    public static String getPhoneNumber() {
        return mPhoneManager.getLine1Number();
    }

    public static String getMacAddress() {
        String result = "";
        WifiManager wifiManager = (WifiManager) mContext.getSystemService(Context.WIFI_SERVICE);
        WifiInfo wifiInfo = wifiManager.getConnectionInfo();
        result = wifiInfo.getMacAddress();
        Log.i(TAG, "macAdd:" + result);
        return result;
    }

    public static String[] getCpuInfo() {
        String str1 = "/proc/cpuinfo";
        String str2 = "";
        String[] cpuInfo = {"", ""};  //1-cpu型号  //2-cpu频率
        String[] arrayOfString;
        try {
            FileReader fr = new FileReader(str1);
            BufferedReader localBufferedReader = new BufferedReader(fr, 8192);
            str2 = localBufferedReader.readLine();
            arrayOfString = str2.split("\\s+");
            for (int i = 2; i < arrayOfString.length; i++) {
                cpuInfo[0] = cpuInfo[0] + arrayOfString[i] + " ";
            }
            str2 = localBufferedReader.readLine();
            arrayOfString = str2.split("\\s+");
            cpuInfo[1] += arrayOfString[2];
            localBufferedReader.close();
        } catch (IOException e) {
        }
        Log.i(TAG, "cpuinfo:" + cpuInfo[0] + " " + cpuInfo[1]);
        return cpuInfo;
    }

    public static String getTotalMemory() {
        String str1 = "/proc/meminfo";// 系统内存信息文件
        String str2;
        String[] arrayOfString;
        long initial_memory = 0;

        try {
            FileReader localFileReader = new FileReader(str1);
            BufferedReader localBufferedReader = new BufferedReader(
                    localFileReader, 8192);
            str2 = localBufferedReader.readLine();// 读取meminfo第一行，系统总内存大小

            arrayOfString = str2.split("\\s+");
            for (String num : arrayOfString) {
                Log.i(str2, num + "\t");
            }

            initial_memory = Integer.valueOf(arrayOfString[1]).intValue() * 1024;// 获得系统总内存，单位是KB，乘以1024转换为Byte
            localBufferedReader.close();

        } catch (IOException e) {
            Log.i(TAG, "getTotalMemory error:" + e.toString());
        }
        return Formatter.formatFileSize(mContext, initial_memory);// Byte转换为KB或者MB，内存大小规格化
    }
}