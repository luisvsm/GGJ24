using System;
using UnityEngine;

namespace noWeekend
{
    public enum EaseType { ElasticIn, ElasticOut, ElasticInOut, QuadIn, QuadOut, QuadInOut, CubeIn, CubeOut, CubeInOut, QuartIn, QuartOut, QuartInOut, QuintIn, QuintOut, QuintIntOut, SineIn, SineOut, SineInOut, BounceIn, BounceOut, BounceInOut, CircIn, CircOut, CircInOut, ExpoIn, ExpoOut, ExpoInOut, BackIn, BackOut, BackInOut, Linear, Animation}

	public static class Ease
    {
        const float Pi = 3.14159f;
        const float Pi2 = Pi / 2;
        const float B1 = 1 / 2.75f;
        const float B2 = 2 / 2.75f;
        const float B3 = 1.5f / 2.75f;
        const float B4 = 2.5f / 2.75f;
        const float B5 = 2.25f / 2.75f;
        const float B6 = 2.625f / 2.75f;


        public static float GetEase(EaseType easeType, float time)
        {
            switch (easeType)
            {
                case EaseType.Linear:
                    return Linear(time);
                case EaseType.ElasticIn:
                    return ElasticIn(time);
                case EaseType.ElasticOut:
                    return ElasticOut(time);
                case EaseType.ElasticInOut:
                    return ElasticInOut(time);
                case EaseType.QuadIn:
                    return QuadIn(time);
                case EaseType.QuadOut:
                    return QuadOut(time);
                case EaseType.QuadInOut:
                    return QuadInOut(time);
                case EaseType.CubeIn:
                    return CubeIn(time);
                case EaseType.CubeOut:
                    return CubeOut(time);
                case EaseType.CubeInOut:
                    return CubeInOut(time);
                case EaseType.QuartIn:
                    return QuartIn(time);
                case EaseType.QuartOut:
                    return QuartOut(time);
                case EaseType.QuartInOut:
                    return QuartInOut(time);
                case EaseType.QuintIn:
                    return QuintIn(time);
                case EaseType.QuintOut:
                    return QuintOut(time);
                case EaseType.QuintIntOut:
                    return QuintInOut(time);
                case EaseType.SineIn:
                    return SineIn(time);
                case EaseType.SineOut:
                    return SineOut(time);
                case EaseType.SineInOut:
                    return SineInOut(time);
                case EaseType.BounceIn:
                    return BounceIn(time);
                case EaseType.BounceOut:
                    return BounceOut(time);
                case EaseType.BounceInOut:
                    return BounceInOut(time);
                case EaseType.CircIn:
                    return CircIn(time);
                case EaseType.CircOut:
                    return CircOut(time);
                case EaseType.CircInOut:
                    return CircInOut(time);
                case EaseType.ExpoIn:
                    return ExpoIn(time);
                case EaseType.ExpoOut:
                    return ExpoOut(time);
                case EaseType.ExpoInOut:
                    return ExpoInOut(time);
                case EaseType.BackIn:
                    return BackIn(time);
                case EaseType.BackOut:
                    return BackOut(time);
                case EaseType.BackInOut:
                    return BackInOut(time);
                default:
                    return time;
            }
        }

