using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace DataAPI.Providers
{
    public class SimpleAuthProvider : OAuthAuthorizationServerProvider
    {
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            return Task.FromResult<object>(null);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            await Task.Factory.StartNew(() =>
            {
                // TODO: validate user/pwd from sql client's connection string
                if (context.UserName != "testuser" || context.Password != "testpwd")
                {
                    context.SetError("invalid_grant", "invalid username or password");
                    return;
                }

                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));

                var ticket = new AuthenticationTicket(identity, new AuthenticationProperties());
                context.Validated(ticket);
            });
        }
    }
}