package plugintest.albert.mylibrary;

import android.util.Log;

public class AndroidLog {
    public static String Tag="Unity";
    public static void V(String message){
        Log.v(Tag,message);
    }
    public static void D(String message){
        Log.d(Tag,message);
    }
    public static void I(String message){
        Log.i(Tag,message);
    }
    public static void W(String message){
        Log.w(Tag,message);
    }
    public static void E(String message){
        Log.e(Tag,message);
    }
}
