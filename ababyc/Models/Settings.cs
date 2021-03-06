﻿namespace ababyc.Models
{
	public class Settings
	{

		static Settings defaultInstance;

		public static Settings Default
		{
			get
			{
				return defaultInstance ?? (defaultInstance = new Settings());
			}
		}

		public Settings()
		{
			ClearAfter = 35;
			ForceUppercase = true;
			Speak = true;
			UseEffects = true;
			UseAnimations = true;
			FadeAfter = 30;
			FadeAway = true;
			FontSize = 150;
		}
		public int ClearAfter
		{
			get; set;
		}
		public bool ForceUppercase
		{
			get;
			set;
		}
		public string FontFamily
		{
			get;
			set;
		}

		public bool Speak
		{
			get;
			set;
		}

		public bool UseEffects
		{
			get;
			set;
		}

		public bool UseAnimations
		{
			get;
			set;
		}

		public int FadeAfter
		{
			get;
			set;
		}
		public bool FadeAway
		{
			get;
			set;
		}
		public int FontSize
		{
			get;
			internal set;
		}
	}
}
