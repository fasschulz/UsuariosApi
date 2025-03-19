using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace UsuariosApi.Authorization;

public class IdadeAuthorization : AuthorizationHandler<IdadeMinima>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IdadeMinima requirement)
    {
        var dtNascClaim = context.User.FindFirst(claim => claim.Type == ClaimTypes.DateOfBirth);

        if (dtNascClaim is null)
            return Task.CompletedTask;

        var dtNasc = Convert.ToDateTime(dtNascClaim.Value);

        var idadeUsuario = DateTime.Today.Year - dtNasc.Year;

        if (dtNasc > DateTime.Today.AddYears(-idadeUsuario))
            idadeUsuario--;

        if (idadeUsuario >= requirement.Idade)
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
