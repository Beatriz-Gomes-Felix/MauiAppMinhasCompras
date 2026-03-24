using Microsoft.Extensions.Logging;
using System.Globalization; // ADICIONADO PARA REGIONALIZAÇÃO R$

namespace MauiAppMinhasCompras
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
                });

            // INÍCIO DA CONFIGURAÇÃO DE REGIONALIZAÇÃO (AGENDA 06)
            var culture = new CultureInfo("pt-BR");
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
            // FIM DA CONFIGURAÇÃO

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}