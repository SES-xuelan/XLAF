using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// iTween hash builder.
/// <para></para>All params: name|audiosource|volume|pitch|time|delay|easetype|looptype|onstart|onstarttarget|onstartparams|onupdate|onupdatetarget|onupdateparams|oncomplete|oncompletetarget|oncompleteparams|ignoretimescale|amount|color|r|g|b|a|NamedColorValue|includechildren|alpha|looktarget|axis|speed|x|y|z|looktime|space|position|path|movetopath|orienttopath|lookahead|islocal|rotation|scale|audioclip|from|to
/// </summary>
public class iTweenHashBuilder
{
	private  Hashtable hashTable = new Hashtable ();

	/// <summary>
	///  Sets the iTween name.
	/// <para></para>an individual name useful for stopping iTweens by name
	/// </summary>
	/// <param name="name">Name.</param>
	public Hashtable SetName (string name)
	{
		hashTable.Add ("name", name);
		return hashTable;
	}

	/// <summary>
	/// Sets the audio source.
	/// <para></para>for which AudioSource to use
	/// </summary>
	/// <param name="audioSource">Audio source.</param>
	public Hashtable SetAudioSource (AudioSource audioSource)
	{
		hashTable.Add ("audiosource", audioSource);
		return hashTable;
	}

	/// <summary>
	/// Sets the volume.
	/// <para></para>for the target level of volume
	/// </summary>
	/// <param name="volume">Volume.</param>
	public Hashtable SetVolume (float volume)
	{
		hashTable.Add ("volume", volume);
		return hashTable;
	}

	/// <summary>
	/// Sets the volume.
	/// <para></para>for the target level of volume
	/// </summary>
	/// <param name="volume">Volume.</param>
	public Hashtable SetVolume (double volume)
	{
		hashTable.Add ("volume", volume);
		return hashTable;
	}

	/// <summary>
	/// Sets the pitch.
	/// <para></para>for the target pitch
	/// </summary>
	/// <param name="pitch">Pitch.</param>
	public Hashtable SetPitch (float pitch)
	{
		hashTable.Add ("pitch", pitch);
		return hashTable;
	}

	/// <summary>
	/// Sets the pitch.
	/// <para></para>for the target pitch
	/// </summary>
	/// <param name="pitch">Pitch.</param>
	public Hashtable SetPitch (double pitch)
	{
		hashTable.Add ("pitch", pitch);
		return hashTable;
	}

	/// <summary>
	/// Sets the time.
	/// <para></para>for the time in seconds the animation will take to complete
	/// </summary>
	/// <param name="time">Time.</param>
	public Hashtable SetTime (float time)
	{
		hashTable.Add ("time", time);
		return hashTable;
	}

	/// <summary>
	/// Sets the time.
	///<para></para> for the time in seconds the animation will take to complete
	/// </summary>
	/// <param name="time">Time.</param>
	public Hashtable SetTime (double time)
	{
		hashTable.Add ("time", time);
		return hashTable;
	}

	/// <summary>
	/// Sets the delay.
	/// <para></para>for the time in seconds the animation will wait before beginning
	/// </summary>
	/// <param name="delay">Delay.</param>
	public Hashtable SetDelay (float delay)
	{
		hashTable.Add ("delay", delay);
		return hashTable;
	}

	/// <summary>
	/// Sets the delay.
	/// <para></para>for the time in seconds the animation will wait before beginning
	/// </summary>
	/// <param name="delay">Delay.</param>
	public Hashtable SetDelay (double delay)
	{
		hashTable.Add ("delay", delay);
		return hashTable;
	}

	/// <summary>
	/// Sets the type of the ease.
	/// <para></para>for the shape of the easing curve applied to the animation
	/// </summary>
	/// <param name="easeType">Ease type.</param>
	public Hashtable SetEaseType (iTween.EaseType easeType)
	{
		hashTable.Add ("easetype", easeType);
		return hashTable;
	}

	/// <summary>
	/// Sets the type of the loop.
	/// <para></para>for the type of loop to apply once the animation has completed
	/// </summary>
	/// <param name="looptype">Looptype.</param>
	public Hashtable SetLoopType (iTween.LoopType looptype)
	{
		hashTable.Add ("looptype", looptype);
		return hashTable;
	}

	/// <summary>
	/// Sets the onStart callback.
	/// <para></para>a function to launch at the beginning of the animation
	/// </summary>
	/// <param name="action">Action.</param>
	public Hashtable SetOnStart (Action  action)
	{
		hashTable.Add ("onstart", action);
		return hashTable;
	}

