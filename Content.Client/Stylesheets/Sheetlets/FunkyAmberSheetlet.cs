using Content.Client.Resources;
using Content.Client.Stylesheets.Palette;
using Content.Client.UserInterface.Controls;
using Content.Client.UserInterface.Screens;
using Content.Client.UserInterface.Systems.Chat.Controls;
using Robust.Client.Graphics;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.CustomControls;
using Robust.Shared.Maths;
using static Content.Client.Stylesheets.StylesheetHelpers;

namespace Content.Client.Stylesheets.Sheetlets;

public sealed class FunkyAmberSheetlet : Sheetlet<PalettedStylesheet>
{
    public override StyleRule[] GetRules(PalettedStylesheet sheet, object config)
    {
        var amber = Color.FromHex("#ff6a00");
        var amberDim = Color.FromHex("#994400");
        var amberDark = Color.FromHex("#331a00");
        var amberPanel = Color.FromHex("#160b00");
        var screenBlack = Color.FromHex("#030100");
        var casing = Color.FromHex("#25211c");
        var casingDark = Color.FromHex("#14110e");
        var danger = Color.FromHex("#9f240e");
        var dangerHover = Color.FromHex("#c93914");
        var dangerPressed = Color.FromHex("#661407");
        var disabled = Color.FromHex("#332216");
        var terminalFont = ResCache.GetFont("/EngineFonts/NotoSans/NotoSansMono-Regular.ttf", 12);
        var terminalFontSmall = ResCache.GetFont("/EngineFonts/NotoSans/NotoSansMono-Regular.ttf", 10);
        var terminalFontLarge = ResCache.GetFont("/EngineFonts/NotoSans/NotoSansMono-Regular.ttf", 14);

        var rootPanel = new StyleBoxFlat
        {
            BackgroundColor = casingDark,
            BorderColor = Color.FromHex("#5c4633"),
            BorderThickness = new Thickness(2),
            ContentMarginLeftOverride = 8,
            ContentMarginRightOverride = 8,
            ContentMarginTopOverride = 8,
            ContentMarginBottomOverride = 8,
        };

        var casingPanel = new StyleBoxFlat
        {
            BackgroundColor = casing,
            BorderColor = Color.FromHex("#6d5a45"),
            BorderThickness = new Thickness(2),
            ContentMarginLeftOverride = 8,
            ContentMarginRightOverride = 8,
            ContentMarginTopOverride = 6,
            ContentMarginBottomOverride = 6,
        };

        var screenPanel = new StyleBoxFlat
        {
            BackgroundColor = screenBlack,
            BorderColor = amberDark,
            BorderThickness = new Thickness(2),
            ContentMarginLeftOverride = 10,
            ContentMarginRightOverride = 10,
            ContentMarginTopOverride = 8,
            ContentMarginBottomOverride = 8,
        };

        var divider = new StyleBoxFlat
        {
            BackgroundColor = amberDark,
            ContentMarginTopOverride = 2,
        };

        var buttonBase = new StyleBoxFlat
        {
            BackgroundColor = amberPanel,
            BorderColor = amberDark,
            BorderThickness = new Thickness(2),
            ContentMarginLeftOverride = 14,
            ContentMarginRightOverride = 14,
            ContentMarginTopOverride = 8,
            ContentMarginBottomOverride = 8,
        };

        var buttonHover = new StyleBoxFlat(buttonBase)
        {
            BackgroundColor = Color.FromHex("#261000"),
            BorderColor = amber,
        };

        var buttonPressed = new StyleBoxFlat(buttonBase)
        {
            BackgroundColor = Color.FromHex("#361500"),
            BorderColor = Color.FromHex("#ff9a3d"),
        };

        var buttonDisabled = new StyleBoxFlat(buttonBase)
        {
            BackgroundColor = disabled,
            BorderColor = Color.FromHex("#553017"),
        };

        var menuButtonBase = new StyleBoxFlat(buttonBase);
        var menuButtonHover = new StyleBoxFlat(buttonHover);
        var menuButtonPressed = new StyleBoxFlat(buttonPressed);
        var menuButtonDisabled = new StyleBoxFlat(buttonDisabled);

        var menuButtonOpenLeft = new StyleBoxFlat(menuButtonBase) { BorderThickness = new Thickness(0, 2, 2, 2) };
        var menuButtonOpenLeftHover = new StyleBoxFlat(menuButtonHover) { BorderThickness = new Thickness(0, 2, 2, 2) };
        var menuButtonOpenLeftPressed = new StyleBoxFlat(menuButtonPressed) { BorderThickness = new Thickness(0, 2, 2, 2) };
        var menuButtonOpenLeftDisabled = new StyleBoxFlat(menuButtonDisabled) { BorderThickness = new Thickness(0, 2, 2, 2) };

        var menuButtonOpenRight = new StyleBoxFlat(menuButtonBase) { BorderThickness = new Thickness(2, 2, 0, 2) };
        var menuButtonOpenRightHover = new StyleBoxFlat(menuButtonHover) { BorderThickness = new Thickness(2, 2, 0, 2) };
        var menuButtonOpenRightPressed = new StyleBoxFlat(menuButtonPressed) { BorderThickness = new Thickness(2, 2, 0, 2) };
        var menuButtonOpenRightDisabled = new StyleBoxFlat(menuButtonDisabled) { BorderThickness = new Thickness(2, 2, 0, 2) };

        var menuButtonSquare = new StyleBoxFlat(menuButtonBase) { BorderThickness = new Thickness(0, 2, 0, 2) };
        var menuButtonSquareHover = new StyleBoxFlat(menuButtonHover) { BorderThickness = new Thickness(0, 2, 0, 2) };
        var menuButtonSquarePressed = new StyleBoxFlat(menuButtonPressed) { BorderThickness = new Thickness(0, 2, 0, 2) };
        var menuButtonSquareDisabled = new StyleBoxFlat(menuButtonDisabled) { BorderThickness = new Thickness(0, 2, 0, 2) };

        var dangerButtonBase = new StyleBoxFlat(buttonBase)
        {
            BackgroundColor = danger,
            BorderColor = Color.FromHex("#3f0d04"),
        };

        var dangerButtonHover = new StyleBoxFlat(dangerButtonBase)
        {
            BackgroundColor = dangerHover,
            BorderColor = Color.FromHex("#ff6a00"),
        };

        var dangerButtonPressed = new StyleBoxFlat(dangerButtonBase)
        {
            BackgroundColor = dangerPressed,
            BorderColor = Color.FromHex("#ff9a3d"),
        };

        var positiveButtonBase = new StyleBoxFlat(buttonBase)
        {
            BackgroundColor = Color.FromHex("#061a08"),
            BorderColor = Color.FromHex("#146b1d"),
        };
        var positiveButtonHover = new StyleBoxFlat(buttonBase)
        {
            BackgroundColor = Color.FromHex("#0d3612"),
            BorderColor = Color.FromHex("#23b832"),
        };
        var positiveButtonPressed = new StyleBoxFlat(buttonBase)
        {
            BackgroundColor = Color.FromHex("#15571d"),
            BorderColor = Color.FromHex("#32fa48"),
        };

        var negativeButtonBase = new StyleBoxFlat(buttonBase)
        {
            BackgroundColor = Color.FromHex("#210704"),
            BorderColor = Color.FromHex("#701205"),
        };
        var negativeButtonHover = new StyleBoxFlat(buttonBase)
        {
            BackgroundColor = Color.FromHex("#3b0d07"),
            BorderColor = danger,
        };
        var negativeButtonPressed = new StyleBoxFlat(buttonBase)
        {
            BackgroundColor = Color.FromHex("#5c140b"),
            BorderColor = dangerHover,
        };

        var retroScrollBarGrabberNormal = new StyleBoxFlat
        {
            BackgroundColor = amberDark,
            BorderColor = Color.FromHex("#5c4633"),
            BorderThickness = new Thickness(1),
            ContentMarginLeftOverride = 10,
            ContentMarginTopOverride = 10,
        };
        var retroScrollBarGrabberHover = new StyleBoxFlat(retroScrollBarGrabberNormal)
        {
            BackgroundColor = amberDim,
            BorderColor = amber,
        };
        var retroScrollBarGrabberGrabbed = new StyleBoxFlat(retroScrollBarGrabberNormal)
        {
            BackgroundColor = amber,
            BorderColor = Color.FromHex("#ff9a3d"),
        };

        var retroSliderBack = new StyleBoxFlat
        {
            BackgroundColor = screenBlack,
            BorderColor = amberDark,
            BorderThickness = new Thickness(1),
            ContentMarginLeftOverride = 6,
            ContentMarginTopOverride = 6,
            ContentMarginRightOverride = 6,
            ContentMarginBottomOverride = 6,
        };
        var retroSliderFill = new StyleBoxFlat
        {
            BackgroundColor = amberPanel,
            BorderColor = amberDim,
            BorderThickness = new Thickness(1),
        };
        var retroSliderGrabber = new StyleBoxFlat
        {
            BackgroundColor = casing,
            BorderColor = amberDim,
            BorderThickness = new Thickness(2),
            ContentMarginLeftOverride = 12,
            ContentMarginTopOverride = 12,
        };

        var retroProgressBarBack = new StyleBoxFlat
        {
            BackgroundColor = screenBlack,
            BorderColor = amberDark,
            BorderThickness = new Thickness(1),
        };
        retroProgressBarBack.SetContentMarginOverride(StyleBox.Margin.Vertical, 14.5f);
        var retroProgressBarFore = new StyleBoxFlat
        {
            BackgroundColor = amber,
        };
        retroProgressBarFore.SetContentMarginOverride(StyleBox.Margin.Vertical, 14.5f);

        var retroTabActive = new StyleBoxFlat
        {
            BackgroundColor = amberPanel,
            BorderColor = amber,
            BorderThickness = new Thickness(2, 2, 2, 0),
            ContentMarginLeftOverride = 10,
            ContentMarginRightOverride = 10,
            ContentMarginTopOverride = 6,
            ContentMarginBottomOverride = 6,
        };
        var retroTabInactive = new StyleBoxFlat
        {
            BackgroundColor = casingDark,
            BorderColor = Color.FromHex("#5c4633"),
            BorderThickness = new Thickness(2, 2, 2, 0),
            ContentMarginLeftOverride = 10,
            ContentMarginRightOverride = 10,
            ContentMarginTopOverride = 6,
            ContentMarginBottomOverride = 6,
        };

        var panelLight = new StyleBoxFlat
        {
            BackgroundColor = casing,
            BorderColor = Color.FromHex("#5c4633"),
            BorderThickness = new Thickness(1),
        };
        var panelDark = new StyleBoxFlat
        {
            BackgroundColor = casingDark,
            BorderColor = Color.FromHex("#3d2f22"),
            BorderThickness = new Thickness(1),
        };
        var panelPositive = new StyleBoxFlat
        {
            BackgroundColor = Color.FromHex("#061a08"),
            BorderColor = Color.FromHex("#146b1d"),
            BorderThickness = new Thickness(1),
        };
        var panelNegative = new StyleBoxFlat
        {
            BackgroundColor = Color.FromHex("#210704"),
            BorderColor = Color.FromHex("#701205"),
            BorderThickness = new Thickness(1),
        };
        var panelHighlight = new StyleBoxFlat
        {
            BackgroundColor = Color.FromHex("#262005"),
            BorderColor = Color.FromHex("#877114"),
            BorderThickness = new Thickness(1),
        };

        return
        [
            // Default Label and general typography overrides
            E<Label>()
                .Font(terminalFont)
                .FontColor(amber),
            E<Label>()
                .Class(StyleClass.FontLarge)
                .Font(terminalFontLarge)
                .FontColor(amber),
            E<Label>()
                .Class(StyleClass.FontSmall)
                .Font(terminalFontSmall)
                .FontColor(amberDim),
            E<Label>()
                .Class(StyleClass.LabelHeading)
                .Font(terminalFontLarge)
                .FontColor(amber),
            E<Label>()
                .Class(StyleClass.LabelHeadingBigger)
                .Font(terminalFontLarge)
                .FontColor(amber),
            E<Label>()
                .Class(StyleClass.LabelSubText)
                .Font(terminalFontSmall)
                .FontColor(amberDim),
            E<Label>()
                .Class(StyleClass.LabelKeyText)
                .Font(terminalFont)
                .FontColor(amber),
            E<Label>()
                .Class(StyleClass.LabelWeak)
                .Font(terminalFontSmall)
                .FontColor(amberDim),
            E<Label>()
                .Class(StyleClass.LabelMonospaceText)
                .Font(terminalFont)
                .FontColor(amber),
            E<Label>()
                .Class(StyleClass.LabelMonospaceHeading)
                .Font(terminalFontLarge)
                .FontColor(amber),
            E<Label>()
                .Class(StyleClass.LabelMonospaceSubHeading)
                .Font(terminalFont)
                .FontColor(amberDim),

            E<Label>()
                .Class(ContainerButton.StyleClassButton)
                .Font(terminalFont)
                .FontColor(amber),
            E<Label>()
                .Class(ContainerButton.StyleClassButton)
                .PseudoDisabled()
                .FontColor(amberDim),
            E<Label>()
                .Class(DefaultWindow.StyleClassWindowTitle)
                .Font(terminalFontLarge)
                .FontColor(amber),
            E<Label>()
                .Class("windowTitleAlert")
                .Font(terminalFontLarge)
                .FontColor(amber),
            E<Label>()
                .Class("FancyWindowTitle")
                .Font(terminalFontLarge)
                .FontColor(amber),
            E<Label>()
                .Class("WindowFooterText")
                .Font(terminalFontSmall)
                .FontColor(amberDim),
            E<RichTextLabel>()
                .Font(terminalFont)
                .FontColor(amber),
            E<TextEdit>()
                .Font(terminalFont)
                .FontColor(amber)
                .Prop(TextEdit.StylePropertyCursorColor, amber)
                .Prop(TextEdit.StylePropertySelectionColor, amberDark),
            E<TextEdit>()
                .Pseudo(TextEdit.StylePseudoClassPlaceholder)
                .FontColor(amberDim),
            E<LineEdit>()
                .Prop(LineEdit.StylePropertyStyleBox, screenPanel)
                .Prop("font-color", amber)
                .Prop("cursor-color", amber)
                .Prop("font", terminalFont),
            E<LineEdit>()
                .Class(LineEdit.StyleClassLineEditNotEditable)
                .Prop("font-color", amberDim),
            E<LineEdit>()
                .Pseudo(LineEdit.StylePseudoClassPlaceholder)
                .Prop("font-color", amberDim),

            // Panels, Titles, Headers, Close Button
            E().Class(DefaultWindow.StyleClassWindowPanel).Panel(rootPanel),
            E().Class(DefaultWindow.StyleClassWindowHeader).Panel(casingPanel),
            E().Class(StyleClass.AlertWindowHeader).Panel(casingPanel),
            E().Class(StyleClass.BorderedWindowPanel).Panel(screenPanel),

            E<TextureButton>()
                .Class(DefaultWindow.StyleClassWindowCloseButton)
                .PseudoNormal()
                .Modulate(danger),
            E<TextureButton>()
                .Class(DefaultWindow.StyleClassWindowCloseButton)
                .PseudoHovered()
                .Modulate(dangerHover),
            E<TextureButton>()
                .Class(DefaultWindow.StyleClassWindowCloseButton)
                .PseudoPressed()
                .Modulate(dangerPressed),
            E<TextureButton>()
                .Class(DefaultWindow.StyleClassWindowCloseButton)
                .PseudoDisabled()
                .Modulate(disabled),

            E<TextureButton>()
                .Class(FancyWindow.StyleClassWindowHelpButton)
                .PseudoNormal()
                .Modulate(amberDim),
            E<TextureButton>()
                .Class(FancyWindow.StyleClassWindowHelpButton)
                .PseudoHovered()
                .Modulate(amber),
            E<TextureButton>()
                .Class(FancyWindow.StyleClassWindowHelpButton)
                .PseudoPressed()
                .Modulate(amberDim),

            // Default Button and Variations
            E<ContainerButton>()
                .Class(ContainerButton.StyleClassButton)
                .PseudoNormal()
                .Box(buttonBase)
                .Modulate(Color.White),
            E<ContainerButton>()
                .Class(ContainerButton.StyleClassButton)
                .PseudoHovered()
                .Box(buttonHover)
                .Modulate(Color.White),
            E<ContainerButton>()
                .Class(ContainerButton.StyleClassButton)
                .PseudoPressed()
                .Box(buttonPressed)
                .Modulate(Color.White),
            E<ContainerButton>()
                .Class(ContainerButton.StyleClassButton)
                .PseudoDisabled()
                .Box(buttonDisabled)
                .Modulate(Color.White),

            // Button variants
            E<ContainerButton>().Class(ContainerButton.StyleClassButton).Class(StyleClass.ButtonOpenLeft).PseudoNormal().Box(buttonBase).Modulate(Color.White),
            E<ContainerButton>().Class(ContainerButton.StyleClassButton).Class(StyleClass.ButtonOpenLeft).PseudoHovered().Box(buttonHover).Modulate(Color.White),
            E<ContainerButton>().Class(ContainerButton.StyleClassButton).Class(StyleClass.ButtonOpenLeft).PseudoPressed().Box(buttonPressed).Modulate(Color.White),
            E<ContainerButton>().Class(ContainerButton.StyleClassButton).Class(StyleClass.ButtonOpenLeft).PseudoDisabled().Box(buttonDisabled).Modulate(Color.White),

            E<ContainerButton>().Class(ContainerButton.StyleClassButton).Class(StyleClass.ButtonOpenRight).PseudoNormal().Box(buttonBase).Modulate(Color.White),
            E<ContainerButton>().Class(ContainerButton.StyleClassButton).Class(StyleClass.ButtonOpenRight).PseudoHovered().Box(buttonHover).Modulate(Color.White),
            E<ContainerButton>().Class(ContainerButton.StyleClassButton).Class(StyleClass.ButtonOpenRight).PseudoPressed().Box(buttonPressed).Modulate(Color.White),
            E<ContainerButton>().Class(ContainerButton.StyleClassButton).Class(StyleClass.ButtonOpenRight).PseudoDisabled().Box(buttonDisabled).Modulate(Color.White),

            E<ContainerButton>().Class(ContainerButton.StyleClassButton).Class(StyleClass.ButtonOpenBoth).PseudoNormal().Box(buttonBase).Modulate(Color.White),
            E<ContainerButton>().Class(ContainerButton.StyleClassButton).Class(StyleClass.ButtonOpenBoth).PseudoHovered().Box(buttonHover).Modulate(Color.White),
            E<ContainerButton>().Class(ContainerButton.StyleClassButton).Class(StyleClass.ButtonOpenBoth).PseudoPressed().Box(buttonPressed).Modulate(Color.White),
            E<ContainerButton>().Class(ContainerButton.StyleClassButton).Class(StyleClass.ButtonOpenBoth).PseudoDisabled().Box(buttonDisabled).Modulate(Color.White),

            E<ContainerButton>().Class(ContainerButton.StyleClassButton).Class(StyleClass.ButtonSquare).PseudoNormal().Box(buttonBase).Modulate(Color.White),
            E<ContainerButton>().Class(ContainerButton.StyleClassButton).Class(StyleClass.ButtonSquare).PseudoHovered().Box(buttonHover).Modulate(Color.White),
            E<ContainerButton>().Class(ContainerButton.StyleClassButton).Class(StyleClass.ButtonSquare).PseudoPressed().Box(buttonPressed).Modulate(Color.White),
            E<ContainerButton>().Class(ContainerButton.StyleClassButton).Class(StyleClass.ButtonSquare).PseudoDisabled().Box(buttonDisabled).Modulate(Color.White),

            E<ContainerButton>().Class(ContainerButton.StyleClassButton).Class(StyleClass.ButtonSmall).PseudoNormal().Box(buttonBase).Modulate(Color.White),
            E<ContainerButton>().Class(ContainerButton.StyleClassButton).Class(StyleClass.ButtonSmall).PseudoHovered().Box(buttonHover).Modulate(Color.White),
            E<ContainerButton>().Class(ContainerButton.StyleClassButton).Class(StyleClass.ButtonSmall).PseudoPressed().Box(buttonPressed).Modulate(Color.White),
            E<ContainerButton>().Class(ContainerButton.StyleClassButton).Class(StyleClass.ButtonSmall).PseudoDisabled().Box(buttonDisabled).Modulate(Color.White),

            E<ContainerButton>().Class(ContainerButton.StyleClassButton).ParentOf(E<Label>()).Font(terminalFont).FontColor(amber),
            E<ContainerButton>().Class(ContainerButton.StyleClassButton).PseudoDisabled().ParentOf(E<Label>()).FontColor(amberDim),

            // Positive / Negative / Danger Buttons
            E<ContainerButton>().Class(ContainerButton.StyleClassButton).Class(StyleClass.Positive).PseudoNormal().Box(positiveButtonBase).Modulate(Color.White),
            E<ContainerButton>().Class(ContainerButton.StyleClassButton).Class(StyleClass.Positive).PseudoHovered().Box(positiveButtonHover).Modulate(Color.White),
            E<ContainerButton>().Class(ContainerButton.StyleClassButton).Class(StyleClass.Positive).PseudoPressed().Box(positiveButtonPressed).Modulate(Color.White),
            E<ContainerButton>().Class(ContainerButton.StyleClassButton).Class(StyleClass.Positive).PseudoDisabled().Box(buttonDisabled).Modulate(Color.White),
            E<ContainerButton>().Class(ContainerButton.StyleClassButton).Class(StyleClass.Positive).ParentOf(E<Label>()).FontColor(Color.FromHex("#4dff62")),
            E<ContainerButton>().Class(ContainerButton.StyleClassButton).Class(StyleClass.Positive).PseudoDisabled().ParentOf(E<Label>()).FontColor(amberDim),

            E<ContainerButton>().Class(ContainerButton.StyleClassButton).Class(StyleClass.Negative).PseudoNormal().Box(negativeButtonBase).Modulate(Color.White),
            E<ContainerButton>().Class(ContainerButton.StyleClassButton).Class(StyleClass.Negative).PseudoHovered().Box(negativeButtonHover).Modulate(Color.White),
            E<ContainerButton>().Class(ContainerButton.StyleClassButton).Class(StyleClass.Negative).PseudoPressed().Box(negativeButtonPressed).Modulate(Color.White),
            E<ContainerButton>().Class(ContainerButton.StyleClassButton).Class(StyleClass.Negative).PseudoDisabled().Box(buttonDisabled).Modulate(Color.White),
            E<ContainerButton>().Class(ContainerButton.StyleClassButton).Class(StyleClass.Negative).ParentOf(E<Label>()).FontColor(Color.FromHex("#ff4f38")),
            E<ContainerButton>().Class(ContainerButton.StyleClassButton).Class(StyleClass.Negative).PseudoDisabled().ParentOf(E<Label>()).FontColor(amberDim),

            E<ContainerButton>().Class(ContainerButton.StyleClassButton).Class(StyleClass.FunkyAmberButton).MinHeight(44),

            E<ContainerButton>().Class(ContainerButton.StyleClassButton).Class(StyleClass.FunkyAmberDangerButton).MinHeight(52),
            E<ContainerButton>().Class(ContainerButton.StyleClassButton).Class(StyleClass.FunkyAmberDangerButton).PseudoNormal().Box(dangerButtonBase).Modulate(Color.White),
            E<ContainerButton>().Class(ContainerButton.StyleClassButton).Class(StyleClass.FunkyAmberDangerButton).PseudoHovered().Box(dangerButtonHover).Modulate(Color.White),
            E<ContainerButton>().Class(ContainerButton.StyleClassButton).Class(StyleClass.FunkyAmberDangerButton).PseudoPressed().Box(dangerButtonPressed).Modulate(Color.White),
            E<ContainerButton>().Class(ContainerButton.StyleClassButton).Class(StyleClass.FunkyAmberDangerButton).PseudoDisabled().Box(buttonDisabled).Modulate(Color.White),
            E<ContainerButton>().Class(ContainerButton.StyleClassButton).Class(StyleClass.FunkyAmberDangerButton).ParentOf(E<Label>()).Font(terminalFont).FontColor(Color.FromHex("#ffd2a0")),
            E<ContainerButton>().Class(ContainerButton.StyleClassButton).Class(StyleClass.FunkyAmberDangerButton).PseudoDisabled().ParentOf(E<Label>()).FontColor(Palettes.Neutral.TextDark),

            // Checkboxes
            E<TextureRect>().Class(CheckBox.StyleClassCheckBox).PseudoNormal().Modulate(amberDim),
            E<TextureRect>().Class(CheckBox.StyleClassCheckBox).PseudoHovered().Modulate(amber),
            E<TextureRect>().Class(CheckBox.StyleClassCheckBox).Class(CheckBox.StyleClassCheckBoxChecked).PseudoNormal().Modulate(amber),
            E<TextureRect>().Class(CheckBox.StyleClassCheckBox).Class(CheckBox.StyleClassCheckBoxChecked).PseudoHovered().Modulate(Color.FromHex("#ff9a3d")),
            E<CheckBox>().ParentOf(E<Label>()).Font(terminalFont).FontColor(amber),
            E<CheckBox>().PseudoDisabled().ParentOf(E<Label>()).FontColor(amberDim),

            // Dropdowns
            E<TextureRect>().Class(OptionButton.StyleClassOptionTriangle).PseudoNormal().Modulate(amber),
            E<TextureRect>().Class(OptionButton.StyleClassOptionTriangle).PseudoHovered().Modulate(Color.FromHex("#ff9a3d")),
            E<Label>().Class(OptionButton.StyleClassOptionButton).Font(terminalFont).FontColor(amber),
            E<PanelContainer>().Class(OptionButton.StyleClassOptionsBackground).Panel(new StyleBoxFlat
            {
                BackgroundColor = screenBlack,
                BorderColor = amberDark,
                BorderThickness = new Thickness(2),
            }),

            // Scrollbars
            E<VScrollBar>().Prop(ScrollBar.StylePropertyGrabber, retroScrollBarGrabberNormal),
            E<VScrollBar>().PseudoHovered().Prop(ScrollBar.StylePropertyGrabber, retroScrollBarGrabberHover),
            E<VScrollBar>().PseudoPressed().Prop(ScrollBar.StylePropertyGrabber, retroScrollBarGrabberGrabbed),
            E<HScrollBar>().Prop(ScrollBar.StylePropertyGrabber, retroScrollBarGrabberNormal),
            E<HScrollBar>().PseudoHovered().Prop(ScrollBar.StylePropertyGrabber, retroScrollBarGrabberHover),
            E<HScrollBar>().PseudoPressed().Prop(ScrollBar.StylePropertyGrabber, retroScrollBarGrabberGrabbed),

            // Sliders
            E<Slider>()
                .Prop(Slider.StylePropertyBackground, retroSliderBack)
                .Prop(Slider.StylePropertyForeground, retroSliderBack)
                .Prop(Slider.StylePropertyGrabber, retroSliderGrabber)
                .Prop(Slider.StylePropertyFill, retroSliderFill),

            // Progress Bars
            E<ProgressBar>()
                .Prop(ProgressBar.StylePropertyBackground, retroProgressBarBack)
                .Prop(ProgressBar.StylePropertyForeground, retroProgressBarFore),

            // Tab Containers
            E<TabContainer>()
                .Prop(TabContainer.StylePropertyPanelStyleBox, screenPanel)
                .Prop(TabContainer.StylePropertyTabStyleBox, retroTabActive)
                .Prop(TabContainer.StylePropertyTabStyleBoxInactive, retroTabInactive),

            // Panels
            E<PanelContainer>().Class(StyleClass.FunkyAmberRoot).Panel(rootPanel),
            E<PanelContainer>().Class(StyleClass.FunkyAmberPanel).Panel(casingPanel),
            E<PanelContainer>().Class(StyleClass.FunkyAmberScreen).Panel(screenPanel),
            E<PanelContainer>().Class(StyleClass.FunkyAmberDivider).Panel(divider),
            E<Label>().Class(StyleClass.FunkyAmberHeading).Font(terminalFontLarge).FontColor(amber),
            E<Label>().Class(StyleClass.FunkyAmberSubHeading).Font(terminalFont).FontColor(amberDim),
            E<Label>().Class(StyleClass.FunkyAmberStatus).Font(terminalFontSmall).FontColor(amberDim),
            E<RichTextLabel>().Class(StyleClass.FunkyAmberStatus).Font(terminalFontSmall),
            E<Label>().Class(StyleClass.FunkyAmberText).Font(terminalFont).FontColor(amber),
            E<TextEdit>().Class(StyleClass.FunkyAmberText).Font(terminalFont).FontColor(amber).Prop(TextEdit.StylePropertyCursorColor, amber).Prop(TextEdit.StylePropertySelectionColor, amberDark),
            E<TextEdit>().Class(StyleClass.FunkyAmberText).Pseudo(TextEdit.StylePseudoClassPlaceholder).FontColor(amberDim),

            E<PanelContainer>().Class(StyleClass.PanelLight).Panel(panelLight),
            E<PanelContainer>().Class(StyleClass.PanelDark).Panel(panelDark),
            E<PanelContainer>().Class(StyleClass.Positive).Panel(panelPositive),
            E<PanelContainer>().Class(StyleClass.Negative).Panel(panelNegative),
            E<PanelContainer>().Class(StyleClass.Highlight).Panel(panelHighlight),
            E<PanelContainer>().Class("BackgroundDark").Panel(panelDark),
            E().Class(StyleClass.BackgroundPanel).Panel(panelLight).Modulate(Color.White),
            E().Class(StyleClass.BackgroundPanelDark).Panel(panelDark).Modulate(Color.White),
            E().Class(StyleClass.BackgroundPanelOpenLeft).Panel(panelLight).Modulate(Color.White),
            E().Class(StyleClass.BackgroundPanelOpenRight).Panel(panelLight).Modulate(Color.White),

            // Chat overrides
            E<PanelContainer>()
                .Class(ChatInputBox.StyleClassChatPanel)
                .Panel(new StyleBoxFlat
                {
                    BackgroundColor = screenBlack,
                    BorderColor = amberDark,
                    BorderThickness = new Thickness(1),
                }),
            E<LineEdit>()
                .Class(ChatInputBox.StyleClassChatLineEdit)
                .Prop(LineEdit.StylePropertyStyleBox, new StyleBoxEmpty())
                .Prop("font-color", amber)
                .Prop("font", terminalFont),
            E()
                .Class(SeparatedChatGameScreen.StyleClassChatContainer)
                .Panel(new StyleBoxFlat
                {
                    BackgroundColor = casingDark,
                    BorderColor = Color.FromHex("#5c4633"),
                    BorderThickness = new Thickness(1),
                }),
            E<OutputPanel>()
                .Class(SeparatedChatGameScreen.StyleClassChatOutput)
                .Panel(new StyleBoxFlat
                {
                    BackgroundColor = screenBlack,
                    BorderColor = amberDark,
                    BorderThickness = new Thickness(1),
                }),
            E<Button>()
                .Class(ChatInputBox.StyleClassChatFilterOptionButton)
                .Box(buttonBase)
                .Modulate(Color.White),
            E<Button>()
                .Class(ChatInputBox.StyleClassChatFilterOptionButton)
                .PseudoHovered()
                .Box(buttonHover)
                .Modulate(Color.White),
            E<Button>()
                .Class(ChatInputBox.StyleClassChatFilterOptionButton)
                .PseudoPressed()
                .Box(buttonPressed)
                .Modulate(Color.White),
            E<Button>()
                .Class(ChatInputBox.StyleClassChatFilterOptionButton)
                .PseudoDisabled()
                .Box(buttonDisabled)
                .Modulate(Color.White),
            E<Button>()
                .Class(ChatInputBox.StyleClassChatFilterOptionButton)
                .ParentOf(E<Label>())
                .Font(terminalFont)
                .FontColor(amber),
            E<ContainerButton>()
                .Class(ChatInputBox.StyleClassChatFilterOptionButton)
                .Box(buttonBase)
                .Modulate(Color.White),
            E<ContainerButton>()
                .Class(ChatInputBox.StyleClassChatFilterOptionButton)
                .PseudoHovered()
                .Box(buttonHover)
                .Modulate(Color.White),
            E<ContainerButton>()
                .Class(ChatInputBox.StyleClassChatFilterOptionButton)
                .PseudoPressed()
                .Box(buttonPressed)
                .Modulate(Color.White),
            E<ContainerButton>()
                .Class(ChatInputBox.StyleClassChatFilterOptionButton)
                .PseudoDisabled()
                .Box(buttonDisabled)
                .Modulate(Color.White),
            E<ContainerButton>()
                .Class(ChatInputBox.StyleClassChatFilterOptionButton)
                .ParentOf(E<Label>())
                .Font(terminalFont)
                .FontColor(amber),

            // MenuButton overrides for FunkyAmber Theme
            E<MenuButton>().PseudoNormal().Box(menuButtonBase).Modulate(Color.White),
            E<MenuButton>().PseudoHovered().Box(menuButtonHover).Modulate(Color.White),
            E<MenuButton>().PseudoPressed().Box(menuButtonPressed).Modulate(Color.White),
            E<MenuButton>().PseudoDisabled().Box(menuButtonDisabled).Modulate(Color.White),

            E<MenuButton>().Class(StyleClass.ButtonOpenLeft).PseudoNormal().Box(menuButtonOpenLeft).Modulate(Color.White),
            E<MenuButton>().Class(StyleClass.ButtonOpenLeft).PseudoHovered().Box(menuButtonOpenLeftHover).Modulate(Color.White),
            E<MenuButton>().Class(StyleClass.ButtonOpenLeft).PseudoPressed().Box(menuButtonOpenLeftPressed).Modulate(Color.White),
            E<MenuButton>().Class(StyleClass.ButtonOpenLeft).PseudoDisabled().Box(menuButtonOpenLeftDisabled).Modulate(Color.White),

            E<MenuButton>().Class(StyleClass.ButtonOpenRight).PseudoNormal().Box(menuButtonOpenRight).Modulate(Color.White),
            E<MenuButton>().Class(StyleClass.ButtonOpenRight).PseudoHovered().Box(menuButtonOpenRightHover).Modulate(Color.White),
            E<MenuButton>().Class(StyleClass.ButtonOpenRight).PseudoPressed().Box(menuButtonOpenRightPressed).Modulate(Color.White),
            E<MenuButton>().Class(StyleClass.ButtonOpenRight).PseudoDisabled().Box(menuButtonOpenRightDisabled).Modulate(Color.White),

            E<MenuButton>().Class(StyleClass.ButtonSquare).PseudoNormal().Box(menuButtonSquare).Modulate(Color.White),
            E<MenuButton>().Class(StyleClass.ButtonSquare).PseudoHovered().Box(menuButtonSquareHover).Modulate(Color.White),
            E<MenuButton>().Class(StyleClass.ButtonSquare).PseudoPressed().Box(menuButtonSquarePressed).Modulate(Color.White),
            E<MenuButton>().Class(StyleClass.ButtonSquare).PseudoDisabled().Box(menuButtonSquareDisabled).Modulate(Color.White),

            E<MenuButton>().Class(StyleClass.ButtonOpenBoth).PseudoNormal().Box(menuButtonSquare).Modulate(Color.White),
            E<MenuButton>().Class(StyleClass.ButtonOpenBoth).PseudoHovered().Box(menuButtonSquareHover).Modulate(Color.White),
            E<MenuButton>().Class(StyleClass.ButtonOpenBoth).PseudoPressed().Box(menuButtonSquarePressed).Modulate(Color.White),
            E<MenuButton>().Class(StyleClass.ButtonOpenBoth).PseudoDisabled().Box(menuButtonSquareDisabled).Modulate(Color.White),

            // OptionButton overrides for FunkyAmber Theme
            E<OptionButton>().PseudoNormal().Box(buttonBase).Modulate(Color.White),
            E<OptionButton>().PseudoHovered().Box(buttonHover).Modulate(Color.White),
            E<OptionButton>().PseudoPressed().Box(buttonPressed).Modulate(Color.White),
            E<OptionButton>().PseudoDisabled().Box(buttonDisabled).Modulate(Color.White),
        ];
    }
}
