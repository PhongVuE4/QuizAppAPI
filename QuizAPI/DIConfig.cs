using Microsoft.Extensions.DependencyInjection;
using Quiz_Infrastructure;
using Quiz_Infrastructure.Repository;
using Quiz_Interfaces.IRepository;
using AutoMapper;

namespace QuizAPI
{
    public static class DIConfig
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
        {
            //Add Repository
            services.AddScoped<IQuestionRepository, QuestionRepository>();

            // Register MongoDbContext
            services.AddSingleton<MongoDBContext>(sp => new MongoDBContext(sp.GetRequiredService<IConfiguration>()));

            //Register Mapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            //Register BackgroundService

            return services;
        }
    }
}