		public static string EaseName(EaseType easeType)
		{
			switch (easeType)
			{
				case EaseType.Linear:
                    return "Linear";
				case EaseType.ElasticIn:
					return "Elastic In";
				case EaseType.ElasticOut:
					return "Elastic Out";
				case EaseType.ElasticInOut:
					return "Elastic In-Out";
				case EaseType.QuadIn:
					return "Quad In";
				case EaseType.QuadOut:
					return "Quad Out";
				case EaseType.QuadInOut:
					return "Quad In-Out";
				case EaseType.CubeIn:
					return "Cube In";
				case EaseType.CubeOut:
					return "Cube Out";
				case EaseType.CubeInOut:
					return "Cube In-Out";
				case EaseType.QuartIn:
					return "Quart In";
				case EaseType.QuartOut:
					return "Quart Out";
				case EaseType.QuartInOut:
					return "Quart In-Out";
				case EaseType.QuintIn:
					return "Quint In";
				case EaseType.QuintOut:
					return "Quint Out";
				case EaseType.QuintIntOut:
					return "Quint In-Out";
				case EaseType.SineIn:
					return "Sine In";
				case EaseType.SineOut:
					return "Sine Out";
				case EaseType.SineInOut:
					return "Sine In-Out";
				case EaseType.BounceIn:
					return "Bounce In";
				case EaseType.BounceOut:
					return "Bounce Out";
				case EaseType.BounceInOut:
					return "Bounce In-Out";
				case EaseType.CircIn:
					return "Circ In";
				case EaseType.CircOut:
					return "Circ Out";
				case EaseType.CircInOut:
					return "Circ In-Out";
				case EaseType.ExpoIn:
					return "Expo In";
				case EaseType.ExpoOut:
					return "Expo Out";
				case EaseType.ExpoInOut:
					return "Expo In-Out";
				case EaseType.BackIn:
					return "Back In";
				case EaseType.BackOut:
					return "Back Out";
				case EaseType.BackInOut:
					return "Back In-Out";
				case EaseType.Animation:
					return "Custom";
				default:
					return "N/A";
			}
		}

		// Linear
		public static float Linear(float t)
		{
            return t;
		}

		// Elastic in.
		public static float ElasticIn(float t)
        {
            return (float)(Math.Sin(13 * Pi2 * t) * Math.Pow(2, 10 * (t - 1)));
        }

        // Elastic out.
        public static float ElasticOut(float t)
        {
            return (float)(Math.Sin(-13 * Pi2 * (t + 1)) * Math.Pow(2, -10 * t) + 1);
        }

        // Elastic in and out.
        public static float ElasticInOut(float t)
        {
            if (t < 0.5)
            {
                return (float)(0.5 * Math.Sin(13 * Pi2 * (2 * t)) * Math.Pow(2, 10 * ((2 * t) - 1)));
            }

            return (float)(0.5 * (Math.Sin(-13 * Pi2 * ((2 * t - 1) + 1)) * Math.Pow(2, -10 * (2 * t - 1)) + 2));
        }

        // Quadratic in.
        public static float QuadIn(float t)
        {
            return t * t;
        }

        // Quadratic out.
        public static float QuadOut(float t)
        {
            return -t * (t - 2);
        }

        // Quadratic in and out.
        public static float QuadInOut(float t)
        {
            return t <= .5 ? t * t * 2 : 1 - (--t) * t * 2;
        }

        // Cubic in.
        public static float CubeIn(float t)
        {
            return t * t * t;
        }

        // Cubic out.
        public static float CubeOut(float t)
        {
            return 1 + (--t) * t * t;
        }

        // Cubic in and out.
        public static float CubeInOut(float t)
        {
            return t <= .5 ? t * t * t * 4 : 1 + (--t) * t * t * 4;
        }

        // Quart in.
        public static float QuartIn(float t)
        {
            return t * t * t * t;
        }

        // Quart out.
        public static float QuartOut(float t)
        {
            return 1 - (t -= 1) * t * t * t;
        }

        // Quart in and out.
        public static float QuartInOut(float t)
        {
            return (float)(t <= .5 ? t * t * t * t * 8 : (1 - (t = t * 2 - 2) * t * t * t) / 2 + .5);
        }

        // Quint in.
        public static float QuintIn(float t)
        {
            return t * t * t * t * t;
        }

        // Quint out.
        public static float QuintOut(float t)
        {
            return (t = t - 1) * t * t * t * t + 1;
        }

        // Quint in and out.
        public static float QuintInOut(float t)
        {
            return ((t *= 2) < 1) ? (t * t * t * t * t) / 2 : ((t -= 2) * t * t * t * t + 2) / 2;
        }

