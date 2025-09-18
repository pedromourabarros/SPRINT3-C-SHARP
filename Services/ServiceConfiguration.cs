using Microsoft.Extensions.DependencyInjection;
using ApostasCompulsivas.Repository;
using ApostasCompulsivas.Services;

namespace ApostasCompulsivas.Services
{
    /// <summary>
    /// Configuração de serviços para injeção de dependência
    /// </summary>
    public static class ServiceConfiguration
    {
        /// <summary>
        /// Configura todos os serviços necessários para a aplicação
        /// </summary>
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            // Configurar banco de dados
            services.AddSingleton<DatabaseContext>();
            
            // Configurar repositórios
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IApostaRepository, ApostaRepository>();
            services.AddScoped<IHistoricoRepository, HistoricoRepository>();
            
            // Configurar serviços de negócio
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<IApostaService, ApostaService>();
            services.AddScoped<IHistoricoService, HistoricoService>();
            services.AddScoped<IFileService, FileService>();
            
            // Configurar serviços de detecção e intervenção
            services.AddScoped<IDetecaoComportamentoService, DetecaoComportamentoService>();
            services.AddScoped<IIntervencaoService, IntervencaoService>();
            services.AddScoped<IAtividadeAlternativaService, AtividadeAlternativaService>();
            services.AddScoped<IRelatorioComportamentalService, RelatorioComportamentalService>();
            
            return services;
        }
    }
}
