namespace BaseOwinService.Providers
{
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.OAuth;

    public class ApiAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId;
            string clientSecret;

            if (!context.TryGetFormCredentials(out clientId, out clientSecret))
            {
                context.Rejected();
            }

            // TODO: validate the client id
            if (clientId != null) await Task.FromResult<object>(context.Validated());
        }

        public override Task GrantClientCredentials(OAuthGrantClientCredentialsContext context)
        {
            // api authentication 

            // TODO: create application claim
            var oAuthIdentity = new ClaimsIdentity(context.Options.AuthenticationType);
            oAuthIdentity.AddClaim(new Claim(ClaimTypes.Name, "ApplicationName"));
            var ticket = new AuthenticationTicket(oAuthIdentity, new AuthenticationProperties());
            context.Validated(ticket);

            return base.GrantClientCredentials(context);
        }

        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            // user authentication

            var userName = context.UserName;
            var password = context.Password;

            // TODO: validate user name and password
            var isValidUser = true;

            if (!isValidUser)
            {
                // TODO: exception message for failed attempt
                context.SetError("invalid_grant", "exception message");
                return base.GrantResourceOwnerCredentials(context);
            }


            // TODO: create user claim
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "user name"));
            context.Validated(identity);

            return base.GrantResourceOwnerCredentials(context);
        }

    }
}