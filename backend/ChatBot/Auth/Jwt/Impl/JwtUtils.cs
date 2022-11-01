using ChatBot.Extensions;
using ChatBot.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace ChatBot.Auth.Jwt.Impl;

[TransientService]
public class JwtUtils : IJwtUtils
{
    IConfiguration _configuration;

    public JwtUtils(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    public string GenerateToken(User account)
    {
        //token generation logic, valid for 1 day, who doesnt like can change it to more/less
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Secret"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("id", account.ID.ToString()) }), //seems familiar?
            Expires = DateTime.Now.AddDays(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor); //create the token
        return tokenHandler.WriteToken(token);
    }

    public Guid? ValidateToken(string token) //returns userId if validation is successful
    {
        if (token is null)
            return null;

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Secret"]);

        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true, //validate the key
                IssuerSigningKey = new SymmetricSecurityKey(key), //set the key
                ValidateIssuer = false, //we dont care about issuer
                ValidateAudience = false, //we dont care about audience
                ClockSkew = TimeSpan.Zero //remove delay of token when expire
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            //returned userId from the token
            return Guid.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);
        }
        catch
        {
            //null if validation fails
            return null;
        }
    }
}