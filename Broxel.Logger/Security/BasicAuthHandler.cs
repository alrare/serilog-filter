using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Broxel.Logger.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Superpower.Model;

namespace Broxel.Logger.Security
{
    public class BasicAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IUserService _userService;

        public BasicAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IUserService userService
        ) : base(options, logger, encoder, clock) 
        {
            _userService = userService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if(!Request.Headers.ContainsKey("Authorization"))
            return AuthenticateResult.Fail("Error de autenticación");

            bool result = false;

            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(new[] { ':' }, 2);
                var email = credentials[0];
                var pass = credentials[1];
                result = _userService.IsUser(email, pass);

            }catch
            {
                return AuthenticateResult.Fail("Ocurrio algo");
            }

            if(!result)
            return AuthenticateResult.Fail("Usuario y/o contraseña invalida");

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "IdUser"),
                new Claim(ClaimTypes.Name, "IdName"),
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name );
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);

        }    
    }
}