	/// <summary>
	/// Sets the onStart callback.
	/// <para></para>a function to launch at the beginning of the animation
	/// </summary>
	/// <param name="action">Action.</param>
	public Hashtable SetOnStart (Action<object>  action)
	{
		hashTable.Add ("onstart", action);
		return hashTable;
	}

	/// <summary>
	/// Sets the onStart callback.
	///<para></para> a function to launch at the beginning of the animation
	/// </summary>
	/// <param name="action">Action.</param>
	public Hashtable SetOnStart (Action<float>  action)
	{
		hashTable.Add ("onstart", action);
		return hashTable;
	}

	/// <summary>
	/// Sets the on start target.
	/// <para></para>for a reference to the GameObject that holds the "onstart" method
	/// </summary>
	/// <param name="go">GameObject</param>
	public Hashtable SetOnStartTarget (GameObject go)
	{
		hashTable.Add ("onstarttarget", go);
		return hashTable;
	}

	/// <summary>
	/// Sets the on start parameters.
	/// <para></para>for arguments to be sent to the "onstart" method
	/// </summary>
	/// <param name="param">Parameter.</param>
	public Hashtable SetOnStartParams (object param)
	{
		hashTable.Add ("onstartparams", param);
		return hashTable;
	}

	/// <summary>
	/// Sets the on update.
	/// <para></para>a function to launch on every step of the animation
	/// </summary>
	/// <param name="action">Action.</param>
	public Hashtable SetOnUpdate (Action action)
	{
		hashTable.Add ("onupdate", action);
		return hashTable;
	}

	/// <summary>
	/// Sets the on update.
	/// <para></para>a function to launch on every step of the animation
	/// </summary>
	/// <param name="action">Action.</param>
	public Hashtable SetOnUpdate (Action<float> action)
	{
		hashTable.Add ("onupdate", action);
		return hashTable;
	}

	/// <summary>
	/// Sets the on update.
	/// <para></para>a function to launch on every step of the animation
	/// </summary>
	/// <param name="action">Action.</param>
	public Hashtable SetOnUpdate (Action<object> action)
	{
		hashTable.Add ("onupdate", action);
		return hashTable;
	}

	/// <summary>
	/// Sets the on start target.
	/// <para></para>for a reference to the GameObject that holds the "onupdate" method
	/// </summary>
	/// <param name="go">GameObject</param>
	public Hashtable SetOnUpdateTarget (GameObject go)
	{
		hashTable.Add ("onupdatetarget", go);
		return hashTable;
	}

	/// <summary>
	/// Sets the on start parameters.
	/// <para></para>for arguments to be sent to the "onupdate" method
	/// </summary>
	/// <param name="param">Parameter.</param>
	public Hashtable SetOnUpdateParams (object param)
	{
		hashTable.Add ("onupdateparams", param);
		return hashTable;
	}

	/// <summary>
	/// Sets the on complete.
	/// <para></para>a function to launch at the end of the animation
	/// </summary>
	/// <param name="action">Action.</param>
	public Hashtable SetOnComplete (Action action)
	{
		hashTable.Add ("oncomplete", action);
		return hashTable;
	}

	/// <summary>
	/// Sets the on complete.
	/// <para></para>a function to launch at the end of the animation
	/// </summary>
	/// <param name="action">Action.</param>
	public Hashtable SetOnComplete (Action<float> action)
	{
		hashTable.Add ("oncomplete", action);
		return hashTable;
	}

	/// <summary>
	/// Sets the on complete.
	/// <para></para>a function to launch at the end of the animation
	/// </summary>
	/// <param name="action">Action.</param>
	public Hashtable SetOnComplete (Action<object> action)
	{
		hashTable.Add ("oncomplete", action);
		return hashTable;
	}

	/// <summary>
	/// Sets the on complete target.
	/// <para></para>for a reference to the GameObject that holds the "oncomplete" method
	/// </summary>
	/// <param name="go">Go.</param>
	public Hashtable SetOnCompleteTarget (GameObject go)
	{
		hashTable.Add ("oncompletetarget", go);
		return hashTable;
	}

	/// <summary>
	/// Sets the on complete parameters.
	/// <para></para>for arguments to be sent to the "oncomplete" method
	/// </summary>
	/// <param name="param">Parameter.</param>
	public Hashtable SetOnCompleteParams (object param)
	{
		hashTable.Add ("oncompleteparams", param);
		return hashTable;
	}

