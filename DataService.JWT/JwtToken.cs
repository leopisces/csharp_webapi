using DataService.Shared.Base;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DataService.JWT
{
    /// <summary>
    /// JWTToken生成类
    /// </summary>
    public class JwtToken
    {
        /// <summary>
        /// 获取基于JWT的Token
        /// </summary>
        /// <param name="claims"></param>
        /// <param name="permissionRequirement"></param>
        /// <returns></returns>
        public async static Task<TokenInfo> BuildJwtToken(List<Claim> claims, PermissionRequirement permissionRequirement)
        {
            return await Task.Run(() =>
            {
                var now = DateTime.UtcNow;
                var jwt = new JwtSecurityToken(
                    issuer: permissionRequirement.Issuer,
                    audience: permissionRequirement.Audience,
                    claims: claims,
                    notBefore: now,
                    expires: now.Add(permissionRequirement.Expiration),
                    signingCredentials: permissionRequirement.SigningCredentials
                );
                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
                var responseJson = new TokenInfo
                {
                    Status = true,
                    Access_Token = encodedJwt,
                    Expires_In = permissionRequirement.Expiration.TotalMilliseconds,
                    Token_Type = "Bearer"
                };
                return responseJson;
            });
        }
    }
}
