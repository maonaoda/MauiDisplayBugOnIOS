using Microsoft.Extensions.Logging;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

#nullable disable
namespace MauiApp6
{
    public class CustomGrid : Grid { }

    // temporary workaround for https://github.com/dotnet/maui/issues/17885
    public class CustomGridHandler : LayoutHandler
    {
#if IOS
        protected override CustomLayoutView CreatePlatformView()
        {
            return new CustomLayoutView()
            {
                CrossPlatformLayout = VirtualView
            };
        }

        public class CustomLayoutView : Microsoft.Maui.Platform.LayoutView
        {
            double _lastMeasureHeight = double.NaN;
            double _lastMeasureWidth = double.NaN;

            public override void LayoutSubviews()
            {
                base.LayoutSubviews();

                var bounds = AdjustForSafeArea(Bounds).ToRectangle();

                var widthConstraint = bounds.Width;

                var size = CrossPlatformLayout.CrossPlatformMeasure(widthConstraint, double.PositiveInfinity);
                if (!(_lastMeasureHeight == size.Height && _lastMeasureWidth == size.Width) &&
                    CrossPlatformLayout is VisualElement element &&
                    (double.IsNaN(_lastMeasureHeight) || Math.Abs(_lastMeasureHeight - size.Height) > 0.001))
                {
                    CacheMeasureConstraintsV2(size.Width, size.Height);
                    element.InvalidateMeasureNonVirtual(Microsoft.Maui.Controls.Internals.InvalidationTrigger.Undefined);
                }
            }

            protected void CacheMeasureConstraintsV2(double widthConstraint, double heightConstraint)
            {
                _lastMeasureWidth = widthConstraint;
                _lastMeasureHeight = heightConstraint;
            }
        }
#endif
    }

    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureMauiHandlers(h =>
                {
                    h.AddHandler(typeof(CustomGrid), typeof(CustomGridHandler));
                })
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
