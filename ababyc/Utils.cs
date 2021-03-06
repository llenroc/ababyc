﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ababyc.Models;
using Xamarin.Forms;

namespace ababyc
{
	public static class ViewExtensions
	{

		public static Task<bool> ColorTo(this VisualElement self, Color fromColor, Color toColor, Action<Color> callback, uint length = 250, Easing easing = null)
		{
			Func<double, Color> transform = (t) =>
			  Color.FromRgba(fromColor.R + t * (toColor.R - fromColor.R),
							 fromColor.G + t * (toColor.G - fromColor.G),
							 fromColor.B + t * (toColor.B - fromColor.B),
							 fromColor.A + t * (toColor.A - fromColor.A));
			return ColorAnimation(self, "ColorTo", transform, callback, length, easing);
		}

		public static void CancelAnimation(this VisualElement self)
		{
			self.AbortAnimation("ColorTo");
		}

		static Task<bool> ColorAnimation(VisualElement element, string name, Func<double, Color> transform, Action<Color> callback, uint length, Easing easing)
		{
			easing = easing ?? Easing.Linear;
			var taskCompletionSource = new TaskCompletionSource<bool>();

			element.Animate<Color>(name, transform, callback, 16, length, easing, (v, c) => taskCompletionSource.SetResult(c));
			return taskCompletionSource.Task;
		}
	}

	public static class Utils
	{
		internal sealed class Timer : CancellationTokenSource
		{
			internal Timer(Action<object> callback, object state, int millisecondsDueTime, int millisecondsPeriod, bool waitForCallbackBeforeNextPeriod = false)
			{
				//Contract.Assert(period == -1, "This stub implementation only supports dueTime.");

				Task.Delay(millisecondsDueTime, Token).ContinueWith(async (t, s) =>
				{
					var tuple = (Tuple<Action<object>, object>)s;

					while (!IsCancellationRequested)
					{
						if (waitForCallbackBeforeNextPeriod)
							tuple.Item1(tuple.Item2);
						else
							Task.Run(() => tuple.Item1(tuple.Item2));

						await Task.Delay(millisecondsPeriod, Token).ConfigureAwait(false);
					}

				}, Tuple.Create(callback, state), CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.OnlyOnRanToCompletion, TaskScheduler.Default);
			}

			protected override void Dispose(bool disposing)
			{
				if (disposing)
					Cancel();

				base.Dispose(disposing);
			}
		}

		private static readonly List<ShapeType> shapes;
		private static readonly Dictionary<Color, string> brushToString;
		private static readonly Random lRandom = new Random(); // BUG BUG: Believe it or not, Random is NOT THREAD SAFE!
		private static readonly Color[] someColors;

		private static readonly string[] sounds = {
													  "giggle.wav",
													  "babylaugh.wav",
													  "babygigl2.wav",
													  "ccgiggle.wav",
													  "laughingmice.wav",
													  "scooby2.wav",
												  };

		static char[] alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

		static Utils()
		{
			shapes = GetEnumAsList<ShapeType>();

			brushToString = new Dictionary<Color, string>
								   {
									{Color.Red, "Red"},
									{Color.Blue, "Blue"},
									{Color.Yellow, "Yellow"},
									{Color.Green, "Green"},
									{Color.Purple, "Purple"},
									{Color.Pink, "Pink"},
									{Color.Fuchsia, "Fuchsia"},
									{Color.Lime, "Lime"},
									{Color.Gray, "Gray"}
								};

			someColors = new Color[brushToString.Count];
			brushToString.Keys.CopyTo(someColors, 0);
		}

		public static Color GetRandomColor()
		{
			Color color = someColors[lRandom.Next(0, someColors.Length)];
			return color;
		}

		public static Color LightenOrDarken(this Color src, double degree)
		{
			Color ret = src.AddLuminosity(degree);
			return ret;
		}


		public static string ColorToString(Color b)
		{
			return brushToString[b];
		}

		public static string GetRandomSoundFile()
		{
			return sounds[lRandom.Next(0, sounds.Length)];
		}

		public static ShapeType GetRandomShape()
		{
			return shapes[lRandom.Next(0, shapes.Count)];
		}

		public static bool GetRandomBoolean()
		{
			if (lRandom.Next(0, 2) == 0)
				return false;
			return true;
		}

		public static int RandomBetweenTwoNumbers(int min, int max)
		{
			return lRandom.Next(min, max + 1);
		}

		public static List<T> GetEnumAsList<T>()
		{
			return Enum.GetValues(typeof(T)).Cast<T>().ToList();
		}

		public static string GetRandomLetter()
		{
			return alpha[lRandom.Next(0, alpha.Length)].ToString();
		}
	}
}
