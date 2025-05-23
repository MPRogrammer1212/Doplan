﻿using Doplan.Data;
using Doplan.Repository;
using Doplan.Services;
using Microsoft.Extensions.Logging;

namespace Doplan;

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
			});


        builder.Services.AddMauiBlazorWebView();
        builder.Services.AddSingleton<DatabaseService>();
		builder.Services.AddSingleton<IRepositories, Repositories>();

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
