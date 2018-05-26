using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Eleks_2018_MicroSocialMedia.AuthHelpers;
using Eleks_2018_MicroSocialMedia.AuthHelpers.interfaces;
using Eleks_2018_MicroSocialMedia.Data;
using Eleks_2018_MicroSocialMedia.Hubs;
using Eleks_2018_MicroSocialMedia.ReadModels;
using Eleks_2018_MicroSocialMedia.Repositories;
using Eleks_2018_MicroSocialMedia.Repositories.Interfaces;
using Eleks_2018_MicroSocialMedia.Services;
using Eleks_2018_MicroSocialMedia.Services.Interfaces;
using Eleks_2018_MicroSocialMedia.UoW;
using Eleks_2018_MicroSocialMedia.UoW.Interfaces;
using Eleks_2018_MicroSocialMedia.WriteModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Eleks_2018_MicroSocialMedia
{
    public class Startup
    {
        private const string SecretKey = "3AJdk1414k3jkljioDAJSKLJKLjkl4kljklsjd";
        private readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(cfg =>
            {
                cfg.CreateMap<WriteModels.Profile, ProfileDto>()
                    .ForMember(prop => prop.IsMyProfile, conf => conf.ResolveUsing((src, dest, destMember, resContext) =>
                        dest.IsMyProfile = (bool)(resContext.Items.ContainsKey("IsMyProfile") ? resContext.Items["IsMyProfile"] : false)))
                    .ForMember(prop => prop.IsFriend, conf => conf.ResolveUsing((src, dest, destMember, resContext) =>
                        dest.IsFriend = (bool)(resContext.Items.ContainsKey("IsFriend") ? resContext.Items["IsFriend"] : false)))
                    .ForMember(prop => prop.FirstName, conf => conf.MapFrom(p => p.FirstName))
                    .ForMember(prop => prop.LastName, conf => conf.MapFrom(p => p.LastName))
                    .ForMember(prop => prop.DateOfBirth, conf => conf.MapFrom(p => p.DateOfBirth))
                    .ForMember(prop => prop.Id, conf => conf.MapFrom(p => p.ExternalId))
                    .ForMember(prop => prop.Geolocation, conf => conf.ResolveUsing((src, dest, destMember, resContext) =>
                        dest.Geolocation =
                            ((bool)(resContext.Items.ContainsKey("IsMyProfile") ? resContext.Items["IsMyProfile"] : false) ||
                            (bool)(resContext.Items.ContainsKey("IsFriend") ? resContext.Items["IsFriend"] : false))
                                ? new GeolocationDto { Latitude = src.Geolocation.Latitude, Longitude = src.Geolocation.Longitude }
                                : new GeolocationDto { Latitude = 0, Longitude = 0 }))
                    .ForMember(prop => prop.IsOnline, conf => conf.MapFrom(p => p.IsOnline));

                cfg.CreateMap<Geolocation, GeolocationDto>()
                    .ForMember(prop => prop.Latitude, conf => conf.MapFrom(p => p.Latitude))
                    .ForMember(prop => prop.Longitude, conf => conf.MapFrom(p => p.Longitude));

                WriteModels.Profile profile = null;
                cfg.CreateMap<Friend, FriendDto>()
                    .ForMember(prop => prop.FirstName, conf => conf.ResolveUsing((src, dest, destMember, resContext) =>
                        resContext.Items.ContainsKey("Profile") ?
                            src.RequestedBy == (WriteModels.Profile)resContext.Items["Profile"] ? dest.FirstName = src.RequestedTo.FirstName : dest.FirstName = src.RequestedBy.FirstName
                            : dest.FirstName = ""))
                    .ForMember(prop => prop.LastName, conf => conf.ResolveUsing((src, dest, destMember, resContext) =>
                        resContext.Items.ContainsKey("Profile") ?
                            src.RequestedBy == (WriteModels.Profile)resContext.Items["Profile"] ? dest.LastName = src.RequestedTo.LastName : dest.LastName = src.RequestedBy.LastName
                            : dest.LastName = ""))
                    .ForMember(prop => prop.FriendFlag, conf => conf.MapFrom(p => p.FriendFlag))
                    .ForMember(prop => prop.Id, conf => conf.ResolveUsing((src, dest, destMember, resContext) =>
                        resContext.Items.ContainsKey("Profile") ?
                            src.RequestedBy == (WriteModels.Profile)resContext.Items["Profile"] ? dest.Id = src.RequestedTo.ExternalId : dest.Id = src.RequestedBy.ExternalId
                            : dest.Id = ""))
                    .ForMember(prop => prop.RequestId, conf => conf.MapFrom(p => p.Id));


                cfg.CreateMap<Friend, GeomarkerDto>()
                    .ForMember(prop => prop.FullName, conf => conf.MapFrom(p => p.RequestedBy == profile ? p.RequestedTo.FirstName + " " + p.RequestedTo.LastName : p.RequestedBy.FirstName + " " + p.RequestedBy.LastName))
                    .ForMember(prop => prop.Geolocation, conf => conf.MapFrom(p => p.RequestedBy == profile ? p.RequestedTo.Geolocation : p.RequestedBy.Geolocation));

                cfg.CreateMap<Geolocation, LastGeolocation>();
                cfg.CreateMap<LastGeolocation, Geolocation>();

                cfg.CreateMap<MessageGroup, MessageGroupDto>()
                    .ForMember(prop => prop.Id, conf => conf.MapFrom(p => p.Id.ToString()))
                    .ForMember(prop => prop.GroupName, conf => conf.MapFrom(p => p.GroupName))
                    .ForMember(prop => prop.MembersCount, conf => conf.MapFrom(p => p.MembersCount));

                cfg.CreateMap<Message, MessageDto>()
                    .ForMember(prop => prop.MessageFrom, conf => conf.MapFrom(p => p.MessageFrom))
                    .ForMember(prop => prop.MessageBody, conf => conf.MapFrom(p => p.MessageBody))
                    .ForMember(prop => prop.MessageTime, conf => conf.MapFrom(p => p.MessageDate))
                    .ForMember(prop => prop.MessageGroupId, conf => conf.MapFrom(p => p.MessageGroupId))
                    .ForMember(prop => prop.IsMyMessage, conf => conf.ResolveUsing((src, dest, destMember, resContext) =>
                        resContext.Items.ContainsKey("Profile") ? src.MessageFrom == (WriteModels.Profile)resContext.Items["Profile"] : false));

                cfg.CreateMap<Post, PostDto>()
                    .ForMember(prop => prop.PostBody, conf => conf.MapFrom(p => p.PostBody))
                    .ForMember(prop => prop.LikesCount, conf => conf.MapFrom(p => p.LikesCount))
                    .ForMember(prop => prop.PostDate, conf => conf.MapFrom(p => p.PostDate))
                    .ForMember(prop => prop.PostId, conf => conf.MapFrom(p => p.Id))
                    .ForMember(prop => prop.PostAuthor, conf => conf.MapFrom(p => p.Profile))
                    .ForMember(prop => prop.IsLikedByMe, conf => conf.ResolveUsing((src, dest, destMember, resContext) =>
                    resContext.Items.ContainsKey("Profile") ? src.IsLikedByMe((WriteModels.Profile)resContext.Items["Profile"]) : false));

                cfg.CreateMap<Meeting, MeetingDto>()
                    .ForMember(prop => prop.Friends, conf => conf.MapFrom(p => p.Friends))
                    .ForMember(prop => prop.MeetingLocation, conf => conf.MapFrom(p => p.MeetingLocation))
                    .ForMember(prop => prop.MeetingTime, conf => conf.MapFrom(p => p.MeetingTime));
            });

            services.AddDbContext<MSMContext>(options => 
                options.UseSqlServer(Configuration.GetConnectionString("SocialMediaDb")));

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            });

            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<MSMContext>()
                .AddDefaultTokenProviders();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = Configuration.GetSection("JwtOptions").GetValue<string>("JwtIssuer"),
                        ValidAudience = Configuration.GetSection("JwtOptions").GetValue<string>("JwtIssuer"),
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("JwtOptions").GetValue<string>("JwtKey"))),
                        ClockSkew = TimeSpan.Zero,
                    };
                    cfg.Events = new JwtBearerEvents()
                    {
                        OnMessageReceived = context =>
                        {
                            if (context.Request.Path.ToString().StartsWith("/hub/"))
                                context.Token = context.Request.Query["auth_token"];
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
            {
                builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithOrigins("http://localhost:8080")
                    .DisallowCredentials();
            }));

            services.AddSingleton<IJwtFactory, JwtFactory>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IProfileRepository, ProfileRepository>();
            services.AddScoped<IFriendRepository, FriendRepository>();
            services.AddScoped<IGeoRepository, GeoRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IMeetingRepository, MeetingRepository>();
            services.AddScoped<ILastGeoRepository, LastGeoRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IMessageGroupRepository, MessageGroupRepository>();
            services.AddScoped<IMessageGroupProfileRepository, MessageGroupProfileRepository>();
            services.AddScoped<IPostRepository, PostRepository>();

            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IProfileService, ProfileService>();

            services.AddScoped<UserHub>();
            services.AddScoped<ChatHub>();

            services.AddSignalR();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseSignalR(conf =>
            {
                conf.MapHub<UserHub>("/hub/userStatus");
                conf.MapHub<ChatHub>("/hub/messaging");
            });

            app.UseMvc();
        }
    }
}
