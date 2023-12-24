using static DarkMode.DarkMode;
using System.Windows.Forms;
using System.Drawing;
using System;
using System.Collections.Generic;

namespace DarkMode.Helper
{
    public static class DarkModeHelper
    {
        public static DarkModeProperties Properties { get; set; }

        /// <summary>
        /// Initialises dark mode for an application. Call this in your application entry point.
        /// </summary>
        /// <param name="properties">The DarkModeProperties object</param>
        public static void AppInit(DarkModeProperties properties)
        {
            if (properties == null) return;

            Properties = properties;
            SetAppTheme(properties.Theme);
        }

        /// <summary>
        /// Initialises dark mode for a control. Call this in the WndProc method for each form.
        /// </summary>
        /// <param name="control">The control to initialise dark mode for</param>
        /// <param name="m">The message</param>
        public static void WndProc(Control control, Message m)
        {
            if (Properties == null) return;

            DarkMode.WndProc(control, m, Properties.Theme, ThemeControl);
        }

        /// <summary>
        /// Themes dark mode controls for a control. Call this in the Loda event handler for each form.
        /// </summary>
        /// <param name="control">The control to theme dark mode for</param>
        public static void WndLoad(Control control)
        {
            if (Properties == null) return;
        }

        /// <summary>
        /// Returns whether dark mode is enabled or not, with respect to the theme.
        /// </summary>
        /// <returns></returns>
        public static bool DarkModeEnabledTheme()
        {
            return DarkMode.DarkModeEnabledTheme(Properties.Theme);
        }

        private static void ThemeControl(Control control)
        {
            if (DarkModeEnabledTheme())
            {
                control.BackColor = Properties.GetMappedColor(control.BackColor);
                control.ForeColor = Properties.GetMappedColor(control.ForeColor);
            }

            SetControlClasses(control, Properties.GetControlClasses(control.GetType()), Properties.Theme);

            var controlOverride = Properties.GetControlOverride(control.GetType());
            if (controlOverride != null)
            {
                controlOverride(control);
            }
        }
    }

    public class DarkModeProperties
    {
        public delegate void ControlOverride(Control control);

        public Theme Theme { get; set; }

        public Dictionary<Color, Color> ColorMap { get; set; } = new Dictionary<Color, Color>();

        public Dictionary<Type, ControlThemingClasses> ControlClasses { get; set; } = new Dictionary<Type, ControlThemingClasses>();

        public Dictionary<Type, ControlOverride> ControlOverrides { get; set; } = new Dictionary<Type, ControlOverride>();

        public Color GetMappedColor(Color color)
        {
            if (ColorMap.TryGetValue(color, out Color mappedColor))
            {
                return mappedColor;
            }
            return color;
        }

        internal ControlThemingClasses GetControlClasses(Type control)
        {
            if (ControlClasses.TryGetValue(control, out ControlThemingClasses themingClasses))
            {
                return themingClasses;
            }
            return GetDefaultControlClasses(control);
        }

        internal ControlOverride GetControlOverride(Type control)
        {
            if (ControlOverrides.TryGetValue(control, out ControlOverride controlOverride))
            {
                return controlOverride;
            }
            return null;
        }
    }

}
