using ChatBot.Extensions;
using ChatBot.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using ChatBot.Models.Enums;
using Microsoft.AspNetCore.Http;

namespace ChatBot.Auth.Jwt.Impl;

[SingletonService]
public class JwtUtils : IJwtUtils
{
    private readonly SecurityKey _key;
    private readonly JwtSecurityTokenHandler _tokenHandler;
	private readonly TokenValidationParameters _validationParameters;

	public JwtUtils(IConfiguration configuration)
    {
        _key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Secret"]));
		_tokenHandler = new JwtSecurityTokenHandler();
        _validationParameters = new TokenValidationParameters {
            ValidateIssuerSigningKey = true, //validate the key
            IssuerSigningKey = _key, //set the key
            ValidateIssuer = false, //we dont care about issuer
            ValidateAudience = false, //we dont care about audience
            ClockSkew = TimeSpan.Zero //remove delay of token when expire
        };
    }

    public string GenerateToken(User account)
    {
        //token generation logic, valid for 1 day, who doesnt like can change it to more/less
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("UserID", account.ID.ToString()),
                new Claim("Role", account.Role.ToString()) //role to the token?
            }), 
            Expires = DateTime.Now.AddDays(1),
            SigningCredentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor); //create the token
        
        return tokenHandler.WriteToken(token);
    }

    /// <returns>The UserID if validation is successful, otherwise null</returns>
    public Guid? ValidateToken(string token)
    {
        try
        {
            var principal = _tokenHandler.ValidateToken(token, _validationParameters, out _);

            return Guid.Parse(principal.Claims.First(x => x.Type == "UserID").Value);
        }
        catch
        {
            //null if validation fails
            return null;
        }
    }
    }