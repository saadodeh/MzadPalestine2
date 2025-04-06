using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace MzadPalestine.API.Filters;

public class ApiControllerConvention : IControllerModelConvention
{
    public void Apply(ControllerModel controller)
    {
        // إضافة Route attribute فقط إذا لم يكن موجوداً
        if (!controller.Selectors.Any(s => s.AttributeRouteModel != null))
        {
            // التأكد من عدم إضافة مسار مكرر
            var routeTemplate = "api/[controller]";
            
            // استخراج اسم الكنترولر الحقيقي (بدون كلمة "Controller")
            var controllerName = controller.ControllerType.Name;
            if (controllerName.EndsWith("Controller"))
            {
                controllerName = controllerName.Substring(0, controllerName.Length - "Controller".Length);
            }
            
            controller.Selectors.Add(new SelectorModel
            {
                AttributeRouteModel = new AttributeRouteModel
                {
                    Template = routeTemplate
                }
            });
        }
    }
} 