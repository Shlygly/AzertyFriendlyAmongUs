using System;
using System.Runtime.InteropServices;
using System.Text;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using HarmonyLib;
using Reactor;
using UnityEngine;

namespace ModTest
{
    [BepInPlugin(Id)]
    [BepInProcess("Among Us.exe")]
    [BepInDependency(ReactorPlugin.Id)]
    public class AzertyFriendly : BasePlugin
    {
        public const string Id = "com.shlygly.azertyfriendly";

        public Harmony Harmony { get; } = new Harmony(Id);

        public override void Load()
        {
            Harmony.PatchAll();
        }

        [DllImport("user32.dll")]
        private static extern long GetKeyboardLayoutName(StringBuilder pwszKLID);

        [HarmonyPatch(typeof(KeyboardJoystick), nameof(KeyboardJoystick.ALEHOJBEIJH))]
        public static class KeyboardPatch
        {
            public static void Postfix(KeyboardJoystick __instance, ref Vector2 __result)
            {
                StringBuilder keyboardType = new StringBuilder(9);
                GetKeyboardLayoutName(keyboardType);
                if (keyboardType[^1] == 'C')
                {
                    __result = new Vector2(
                        (Input.GetKey("d") ? 1 : 0) + (Input.GetKey("q") ? -1 : 0),
                        (Input.GetKey("z") ? 1 : 0) + (Input.GetKey("s") ? -1 : 0)
                    ).normalized;
                }
            }
        }
    }
}
