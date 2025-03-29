using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using ReizzzToDo.BAL.Common;
using ReizzzToDo.BAL.Extensions;

namespace BAL.UnitTest.Common
{
    public class HttpContextAccessorTestSetup
    {
        protected IHttpContextAccessor _httpContextAccessorMock { get; set; }
        protected ClaimsPrincipal claimsPrincipal;
        public HttpContextAccessorTestSetup()
        {
            _httpContextAccessorMock = Substitute.For<IHttpContextAccessor>();

            // Mock user's claims
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,"1")
            };
            var identity = new ClaimsIdentity(claims, "Test Auth Type");
            claimsPrincipal = new ClaimsPrincipal(identity);
            _httpContextAccessorMock.HttpContext.Returns(new DefaultHttpContext { User = claimsPrincipal });
        }
    }
}
