using AutoMapper;
using Microsoft.AspNetCore.Identity;
using UsuariosApi.Data.DTOs;
using UsuariosApi.Models;

namespace UsuariosApi.Services;

public class UsuarioService
{
    private IMapper _mapper;
    private UserManager<Usuario> _userManager;
    private SignInManager<Usuario> _signInManager;
    private TokenService _tokenService;

    public UsuarioService(IMapper mapper, UserManager<Usuario> userManager, 
        SignInManager<Usuario> signInManager, TokenService tokenService)
    {
        _mapper = mapper;
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
    }


    public async Task CadastrarAsync(CreateUsuarioDto dto)
    {
        Usuario usuario = _mapper.Map<Usuario>(dto);

        var result = await _userManager.CreateAsync(usuario, dto.Password);      
        
        if(!result.Succeeded)
            throw new ApplicationException(result.Errors?.FirstOrDefault()?.Description);
    }

    public async Task<string> LoginAsync(LoginUsuarioDto dto)
    {
       var result = await _signInManager.PasswordSignInAsync(dto.Username, dto.Password, false, false);

       if(!result.Succeeded)
           throw new ApplicationException("Usuário não autenticado");

        var usuario = _signInManager.UserManager.Users
             .FirstOrDefault(user => user.NormalizedUserName == dto.Username.ToUpper());

        return _tokenService.GenerateToken(usuario);
    }
}