	/// <summary>
	/// Sets the ignore time scale.
	/// <para></para>setting this to true will allow the animation to continue independent of the current time which is helpful for animating menus after a game has been paused by setting Time.timeScale=0
	/// </summary>
	/// <param name="param">If set to <c>true</c> b.</param>
	public Hashtable SetIgnoreTimeScale (bool b)
	{
		hashTable.Add ("ignoretimescale", b);
		return hashTable;
	}

	/// <summary>
	/// Sets the amount.
	/// <para></para>[CameraFadeFrom|CameraFadeTo]for how transparent the Texture2D that the camera fade uses is
	/// </summary>
	/// <param name="amount">Amount.</param>
	public Hashtable SetAmount (float amount)
	{
		hashTable.Add ("amount", amount);
		return hashTable;
	}

	/// <summary>
	/// Sets the amount.
	/// <para></para>[CameraFadeFrom|CameraFadeTo]for how transparent the Texture2D that the camera fade uses is
	/// </summary>
	/// <param name="amount">Amount.</param>
	public Hashtable SetAmount (double amount)
	{
		hashTable.Add ("amount", amount);
		return hashTable;
	}

	/// <summary>
	/// Sets the color.
	/// <para></para>the color to fade the object to
	/// </summary>
	/// <param name="color">Color.</param>
	public Hashtable SetColor (Color color)
	{
		hashTable.Add ("color", color);
		return hashTable;
	}

	/// <summary>
	/// Sets the color r.
	///<para></para> for the individual setting of the color red
	/// </summary>
	/// <param name="r">The red component.</param>
	public Hashtable SetColorR (float r)
	{
		hashTable.Add ("r", r);
		return hashTable;
	}

	/// <summary>
	/// Sets the color r.
	/// <para></para>for the individual setting of the color red
	/// </summary>
	/// <param name="r">The red component.</param>
	public Hashtable SetColorR (double r)
	{
		hashTable.Add ("r", r);
		return hashTable;
	}

	/// <summary>
	/// Sets the color g.
	/// <para></para>for the individual setting of the color green
	/// </summary>
	/// <param name="g">The green component.</param>
	public Hashtable SetColorG (float g)
	{
		hashTable.Add ("g", g);
		return hashTable;
	}

	/// <summary>
	/// Sets the color g.
	///<para></para> for the individual setting of the color green
	/// </summary>
	/// <param name="g">The green component.</param>
	public Hashtable SetColorG (double g)
	{
		hashTable.Add ("g", g);
		return hashTable;
	}

	/// <summary>
	/// Sets the color b.
	///<para></para> for the individual setting of the color blue
	/// </summary>
	/// <param name="b">The blue component.</param>
	public Hashtable SetColorB (float b)
	{
		hashTable.Add ("b", b);
		return hashTable;
	}

	/// <summary>
	/// Sets the color b.
	///<para></para> for the individual setting of the color blue
	/// </summary>
	/// <param name="b">The blue component.</param>
	public Hashtable SetColorB (double b)
	{
		hashTable.Add ("b", b);
		return hashTable;
	}

	/// <summary>
	/// Sets the color a.
	/// <para></para>for the individual setting of the alpha
	/// </summary>
	/// <param name="a">The alpha component.</param>
	public Hashtable SetColorA (float a)
	{
		hashTable.Add ("a", a);
		return hashTable;
	}

	/// <summary>
	/// Sets the color a.
	///<para></para> for the individual setting of the alpha
	/// </summary>
	/// <param name="a">The alpha component.</param>
	public Hashtable SetColorA (double a)
	{
		hashTable.Add ("a", a);
		return hashTable;
	}

	/// <summary>
	/// Sets the named color value.
	///<para></para> for which color of a shader to use. Uses "_Color" by default.
	/// </summary>
	/// <param name="namedValueColor">Named value color.</param>
	public Hashtable SetNamedColorValue (iTween.NamedValueColor namedValueColor)
	{
		hashTable.Add ("namedcolorvalue", namedValueColor);
		return hashTable;
	}

	/// <summary>
	/// Sets the include children.
	/// <para></para>for whether or not to include children of this GameObject. True by default
	/// </summary>
	/// <param name="b">If set to <c>true</c> b.</param>
	public Hashtable SetIncludeChildren (bool b)
	{
		hashTable.Add ("includechildren", b);
		return hashTable;
	}

