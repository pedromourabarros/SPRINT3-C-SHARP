using ApostasCompulsivas.Repository;
using ApostasCompulsivas.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ApostasCompulsivas
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            // Pegar a connection string
            var dbContext = new DatabaseContext();
            var connectionString = dbContext.GetConnectionString();

            // Repositórios
            services.AddSingleton<IUsuarioRepository>(_ => new UsuarioRepository(connectionString));
            services.AddSingleton<IApostaRepository>(_ => new ApostaRepository(connectionString));
            services.AddSingleton<IHistoricoRepository>(_ => new HistoricoRepository(connectionString));

            // Serviços
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<IApostaService, ApostaService>();
            services.AddScoped<IHistoricoService, HistoricoService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IDetecaoComportamentoService, DetecaoComportamentoService>();
            services.AddScoped<IIntervencaoService, IntervencaoService>();
            services.AddScoped<IAtividadeAlternativaService, AtividadeAlternativaService>();
            services.AddScoped<IRelatorioComportamentalService, RelatorioComportamentalService>();
        }
    }
}
