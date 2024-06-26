﻿using SimpleToolkit.Core;
using SimpleToolkit.SimpleShell;

using Microsoft.Extensions.Logging;

namespace ServiceClient
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .UseSimpleShell()
                .UseSimpleToolkit();

#if ANDROID || IOS
            builder.DisplayContentBehindBars();
#endif

            return builder.Build();
        }
    }
}
