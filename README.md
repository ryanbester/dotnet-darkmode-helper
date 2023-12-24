# DarkMode.Helper

Contains Helper functions for [.NET DarkMode](https://github.com/ryanbester/dotnet-darkmode).

## Usage

This Helper simplifies the process of implement .NET DarkMode by moving the theming settings centrally to a single point, and providing wrapper methods for responding to dark mode changes.

In program entry point:

```cs
// Create new DarkMode Properties object
var darkModeProperties = new DarkModeProperties()
{
    Theme = SettingsManager.Settings.Theme,
};

// Color mapping
darkModeProperties.ColorMap[Color.White] = Color.Black;
darkModeProperties.ColorMap[Color.Black] = Color.White;

// Control classes
darkModeProperties.ControlClasses[typeof(Button)] = new DarkMode.DarkMode.ControlThemingClasses(){
        DarkModeClassName = "DarkMode_Explorer", LightModeClassName = "Explorer"
};

// Control overrides

darkModeProperties.ControlOverrides[typeof(Button)] = (control) =>
{
    // Do some theming stuff here
};

// Initialising DarkMode
DarkModeHelper.AppInit(darkModeProperties);
```

In Form:

```cs
protected override void WndProc(ref Message m)
{
    DarkModeHelper.WndProc(this, m);
    base.WndProc(ref m);
}

private void Form1_Load(object sender, EventArgs e)
{
    DarkModeHelper.WndLoad(this);
}
```

This library provides three main methods to theming controls:

### Color Mapping

Color mapping is simply replacing an existing color with another color.

This only works for `BackColor` and `ForeColor` properties at the moment.

### Control Classes

Some controls contain custom classes to enable dark mode theming. For example, buttons contain a `DarkMode_Explorer` class.

This dictionary allows you to specify a class name to use for dark mode and light mode.

### Control Overrides

If color mapping or changing classes does not do the trick, and you need more precise theming controls, a callback can be hooked up for each type of control.

## Methods

### `AppInit(DarkModeProperties)`

Call in your application entry point to set the DarkMode properties.

### `WndProc(Control, Message)`

Override `WndProc` in each Form and call there.

### `WndLoad(Control)`

Call in the `Load` event for each Form.

### `DarkModeEnabledTheme()`

Returns a boolean declaring whether dark mode is enabled or not, with respect to the theme. For example, if the light theme is explicitly set, this will always be `false`.

### `DarkModeProperties.GetMappedColor(Color)`

Returns the mapped color for the specified color, or the input color if one is not mapped.
