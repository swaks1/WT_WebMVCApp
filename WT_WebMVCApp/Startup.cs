using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using WT_WebMVCApp.Helpers;
using WT_WebMVCApp.Services;

namespace WT_WebMVCApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //maybe this has to be Signleton aslo ?/
            services.AddScoped<IWorkoutTrackerHttpClient, WorkoutTrackerHttpClient>();

            services.AddScoped<IWorkoutTrackerService, WorkoutTrackerService>();

            //Authenticaiton and Authorization with IdentityServer4
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("Cookies", (options) =>
             {
                 options.AccessDeniedPath = "/Home/AccessDenied";
             })
            .AddOpenIdConnect("oidc", options =>
            {
                options.SignInScheme = "Cookies";
                options.Authority = WorkotTrackerHelper.IdentityServerUrl;
                options.ClientId = "wtmvcapp";
                options.ResponseType = "code id_token";
                //options.CallbackPath = new PathString("...")
                //options.SignedOutCallbackPath = new PathString("...")
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("address");
                options.Scope.Add("roles");
                options.Scope.Add("subscriptionlevel");
                options.Scope.Add("country");
                options.Scope.Add("wtapi"); //scope for the API
                options.Scope.Add("offline_access"); //scope for refresh tokens
                options.SaveTokens = true;
                options.ClientSecret = "secret";
                options.GetClaimsFromUserInfoEndpoint = true;
                options.ClaimActions.Remove("amr");
                options.ClaimActions.DeleteClaim("sid");
                options.ClaimActions.DeleteClaim("idp");
                //options.ClaimActions.DeleteClaim("address");
                options.ClaimActions.MapUniqueJsonKey("role", "role");
                options.ClaimActions.MapUniqueJsonKey("subscriptionlevel", "subscriptionlevel");
                options.ClaimActions.MapUniqueJsonKey("country", "country");

                //for using roles
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = JwtClaimTypes.GivenName,
                    RoleClaimType = JwtClaimTypes.Role,
                };

            });

            #region Policy for authorization if needed... (Roles based authorization is used now so this is commented...)
            //for using Policy instead of roles..in this example the country of the user must be mkd to continue... 
            //the actions are decorated with[Authorize(Policy = "MustOwnImage")]
            //services.AddAuthorization(authorizationOptions =>
            //{
            //    authorizationOptions.AddPolicy(
            //        "MustOwnImage",
            //        policyBuilder =>
            //        {
            //            policyBuilder.RequireAuthenticatedUser();
            //            policyBuilder.RequireClaim("country", "mkd");
            //        });
            //});
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            //Authentication and Authorizaiton Enabled
            app.UseAuthentication();

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=WorkoutExercise}/{action=Index}/{id?}");
            });
        }
    }
}
