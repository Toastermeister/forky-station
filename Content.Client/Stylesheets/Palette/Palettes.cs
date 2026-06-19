namespace Content.Client.Stylesheets.Palette;

/// <summary>
///     Stores all style palettes in one accessible location
/// </summary>
/// <remarks>
///     Technically not limited to only colors, can store like, standard padding amounts, and font sizes, maybe?
/// </remarks>
public static class Palettes
{
    // muted tones
    public static readonly ColorPalette Navy = ColorPalette.FromHexBase("#4f5376", lightnessShift: 0.05f, chromaShift: 0.0045f);
    public static readonly ColorPalette Cyan = ColorPalette.FromHexBase("#42586a", lightnessShift: 0.05f, chromaShift: 0.0045f);
    public static readonly ColorPalette Slate = ColorPalette.FromHexBase("#545562");
    public static readonly ColorPalette Neutral = ColorPalette.FromHexBase("#555555");

    // funky amber tones
    public static readonly ColorPalette FunkyAmberPrimary = new ColorPalette(
        Base: Color.FromHex("#ff6a00"),
        LightnessShift: 0.06f,
        ChromaShift: 0.00f,
        Element: Color.FromHex("#160b00"),
        HoveredElement: Color.FromHex("#261000"),
        PressedElement: Color.FromHex("#361500"),
        DisabledElement: Color.FromHex("#332216"),
        Background: Color.FromHex("#030100"),
        BackgroundLight: Color.FromHex("#160b00"),
        BackgroundDark: Color.FromHex("#030100"),
        Text: Color.FromHex("#ff6a00"),
        TextDark: Color.FromHex("#994400")
    );

    public static readonly ColorPalette FunkyAmberSecondary = new ColorPalette(
        Base: Color.FromHex("#25211c"),
        LightnessShift: 0.06f,
        ChromaShift: 0.00f,
        Element: Color.FromHex("#25211c"),
        HoveredElement: Color.FromHex("#302a24"),
        PressedElement: Color.FromHex("#1b1814"),
        DisabledElement: Color.FromHex("#14110e"),
        Background: Color.FromHex("#25211c"),
        BackgroundLight: Color.FromHex("#25211c"),
        BackgroundDark: Color.FromHex("#14110e"),
        Text: Color.FromHex("#ff6a00"),
        TextDark: Color.FromHex("#994400")
    );

    // status tones
    public static readonly ColorPalette Red = ColorPalette.FromHexBase("#b62124", chromaShift: 0.02f);
    public static readonly ColorPalette Amber = ColorPalette.FromHexBase("#c18e36");
    public static readonly ColorPalette Green = ColorPalette.FromHexBase("#3c854a");
    public static readonly StatusPalette Status = new([Red.Base, Amber.Base, Green.Base]);

    // highlight tones
    public static readonly ColorPalette Gold = ColorPalette.FromHexBase("#a88b5e");
    public static readonly ColorPalette Maroon = ColorPalette.FromHexBase("#9b2236");

    // Intended to be used with `ModulateSelf` to darken / lighten something
    public static readonly ColorPalette AlphaModulate = ColorPalette.FromHexBase("#ffffff");

}
