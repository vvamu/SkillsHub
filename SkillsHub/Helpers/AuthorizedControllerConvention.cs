using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace SkillsHub.Helpers;

public class AuthorizedControllerConvention : IControllerModelConvention
{
    public void Apply(ControllerModel controller)
    {
        // Check if the controller has the [Authorize] attribute
        var hasAuthorizeAttribute = controller.ControllerType.GetCustomAttributes(typeof(AuthorizeAttribute), true).Any();

        // If the controller is authorized, add the "CRM" prefix to its route
        if (hasAuthorizeAttribute)
        {
            if (controller.Selectors.Any() && controller.Selectors[0].AttributeRouteModel != null)
            {
                controller.Selectors[0].AttributeRouteModel.Template = "CRM/" + controller.Selectors[0].AttributeRouteModel.Template;
            }
        }
    }
}
