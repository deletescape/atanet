namespace Atanet.WebApi
{
    using AutoMapper;
    using FluentValidation;
    using FluentValidation.AspNetCore;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http.Features;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using Atanet.DataAccess.Context;
    using Atanet.Model.Settings;
    using Atanet.Services;
    using Atanet.Services.ApiResult;
    using Atanet.Services.Assembly;
    using Atanet.Services.BusinessRules.Interfaces;
    using Atanet.Services.BusinessRules.Registry;
    using Atanet.Services.BusinessRules.Registry.Interfaces;
    using Atanet.Services.Comments;
    using Atanet.Services.Common;
    using Atanet.Services.Files;
    using Atanet.Services.Posts;
    using Atanet.Services.UoW;
    using Atanet.Services.Validation;
    using Atanet.Validation.Dto.Newsletter;
    using Atanet.WebApi.Infrastructure;
    using Atanet.WebApi.Infrastructure.Filters;
    using Atanet.WebApi.Infrastructure.Middleware;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Data.SqlClient;
    using System.Net.NetworkInformation;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Atanet.WebApi.Infrastructure.Authorization;
    using Atanet.Services.Authentication;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.AspNetCore.Http;
    using Atanet.Services.Scoring;
    using Atanet.Model.Mappings.User;
    using Atanet.Services.Posts.Reactions;

    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            this.Configuration = configuration;
            this.HostingEnvironment = hostingEnvironment;
        }

        public IConfiguration Configuration { get; }

        public IHostingEnvironment HostingEnvironment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue;
                x.MultipartHeadersLengthLimit = int.MaxValue;
            });

            services.AddOptions();
                        services.Configure<AtanetSettings>(this.Configuration.GetSection("AtanetSettings"));
            services.AddSingleton<IConfiguration>(this.Configuration);
            services.AddSingleton<AtanetSettings>(x => x.GetService<IOptions<AtanetSettings>>().Value);
            services.AddSingleton<IApiResultService, ApiResultService>();

            services.AddCors(x => x.AddDefaultPolicy(builder => builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin()
                .AllowCredentials()));

            services.AddAutoMapper(x => x.AddProfiles(this.GetAssemblies().ToList()));
            var mvc = services.AddMvc(config =>
            {
                config.Filters.Add(typeof(ValidateActionFilter));
            });

            mvc.AddFluentValidation(fv =>
            {
                fv.ValidatorFactoryType = typeof(ValidationService);
                foreach (var assembly in AssemblyUtilities.GetAtanetAssemblies())
                {
                    fv.RegisterValidatorsFromAssembly(assembly);
                }
            });

            var apiResultServiceInstance = services.BuildServiceProvider().GetService<IApiResultService>();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.SecurityTokenValidators.Clear();
                o.SecurityTokenValidators.Add(new GoogleTokenValidator(apiResultServiceInstance));
            });

            services.AddTransient<IValidatorFactory, ValidationService>();
            mvc.AddMvcOptions(o => o.Filters.Add(typeof(GlobalExceptionFilter)));
            services.AddSwaggerGen(x => x.OperationFilter<SwaggerFilter>());
            var connectionString = new ConnectionStringBuilder().ConstructConnectionStringFromEnvironment();
            services.AddDbContext<AtanetDbContext>(options =>
            {
                options.UseMySql(connectionString);
            });

            services.AddSingleton<IUnitOfWorkFactory, UnitOfWorkFactory>();
            this.ConfigureBusinessRules(services);
            services.AddScoped<IPostCreationService, PostCreationService>();
            services.AddScoped<IPostFilterService, PostFilterService>();
            services.AddScoped<ICommentCreationService, CommentCreationService>();
            services.AddScoped<ICommentFilterService, CommentFilterService>();
            services.AddScoped<IFileCreationService, FileCreationService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPictureService, PictureService>();
            services.AddScoped<IQueryService, QueryService>();
            services.AddScoped<IScoreService, ScoreService>();
            services.AddScoped<IPostReactionCreationService, PostReactionCreationService>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IAssemblyContainer>(x => new AssemblyContainer(this.GetAssemblies()));
            services.AddSingleton<IPagingValidator, PagingValidator>();
            services.AddSingleton<IBusinessRuleRegistry, BaseBusinessRuleRegistry>();
            services.AddSingleton<IConnectionStringBuilder, ConnectionStringBuilder>();
            ServiceLocator.SetServiceLocator(() => services.BuildServiceProvider());

            var context = services.BuildServiceProvider().GetService<AtanetDbContext>();
            context.Database.EnsureCreated();

            // This instance needs to be created for the compiler to reference the Atanet.Validation assembly
            var instance = new PostDtoValidator();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            // Specify URL here, if it should run under a sub-url
            const string subUrl = "";
            app.Map(subUrl, x => this.ConfigureCore(x, env, serviceProvider));
        }

        private void ConfigureCore(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            app.UseCors();
            app.UseAuthentication().UseMvc();
            app.UseMiddleware<NotFoundMiddleware>();
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUi();
            }
        }

        private void ConfigureBusinessRules(IServiceCollection services)
        {
            var atanetAssemblies = this.GetAssemblies();
            foreach (var atanetAssembly in atanetAssemblies)
            {
                var types = atanetAssembly.GetTypes();
                var businessRules = types.Where(x => x.GetInterfaces().Contains(typeof(IBusinessRuleBase)) && !x.IsInterface);
                foreach (var businessRule in businessRules)
                {
                    if (!businessRule.IsAbstract && !businessRule.IsGenericType)
                    {
                        services.AddTransient(businessRule);
                    }
                }
            }
        }            

        private IEnumerable<Assembly> GetAssemblies() =>
            AssemblyUtilities.GetAtanetAssemblies();
    }
}