	/// <summary>
	/// Sets the alpha.
	/// <para></para>for the initial alpha value of the animation.
	/// </summary>
	/// <param name="alpha">Alpha.</param>
	public Hashtable SetAlpha (float alpha)
	{
		hashTable.Add ("alpha", alpha);
		return hashTable;
	}

	/// <summary>
	/// Sets the looktarget.
	/// <para></para>for a target the GameObject will look at.
	/// </summary>
	/// <param name="transform">Transform.</param>
	public Hashtable SetLooktarget (Transform transform)
	{
		hashTable.Add ("looktarget", transform);
		return hashTable;
	}

	/// <summary>
	/// Sets the looktarget.
	/// <para></para>for a target the GameObject will look at.
	/// </summary>
	/// <param name="vector3">Vector3.</param>
	public Hashtable SetLooktarget (Vector3 vector3)
	{
		hashTable.Add ("looktarget", vector3);
		return hashTable;
	}

	/// <summary>
	/// Sets the axis.
	/// <para></para>Restricts rotation to the supplied axis only.
	/// </summary>
	/// <param name="axis">axis include [x  y  z]</param>
	public Hashtable SetAxis (Axis axis)
	{
		hashTable.Add ("axis", axis);
		return hashTable;
	}

	/// <summary>
	/// Sets the speed.
	/// <para></para>can be used instead of time to allow animation based on speed
	/// </summary>
	/// <param name="speed">Speed.</param>
	public Hashtable SetSpeed (float speed)
	{
		hashTable.Add ("speed", speed);
		return hashTable;
	}

	/// <summary>
	/// Sets the speed.
	/// <para></para>can be used instead of time to allow animation based on speed
	/// </summary>
	/// <param name="speed">Speed.</param>
	public Hashtable SetSpeed (double speed)
	{
		hashTable.Add ("speed", speed);
		return hashTable;
	}

	/// <summary>
	/// Sets the amount.
	/// <para></para>[MoveAdd]for a point in space the GameObject will animate to.
	/// </summary>
	/// <param name="amount">Amount.</param>
	public Hashtable SetAmount (Vector3 amount)
	{
		hashTable.Add ("amount", amount);
		return hashTable;
	}

	/// <summary>
	/// Sets the x.
	/// <para></para>for the individual setting of the x axis
	/// </summary>
	/// <param name="x">The x coordinate.</param>
	public Hashtable SetX (float x)
	{
		hashTable.Add ("x", x);
		return hashTable;
	}

	/// <summary>
	/// Sets the x.
	/// <para></para>for the individual setting of the x axis
	/// </summary>
	/// <param name="x">The x coordinate.</param>
	public Hashtable SetX (double x)
	{
		hashTable.Add ("x", x);
		return hashTable;
	}

	/// <summary>
	/// Sets the y.
	///<para></para> for the individual setting of the y axis
	/// </summary>
	/// <param name="y">The y coordinate.</param>
	public Hashtable SetY (float y)
	{
		hashTable.Add ("y", y);
		return hashTable;
	}

	/// <summary>
	/// Sets the y.
	///<para></para> for the individual setting of the y axis
	/// </summary>
	/// <param name="y">The y coordinate.</param>
	public Hashtable SetY (double y)
	{
		hashTable.Add ("y", y);
		return hashTable;
	}

	/// <summary>
	/// Sets the z.
	///  <para></para>for the individual setting of the z axis
	/// </summary>
	/// <param name="z">The z coordinate.</param>
	public Hashtable SetZ (float z)
	{
		hashTable.Add ("z", z);
		return hashTable;
	}

	/// <summary>
	/// Sets the z.
	///<para></para>  for the individual setting of the z axis
	/// </summary>
	/// <param name="z">The z coordinate.</param>
	public Hashtable SetZ (double z)
	{
		hashTable.Add ("z", z);
		return hashTable;
	}

	/// <summary>
	/// Sets the look time.
	///<para></para> for the time in seconds the object will take to look at either the "looktarget" or "orienttopath"
	/// </summary>
	/// <param name="lookTime">Look time.</param>
	public Hashtable SetLookTime (float lookTime)
	{
		hashTable.Add ("looktime", lookTime);
		return hashTable;
	}

	/// <summary>
	/// Sets the space.
	/// <para></para>for applying the transformation in either the world coordinate or local cordinate system. Defaults to local space
	/// </summary>
	/// <param name="space">Space.</param>
	public Hashtable SetSpace (Space space)
	{
		hashTable.Add ("space", space);
		return hashTable;
	}

