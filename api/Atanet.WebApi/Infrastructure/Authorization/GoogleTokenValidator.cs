namespace Atanet.WebApi.Infrastructure.Authorization
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using Atanet.Services.ApiResult;
    using Atanet.Services.Exceptions;
    using Google.Apis.Auth;
    using Microsoft.IdentityModel.Tokens;

    public class GoogleTokenValidator : ISecurityTokenValidator
    {
        private readonly JwtSecurityTokenHandler tokenHandler;

        private readonly IApiResultService apiResultService;

        public GoogleTokenValidator(IApiResultService apiResultService)
        {
            this.tokenHandler = new JwtSecurityTokenHandler();
            this.apiResultService = apiResultService;
        }

        public bool CanValidateToken => true;

        public int MaximumTokenSizeInBytes { get; set; } = TokenValidationParameters.DefaultMaximumTokenSizeInBytes;

        public bool CanReadToken(string securityToken)
        {
            return this.tokenHandler.CanReadToken(securityToken);
        }

        public ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            validatedToken = null;
            var clientId = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID");
            var payload = GoogleJsonWebSignature.ValidateAsync(securityToken, new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new[] { clientId }
            }).Result;

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, payload.Name),
                new Claim(ClaimTypes.Name, payload.Name),
                new Claim(JwtRegisteredClaimNames.FamilyName, payload.FamilyName),
                new Claim(JwtRegisteredClaimNames.GivenName, payload.GivenName),
                new Claim(JwtRegisteredClaimNames.Email, payload.Email),
                new Claim(JwtRegisteredClaimNames.Sub, payload.Subject),
                new Claim(JwtRegisteredClaimNames.Iss, payload.Issuer),
            };

            try
            {
                var principle = new ClaimsPrincipal(new ClaimsIdentity(claims, "Bearer"));
                return principle;
            }
            catch (Exception e)
            {
                throw new ApiException(this.apiResultService.InternalServerErrorResult(e));
            }
        }
    }
}