        // Sine in.
        public static float SineIn(float t)
        {
            return (float)(-Math.Cos(Pi2 * t) + 1);
        }

        // Sine out.
        public static float SineOut(float t)
        {
            return (float)(Math.Sin(Pi2 * t));
        }

        // Sine in and out
        public static float SineInOut(float t)
        {
            return (float)(-Math.Cos(Pi * t) / 2 + .5);
        }

        // Bounce in.
        public static float BounceIn(float t)
        {
            t = 1 - t;
            if (t < B1) return (float)(1 - 7.5625 * t * t);
            if (t < B2) return (float)(1 - (7.5625 * (t - B3) * (t - B3) + .75));
            if (t < B4) return (float)(1 - (7.5625 * (t - B5) * (t - B5) + .9375));
            return (float)(1 - (7.5625 * (t - B6) * (t - B6) + .984375));
        }

        // Bounce out.
        public static float BounceOut(float t)
        {
            if (t < B1) return (float)(7.5625 * t * t);
            if (t < B2) return (float)(7.5625 * (t - B3) * (t - B3) + .75);
            if (t < B4) return (float)(7.5625 * (t - B5) * (t - B5) + .9375);
            return (float)(7.5625 * (t - B6) * (t - B6) + .984375);
        }

        // Bounce in and out.
        public static float BounceInOut(float t)
        {
            if (t < .5)
            {
                t = 1 - t * 2;
                if (t < B1) return (float)((1 - 7.5625 * t * t) / 2);
                if (t < B2) return (float)((1 - (7.5625 * (t - B3) * (t - B3) + .75)) / 2);
                if (t < B4) return (float)((1 - (7.5625 * (t - B5) * (t - B5) + .9375)) / 2);
                return (float)((1 - (7.5625 * (t - B6) * (t - B6) + .984375)) / 2);
            }
            t = t * 2 - 1;
            if (t < B1) return (float)((7.5625 * t * t) / 2 + .5);
            if (t < B2) return (float)((7.5625 * (t - B3) * (t - B3) + .75) / 2 + .5);
            if (t < B4) return (float)((7.5625 * (t - B5) * (t - B5) + .9375) / 2 + .5);
            return (float)((7.5625 * (t - B6) * (t - B6) + .984375) / 2 + .5);
        }

        // Circle in.
        public static float CircIn(float t)
        {
            return (float)(-(Math.Sqrt(1 - t * t) - 1));
        }

        // Circle out
        public static float CircOut(float t)
        {
            return (float)(Math.Sqrt(1 - (t - 1) * (t - 1)));
        }

        // Circle in and out.
        public static float CircInOut(float t)
        {
            return (float)(t <= .5 ? (Math.Sqrt(1 - t * t * 4) - 1) / -2 : (Math.Sqrt(1 - (t * 2 - 2) * (t * 2 - 2)) + 1) / 2);
        }

        // Exponential in.
        public static float ExpoIn(float t)
        {
            return (float)(Math.Pow(2, 10 * (t - 1)));
        }

        // Exponential out.
        public static float ExpoOut(float t)
        {
            return (float)(-Math.Pow(2, -10 * t) + 1);
        }

        // Exponential in and out.
        public static float ExpoInOut(float t)
        {
            return (float)(t < .5 ? Math.Pow(2, 10 * (t * 2 - 1)) / 2 : (-Math.Pow(2, -10 * (t * 2 - 1)) + 2) / 2);
        }

        // Back in.
        public static float BackIn(float t)
        {
            return (float)(t * t * (2.70158 * t - 1.70158));
        }

        // Back out.
        public static float BackOut(float t)
        {
            return (float)(1 - (--t) * (t) * (-2.70158 * t - 1.70158));
        }

        // Back in and out.
        public static float BackInOut(float t)
        {
            t *= 2;
            if (t < 1) return (float)(t * t * (2.70158 * t - 1.70158) / 2);
            t--;
            return (float)((1 - (--t) * (t) * (-2.70158 * t - 1.70158)) / 2 + .5);
        }
    }
}