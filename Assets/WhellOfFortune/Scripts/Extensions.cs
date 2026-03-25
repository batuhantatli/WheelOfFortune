using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace WhellOfFortune.Scripts
{
    public static class Extensions
    {
        private static readonly string[] Suffixes = { "", "K", "M", "B", "T" };

        public static string ToShortString(this double value, int decimalPlaces = 1)
        {
            if (value == 0)
                return "0";

            int suffixIndex = (int)Math.Floor(Math.Log10(Math.Abs(value)) / 3);
            suffixIndex = Math.Min(suffixIndex, Suffixes.Length - 1);

            double shortValue = value / Math.Pow(1000, suffixIndex);

            return shortValue % 1 == 0
                ? ((int)shortValue).ToString() + Suffixes[suffixIndex]
                : Math.Round(shortValue, decimalPlaces).ToString($"F{decimalPlaces}") + Suffixes[suffixIndex];
        }

        public static string ToShortString(this int value, int decimalPlaces = 1)
        {
            return ((double)value).ToShortString(decimalPlaces);
        }

        public static string ToShortString(this float value, int decimalPlaces = 1)
        {
            return ((double)value).ToShortString(decimalPlaces);
        }
        
        public static void AnimateNumber(this TMP_Text text, MonoBehaviour mono, int to, float duration = 3f, string prefix = "")
        {
            // Mevcut text'ten prefix'i kaldır
            string raw = text.text.Replace(prefix, "").Trim();

            int from = 0;
            if (int.TryParse(raw.Replace(".", "").Replace(",", ""), out int parsed))
                from = parsed;

            mono.StartCoroutine(AnimateNumberCoroutine(text, from, to, duration, prefix));
        }

        private static IEnumerator AnimateNumberCoroutine(TMP_Text text, int from, int to, float duration, string prefix)
        {
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                int value = Mathf.RoundToInt(Mathf.Lerp(from, to, t));
                text.text = $"{prefix}{value:N0}";
                yield return null;
            }

            text.text = $"{prefix}{to:N0}";
        }
    }
}