using BAL.UnitTest.Common;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using ReizzzToDo.BAL.Common;
using ReizzzToDo.BAL.Extensions;

namespace BAL.UnitTest.Services
{
    public class HttpContextAccessorExtensionTest : HttpContextAccessorTestSetup
    {

        [Fact]
        protected void GetJwtSubValue_Should_ReturnApplicationException_WhenCantAccessJwt()
        {
            // Arrange
            _httpContextAccessorMock.HttpContext.Returns(new DefaultHttpContext());
            // Act
            Func<long> act = () => _httpContextAccessorMock.GetJwtSubValue();

            // Assert
            act.Should().Throw<ApplicationException>()
                .WithMessage(ErrorMessage.JwtClaimCantReached);
        }
        [Fact]
        protected void GetJwtSubValue_Should_ReturnJwtSubValue()
        {
            // Arrange

            // Act
            long jwtSub = _httpContextAccessorMock.GetJwtSubValue();

            // Assert
            jwtSub.Should().Be(1);
        }
    }
}
