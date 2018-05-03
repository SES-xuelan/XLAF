using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iTweenHashBuilder
{
	private  Hashtable hashTable = new Hashtable ();

	/// <summary>
	///  Sets the iTween name.
	/// </summary>
	/// <returns>an individual name useful for stopping iTweens by name</returns>
	/// <param name="name">Name.</param>
	public Hashtable setName (string name)
	{
		hashTable.Add ("name", name);
		return hashTable;
	}

	/// <summary>
	/// Sets the audio source.
	/// </summary>
	/// <returns>for which AudioSource to use</returns>
	/// <param name="audioSource">Audio source.</param>
	public Hashtable setAudioSource (AudioSource audioSource)
	{
		hashTable.Add ("audiosource", audioSource);
		return hashTable;
	}

	/// <summary>
	/// Sets the volume.
	/// </summary>
	/// <returns>The volume.</returns>
	/// <param name="volume">Volume.</param>
	public Hashtable setVolume (float volume)
	{
		hashTable.Add ("volume", volume);
		return hashTable;
	}

	/// <summary>
	/// Sets the volume.
	/// </summary>
	/// <returns>The volume.</returns>
	/// <param name="volume">Volume.</param>
	public Hashtable setVolume (double volume)
	{
		hashTable.Add ("volume", volume);
		return hashTable;
	}

	/// <summary>
	/// Sets the pitch.
	/// </summary>
	/// <returns>The pitch.</returns>
	/// <param name="pitch">Pitch.</param>
	public Hashtable setPitch (float pitch)
	{
		hashTable.Add ("pitch", pitch);
		return hashTable;
	}

	/// <summary>
	/// Sets the pitch.
	/// </summary>
	/// <returns>The pitch.</returns>
	/// <param name="pitch">Pitch.</param>
	public Hashtable setPitch (double pitch)
	{
		hashTable.Add ("pitch", pitch);
		return hashTable;
	}

	/// <summary>
	/// Sets the time.
	/// </summary>
	/// <returns>The time.</returns>
	/// <param name="time">Time.</param>
	public Hashtable setTime (float time)
	{
		hashTable.Add ("time", time);
		return hashTable;
	}

	/// <summary>
	/// Sets the time.
	/// </summary>
	/// <returns>The time.</returns>
	/// <param name="time">Time.</param>
	public Hashtable setTime (double time)
	{
		hashTable.Add ("time", time);
		return hashTable;
	}

	/// <summary>
	/// Sets the delay.
	/// </summary>
	/// <returns>The delay.</returns>
	/// <param name="delay">Delay.</param>
	public Hashtable setDelay (float delay)
	{
		hashTable.Add ("delay", delay);
		return hashTable;
	}

	/// <summary>
	/// Sets the delay.
	/// </summary>
	/// <returns>The delay.</returns>
	/// <param name="delay">Delay.</param>
	public Hashtable setDelay (double delay)
	{
		hashTable.Add ("delay", delay);
		return hashTable;
	}

	/// <summary>
	/// Sets the type of the ease.
	/// </summary>
	/// <returns>The ease type.</returns>
	/// <param name="easeType">Ease type.</param>
	public Hashtable setEaseType (iTween.EaseType easeType)
	{
		hashTable.Add ("easetype", easeType);
		return hashTable;
	}
	//http://www.pixelplacement.com/itween/documentation.php    next is looptype
}
