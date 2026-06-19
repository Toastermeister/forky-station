using Content.Client.Stylesheets.Palette;

namespace Content.Client.Stylesheets.Stylesheets;

public partial class SystemStylesheet
{
    public override ColorPalette PrimaryPalette => FunkyAmberEnabled ? Palettes.FunkyAmberPrimary : Palettes.Cyan;
    public override ColorPalette SecondaryPalette => FunkyAmberEnabled ? Palettes.FunkyAmberSecondary : Palettes.Neutral;
    public override ColorPalette PositivePalette => Palettes.Green;
    public override ColorPalette NegativePalette => Palettes.Red;
    public override ColorPalette HighlightPalette => Palettes.Maroon;
}
