using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using KyleDarkMagic;
using UnityEditor;
using UnityEngine;

namespace KyleDarkMagic
{
	[InitializeOnLoad]
	public static  class KyleMagicRoom  {

		static  KyleMagicRoom()
		{
			if (EditorPrefs.GetBool(USE_LUA_HOT_UPDATE_KEY, false))
			{
				StartKyleServer();
			}

			EditorApplication.update += OnEditorUpdate;
		}

		
		private static void OnEditorUpdate()
		{
			if (messageQuque.Count > 0)
			{
				var data = messageQuque.Dequeue();
				
				if (EditorPrefs.GetBool(USE_LUA_HOT_UPDATE_KEY, false))
				{
				
					Debug.LogError("Msg ID  "+data.MessageType+" cotent "+data.Content);
				}
				else
				{
					Debug.Log("Ignore");
				}
			}
		}

		enum eCommanType
		{
			FilePath=1,
			SelectContent=2,
		}
		
		public class MessageUnit
		{
			public int MessageType;
			public string Content;
		}
		
		
		static Queue<MessageUnit> messageQuque=new Queue<MessageUnit>();
		const int INT32_LENGTH=4;
		static private UDPSocket curSocket = null;
		static void StartKyleServer ()
		{
			try
			{
				if (curSocket == null)
				{
					curSocket = new UDPSocket();
					curSocket.Server("127.0.0.1", 27000, OnMsgCalllBack);
				}
				
			}
			catch (Exception e)
			{
				Debug.Log(e);
				throw;
			}
		}

		
		static void OnMsgCalllBack(byte[] bytes)
		{
			
			var outputValue = BitConverter.ToInt32(bytes, 0);
			var content=UTF8Encoding.UTF8.GetString(bytes, INT32_LENGTH, bytes.Length-INT32_LENGTH);
			messageQuque.Enqueue(new MessageUnit(){MessageType = outputValue,Content = content});
			
		}

		private const string USE_LUA_HOT_UPDATE_KEY = "KyleLuaHotFx";
		private static bool isUseLuaHotFix = false;
		[PreferenceItem("KyleDarkMagic")]
		public static void PreferencesGUI()
		{
			
			isUseLuaHotFix = EditorPrefs.GetBool(USE_LUA_HOT_UPDATE_KEY, false);
			GUILayout.Label("Using Lua hot fix: "+ isUseLuaHotFix);
			if (isUseLuaHotFix)
			{
				if (GUILayout.Button("TurnOffLuaHotFix",GUILayout.Width(200)))
				{
					EditorPrefs.SetBool(USE_LUA_HOT_UPDATE_KEY,false);
				}
			}
			else
			{
				if (GUILayout.Button("TurnOnLuaHotFix",GUILayout.Width(200)))
				{
					EditorPrefs.SetBool(USE_LUA_HOT_UPDATE_KEY,true);
					StartKyleServer();
				}
			}
		}
	}
	
	
}