	/// <summary>
	/// Sets the position.
	/// <para></para>for a point in space the GameObject will animate to.
	/// </summary>
	/// <param name="transform">Transform.</param>
	public Hashtable SetPosition (Transform transform)
	{
		hashTable.Add ("position", transform);
		return hashTable;
	}

	/// <summary>
	/// Sets the position.
	/// <para></para>for a point in space the GameObject will animate to.
	/// </summary>
	/// <param name="vector3">Vector3.</param>
	public Hashtable SetPosition (Vector3 vector3)
	{
		hashTable.Add ("position", vector3);
		return hashTable;
	}

	/// <summary>
	/// Sets the path.
	///<para></para> for a list of points to draw a Catmull-Rom through for a curved animation path
	/// </summary>
	/// <param name="vector3Array">Vector3 array.</param>
	public Hashtable SetPath (Vector3[] vector3Array)
	{
		hashTable.Add ("path", vector3Array);
		return hashTable;
	}

	/// <summary>
	/// Sets the path.
	///<para></para> for a list of points to draw a Catmull-Rom through for a curved animation path
	/// </summary>
	/// <param name="transformArray">transformy array.</param>
	public Hashtable SetPath (Transform[] transformArray)
	{
		hashTable.Add ("path", transformArray);
		return hashTable;
	}

	/// <summary>
	/// Sets the move to path.
	/// <para></para>for whether to automatically generate a curve from the GameObject's current position to the beginning of the path. True by default.
	/// </summary>
	/// <param name="isMovToPath">If set to <c>true</c> is move to path.</param>
	public Hashtable SetMoveToPath (bool isMoveToPath)
	{
		hashTable.Add ("movetopath", isMoveToPath);
		return hashTable;
	}

	/// <summary>
	/// Sets the orient to path.
	/// <para></para>for whether or not the GameObject will orient to its direction of travel. False by default
	/// </summary>
	/// <param name="isOrientToPath">If set to <c>true</c> is orient to path.</param>
	public Hashtable SetOrientToPath (bool isOrientToPath)
	{
		hashTable.Add ("orienttopath", isOrientToPath);
		return hashTable;
	}

	/// <summary>
	/// Sets the look ahead.
	/// <para></para>for how much of a percentage (from 0 to 1) to look ahead on a path to influence how strict "orienttopath" is and how much the object will anticipate each curve
	/// </summary>
	/// <param name="lookAhead">Look ahead.</param>
	public Hashtable SetLookAhead (float lookAhead)
	{
		hashTable.Add ("lookahead", lookAhead);
		return hashTable;
	}

	/// <summary>
	/// Sets the look ahead.
	/// <para></para>for how much of a percentage (from 0 to 1) to look ahead on a path to influence how strict "orienttopath" is and how much the object will anticipate each curve
	/// </summary>
	/// <param name="lookAhead">Look ahead.</param>
	public Hashtable SetLookAhead (double lookAhead)
	{
		hashTable.Add ("lookahead", lookAhead);
		return hashTable;
	}

	/// <summary>
	/// Sets the is local.
	/// <para></para>for whether to animate in world space or relative to the parent. False be default.
	/// </summary>
	/// <param name="isLocal">If set to <c>true</c> is local.</param>
	public Hashtable SetIsLocal (bool isLocal)
	{
		hashTable.Add ("islocal", isLocal);
		return hashTable;
	}

	/// <summary>
	/// Sets the rotation.
	/// <para></para>for the target Euler angles in degrees to rotate to.
	/// </summary>
	/// <param name="transform">Transform.</param>
	public Hashtable SetRotation (Transform transform)
	{
		hashTable.Add ("rotation", transform);
		return hashTable;
	}

	/// <summary>
	/// Sets the rotation.
	///<para></para> for the target Euler angles in degrees to rotate to.
	/// </summary>
	/// <param name="vector3">Vector3.</param>
	public Hashtable SetRotation (Vector3 vector3)
	{
		hashTable.Add ("rotation", vector3);
		return hashTable;
	}

	/// <summary>
	/// Sets the scale.
	///<para></para> for the initial scale
	/// </summary>
	/// <param name="transform">Transform.</param>
	public Hashtable SetScale (Transform transform)
	{
		hashTable.Add ("scale", transform);
		return hashTable;
	}

