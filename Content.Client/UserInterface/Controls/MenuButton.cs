using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using Robust.Client.Graphics;
using Robust.Client.Input;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.Graphics;
using Robust.Shared.Input;
using Robust.Shared.Utility;

using Content.Shared.CCVar;
using Robust.Shared.Configuration;
using Robust.Shared.IoC;

namespace Content.Client.UserInterface.Controls;

public sealed class MenuButton : ContainerButton
{
    [Dependency] private readonly IInputManager _inputManager = default!;
    [Dependency] private readonly IConfigurationManager _cfg = default!;

    public const string StyleClassLabelTopButton = "topButtonLabel";
    // public const string StyleClassRedTopButton = "topButtonLabel";

    private bool IsFunkyAmberEnabled => _cfg.IsCVarRegistered(CCVars.FunkyAmberEnabled.Name) && _cfg.GetCVar(CCVars.FunkyAmberEnabled);
    private Color ColorNormal => IsFunkyAmberEnabled ? Color.FromHex("#ff6a00") : Color.FromHex("#99a7b3");
    private Color ColorHovered => IsFunkyAmberEnabled ? Color.FromHex("#ff9a3d") : Color.FromHex("#acbac6");
    private Color ColorPressed => IsFunkyAmberEnabled ? Color.FromHex("#994400") : Color.FromHex("#75838e");

    private const float VertPad = 4f;

    private BoundKeyFunction? _function;
    private readonly BoxContainer _root;
    private readonly TextureRect? _buttonIcon;
    private readonly Label? _buttonLabel;

    public string AppendStyleClass { set => AddStyleClass(value); }
    public Texture? Icon { get => _buttonIcon!.Texture; set => _buttonIcon!.Texture = value; }

    public BoundKeyFunction? BoundKey
    {
        get => _function;
        set
        {
            _function = value;
            _buttonLabel!.Text = _function == null ? "" : BoundKeyHelper.ShortKeyName(_function.Value);
        }
    }

    public BoxContainer ButtonRoot => _root;

    public MenuButton()
    {
        IoCManager.InjectDependencies(this);
        _buttonIcon = new TextureRect()
        {
            TextureScale = new Vector2(0.5f, 0.5f),
            HorizontalAlignment = HAlignment.Center,
            VerticalAlignment = VAlignment.Center,
            VerticalExpand = true,
            Margin = new Thickness(0, VertPad),
            ModulateSelfOverride = ColorNormal,
            Stretch = TextureRect.StretchMode.KeepCentered
        };
        _buttonLabel = new Label
        {
            Text = "",
            HorizontalAlignment = HAlignment.Center,
            ModulateSelfOverride = ColorNormal,
            StyleClasses = {StyleClassLabelTopButton}
        };
        _root = new BoxContainer
        {
            Orientation = BoxContainer.LayoutOrientation.Vertical,
            Children =
            {
                _buttonIcon,
                _buttonLabel
            }
        };
        AddChild(_root);
        ToggleMode = true;
    }

    protected override void EnteredTree()
    {
        _inputManager.OnKeyBindingAdded += OnKeyBindingChanged;
        _inputManager.OnKeyBindingRemoved += OnKeyBindingChanged;
        _inputManager.OnInputModeChanged += OnKeyBindingChanged;
    }

    protected override void ExitedTree()
    {
        _inputManager.OnKeyBindingAdded -= OnKeyBindingChanged;
        _inputManager.OnKeyBindingRemoved -= OnKeyBindingChanged;
        _inputManager.OnInputModeChanged -= OnKeyBindingChanged;
    }


    private void OnKeyBindingChanged(IKeyBinding obj)
    {
        _buttonLabel!.Text = _function == null ? "" : BoundKeyHelper.ShortKeyName(_function.Value);
    }

    private void OnKeyBindingChanged()
    {
        _buttonLabel!.Text = _function == null ? "" : BoundKeyHelper.ShortKeyName(_function.Value);
    }

    protected override void StylePropertiesChanged()
    {
        // colors of children depend on style, so ensure we update when style is changed
        base.StylePropertiesChanged();
        UpdateChildColors();
    }

    private void UpdateChildColors()
    {
        if (_buttonIcon == null || _buttonLabel == null) return;
        switch (DrawMode)
        {
            case DrawModeEnum.Normal:
                _buttonIcon.ModulateSelfOverride = ColorNormal;
                _buttonLabel.ModulateSelfOverride = ColorNormal;
                break;

            case DrawModeEnum.Pressed:
                _buttonIcon.ModulateSelfOverride = ColorPressed;
                _buttonLabel.ModulateSelfOverride = ColorPressed;
                break;

            case DrawModeEnum.Hover:
                _buttonIcon.ModulateSelfOverride = ColorHovered;
                _buttonLabel.ModulateSelfOverride = ColorHovered;
                break;

            case DrawModeEnum.Disabled:
                break;
        }
    }


    protected override void DrawModeChanged()
    {
        base.DrawModeChanged();
        UpdateChildColors();
    }
}
