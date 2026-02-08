namespace CleanArchitecture.Blazor.Server.UI.Themes;

public static class Theme
{
    public static MudTheme ApplicationTheme()
    {
        var theme = new MudTheme
        {
            PaletteLight = new PaletteLight
            {
                Primary = "#4F46E5", // Indigo-600, vibrant medical professional
                PrimaryContrastText = "#ffffff",
                PrimaryDarken = "#4338CA", // Indigo-700
                PrimaryLighten = "#6366F1", // Indigo-500
                Secondary = "#6B7280", // Gray-500, clean neutral
                SecondaryContrastText = "#ffffff",
                SecondaryLighten = "#9CA3AF", // Gray-400
                SecondaryDarken = "#4B5563", // Gray-600
                Success = "#10b981", // Emerald-500
                Info = "#3B82F6", // Blue-500
                Tertiary = "#8B5CF6", // Violet-500
                TertiaryContrastText = "#ffffff",
                TertiaryDarken = "#7C3AED", // Violet-600
                TertiaryLighten = "#A78BFA", // Violet-400

                Warning = "#F59E0B", // Amber-500
                WarningContrastText = "#92400E", // Amber-800
                WarningDarken = "#D97706", // Amber-600
                WarningLighten = "#FBBF24", // Amber-400

                Error = "#EF4444", // Red-500
                ErrorContrastText = "#ffffff",
                ErrorDarken = "#DC2626", // Red-600
                ErrorLighten = "#F87171", // Red-400

                Black = "#111827", // Gray-900
                White = "#ffffff",
                AppbarBackground = "#ffffff", // Pure white AppBar
                AppbarText = "#111827",
                Background = "#F5F6FA", // Very light lavender-gray, MedEx style
                BackgroundGray = "#F8F9FC", // Alternative soft gray-lavender
                Surface = "#ffffff",
                DrawerBackground = "#FAFBFC", // Very subtle off-white
                TextPrimary = "#111827", // Gray-900
                TextSecondary = "#6B7280", // Gray-500

                TextDisabled = "#9CA3AF", // Gray-400
                ActionDefault = "#374151", // Gray-700
                ActionDisabled = "rgba(107, 114, 128, 0.4)",
                ActionDisabledBackground = "rgba(107, 114, 128, 0.1)",
                Divider = "#E5E7EB", // Gray-200
                DividerLight = "#F3F4F6", // Gray-100
                TableLines = "#E5E7EB",
                LinesDefault = "#E5E7EB",
                LinesInputs = "#D1D5DB", // Gray-300
            },
            PaletteDark = new PaletteDark
            {
                Primary = "#818CF8", // Indigo-400. Vibrant on dark
                PrimaryContrastText = "#1E1B4B", // Indigo-950
                PrimaryDarken = "#6366F1", // Indigo-500
                PrimaryLighten = "#A5B4FC", // Indigo-300
                Secondary = "#78716c", // Neutral gray
                Success = "#22c55e", // Green for success
                Info = "#0ea5e9", // Sky blue for info (shadcn sky-500)
                InfoDarken = "#0284c7", // Darker sky blue (shadcn sky-600)
                InfoLighten = "#38bdf8", // Lighter sky blue (shadcn sky-400)

                Tertiary = "#6366f1",
                TertiaryContrastText = "#fafafa",
                TertiaryDarken = "#4f46e5",
                TertiaryLighten = "#818cf8",

                Warning = "#f59e0b", // Orange for warning
                WarningContrastText = "#fafafa",
                WarningDarken = "#d97706",
                WarningLighten = "#fbbf24",

                Error = "#dc2626", // Red for error
                ErrorContrastText = "#fafafa",
                ErrorDarken = "#b91c1c",
                ErrorLighten = "#ef4444",

                Black = "#020817",
                White = "#fafafa",
                Background = "#0c0a09", // shadcn/ui dark background
                Surface = "#171717", // Deeper surface color
                AppbarBackground = "#0c0a09",
                AppbarText = "#fafafa",
                DrawerText = "#fafafa",
                DrawerBackground = "#0c0a09",
                TextPrimary = "#fafafa", // shadcn/ui white text
                TextSecondary = "#a1a1aa", // Neutral gray secondary text
                TextDisabled = "rgba(161, 161, 170, 0.5)",
                ActionDefault = "#e5e5e5",
                ActionDisabled = "rgba(161, 161, 170, 0.3)",
                ActionDisabledBackground = "rgba(161, 161, 170, 0.1)",
                Divider = "rgba(255, 255, 255, 0.1)", // shadcn/ui divider color
                DividerLight = "rgba(161, 161, 170, 0.1)",
                TableLines = "rgba(255, 255, 255, 0.1)",
                LinesDefault = "rgba(255, 255, 255, 0.1)",
                LinesInputs = "rgba(161, 161, 170, 0.2)",
                DarkContrastText = "#020817",
                SecondaryContrastText = "#fafafa",
                SecondaryDarken = "#57534e",
                SecondaryLighten = "#a8a29e"

            },
            LayoutProperties = new LayoutProperties
            {
                AppbarHeight = "64px",
                DefaultBorderRadius = "12px", // Rounded, modern feel
                DrawerWidthLeft = "270px",
                DrawerMiniWidthRight = "260px"
            },
            Typography = new Typography
            {
                Default = new DefaultTypography
                {
                    FontSize = ".875rem",
                    FontWeight = "400",
                    LineHeight = "1.43",
                    LetterSpacing = "normal",
                    FontFamily = ["Inter var", "Inter", "ui-sans-serif", "system-ui", "-apple-system", "Segoe UI", "Roboto", "Helvetica Neue", "Arial", "Noto Sans", "sans-serif", "Apple Color Emoji", "Segoe UI Emoji"]
                },
                H1 = new H1Typography
                {
                    FontSize = "2.25rem",
                    FontWeight = "800",
                    LineHeight = "2.5rem",
                    LetterSpacing = "-0.025em"
                },
                H2 = new H2Typography
                {
                    FontSize = "1.875rem",
                    FontWeight = "600",
                    LineHeight = "2.25rem",
                    LetterSpacing = "-0.025em"
                },
                H3 = new H3Typography
                {
                    FontSize = "1.5rem",
                    FontWeight = "600",
                    LineHeight = "2rem",
                    LetterSpacing = "-0.025em"
                },
                H4 = new H4Typography
                {
                    FontSize = "1.25rem",
                    FontWeight = "600",
                    LineHeight = "1.75rem",
                    LetterSpacing = "-0.025em"
                },
                H5 = new H5Typography
                {
                    FontSize = "1.125rem",
                    FontWeight = "600",
                    LineHeight = "1.75rem",
                    LetterSpacing = "-0.025em"
                },
                H6 = new H6Typography
                {
                    FontSize = "1rem",
                    FontWeight = "600",
                    LineHeight = "1.25rem",
                    LetterSpacing = "-0.025em"
                },
                Button = new ButtonTypography
                {
                    FontSize = ".875rem",
                    FontWeight = "500",
                    LineHeight = "1.75rem",
                    LetterSpacing = "normal",
                    TextTransform = "none"
                },
                Subtitle1 = new Subtitle1Typography
                {
                    FontSize = ".875rem",
                    FontWeight = "400",
                    LineHeight = "1.5rem",
                    LetterSpacing = ".00938em",
                },
                Subtitle2 = new Subtitle2Typography
                {
                    FontSize = "1rem",
                    FontWeight = "500",
                    LineHeight = "1.75rem",
                    LetterSpacing = ".00714em"
                },
                Body1 = new Body1Typography
                {
                    FontSize = ".875rem",
                    FontWeight = "400",
                    LineHeight = "1.5rem",
                    LetterSpacing = ".00938em"
                },
                Body2 = new Body2Typography
                {
                    FontSize = ".75rem",
                    FontWeight = "400",
                    LineHeight = "1.25rem",
                    LetterSpacing = ".01071em"
                },
                Caption = new CaptionTypography
                {
                    FontSize = "0.75rem",
                    FontWeight = "400",
                    LineHeight = "1.5rem",
                    LetterSpacing = ".03333em"
                },
                Overline = new OverlineTypography
                {
                    FontSize = "0.75rem",
                    FontWeight = "400",
                    LineHeight = "1.75rem",
                    LetterSpacing = ".03333em",
                    TextTransform = "none"
                }
            }
        };
        return theme;
    }
}