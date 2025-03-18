using AutoMapper;
using Microsoft.AspNetCore.Identity;
using UsuariosApi.Data.DTOs;
using UsuariosApi.Models;

namespace UsuariosApi.Services;

public class CadastroService
{
    private IMapper _mapper;
    private UserManager<Usuario> _userManager;

    public CadastroService(IMapper mapper, UserManager<Usuario> userManager)
    {
        _mapper = mapper;
        _userManager = userManager;
    }


    public async Task CadastrarAsync(CreateUsuarioDto dto)
    {
        Usuario usuario = _mapper.Map<Usuario>(dto);

        var result = await _userManager.CreateAsync(usuario, dto.Password);      
        
        if(!result.Succeeded)
            throw new ApplicationException(result.Errors?.FirstOrDefault()?.Description);
    }
}
