using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Newtonsoft.Json;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TestId4
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            if (context.Request.Client.ClientId == "hms_client")
            {
                context.Result = new GrantValidationResult(
                 subject: context.UserName,
                 authenticationMethod: OidcConstants.AuthenticationMethods.Password,
                 claims: new Claim[] {
                    new Claim("role","1"),
                    new Claim("hotel",Guid.NewGuid().ToString()),
                    new Claim("store",Guid.NewGuid().ToString()),
                    new Claim("user",Guid.NewGuid().ToString())
                 });
                return Task.FromResult(0);
            }

            //验证失败
            context.Result = new GrantValidationResult(
                TokenRequestErrors.InvalidGrant,
                "invalid custom credential"
                );
            return Task.FromResult(0);
        }
    }
}
