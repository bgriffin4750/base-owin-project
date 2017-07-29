using System;
using Microsoft.Owin.Security.Infrastructure;

namespace BaseOwinService.Providers
{
    public class RefreshTokenProvider : AuthenticationTokenProvider
    {
        public override void Create(AuthenticationTokenCreateContext context)
        {
            context.Ticket.Properties.ExpiresUtc = new DateTimeOffset(DateTime.UtcNow.AddMinutes(15));
            context.SetToken(context.SerializeTicket());
        }

        public override void Receive(AuthenticationTokenReceiveContext context)
        {
            context.DeserializeTicket(context.Token);
        }
    }
}