	/// <summary>
	/// Sets the scale.
	/// <para></para>for the initial scale
	/// </summary>
	/// <param name="vector3">Vector3.</param>
	public Hashtable SetScale (Vector3 vector3)
	{
		hashTable.Add ("scale", vector3);
		return hashTable;
	}

	/// <summary>
	/// Sets the audioclip.
	/// <para></para>for a reference to the AudioClip to be played.
	/// </summary>
	/// <param name="audioClip">Audio clip.</param>
	public Hashtable SetAudioclip (AudioClip audioClip)
	{
		hashTable.Add ("audioclip", audioClip);
		return hashTable;
	}

	/// <summary>
	/// Sets from.
	/// <para></para>[ValueTo ]for the starting value.
	/// </summary>
	/// <param name="from">Value from.</param>
	public Hashtable SetFrom (float from)
	{
		hashTable.Add ("from", from);
		return hashTable;
	}

	/// <summary>
	/// Sets from.
	///<para></para> [ValueTo ]for the starting value.
	/// </summary>
	/// <param name="from">Value from.</param>
	public Hashtable SetFrom (double from)
	{
		hashTable.Add ("from", from);
		return hashTable;
	}

	/// <summary>
	/// Sets from.
	/// <para></para>[ValueTo ]for the starting value.
	/// </summary>
	/// <param name="from">Value from.</param>
	public Hashtable SetFrom (Vector2 from)
	{
		hashTable.Add ("from", from);
		return hashTable;
	}

	/// <summary>
	/// Sets from.
	/// <para></para>[ValueTo ]for the starting value.
	/// </summary>
	/// <param name="from">Value from.</param>
	public Hashtable SetFrom (Vector3 from)
	{
		hashTable.Add ("from", from);
		return hashTable;
	}

	/// <summary>
	/// Sets from.
	/// <para></para>[ValueTo ]for the starting value.
	/// </summary>
	/// <param name="from">Value from.</param>
	public Hashtable SetFrom (Rect from)
	{
		hashTable.Add ("from", from);
		return hashTable;
	}

	/// <summary>
	/// Sets from.
	///<para></para> [ValueTo ]for the starting value.
	/// </summary>
	/// <param name="from">Value from.</param>
	public Hashtable SetFrom (Color from)
	{
		hashTable.Add ("from", from);
		return hashTable;
	}

	/// <summary>
	/// Sets to.
	///<para></para>[ValueTo ]for the ending value.
	/// </summary>
	/// <returns>The to.</returns>
	/// <param name="to">Value to.</param>
	public Hashtable SetTo (float to)
	{
		hashTable.Add ("to", to);
		return hashTable;
	}

	/// <summary>
	/// Sets to.
	/// <para></para>[ValueTo ]for the ending value.
	/// </summary>
	/// <returns>The to.</returns>
	/// <param name="to">Value to.</param>
	public Hashtable SetTo (double to)
	{
		hashTable.Add ("to", to);
		return hashTable;
	}

	/// <summary>
	/// Sets to.
	/// <para></para>[ValueTo ]for the ending value.
	/// </summary>
	/// <returns>The to.</returns>
	/// <param name="to">Value to.</param>
	public Hashtable SetTo (Vector2 to)
	{
		hashTable.Add ("to", to);
		return hashTable;
	}

	/// <summary>
	/// Sets to.
	/// <para></para>[ValueTo ]for the ending value.
	/// </summary>
	/// <returns>The to.</returns>
	/// <param name="to">Value to.</param>
	public Hashtable SetTo (Vector3 to)
	{
		hashTable.Add ("to", to);
		return hashTable;
	}

	/// <summary>
	/// Sets to.
	/// <para></para>[ValueTo ]for the ending value.
	/// </summary>
	/// <returns>The to.</returns>
	/// <param name="to">Value to.</param>
	public Hashtable SetTo (Color to)
	{
		hashTable.Add ("to", to);
		return hashTable;
	}

	/// <summary>
	/// Sets to.
	/// <para></para>[ValueTo ]for the ending value.
	/// </summary>
	/// <returns>The to.</returns>
	/// <param name="to">Value to.</param>
	public Hashtable SetTo (Rect to)
	{
		hashTable.Add ("to", to);
		return hashTable;
	}

	public Hashtable Build ()
	{
		return hashTable;
	}

	public class Axis
	{
		public static string x = "x";
		public static string y = "y";
		public static string z = "z";
	}
}
