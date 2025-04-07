using Microsoft.OpenApi.Models;

namespace backend.Infraestructure
{
    public static class SwaggerConfig
    {
        public static void AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "API Scrapper", Version = "v1" });

                options.AddSecurityDefinition("BasicAuth", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "basic",
                    Description = "Autenticación básica. Usa las credenciales de usuario: 'usuario' y 'contraseña'."
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "BasicAuth"
                            }
                        },
                        new string[] { }
                    }
                });
            });
        }
    }
}
