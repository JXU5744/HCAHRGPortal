using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using HCAaudit.Service.Portal.AuditUI.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Owin.Security.OpenIdConnect;
using HCAaudit.Service.Portal.AuditUI.Models;
using Microsoft.EntityFrameworkCore;

namespace HCAaudit.Service.Portal.AuditUI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private IWebHostEnvironment HostEnvironment { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddMvc(options => options.EnableEndpointRouting = false);

            services.AddLogging();
            services.AddDbContext<AuditToolContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:HRAuditDatabase"]));
            services.AddDistributedMemoryCache();
            services.AddHttpContextAccessor();
            services.Configure<IISServerOptions>(options =>
            {
                options.AutomaticAuthentication = false;
            });
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(600);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
                .AddCookie(options =>
                {
                    options.Cookie.MaxAge = new TimeSpan(0, int.Parse(Configuration["OidcTimeout"]), 0);
                    // force HTTPS when not running locally
                    options.Cookie.SecurePolicy = HostEnvironment.IsDevelopment() ? CookieSecurePolicy.None : CookieSecurePolicy.Always;
                })
                .AddOpenIdConnect(options =>
                {
                    options.SignOutScheme = "Cookies";
                    options.SignInScheme = "Cookies";
                    options.Authority = Configuration.GetValue<string>("OidcAuthority");
                    options.MetadataAddress = Configuration["OidcAuthority"] + Configuration["OidcMetadataEndpoint"];
                    options.RequireHttpsMetadata = true;
                    options.ClaimsIssuer = Configuration["OidcAAuthority"];
                    options.CallbackPath = Configuration["OidcCallbackPath"];
                    options.ClientId = Configuration.GetValue<string>("OidcAudience");
                    options.SaveTokens = Configuration.GetValue<bool>("OidcSaveTokens");
                    options.ResponseType = "code";
                    //Clear default scopes before adding custom HCA scopes
                    options.Scope.Clear();
                    options.Scope.Add(Configuration.GetValue<string>("OidcScope"));
                    options.UsePkce = true;
                    options.DisableTelemetry = true;
                    options.GetClaimsFromUserInfoEndpoint = false;
                    options.RefreshOnIssuerKeyNotFound = true;
                    //Time allowed for user to authenticate (uid/pw, MFA, change password, etc)
                    options.RemoteAuthenticationTimeout = new TimeSpan(0, Configuration.GetValue<int>("OidcTimeout"), 0);
                    //Logout handler
                    options.Events.OnRedirectToIdentityProviderForSignOut = context =>
                    {
                        //TODO: Log this.
                        var logoutUri = GetAbsoluteUri(Configuration["OidcLogoutEndpoint"], Configuration["OidcAuthority"]);
                        context.Response.Redirect(logoutUri);
                        context.HandleResponse();
                        return Task.CompletedTask;
                    };
                    //Initiating OAuth authorization request handler
                    options.Events.OnRedirectToIdentityProvider = context =>
                    {
                        //TODO: Log this.
                        return Task.CompletedTask;
                    };
                    //Authentication Failed handler (this shouldn't happen with current HCA IDP solution)
                    options.Events.OnAuthenticationFailed = context =>
                    {
                        //TODO: Log this.
                        return Task.CompletedTask;
                    };
                    //Authorization Code received handler
                    options.Events.OnAuthorizationCodeReceived = context =>
                    {
                        //TODO: Log this.
                        return Task.CompletedTask;
                    };
                    //Tokens received handler
                    options.Events.OnTokenResponseReceived = context =>
                    {
                        //TODO: Log this.
                        if (!String.IsNullOrEmpty(context.TokenEndpointResponse?.AccessToken))
                        {
                            // store and update access token value for ADAPI
                            context.HttpContext.Session.SetString("access_token", context.TokenEndpointResponse.AccessToken);
                        }
                        if (!String.IsNullOrEmpty(context.TokenEndpointResponse?.IdToken))
                        {
                            // store and update id token value for ADAPI
                            context.HttpContext.Session.SetString("id_token", context.TokenEndpointResponse.IdToken);
                        }
                        return Task.CompletedTask;
                    };
                    //Tokens validated handler
                    options.Events.OnTokenValidated = context =>
                    {
                        //TODO: Log this.
                        return Task.CompletedTask;
                    };
                });
            services.AddAuthorization();
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IAuthService, AuthService>();

            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddSingleton(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            HostEnvironment = env;
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseStaticFiles();
            app.UseRouting();
            //app.UseSession();
            //app.UseAuthentication();
            //app.UseAuthorization();
            //app.UseHttpsRedirection();
            //app.UseCookiePolicy();

            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute("default", "{controller=Login}/{action=Index}/{id?}");
            //});

            app.UseEndpoints(endpoint =>
            {
                endpoint.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=login}/{action=Index}/{id?}");
            });
        }

        #region"Private Member"
        private string GetAbsoluteUri(string signoutUri, string authority)
        {
            var signOutUri = new Uri(signoutUri, UriKind.RelativeOrAbsolute);
            var authorityUri = new Uri(authority, UriKind.Absolute);
            var uri = signOutUri.IsAbsoluteUri ? signOutUri : new Uri(authorityUri, signOutUri);
            return uri.AbsoluteUri;
        }
        #endregion
    }
}
