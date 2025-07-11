using Application.DTOs;
using Domain.Entities;

public interface ITokenHandler
	{
		public Token CreateAccessToken(AppUser user, string role);
	}